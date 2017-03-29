---
layout: post
title:  ".Net Core中使用md5"
date:   2017-03-29 10:00:00
categories: [dotnet, dotnet core]
tags: [dotnet, md5]
description: 
---

<!--more-->

.NET Core中使用```System.Security.Cryptography.MD5```类，找不到请添加NuGet：```System.Security.Cryptography.Algorithms```

```csharp
public static string GetMD5(string str)
{
    var md5 = System.Security.Cryptography.MD5.Create();
    byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

    StringBuilder sBuilder = new StringBuilder();

    for (int i = 0; i < data.Length; i++)
    {
        sBuilder.Append(data[i].ToString("x2"));
    }

    return sBuilder.ToString();
}
```
