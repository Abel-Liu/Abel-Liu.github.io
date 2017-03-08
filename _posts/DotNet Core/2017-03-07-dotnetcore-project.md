---
layout: post
title:  ".Net Core项目输出设置"
date:   2017-03-07 16:00:00
categories: [dotnet, dotnet core]
tags: [dotnet, core, project, output]
description: 
---
<!--more-->

**升级到VS2017后项目配置改回了csproj，用不到此配置了。。**

* 在.Net Core编译时拷贝文件到输出目录。

编辑project.json，使用buildOptions选项。

```json
"buildOptions": {
    "copyToOutput": [ "Rule/", "CollectorConfig.json", "log4net.config", "run.bat" ]
  }
```

copyToOutput中的内容会复制到输出目录，即bin下的相关目录。```Rule/```表示目录，其他为文件。

* 发布项目时拷贝文件

同样编辑project.json，使用publishOptions选项。

``` json
"publishOptions": {
    "include": [ "Rule/", "CollectorConfig.json", "log4net.config", "run.bat" ]
  }
```

* 改变项目文件名

项目编译出的程序集名默认是和项目名字一样的，想修改生成的文件名使用buildOptions下的outputName选项。

```json
"buildOptions": {
    "outputName": "your name"
  }
```

这时编译出的文件名为 your name.dll 。


完整配置：

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/project.json.jpg" border="0" alt="project.json"/>
</div>

参考：[https://docs.microsoft.com/en-us/dotnet/articles/core/tools/project-json](https://docs.microsoft.com/en-us/dotnet/articles/core/tools/project-json)