---
layout: post
title:  "Asp.Net Core中使用RSA加密"
date:   2017-09-09 20:00:00
categories: [dotnet, dotnet core]
tags: [dotnet, rsa, middleware, actionfilter]
description: 
---

本篇文章记录Asp.Net Core中使用RSA进行前后端加密，使用中间件和ActionFilter处理加密逻辑而不用大量修改代码。

<!--more-->

我们以Ajax用户登录为例。首先一个未加密的登录代码一般是这个样子：

```js
var data = {
    "user_name": $("#login-username").val(),
    "password": $("#login-password").val()
};

$.post("/User/Login", data, function (result) {
    //login result
});
```

```csharp
[HttpPost]
public JsonResult Login(UserLogin model)
{
    //login logic
}
```

我们先在后端生成公钥和私钥，将公钥传给前端，JS使用公钥加密数据后再传输，后端接收到数据后解密。

* 前后端RSA代码

添加Nuget包：System.Security.Cryptography.Algorithms、System.Security.Cryptography.Csp

```csharp
using System.Security.Cryptography;

public static class RSAUtil
{
    private static RSAParameters PrivateParameter;
    private static RSAParameters PublicParameters;
    public static (string PublicExponent, string PublicModulus) PublicPars;

    static RSAUtil()
    {
        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
        {
            PrivateParameter = RSA.ExportParameters(true);
            PublicParameters = RSA.ExportParameters(false);

            PublicPars = (BytesToHexString(PublicParameters.Exponent), 
                BytesToHexString(PublicParameters.Modulus));
        }
    }

    public static string Decrypt(string oldstr)
    {
        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
        {
            RSA.ImportParameters(PrivateParameter);
            byte[] bytes = RSA.Decrypt(HexStringToBytes(oldstr), false);
            System.Text.ASCIIEncoding enc = new ASCIIEncoding();

            return enc.GetString(bytes);
        }
    }

    public static byte[] Encrypt(byte[] DataToEncrypt, bool DoOAEPPadding = false)
    {
        try
        {
            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(PublicParameters);

                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            return encryptedData;
        }
        catch (CryptographicException e)
        {
            return null;
        }
    }

    private static string BytesToHexString(this byte[] input)
    {
        StringBuilder hexString = new StringBuilder(64);

        for (int i = 0; i < input.Length; i++)
        {
            hexString.Append(String.Format("{0:X2}", input[i]));
        }
        return hexString.ToString();
    }

    private static byte[] HexStringToBytes(this string hex)
    {
        if (hex.Length == 0)
        {
            return new byte[] { 0 };
        }

        if (hex.Length % 2 == 1)
        {
            hex = "0" + hex;
        }

        byte[] result = new byte[hex.Length / 2];

        for (int i = 0; i < hex.Length / 2; i++)
        {
            result[i] = byte.Parse(hex.Substring(2 * i, 2), 
                System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        return result;
    }
}
```

我们生成的公钥保存在```public static (string PublicExponent, string PublicModulus) PublicPars``` 前端需要用到```PublicExponent```和```PublicModulus```来进行加密。将这两个值保存在页面中，使JS可以取到。前端代码如下：

```js
<script src="~/lib/jquery/dist/jquery.js"></script>

<script src="~/js/rsa/Barrett.js"></script>
<script src="~/js/rsa/BigInt.js"></script>
<script src="~/js/rsa/RSA.js"></script>

<script>
    setMaxDigits(129);
    var _rk = new RSAKeyPair("@ViewBag.PublicExponent", "", "@ViewBag.PublicModulus");

    function login () {
        var data = {
            "user_name": $("#login-username").val(),
            "password": $("#login-password").val()
        };

        var _p = encryptedString(_rk, JSON.stringify(data));

        $.post("/User/Login", { "_p": _p }, function (result) {
            //login result
        });
    }
</script>
```

其中 ```Barrett.js```、```BigInt.js```、```RSA.js``` 三个是前端RSA类库，可从<a target="_blank" href="/r/rsa.zip">这里</a>下载。

我这里直接将整个Post的data进行加密，组成一个新的对象进行传输，这样一个好处是为了兼容后台代码，后台接口的参数是user_name和password，我们将解密操作放在中间件中，这样就不用一个个改后台接口。

* 使用中间件

新建一个类，代码如下：

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RSAMiddleware
{
    private readonly RequestDelegate _next;

    public RSAMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        if (context.Request.Method.ToUpper() == "POST")
        {
            var old = context.Request.Form["_p"];

            if (!string.IsNullOrWhiteSpace(old))
            {
                old = RSAUtil.Decrypt(old);

                Dictionary<string, StringValues> dictValues = new Dictionary<string, StringValues>();
                var obj = JsonConvert.DeserializeObject(old) as JObject;

                foreach (JProperty o in obj.AsJEnumerable())
                {
                    dictValues.Add(o.Name, new StringValues(o.Value.ToString()));
                }

                context.Request.Form = new FormCollection(dictValues);
            }
        }

        // Call the next delegate/middleware in the pipeline
        return this._next(context);
    }
}

public static class RSAMiddlewareExtensions
{
    public static IApplicationBuilder UseRSATransfer(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RSAMiddleware>();
    }
}

```

然后在Startup中启用中间件

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
    loggerFactory.AddDebug();

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseRSATransfer();

    app.UseStaticFiles();

    app.UseSession();

    app.UseMvc(routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
    });
}
```

