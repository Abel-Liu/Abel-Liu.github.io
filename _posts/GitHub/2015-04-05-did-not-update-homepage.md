---
layout: post
title:  "GitHub Pages不更新首页"
date:   2015-04-05 17:00:00
categories: GitHub
tags: [GitHub, hompage]
description: 奇葩BUG无处不在
---

&emsp;&emsp;最近更新博客，发现首页始终不更新，而其他页面都正常，各种改各种测试，感觉可能是GitHub的问题，遂联系GitHub的工作人员，答复真的很快！  

<!--more-->

&emsp;&emsp;一番沟通之后发现了问题所在，博客编译后首页是index.html，而我前几天fork了一个项目名字就叫index.html，所以编译受到影响，后来把这个项目删掉再编译就OK了。所以有些BUG真心是想不到的。

&emsp;&emsp;特别感谢GitHub的工作人员Jamie帮我解决了问题，还主动帮我域名加了A记录，大赞。
