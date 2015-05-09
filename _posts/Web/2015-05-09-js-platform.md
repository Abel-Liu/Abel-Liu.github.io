---
layout: post
title:  "JS判断设备平台"
date:   2015-05-09 21:00:00
categories: Web
tags: [js]
description: 
---

通过JS判断当前访问网站的设备信息，是否是IOS或Android。

<!--more-->

{% highlight js %}
$(document).ready(function () {
    var g_sUA = navigator.userAgent.toLowerCase();
    var android = g_sUA.match(/(android)\s+([\d.]+)/);
    var ios = g_sUA.match(/(ipad|iphone|ipod).*os\s([\d_]+)/);
    if (android || ios) {
        alert("mobile");
    }
});
{% endhighlight %}
