---
layout: post
title:  "查询SQL Server数据库结构"
date:   2015-04-24 23:00:00
categories: sqlserver
tags: [sqlserver, 数据库结构]
description: 使用SQL语句获取数据库的结构信息
---

&emsp;&emsp;有时写文档想导出现有数据库的结构，又不想麻烦使用其他软件，现提供一个SQL语句可以查询出数据库中所有表和字段信息，包含字段类型、是否可空、默认值等，如下。

<div style="text-align: center;">
<img style="max-width:100%;" src="http://i1373.photobucket.com/albums/ag384/abel_liu/GitHub/dbstruc_zpsdhzbewsv.jpg" border="0" alt="dbstru"/>
</div>

{% highlight mysql %}
SELECT
    (CASE WHEN C.column_id = 1 THEN O.name ELSE N'' END) as TableName,
    C.name as ColumnName,
    T.name as DataType,
    C.max_length as Length,
    (CASE WHEN C.is_nullable = 1 THEN N'√' ELSE N'' END) as Nullable,
    ISNULL(D.definition, N'') as DefaultValue,
    ISNULL(PFD.[value], N'') as ColumnDesc
FROM sys.columns C
	INNER JOIN sys.objects O
        ON C.[object_id] = O.[object_id]
            AND O.type = 'U'
            AND O.is_ms_shipped = 0
    INNER JOIN sys.types T
        ON C.user_type_id = T.user_type_id
    LEFT JOIN sys.default_constraints D
        ON C.[object_id] = D.parent_object_id
            AND C.column_id=D.parent_column_id
            AND C.default_object_id=D.[object_id]
    LEFT JOIN sys.extended_properties PFD
        ON PFD.class = 1 
            AND C.[object_id] = PFD.major_id 
            AND C.column_id = PFD.minor_id
ORDER BY O.name, C.column_id
{% endhighlight %}
