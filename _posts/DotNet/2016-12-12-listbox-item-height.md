---
layout: post
title:  "修改WinForm ListBox的项目高度"
date:   2016-12-12 15:30:00
categories: [dotnet]
tags: [winform, listbox, item, height]
description: 
---

<!--more-->

WinForm中默认的ListBox控件项目高度比较低，如果需要增加高度可使用以下办法。

先看两张截图：

* 默认ListBox

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/listbox1.jpg" border="0" alt="listbox"/>
</div>

* 增加高度后

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/listbox2.jpg" border="0" alt="listbox"/>
</div>

方法如下：

* 修改ListBox控件的DrawMode属性为OwnerDrawFixed
* 修改ListBox控件的ItemHeight属性为你需要的高度
* 添加ListBox的DrawItem事件

```csharp
    private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        e.DrawFocusRectangle();
        var font = new Font(e.Font.Name, 12);

        e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), font, new SolidBrush(Color.Black), e.Bounds);
    }
```

