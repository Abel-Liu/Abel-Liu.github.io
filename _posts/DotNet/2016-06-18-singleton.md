---
layout: post
title:  "单例模式"
date:   2016-06-18 19:40:00
categories: dotnet
tags: [singleton]
description: 
---

<!--more-->

单例的意思是说在系统中一个类保证只有一个实例，不知道概念的左拐度娘。

基本写法

```csharp
    public class Singleton
    {
        private Singleton()
        {
            //do something
        }

        private static Singleton instance = null;

        public static Singleton GetInstance()
        {
            if ( instance == null )
                instance = new Singleton();

            return instance;
        }
        
    }
```

仔细一看直接定义一个静态变量不就是了吗，所以在C#中特别简单

```csharp
    public class Singleton
    {
        private Singleton() { }

        public static readonly Singleton Instance = new Singleton();

    }
```

[咧嘴坏笑.jpg]

但是，这里有一个区别，第二种虽然只用了两行代码，但在程序开始运行后就实例化了，而第一种只有在第一次用到时才实例化，所以存在懒加载的问题，在实际使用中根据情况选择。

回到第一种写法，这种写法一般使用没什么问题，但多线程情况下就不一定了，如果第一个线程判断instance为null，进行实例化，但是实例化没完成前有其他的线程进行if判断，就会重复实例化，所以比较完善的写法是加锁。

```csharp
    public class Singleton
    {
        private Singleton()
        {
            //do something
        }

        private static readonly object mutex = new object();

        private static volatile Singleton instance = null;

        public static Singleton GetInstance()
        {
            if ( instance == null )
            {
                lock ( mutex )
                {
                    if ( instance == null )
                        instance = new Singleton();
                }
            }

            return instance;
        }
        
    }
```

（不知道volatile的看我转载的文章）

用两个if也是为了防止不同线程重复实例化。

Enjoy.

