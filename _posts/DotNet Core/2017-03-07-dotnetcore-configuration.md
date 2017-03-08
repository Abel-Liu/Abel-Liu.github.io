---
layout: post
title:  ".Net Core使用配置文件"
date:   2017-03-07 20:00:00
categories: [dotnet, dotnet core]
tags: [dotnet, core, configuration]
description: 
---
<!--more-->

.NET Core中使用Json文件来存储配置。在项目中新建```appsettings.json```文件，内容示例：

```json
{
  "option1": "value1",
  "option2": 2,

  "subsection": {
    "suboption1": "subvalue1"
  }
}
```

读取和操作配置需要安装以下两个NuGet包。

```
Microsoft.Extensions.Configuration
Microsoft.Extensions.Configuration.Json
```

使用示例：

```csharp
using System;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp1
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            string baseDirectory = AppContext.BaseDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(baseDirectory)
                .AddJsonFile("appsettings.json", true, true);

            Configuration = builder.Build();

            Console.WriteLine($"option1 = {Configuration["option1"]}");
            Console.WriteLine($"option2 = {Configuration["option2"]}");
            Console.WriteLine($"suboption1 = {Configuration["subsection:suboption1"]}");

            Console.ReadLine();
        }
    }
}
```
