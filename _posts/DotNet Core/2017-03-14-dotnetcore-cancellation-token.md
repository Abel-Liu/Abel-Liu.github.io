---
layout: post
title:  ".Net Core中结束线程"
date:   2017-03-14 20:00:00
categories: [dotnet, dotnet core]
tags: [dotnet, thread, cancellationtoken, abort]
description: 
---

本文讲解在.NET Core中使用CancellationToken结束线程。

<!--more-->

在.NET Framework中结束线程非常容易和熟悉，直接Thread.Abort()，但在.NET Core中没有Abort方法了。而且在.NET Framework中也不再推荐直接Abort掉线程，因为这使正在运行的任务变得不可预期，有时候可能影响数据或逻辑。

推荐的方式是使用CancellationToken，通过通知正在运行的线程，使其安全结束。其实我们以前也实现过这种方式，比如设置一个共享bool型变量，标识线程是否需要结束，主线程中设置bool值，线程中判断需要结束时就放弃当前的任务来结束。CancellationToken的方式其实类似，不过也有更高级的使用方式。

下面是一个我们会使用的一个经典场景，看CancellationToken的用法：

```csharp
class Program
{
    static void Main(string[] args)
    {
        MyWork work = new MyWork();
        work.Run();

        //other code

        work.Cancel();
    }
}

public class MyWork
{
    private CancellationTokenSource cts;
    private CancellationToken token;

    public MyWork()
    {
        cts = new CancellationTokenSource();
        token = cts.Token;
    }

    public void Run()
    {
        Thread t = new Thread(new ThreadStart(DoSomething));
        t.Start();
    }

    private void DoSomething()
    {
        while (true)
        {
            //do your work
            Thread.Sleep(1000);

            if (token.IsCancellationRequested)
            {
                return;
            }
        }
    }

    public void Cancel()
    {
        cts.Cancel();
        cts.Dispose();
    }
}
```

注意CancellationTokenSource在线程结束后必须Dispose。

当然如果确实想用Abort也有办法，通过反射调用Thread内部的结束方法。

```csharp
public static void Abort(Thread thread)
{
    MethodInfo abort = null;
    foreach(MethodInfo m in thread.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
    {
        if (m.Name.Equals("AbortInternal") && m.GetParameters().Length == 0)
            abort = m;
    }

    if (abort == null)
    {
        throw new Exception("Failed to get Thread.Abort method");
    }
    
    abort.Invoke(thread, new object[0]);
}
```

更多CancellationToken用法参考文档：[https://msdn.microsoft.com/en-us/library/dd997364(v=vs.110).aspx](https://msdn.microsoft.com/en-us/library/dd997364(v=vs.110).aspx)