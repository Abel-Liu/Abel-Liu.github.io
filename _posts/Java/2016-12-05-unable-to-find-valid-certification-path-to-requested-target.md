---
layout: post
title:  "Java 异常 unable to find valid certification path to requested target"
date:   2016-12-05 16:00:00
categories: java
tags: [java, cert, https, ssl, store, trust, PKIX]
description: 
---
在做SiteWhere转发到Azure EventHub 时遇到Java的问题，报错：

```
javax.net.ssl.SSLException: Connection has been shutdown: javax.net.ssl.SSLHandshakeException: sun.security.validator.ValidatorException: PKIX path building failed: sun.security.provider.certpath.SunCertPathBuilderException: unable to find valid certification path to requested target
```

<!--more-->

这是证书方面的错误，找不到所请求的地址的有效证书。网上的各种方案核心内容差不多，最后的细节不一样，我试验有效的解决方案如下。

使用InstallCert.java来下载和保存证书，源码：[https://github.com/escline/InstallCert](https://github.com/escline/InstallCert)

步骤说明：

* 下载InstallCert.java并编译

```
javac InstallCert.java
```

* 运行InstallCert，参数是你调用的网址，如果是80端口可以省略port

```
java InstallCert [host]:[port]

例：java InstallCert abc.com:443
运行后会列出证书让你选择，输入1回车，最后会在当前目录生成一个jssecacerts文件。
```

* 导出证书文件

```
keytool -exportcert -alias [host]-1 -keystore jssecacerts -storepass changeit -file [host].cer

例：keytool -exportcert -alias abc.com-1 -keystore jssecacerts -storepass changeit -file abc.com.cer
```

*有些文章说将生成的jssecacerts文件拷贝到$JAVA_HOME\jre\lib\security目录，我测试的并不行，所以还需要进行下一步*

* 将证书文件导入系统keystore

```
keytool -importcert -alias [host] -keystore [path to system keystore] -storepass changeit -file [host].cer

例：keytool -importcert -alias abc.com -keystore "C:\Program Files\Java\jre1.8.0_111\lib\security\cacerts" -storepass changeit -file abc.com.cer
```

根据需要重新运行你的代码或系统。

参考：[https://www.mkyong.com/webservices/jax-ws/suncertpathbuilderexception-unable-to-find-valid-certification-path-to-requested-target](https://www.mkyong.com/webservices/jax-ws/suncertpathbuilderexception-unable-to-find-valid-certification-path-to-requested-target/)
