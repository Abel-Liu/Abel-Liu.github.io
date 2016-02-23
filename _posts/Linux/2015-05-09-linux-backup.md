---
layout: post
title:  "Linux备份目录"
date:   2015-05-09 19:00:00
categories: linux
tags: [Linux, backup]
description: 
---
维护服务器过程中备份是必不可少的任务，本文介绍Linux下定期备份目录的办法。<!--more-->

编写脚本将指定目录打成tar包放在备份目录，并记录相关日志，并且实现清理一段时间之前的备份文件。再结合cron服务实现定期执行备份（cron服务可定期执行一些任务，详细用法见[这篇文章](http://www.abelliu.com/linux/2015/04/22/crontab/)）。

先上代码。

{% highlight bash %}
#!/bin/bash
#Auto backup

############## Config ##################
DATE=`date +"%Y-%m-%d"`
SourceDir=/home/abeltest/somefiles  #the dir need backup
BakDir=/home/abeltest/backup
LogFile=/home/abeltest/backup/log/$DATE.log
RetainDay=7  #delete backup xx days ago
############## Begin ##################
mkdir -p $BakDir/
mkdir -p `dirname $LogFile`

echo "-------------" >> $LogFile
echo "$(date +"%Y-%m-%d %H:%M:%S") backup start" >> $LogFile

SrcName=`basename $SourceDir`
PackFile=$SrcName-$DATE.tar.gz

cd `dirname $SourceDir`

tar -zcvf $BakDir/$PackFile $SrcName
echo "$(date +"%Y-%m-%d %H:%M:%S") $SourceDir backup to $BakDir/$PackFile" >> $LogFile

OldFile=$BakDir/$SrcName-$(date --date="$RetainDay days ago" +"%Y-%m-%d").tar.gz

if [ -f $OldFile ]
	then
  	rm -rf $OldFile > /dev/null
  	echo "$(date +"%Y-%m-%d %H:%M:%S") $OldFile was deleted" >> $LogFile
fi

echo "$(date +"%Y-%m-%d %H:%M:%S") backup completed" >> $LogFile
exit 0
{% endhighlight %}

以下几个变量需要配置

* DATE --------- 当前日期，不需要修改
* SourceDir --- 需要备份的目录，绝对路径
* BakDir -------- 存放备份的目录
* LogFile ------- Log文件，配置存放路径，最后的<code>$DATE.log</code>是日志文件名，表示以当前日期命名
* RetainDay --- 过期天数，脚本会删除备份日期早于此天数的备份文件

