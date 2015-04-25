---
layout: post
title:  "JS生成当前页面二维码"
date:   2015-04-11 15:00:00
categories: Web
tags: [qrcode, 二维码, jquery]
description: 使用JS生成当前页面二维码
---

最近研究了怎么显示当前页面的二维码，使用JQuery的插件可以很简单地完成。

* 首先引用JQuery和生成二维码的插件

{% highlight html %}
<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
<script src="http://cdn.bootcss.com/jquery.qrcode/1.0/jquery.qrcode.min.js"></script>
{% endhighlight %}

* 页面中添加一个div作为生成二维码的容器

{% highlight html %}
<div id="qrcode"></div>
{% endhighlight %}

* 使用以下JS生成

{% highlight js %}
$("#qrcode").qrcode({
   text: window.location.href, //设置字符串为当前url
   width: 150,
   height: 150
});
{% endhighlight %}

[查看demo](http://staticres.github.io/Projects/qrcode.html)

[查看完整代码](https://github.com/staticres/StaticRes.github.io/blob/master/Projects/qrcode.html)