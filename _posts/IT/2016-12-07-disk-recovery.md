---
layout: post
title:  "找回U盘丢失空间"
date:   2016-12-07 20:50:00
categories: it
tags: [disk, recovery]
description: 
---

<!--more-->

因为向U盘烧录烧录了Windows 10 IoT系统，结果U盘变成了只有几十M大小，格式化也不会恢复，磁盘管理中看到有几个零散分区和未分配的空间，使用Windows自带的DISKPART工具来修复。

CMD中输入diskpart进入工具。

1. 使用list disk查看现有磁盘
2. select disk 2 选中需要修复的U盘，注意看清序号
3. 再使用list disk确认没有选错，当前选择的磁盘前面有一个星号
4. 使用clean清除整个磁盘

磁盘管理中可以看到空间恢复了，自己建立分区即可。

如下：

<div style="text-align: center;">
<img class="onerow-imgfix" src="/r/diskpart.png" border="0" alt="diskpart"/>
</div>

