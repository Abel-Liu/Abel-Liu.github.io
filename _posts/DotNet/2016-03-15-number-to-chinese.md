---
layout: post
title:  "阿拉伯数字转为汉语"
date:   2016-03-15 14:00:00
categories: dotnet
tags:
description: 
---

<!--more-->

目前只支持到万。

例：54321 --> 五万四千三百二十一 

```csharp
/// <summary>
/// 阿拉伯数字转为汉语
/// </summary>
public static string NumberToChinese( this int number )
{
    string[] unit = new string[] { "", "十", "百", "千", "万" };
    string[] text = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

    string result = string.Empty;
    for ( int i = number.ToString().Length; i > 0; i-- )
    {
        var n = ( int )( number / Math.Pow( 10, i - 1 ) );

        n %= 10;

        result += text[n] + ( n == 0 ? string.Empty : unit[i - 1] );
        result = result.Replace( "零零", "零" );
    }

    result = result.Trim( '零' );

    if ( number > 9 && number < 20 )
        result = result.Substring( 1 );

    return result;
}
```