---
layout: post
title:  "SQL Server日期操作技巧"
date:   2015-04-23 21:30:00
categories: sqlserver
tags: [sqlserver]
description: 
---


使用SQL Server时有时会用到复杂的日期计算，以下总结几个遇到的技巧。<!--more-->

##### 1 获取上个周日是哪天

        {% highlight mysql %}
        SELECT DATEADD(DAY, -(@@DATEFIRST + DATEPART(WEEKDAY, GETDATE()) - 1) % 7, GETDATE())
        {% endhighlight %}

##### 2 获取某天是星期几，因为SQL Server的区域设置不同，或手动更改了@@DATEFIRST值，获取到的星期是不一样的，所以用以下方式可以绝对获取到正确值

        {% highlight mysql %}
        SELECT (DATEPART(WEEKDAY, '2015-4-23') + @@DATEFIRST - 1) % 7
        {% endhighlight %}

##### 3 获取一段时间内的工作日数（包含开始日期和结束日期）

        {% highlight mysql %}
CREATE FUNCTION get_weekday_count
(
	@start_time VARCHAR(20),
	@end_time VARCHAR(20)
)
RETURNS int
AS
BEGIN
	DECLARE @result INT
	SET @result = 0
	WHILE DATEDIFF(DAY, @start_time, @end_time) >= 0
	BEGIN
	IF (DATEPART(WEEKDAY, @start_time) + @@DATEFIRST - 1) % 7 BETWEEN 1 AND 5
	BEGIN SET @result = @result + 1 END
	SET @start_time = DATEADD(DAY, 1, @start_time)
	END
	RETURN @result
END
        {% endhighlight %}