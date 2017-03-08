---
layout: post
title:  "SQL Server分割字符串"
date:   2015-04-25 08:20:00
categories: sqlserver
tags: [sqlserver, split]
description:
---

使用自定义函数分割字符串，返回结果为一个表。

使用方法

{% highlight mysql %}
SELECT * FROM [fn_split_str]('aa,bb,cc,,dd', ',', 1)
{% endhighlight %}

<!--more-->

返回结果

<div style="text-align: center;">
<img src="http://i1373.photobucket.com/albums/ag384/abel_liu/GitHub/splitstr_zpsadxizmql.jpg" border="0" alt="splitstr"/>
</div>

完整代码

```sql
ALTER FUNCTION [dbo].[fn_split_str]
(
    @Input NVARCHAR(MAX),            --要分割的文本
    @Separator NVARCHAR(MAX) = ',',  --分隔符
    @RemoveEmptyEntries BIT = 1      --是否移除空字符串
)
RETURNS @Result TABLE 
(
    [Id] INT IDENTITY(1,1),
    [Value] NVARCHAR(MAX)
) 
AS
BEGIN 
    DECLARE @Index INT, @Entry NVARCHAR(MAX)
    SET @Index = CHARINDEX(@Separator,@Input)

    WHILE (@Index > 0)
    BEGIN
        SET @Entry = LTRIM(RTRIM(SUBSTRING(@Input, 1, @Index-1)))
        
        IF (@RemoveEmptyEntries = 0) OR (@RemoveEmptyEntries = 1 AND @Entry <> '')
            BEGIN
                INSERT INTO @Result([Value]) VALUES(@Entry)
            END

        SET @Input = SUBSTRING(@Input, @Index + DATALENGTH(@Separator) / 2, LEN(@Input))
        SET @Index = CHARINDEX(@Separator, @Input)
    END

    SET @Entry = LTRIM(RTRIM(@Input))
    IF (@RemoveEmptyEntries = 0) OR (@RemoveEmptyEntries = 1 and @Entry <> '')
        BEGIN
            INSERT INTO @Result([Value]) VALUES(@Entry)
        END

    RETURN
END
```
