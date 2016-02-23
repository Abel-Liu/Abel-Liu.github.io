---
layout: post
title:  "响应式网页设计"
date:   2015-04-19 9:00:00
categories: web
tags: [responsive]
description: 根据浏览器大小自动调整显示效果
---

&emsp;&emsp;响应式网页设计可以根据不同的环境（窗口大小、PC or Mobile）进行相应的布局调整，本文举一个简单的例子说明一种简单的实现方法。

<!--more-->

&emsp;&emsp;如图所示，正常情况网页包含左右两个部分，左边定义为网页的主要区域，右边放一些导航之类的，可根据浏览器大小同比例缩放。

<div style="text-align: center;">
<img style="width:90%;" src="http://i1373.photobucket.com/albums/ag384/abel_liu/GitHub/responsive1_zpsdfhmkkzj.jpg" border="0" alt="responsive1"/>
</div>

&emsp;&emsp;但当窗口过小时，内容显示就会太紧凑，这时将右边部分移动到主区域的下方垂直排列，以获取更好的显示效果，这也是响应式设计的初衷。如下图所示。

<div style="text-align: center;">
<img style="width:90%; height:200px;" src="http://i1373.photobucket.com/albums/ag384/abel_liu/GitHub/responsive2_zpsbmgd0gnr.jpg" border="0" alt="responsive2"/>
</div>

&emsp;&emsp;实现此效果推荐用@Media Query方法，本博客也是采用此方式，主要代码如下。

* CSS代码

{% highlight css %}
.wrap:before,
.wrap:after {
    content: "";
    display: table;
    clear: both;
}

.content-split .column {
    float: left;
}

.content-col-1 {
    width: 800px;
    width: -webkit-calc(77% - 20px);
    width: -moz-calc(77% - 20px);
    width: -o-calc(77% - 20px);
    width: calc(77% - 20px);
    margin-right: 20px;
    background-color: green;
}

.content-col-2 {
    width: 200px;
    width: -webkit-calc(23%);
    width: -moz-calc(23%);
    width: -o-calc(23%);
    width: calc(23%);
    background-color: red;
}

@media screen and (max-width: 750px) {
    .content-split .column {
        float: none;
        clear: both;
        width: auto;
        margin-top: 10px;
        margin-right:0px;
    }
}
{% endhighlight %}

* Html代码

{% highlight html %}
<div class="content-split">
    <div class="wrap">
        <div class="content-col-1 column">
            <div style="height:150px;">
                left
            </div>
        </div>
        <div class="content-col-2 column">
            <div style="height:100px;">
                right
            </div>
        </div>
    </div>
</div>
{% endhighlight %}

* wrap的核心是<code>display: table;</code>，表示子元素的排列方式是table类型，会从左到右依次排列。还需要有<code>float: left;</code>
* <code>content-col</code>定义了两列不同比例的宽度。
* <code>@media screen and (max-width: 750px)</code>意思是当浏览器宽度小于750px时，改变指定的样式。清除浮动，并将宽度设为自动。

[查看demo](http://abel-liu.github.io/res/projects/sample-page/responsive.html)

[查看完整代码](https://github.com/Abel-Liu/Abel-Liu.github.io/blob/master/r/Projects/responsive.html)

本人是初学者，不完善的地方还请斧正。
