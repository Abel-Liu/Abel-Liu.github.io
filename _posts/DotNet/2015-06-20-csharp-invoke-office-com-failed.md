---
layout: post
title:  "C#调用Office的COM组件失败的解决方法"
date:   2015-06-20 12:00:00
categories: DotNet
tags: [csharp, office, com]
description: 
---
用C#调用Office COM组件时经常会遇到如下错误。

<i>检索COM类工厂中CLSID为{000209FF-0000-0000-C000-000000000046}的组件时失败，原因是出现以下错误：80070005</i> <!--more-->

<div style="text-align: center;">
<img style="width:100%;" src="/r/office-com.jpg" border="0" alt="office"/>
</div>

这个问题是因为没有调用COM组件的权限，我开发环境下（Win7 Ultimate x64和Office 2010 x86）并没有出现这个问题，而部署到正式环境（Windows Server 2008 R2和Office 2010 x64）出现了这个问题。

网上一般解决这个问题的方法有两种：

1. 到组件服务里设置DCOM配置，找到Microsoft Word Application，在属性里设置标识。但我两个环境都没找到。
2. 在web.config中添加配置<code>&lt;identity impersonate="true" userName="Administrator" password="xxx"/&gt;</code>，userName和password为你计算机的用户名和密码。

根据第2个方法添加配置后程序可以运行了，但是出现了新的问题，读取不到Office文档，

{% highlight csharp %}
Application app = new Application();
Document doc = app.Documents.Open(ref path);
{% endhighlight %}

如上，doc始终为null，费力Google以后终于找到了问题。

Windows Server 2008官方不支持Office互操作，Office互操作依赖于“Desktop”目录来进行打开/保存文档，由于2008目录结构不同，systemprofile下缺少这个目录，所以在<code>C:\Windows\System32\config\systemprofile</code>下建立名为“Desktop”的目录即可

参考：

* <http://stackoverflow.com/questions/14937392/object-of-interop-word-document-class-is-null-on-windows-server-2008-word-open>
* <http://stackoverflow.com/questions/4438121/is-office-2003-interop-supported-on-windows-server-2008>