---
layout: post
title:  "Windows通用程序（UWP）中实现StackPanel滚动"
date:   2016-12-07 20:00:00
categories: [dotnet, uwp]
tags: [uwp, stackpanel, scroll, scrollviewer]
description: 
---

<!--more-->

在UWP程序中使用了StackPanel，想实现超出区域自动滚动，实验的有效方法是把StackPanel放在ScrollViewer中，Xaml代码如下。

```csharp
    <ScrollViewer VerticalScrollBarVisibility="Auto" VerticalScrollMode="Enabled" Padding="20" >
        <StackPanel VerticalAlignment="Top"  HorizontalAlignment="Left" >
            <TextBox Name="txtMqttHost" Header="MQTT host name" Text="" Width="400" ></TextBox>
            <TextBox Name="txtMqttPort" Header="MQTT port" Text="1883" ></TextBox>
            <TextBox Name="txtOutbound" Header="MQTT outbound sitewhere topic" Text="SiteWhere/input/json2" ></TextBox>
            <TextBox Name="txtHardware" Header="Hardware ID" Text="" ></TextBox>
            <TextBox Name="txtSpecToken" Header="Device specification token" Text="" ></TextBox>
            <TextBox Name="txtSiteToken" Header="Site token" Text="" ></TextBox>
            <TextBox Name="txtProcessor" Header="Processor" Text="" ></TextBox>
            <TextBox Name="txtCollector" Header="Collector measurement classnames" Text="" ></TextBox>
            <TextBlock Name="txtError" Foreground="Red" Text="" ></TextBlock>
        </StackPanel>
    </ScrollViewer>
```