这样当我们收到一个POST请求并且检测到参数为_p时，我们知道是RSA加密过的，解密后将数据对象加入POST参数中，然后继续请求后台接口，这样后台代码完全不用修改，而且不管前端是否加密都支持。

* 使用ActionFilter简化前端获取公钥

上面代码可以看到我们使用ViewBag保存公钥数据，当然不可能每个页面中都设置一次ViewBag的值，我们使用ActionFilter来自动给每个页面设置ViewBag。

新建一个类，代码如下：

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ProcessViewResultAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        //rsa public key
        var controller = (context.Controller as Controller);
        controller.ViewBag.PublicExponent = RSAUtil.PublicPars.PublicExponent;
        controller.ViewBag.PublicModulus = RSAUtil.PublicPars.PublicModulus;

        base.OnResultExecuting(context);
    }
}
```

在Startup中添加我们的Filter

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc(option => option.Filters.Add(new ProcessViewResultAttribute()))
        .AddJsonOptions(o => o.SerializerSettings.ContractResolver = new DefaultContractResolver());
}
```

现在每个Controller执行完返回时都会给ViewBag设置公钥的值。

* Linux中遇到的问题

使用 ```RSACryptoServiceProvider``` 在Linux中会报错not supported on this platform，Linux和MAC中不支持此方法，在跨平台时使用 ```RSA.Create()``` 来创建RSA对象，使用方法有一些变化：

```csharp
public static string Decrypt(string oldstr)
{
    using (var rsa = RSA.Create())
    {
        rsa.ImportParameters(PrivateParameter);
        byte[] bytes = rsa.Decrypt(HexStringToBytes(oldstr), RSAEncryptionPadding.Pkcs1);

        return new ASCIIEncoding().GetString(bytes);
    }
}
```

```rsa.Decrypt``` 方法需要提供 ```RSAEncryptionPadding``` 参数，这里测试使用 ```RSAEncryptionPadding.Pkcs1``` 是正常的。

但这时在Linux上生成的密钥在js中有问题，页面直接卡死，所以我这里先在Windows中生成密钥(RSAParameters)，然后序列化为字符串，使用时再反序列化为对象，如下。

```csharp
private static RSAParameters PrivateParameter;
private static RSAParameters PublicParameters;
public static (string PublicExponent, string PublicModulus) PublicPars;

static RSAUtil()
{
    //Windows下用此生成
    //using (var rsa = new RSACryptoServiceProvider())
    //{
    //    PrivateParameter = rsa.ExportParameters(true);
    //    PublicParameters = rsa.ExportParameters(false);
    //    PublicPars = (BytesToHexString(PublicParameters.Exponent), 
    //         BytesToHexString(PublicParameters.Modulus));
    //    var privateKey = JsonConvert.SerializeObject(PrivateParameter);
    //    var publicKey = JsonConvert.SerializeObject(PublicParameters);
    //}

    PrivateParameter = JsonConvert.DeserializeObject<RSAParameters>("your private key");
    PublicParameters = JsonConvert.DeserializeObject<RSAParameters>("your public key");

    PublicPars = ("public key.Exponent", "public key.Modulus");
}
```

**Update**

升级到NET Core 2以后RSAParameters序列化不完全，需要自己改一下序列化的方式，替换上边的 JsonConvert.SerializeObject 和 JsonConvert.DeserializeObject：

```chsarp
private static string Serialize(RSAParameters parameters)
{
    var parasJson = new RSAParametersJson()
    {
        Modulus = parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
        Exponent = parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
        P = parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
        Q = parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
        DP = parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
        DQ = parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
        InverseQ = parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
        D = parameters.D != null ? Convert.ToBase64String(parameters.D) : null
    };

    return JsonConvert.SerializeObject(parasJson);
}

private static RSAParameters Deserialize(string parametersJson)
{
    var paramsJson = JsonConvert.DeserializeObject<RSAParametersJson>(parametersJson);

    RSAParameters parameters = new RSAParameters();

    parameters.Modulus = paramsJson.Modulus != null ? Convert.FromBase64String(paramsJson.Modulus) : null;
    parameters.Exponent = paramsJson.Exponent != null ? Convert.FromBase64String(paramsJson.Exponent) : null;
    parameters.P = paramsJson.P != null ? Convert.FromBase64String(paramsJson.P) : null;
    parameters.Q = paramsJson.Q != null ? Convert.FromBase64String(paramsJson.Q) : null;
    parameters.DP = paramsJson.DP != null ? Convert.FromBase64String(paramsJson.DP) : null;
    parameters.DQ = paramsJson.DQ != null ? Convert.FromBase64String(paramsJson.DQ) : null;
    parameters.InverseQ = paramsJson.InverseQ != null ? Convert.FromBase64String(paramsJson.InverseQ) : null;
    parameters.D = paramsJson.D != null ? Convert.FromBase64String(paramsJson.D) : null;
    return parameters;
}

protected class RSAParametersJson
{
    public string Modulus { get; set; }
    public string Exponent { get; set; }
    public string P { get; set; }
    public string Q { get; set; }
    public string DP { get; set; }
    public string DQ { get; set; }
    public string InverseQ { get; set; }
    public string D { get; set; }
}
```

至此一切运行正常，Enjoy.

参考：[https://stackoverflow.com/questions/41986995/implement-rsa-in-net-core](https://stackoverflow.com/questions/41986995/implement-rsa-in-net-core)