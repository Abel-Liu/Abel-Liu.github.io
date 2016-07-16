---
layout: post
title:  "Web API ActionFilterAttribute用法"
date:   2016-07-16 12:00:00
categories: dotnet
tags: [webapi, actionfilter]
description: 
---

<!--more-->

ActionFilterAttribute顾名思义是过滤API请求，可应用于Controller或Action，OnActionExecuting和OnActionExecuted两个方法可在操作执行之前和之后被调用，通过写自己的ActionFilter可实现API请求过滤、验证、记日志等。API请求的执行顺序：

<div style="text-align: center;">
    <img src="/r/webapirequest.jpg" border="0" alt="webapirequest">
</div>

* 首先定义一个过滤器：

```csharp
    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
    public class VerifyTokenAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting( HttpActionContext actionContext )
        {
            //do your verify

            base.OnActionExecuting( actionContext );
        }

        public override void OnActionExecuted( HttpActionExecutedContext actionExecutedContext )
        {
            //log.write("API executed")

            base.OnActionExecuted( actionExecutedContext );
        }
    }
```

（注意Web API的过滤器继承于System.Web.Http.Filters.ActionFilterAttribute，不要和MVC的Filter搞混）

* 然后在App_Start/WebApiConfig.cs中添加自己的过滤器，如下最后一行

```csharp
    public static class WebApiConfig
    {
        public static void Register( HttpConfiguration config )
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add( new VerifyTokenAttribute() );
        }
    }
```

* 给需要的Action或Controller添加自己定义的属性：

（过滤器加在Controller则内部所有Action都会应用过滤，加在一个Action上则只有这个操作生效。）

```csharp
    [VerifyToken]
    public class MsgController : ApiController
    {
        [HttpPost]
        public MsgGetListResult GetMsgList( BaseRequest model )
        {
            MsgGetListResult result = new MsgGetListResult()
            {
                Result = 1,
                ResultMessage = "",
            };

            return result;
        }
    }
```

* 如果个别的Action不想做验证怎么办？我们写一个空的Attribute，在调用的时候判断：

```csharp
    public class DoNotVerifyTokenAttribute : System.Attribute
    {
    }



    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
    public class VerifyTokenAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting( HttpActionContext actionContext )
        {
            //判断是否包含DoNotVerifyTokenAttribute
            if ( !actionContext.ActionDescriptor
                    .GetCustomAttributes<DoNotVerifyTokenAttribute>().Any() )
            {
                //verify
            }

            base.OnActionExecuting( actionContext );
        }
    }



    [VerifyToken]
    public class MsgController : ApiController
    {
        [HttpPost]
        public MsgGetListResult GetMsgList( BaseRequest model )
        {
           //会被验证
        }

        [DoNotVerifyToken]
        [HttpPost]
        public MsgGetListResult NotVerify( string str )
        {
            //不走验证
        }

    }
```
