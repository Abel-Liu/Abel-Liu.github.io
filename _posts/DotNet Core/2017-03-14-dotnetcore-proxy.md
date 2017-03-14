---
layout: post
title:  ".Net Core中使用Proxy请求数据"
date:   2017-03-14 16:00:00
categories: [dotnet, dotnet core]
tags: [dotnet, csharp, proxy, httpclient]
description: 
---

本文讲解在.NET Core中使用HttpClient请求网址，以及代理的使用。

<!--more-->

之前在.NET Framework中一直用HttpWebRequest来请求数据，.NET Core中没有这些了，转而使用HttpClient。HttpClient在System.Net.Http命名空间中，.NET Core中需要安装 ```System.Net.Http``` NuGet包。

用HttpClient请求一个地址很简单：

```csharp
var client = new HttpClient();
var html = client.GetStringAsync("http://baidu.com").Result;
```

如果POST Json数据：

```csharp
var client = new HttpClient();
var content = new StringContent("{ \"data\" : \"some data\" }", Encoding.UTF8, "application/json");

var msg = client.PostAsync("http://someurl", content).Result;

var result = msg.Content.ReadAsStringAsync().Result;
```

还有POST Form数据：

```csharp
List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>();

formData.Add(new KeyValuePair<string, string>("key1", "value1"));
formData.Add(new KeyValuePair<string, string>("key1", "value1"));

var formContent = new FormUrlEncodedContent(formData.ToArray());

var client = new HttpClient();
var msg = client.PostAsync("http://someurl", formContent).Result;

var result = msg.Content.ReadAsStringAsync().Result;
```

以上代码在Framework和Core中都是一样的。下面来看看使用代理的情况。

在**.NET Framework**中使用代理：

```csharp
var proxy = new WebProxy("http://1.2.3.4:port");

HttpClientHandler httpClientHandler = new HttpClientHandler()
{
    Proxy = proxy
    //其他属性使用默认值即可
};

var client = new HttpClient(httpClientHandler);

var html = client.GetStringAsync("http://baidu.com").Result;
```

WebProxy是在System.Net命名空间下，但是.NET Core中没有这个类，所以需要自己写一个，只要实现```System.Net.IWebProxy```接口即可。

```csharp
public class CoreWebProxy : IWebProxy
{
    public readonly Uri Uri;
    private readonly bool bypass;

    public CoreWebProxy(Uri uri, ICredentials credentials = null, bool bypass = false)
    {
        Uri = uri;
        this.bypass = bypass;
        Credentials = credentials;
    }

    public ICredentials Credentials { get; set; }

    public Uri GetProxy(Uri destination) => Uri;

    public bool IsBypassed(Uri host) => bypass;

    public override int GetHashCode()
    {
        if (Uri == null)
        {
            return -1;
        }

        return Uri.GetHashCode();
    }
}
```

使用方式一样：

```csharp
var proxy = new CoreWebProxy(new Uri("http://1.2.3.4:port"));
//...
```

That's it!

随便说一下不同代理的区别，代理一共分为三级

* 透明代理(Transparent) —— level 3
 
&emsp;&emsp;不会隐藏你的IP，转发的请求头中包含你的IP地址

* 匿名代理(Anonymous 也称一般代理) —— Level 2

&emsp;&emsp;不会透露你的IP，但是所请求的服务知道是经过代理的，有些服务商通过高级手段可以追踪到你的真实IP

* 高匿代理(Elite)  —— Level 1

&emsp;&emsp;最高级别的代理，被调用的服务完全不知道请求是否经过了代理，当然也看不到你的IP地址