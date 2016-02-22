---
layout: post
title:  "SQL Server发送邮件（带表格）"
date:   2016-02-22 21:00:00
categories: sqlserver
tags: [sqlserver, mail, HTML, table]
description:
---

编写SQL语句发送HTML格式邮件（表格的用法）。

<!--more-->

先在“管理 -- 数据库邮件”中添加发送邮件的配置，记下配置名称。

使用以下SQL语句发送邮件：

{% highlight mysql %}
declare @mailBody nvarchar(max), @mailSubject nvarchar(max)

set @mailSubject = N'Test Mail'
     
set @mailBody = '<html><head>' +
    '<style>' +
    'body, th, td { font-size: 12px; font-family: Microsoft YaHei, tahoma, arial, "Hiragino Sans GB", 宋体, sans-serif; white-space: nowrap; }' +
    'h3 { font-size: 14px; font-weight: bold; margin-bottom: 0px; }' +
    'table { border-collapse: collapse; border: solid 2px gray; }' +
    'th { padding: 3px; font-weight: bold; background-color: silver; }' +
    'td { padding: 3px; }' +
    '</style>' +
    '</head><body>'

set @mailBody = @mailBody + N'<h3>Data List</h3>'
set @mailBody = @mailBody + 
    N'<table border="1" bordercolor="gray"><tr>' +
    N'<th>列名1</th><th>列名2</th>' +
    N'</tr>'
set @mailBody = @mailBody + isnull(cast (
(
    SELECT  field1 as td, '', field2 as td, '' FROM tb
    for xml path('tr'), elements
) as nvarchar(max)), '')
set @mailBody = @mailBody + '</table>'

set @mailBody = @mailBody + '</body></html>'

-- 发送邮件
exec msdb.dbo.sp_send_dbmail @profile_name = 'YourProfile', @recipients  = 'someone@abc.com', @subject = @mailSubject, @body = @mailBody, @body_format = 'HTML'

{% endhighlight %}


