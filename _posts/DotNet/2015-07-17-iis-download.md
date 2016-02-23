---
layout: post
title:  "配置IIS指定类型文件可下载"
date:   2015-07-17 17:00:00
categories: dotnet
tags: [iis]
description: 
---
通过配置IIS的MIME类型，指定某种类型文件的处理方式。
<!--more-->

在IIS中选中一个网站，打开“MIME 类型”功能，添加/修改要处理的文件扩展名，在打开窗口中填写扩展名（例：txt）和MIME类型（application/octet-stream）保存即可。

使用浏览器或下载软件即可下载诸如http://xx.com/a.txt。