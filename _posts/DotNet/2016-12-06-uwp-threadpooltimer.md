---
layout: post
title:  "Windows通用程序（UWP）中的定时器"
date:   2016-12-06 12:00:00
categories: dotnet
tags: [uwp, timer, thread, threadpool]
description: 
---

<!--more-->

在UWP程序中想实现在线程中执行定时任务，但是没有Thread的概念，不能用Thread.Sleep来暂停线程，微软的建议是在UWP中使用Windows.System.Threading.ThreadPoolTimer，使用方法如下。


```csharp
    TimeSpan period = TimeSpan.FromSeconds(10);

    var timer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
    {
        try
        {
            //do something
        }
        catch (Exception e)
        {
            
        }
    },
    period);
```

创建出timer以后，就会每隔10秒执行一次，如果想停止，调用<code>ThreadPoolTimer.Cancel()</code>。

关于线程和异步的使用方法，参考微软的官方文档[https://msdn.microsoft.com/zh-cn/windows/uwp/threading-async/index](https://msdn.microsoft.com/zh-cn/windows/uwp/threading-async/index)
