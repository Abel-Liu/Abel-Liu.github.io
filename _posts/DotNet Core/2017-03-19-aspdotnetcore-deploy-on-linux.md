---
layout: post
title:  "ASP.Net Core部署到Linux"
date:   2017-03-19 22:00:00
categories: [dotnet, dotnet core]
tags: [dotnet, core, deploy, Linux]
description: 
---

本文讲解ASP.NET Core部署到Linux的过程。

<!--more-->

* 在Linux上安装.NET Core

本文是在CentOS 7上部署的，按照官网步骤，简单解释一下：

```shell
先安装依赖包
sudo yum install libunwind libicu

从指定地址下载文件，保存为dotnet.tar.gz
curl -sSL -o dotnet.tar.gz https://go.microsoft.com/fwlink/?linkid=843449

创建/opt/dotnet目录，然后解压刚才下载的文件到此目录
sudo mkdir -p /opt/dotnet && sudo tar zxf dotnet.tar.gz -C /opt/dotnet

将dotnet链接到/usr/local/bin，这样才能找得到dotnet命令
sudo ln -s /opt/dotnet/dotnet /usr/local/bin
```

在shell中输入dotnet可以看到输出信息，证明安装成功。

其他版本参考官网：[https://www.microsoft.com/net/core#linuxcentos](https://www.microsoft.com/net/core#linuxcentos)

* 发布并拷贝文件到Linux

新建一个.NET Core Web Application，发布项目到本地目录，将发布好的目录上传到Linux，可以使用FTP工具。我们以HelloWorld作为项目名，输出的项目主文件就是HelloWorld.dll。

现在运行dotnet HelloWorld.dll即可启动网站，本地访问http://localhost:5000即可打开我们的网站（dotnet默认的端口是5000）。但是localhost只能本地访问，我们想要在远程访问需要更改绑定地址。

* 绑定IP

第一种方式：在Program.cs中加上```UseUrls("http://::5000")```来绑定所有IP，也可以改为其他端口号。

ps:也可以绑定多个端口```UseUrls("http://::5000;http://::5001")```

```csharp
using Microsoft.AspNetCore.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var host = new WebHostBuilder()
            .UseUrls("http://::5000")
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .UseApplicationInsights()
            .Build();

        host.Run();
    }
}
```

第二种方式（**推荐**）：使用hosting.json来配置

在项目中添加hosting.json文件，写入以下内容：

```json
{
  "server.urls": "http://::5000",
  "environment": "Production"
}
```

然后在Program里使用配置。注意删除UseUrls()，添加UseConfiguration()。

```csharp
using Microsoft.Extensions.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("hosting.json", optional: true)
        .Build();

        var host = new WebHostBuilder()
            //.UseUrls("http://::5000;http://::5001")
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .UseApplicationInsights()
            .UseConfiguration(config) //添加此行
            .Build();

        host.Run();
    }
}
```

第三种方式：使用命令行参数

首先添加```Microsoft.Extensions.Configuration.CommandLine``` Nuget包。

然后在Program中使用：

```csharp
using Microsoft.Extensions.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
        .AddCommandLine(args)
        .Build();

        var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .UseApplicationInsights()
            .Build();

        host.Run();
    }
}
```

运行时添加参数：```dotnet HelloWorld.dll --server.urls "http://::5001"```

我实际项目中同时用了hosting.json和命令行参数，这样最方便。

```csharp
public static void Main(string[] args)
{
    var configBuilder = new ConfigurationBuilder();

    if (args != null && args.Length > 0)
        configBuilder.AddCommandLine(args);
    else
        configBuilder.AddJsonFile("hosting.json", optional: true);

    var config = configBuilder.Build();

    var host = new WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseIISIntegration()
        .UseStartup<Startup>()
        .UseConfiguration(config)
        .Build();

    host.Run();
}
```

* 防火墙设置

要想外网访问网站，需要配置防火墙，CentOS 7是使用firewall-cmd，如果是之前的版本使用iptables，关于firewall-cmd和iptables的用法不再赘述。

```shell
firewall-cmd --add-port=5000/tcp --permanent
firewall-cmd --reload
```

现在可以通过外网访问我们的网站了。

* 创建服务

现在我们是在shell中执行dotnet命令来启动网站，如果shell关闭网站也就停止了，所以我们需要创建一个服务来启动网站，而且可以开机启动。先创建一个服务文件。

***不会使用vi编辑器的自行百度，稍后我也会写一个vi的教程。***

```shell
sudo vi /etc/systemd/system/HelloWorld.service
```

输入以下内容并保存：

```shell
[Unit]
Description=HelloWorld Website

[Service]
WorkingDirectory=/root/HelloWorld
ExecStart=/usr/local/bin/dotnet /root/HelloWorld/HelloWorld.dll
Restart=always
RestartSec=10
SyslogIdentifier=HelloWorld-website
User=root
Environment=ASPNETCORE_ENVIRONMENT=Production 

[Install]
WantedBy=multi-user.target
```

/root/HelloWorld是网站目录，User是启动服务的用户，请自行更改需要的用户名。

然后我们先enable服务，然后start，启用并启动服务后就不用管了，以后服务就会一直运行。我们可以用status命令查看服务状态。

```shell
systemctl enable HelloWorld.service
systemctl start HelloWorld.service
systemctl status HelloWorld.service
```

systemctl的常用命令

```shell
systemctl list-unit-files --type=service | grep xx.service

systemctl start xx.service
systemctl restart xx.service
systemctl stop xx.service
systemctl reload xx.service
systemctl status xx.service

systemctl enable xx.service
systemctl disable xx.service
```

本教程到此结束，下篇讲解Nginx的使用。