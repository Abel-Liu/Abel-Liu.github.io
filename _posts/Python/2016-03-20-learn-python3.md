---
layout: learnpython
title:  "Python学习笔记（三） 控制语句"
date:   2016-03-20 11:00:00
categories: python
tags: [python, pylesson]
description: 
---
<!--more-->

* if

```python
s = input()
score = int(s)

if score >= 80:
    print('A')
elif score < 80 and score >= 60:
    print('B')
else:
    print('C')
```

另外，if判断一个变量时，只要是非零、非空字符串、非空list就是True

* for

```python
items = ['a', 'b', 'c']
for i in items:
    print(i)
```

还可以用range()生成一个包含连续数字的list，range(x)的范围是0到x-1

```python
sum = 0
for i in range(5):
    sum = sum + i
print(sum)
```

结果是10

* while

```python
sum = 0
i = 4

while i > 0:
    sum = sum + i
    i = i - 1

print(sum)
```

结果也是10
