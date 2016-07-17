---
layout: post
title:  "Web API 通过FilterProvider筛选/排除特定Filter"
date:   2016-07-17 7:00:00
categories: dotnet
tags: [webapi, filterprovider, actionfilter]
description: 
---

我们通过在Controller上加Filter实现控制器下所有操作都应用过滤器，但其中个别操作不想应用过滤器怎么办？上篇我们通过定义一个空过滤器，在执行的时候进行判断，但每排除一个就需要写一个空过滤器，这样判断太麻烦太臃肿，而且容易出错。现在提供一个一劳永逸又方便的解决方案，IFilterProvider。

<!--more-->

IFilterProvider起到控制所有过滤器的作用，如下

<div style="text-align: center;">
    <img src="/r/webapifilterprovider.jpg" border="0" alt="webapifilterprovider">
</div>

* 假设我们有一个授权验证过滤器

```csharp
    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
    public class VerifyTokenAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting( HttpActionContext actionContext )
        {
            //do your verify

            base.OnActionExecuting( actionContext );
        }
    }
```

* 我们写一个特殊的过滤器，实现排除其他过滤器的功能（别忘了在App_Start/WebApiConfig.cs中添加）

```csharp
    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
    public class ExcludeFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private Type filterType;

        public ExcludeFilterAttribute( Type filterType )
        {
            this.filterType = filterType;
        }

        public Type FilterType
        {
            get
            {
                return this.filterType;
            }
        }
    }
```

* 下边整个Controller都会应用VerifyTokenAttribute，在我们想排除的操作上加Exclude过滤器

```csharp
    [VerifyToken]
    public class MsgController : ApiController
    {
        [HttpPost]
        public MsgGetListResult GetMsgList( BaseRequest model )
        {
            //会走VerifyTokenAttribute过滤器
        }

        [ExcludeFilter( typeof( VerifyTokenAttribute ) )]
        [HttpPost]
        public MsgGetListResult notverify( string a )
        {
            //排除VerifyTokenAttribute过滤器
        }

    }
```

* 要想实现这种效果，我们实现一个自己的IFilterProvider，接口有一个GetFilters方法，返回自己需要应用的过滤器集合

```csharp
    public class ExcludeFilterProvider : IFilterProvider
    {
        public IEnumerable<FilterInfo> GetFilters( HttpConfiguration configuration, HttpActionDescriptor actionDescriptor )
        {
            //应用在Action上的过滤器
            var actionFilters = actionDescriptor.GetFilters();
            //应用在Controller上的过滤器
            var controllerFilters = actionDescriptor.ControllerDescriptor.GetFilters();

            var allFilters = actionFilters.Concat( controllerFilters );

            IEnumerable<ExcludeFilterAttribute> excludeFilters
                = ( from f in allFilters
                    where f is ExcludeFilterAttribute
                    select f as ExcludeFilterAttribute );

            List<Type> filterTypesToRemove = new List<Type>();
            foreach ( ExcludeFilterAttribute excludeFilter in excludeFilters )
            {
                filterTypesToRemove.Add( excludeFilter.FilterType );
            }

            IEnumerable<FilterInfo> result
                = ( from f in allFilters
                    where !filterTypesToRemove.Contains( f.GetType() )
                    select new FilterInfo( f, FilterScope.Action ) );

            return result;
        }
    }
```

* 最后我们需要在App_Start/WebApiConfig.cs替换默认的FilterProvider

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

            #region Add Filters

            config.Filters.Add( new VerifyTokenAttribute() );
            config.Filters.Add( new ExcludeFilterAttribute( null ) );

            #endregion


            #region Set FilterProvider

            var providers = config.Services.GetFilterProviders();

            var defaultprovider = providers.Single( i => i is ActionDescriptorFilterProvider );
            config.Services.Remove( typeof( IFilterProvider ), defaultprovider );

            var configprovider = providers.Single( i => i is ConfigurationFilterProvider );
            config.Services.Remove( typeof( IFilterProvider ), configprovider );

            config.Services.Add( typeof( IFilterProvider ), new ExcludeFilterProvider() );

            #endregion

        }
    }
```

以后想排除过滤器，只需加上**[ExcludeFilter( typeof( 要排除的过滤器 ) )]**

