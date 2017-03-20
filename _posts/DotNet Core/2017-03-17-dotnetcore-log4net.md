---
layout: post
title:  ".Net Core中使用log4net及乱码解决"
date:   2017-03-17 16:00:00
categories: [dotnet, dotnet core]
tags: [dotnet, log4net]
description: 
---

本文不细讲log4net的用法，只介绍在.NET Core中的不同和乱码问题。

<!--more-->

在.NET Core中，log4net的配置和用法基本一样，下面是几个遇到的问题点。

* 在Main做初始化和创建Repository

本来按照之前的方式在AssemblyInfo中初始化配置，项目中直接GetLogger()，如下：

```csharp
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
```

但是在部署到Linux上运行的时候报找不到文件错误，因为XmlConfigurator初始化的时候在应用程序启动路径下寻找log4net.config文件，而在Linux下启动路径不是我们的程序目录，其实Windows下如果不在程序目录使用cmd也会有一样的错误，比如```C:\>dotnet d:\your\app.dll```，而切换到程序所在目录运行就OK，但是，在Linux下无论怎么运行都不OK。所以换为在Main方法中配置：

```csharp
public static void Main(string[] args)
{
    string baseDirectory = AppContext.BaseDirectory;
    var logCfg = new FileInfo(Path.Combine(baseDirectory, "log4net.config"));
    var repo = LogManager.CreateRepository("default");
    XmlConfigurator.ConfigureAndWatch(repo, logCfg);

    var logger = log4net.LogManager.GetLogger("default", "mylog");
}
```

.NET Core 中 XmlConfigurator.ConfigureAndWatch()方法少了很多重载，必须手动创建一个Repository并在GetLogger时指定，才能正确获取到logger，否则不会写内容。

* 控制台乱码

在.NET Core中将日志写到控制台，中文是乱码，就算在ConsoleAppender中配置Encoding也不行，而写到文件中的正常。后来找到解决办法，添加```System.Text.Encoding.CodePages```NuGet包，在Main方法中加一行配置代码：

```csharp
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
```

这时候控制台输出正常了，但是文件里又乱码了，这次在FileAppender配置里加上Encoding就好了：

```xml
<appender name="RollingLogFileAppender" type="log4net.Appender.RollingFileAppender">
    <param name="Encoding" value="utf-8" />
    //其他配置省略
</appender>
```