---
layout: post
title:  "Jekyll自动生成Sitemap"
date:   2015-04-10 12:00:00
categories: github
tags: [Jekyll, sitemap, ruby, gem, GitHub]
description: 介绍如何让Jekyll自动生成Sitemap及Could not find a valid gem处理
---

通过配置可让Jekyll编译的时候自动生成站点的Sitemap，在_config.yml中添加如下信息即可：
{% highlight console %}
gems:
  - jekyll-sitemap
{% endhighlight %}

<!--more-->

本地编译要安装jekyll-sitemap组件<code>gem install jekyll-sitemap</code>，但安装中提示错误 Could not find a valid gem
{% highlight console %}
C:\Users\Abel-work>gem install jekyll-sitemap
ERROR:  Could not find a valid gem 'jekyll-sitemap' (>= 0), here is why:
          Unable to download data from https://rubygems.org/ - SSL_connect retur
ned=1 errno=0 state=SSLv3 read server certificate B: certificate verify failed (
https://rubygems.org/latest_specs.4.8.gz)
{% endhighlight %}

查到好多同样问题的，我重新设置源解决了问题。添加和移除源的命令如下：
{% highlight console %}
gem source --remove https://rubygems.org
gem source -a https://rubygems.org
{% endhighlight %}

若问题仍然存在，试试将https源替换为http的 <code>gem source -a http://rubygems.org</code>