---
layout: learnpython
title:  "Python学习笔记（一）"
date:   2016-03-16 14:00:00
categories: python
tags: [python, pylesson]
description: 
---

<!--more-->

* 基本命令

命令行输入python，按回车进入python环境

```
D:\>python
Python 3.5.1 (v3.5.1:37a07cee5969, Dec  6 2015, 01:54:25) [MSC v.1900 64 bit (AMD64)] on win32
Type "help", "copyright", "credits" or "license" for more information.
>>>
```

* print() 打印，使用<code>,</code>连接字符串

```
>>> print('hello world')
hello world
>>> print('hello', 'Jim', 'Green')
hello Jim Green
>>> print(100 + 200)
300
```


> python中单引号和双引号都可以表示字符串

* 使用 \ 进行转义，使用<code>r''</code>表示<code>''</code>内的字符串不转义

```
>>> print('hello \'world\'')
hello 'world'
>>> print(r'hello \'world\'')
hello \'world\'
```


可使用<code>'''.....'''</code>进行换行

```python
print('''aaa
bbb
ccc''')
```

* input() 接收用户输入

input('Enter name:') 显示提示语

```
>>> name = input('Enter name:\n')
Enter name:
Jim
>>> print('hello', name)
hello Jim
>>>
```

* 布尔值

Python中布尔值用True、False表示，布尔值的运算用and、or、not

* 将代码保存为文件

将以下代码保存为first.py，命令行进入文件所在目录，输入<code>python first.py</code>运行

```python
name = input('Enter name:\n')
print('hello', name)
```

运行结果：

```
D:\>python first.py
Enter name:
Jim
hello Jim

D:\>
```

* 代码格式

使用<code>#</code>进行注释，行尾冒号<code>:</code>进行缩进

```python
#判断语句

a = -30
if a > 0:
    print('+')
else:
    print('-')
```

>Python大小写敏感，大小写不正确将导致编译错误