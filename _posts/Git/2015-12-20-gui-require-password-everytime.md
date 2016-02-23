---
layout: post
title:  "解决Windows下Git GUI每次同步需要输入密码"
categories: [git]
date:   2015-12-20 16:30:00
tags: [Git GUI]
description: 
---

Windows下使用GUI每次PULL和PUSH都需要输入用户名和密码，不胜其烦，下面介绍一种办法保存授权信息，以后就不用输入了。

<!--more-->

* 添加一个HOME环境变量，值为%USERPROFILE%
* 在“运行”中输入%Home%打开此目录，新建一个名为“_netrc”的文件。
* 编辑_netrc文件，输入Git服务器名、用户名、密码。如：

    > machine yourgitserver.com
    >
    > login username
    >
    > password yourpassword


以后再同步此服务器上的仓库就会使用配置好的用户信息。