---
layout: post
title:  "强制Windows服务以32位运行"
date:   2016-03-04 14:00:00
categories: dotnet
tags: [service, 32bit]
description: 
---
最近遇到一个问题，在Windows服务中调用一个第三方的API，这个API是编译为x86的，所以必须通过x86的程序去调。测试程序可以正常调用，但是部署为Windows服务去调的时候就出问题了。原因是在64位机器上Windows服务默认是以64位启动的。
<!--more-->

各种折腾以后找到了解决办法：使用[CorFlags](https://msdn.microsoft.com/en-us/library/ms164699.aspx)工具改变可执行文件的头信息，使其以32位启动。

<code>corflags.exe /32bit+ myservice.exe</code>  （只需执行一次，以后都会以32位运行）

CorFlags会随Visual Studio自动安装，位于 C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\CorFlags.exe（根据VS版本会有不同）

参考：[http://stackoverflow.com/questions/1079066/forcing-a-net-windows-service-to-run-as-32-bit-on-a-64-bit-machine](http://stackoverflow.com/questions/1079066/forcing-a-net-windows-service-to-run-as-32-bit-on-a-64-bit-machine)