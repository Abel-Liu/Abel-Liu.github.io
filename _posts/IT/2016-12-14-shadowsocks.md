---
layout: post
title:  "ShadowSocks使用教程"
date:   2016-12-14 11:00:00
categories: it
tags: [shadowsocks, SwitchyOmega]
description: 
---

<!--more-->

### 下载客户端

Windows版[https://github.com/shadowsocks/shadowsocks-windows/releases](https://github.com/shadowsocks/shadowsocks-windows/releases)

Mac版[https://github.com/shadowsocks/shadowsocks-iOS/releases](https://github.com/shadowsocks/shadowsocks-iOS/releases)

以下教程以Windows为例。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss1.jpg" border="0" alt=""/>
</div>

### 配置

填写服务器地址、端口、密码和加密方式。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss2.jpg" border="0" alt=""/>
</div>

开启“启用系统代理”后电脑上的所有网络连接都会走代理，现在已经可以fq了。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss3.jpg" border="0" alt=""/>
</div>

启用系统代理会增加流量使用，而且也不方便，最方便的是使用Chrome配合SwitchyOmega扩展实现自动分流、最大限度的自定义。

### 使用SwitchyOmega

从谷歌商店安装（需fq）[https://chrome.google.com/webstore/detail/proxy-switchyomega/padekgcemlokbadohgkifijomclgjgif](https://chrome.google.com/webstore/detail/proxy-switchyomega/padekgcemlokbadohgkifijomclgjgif)

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss4.jpg" border="0" alt=""/>
</div>

进入SwitchyOmega选项。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss5.jpg" border="0" alt=""/>
</div>

新建情景模式。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss6.jpg" border="0" alt=""/>
</div>

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss7.jpg" border="0" alt=""/>
</div>

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss8.jpg" border="0" alt=""/>
</div>

代理端口填ShadowSocks配置时的端口。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss9.jpg" border="0" alt=""/>
</div>

取消ShadowSocks的系统代理。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss10.jpg" border="0" alt=""/>
</div>

选择刚才创建的情景模式。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss11.jpg" border="0" alt=""/>
</div>

这时候打开Chrome网页就会走代理，如果不想走代理的时候，例如国内网站，就切换为“直接连接”。

手动来回切换太麻烦，SwitchyOmega提供了一种自动切换模式，可以根据访问的网页地址自动选择使用哪种模式。

我们再新建一种自动切换模式。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss12.jpg" border="0" alt=""/>
</div>

点击添加规则列表。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss13.jpg" border="0" alt=""/>
</div>

注意情景模式选我们创建的ShadowSocks模式。
列表网址填：http://guaizi0129-001-site1.itempurl.com/f/gfw.txt

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss14.jpg" border="0" alt=""/>
</div>

点击立即更新情景模式，保存选项。

然后Chrome中选择我们创建的自动模式。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss15.jpg" border="0" alt=""/>
</div>

现在打开网页可以自动进行区分。

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/ss/ss16.jpg" border="0" alt=""/>
</div>