---
layout: learnpython
title:  "Python学习笔记（四） 高级操作"
date:   2016-03-20 14:00:00
categories: python
tags: [python, pylesson]
description: 
---
<!--more-->

* 切片

我们一般取元素是通过索引，如果要取多个元素就得循环，来看Python更强大的方法

现有一个list，取前三个元素

```python
>>> items = ['a', 'b', 'c', 'd', 'e']
>>> items[0:3]
['a', 'b', 'c']
```

items[0:3]的意思是取出索引从0到3的元素，但不包括3 。然后看取从1到4

```python
>>> items[1:4]
['b', 'c', 'd']
```

若从0开始，0可以省略

```python
>>> items[:3]
['a', 'b', 'c']
```

前面说过list可以通过负索引倒着取值，那还可以这样

```python
>>> items[-4:-1]
['b', 'c', 'd']
```

注意第一个数必须比第二个数小，不然取不到任何元素

现在我们创建一个0-99的list

```python
>>> items = list(range(100))
```

取前10个数

```python
>>> items[:10]
[0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
```

前10个数中每2个取一个

```python
>>> items[:10:2]
[0, 2, 4, 6, 8]
```

所有数，每5个取一个

```python
>>> items[::5]
[0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95]
```

切片还可以用来截取字符串

```python
>>> '12345'[:3]
'123'
>>> '12345'[::2]
'135'
```

* 迭代

前面说过for循环，for...in...就是迭代，下面再举一些例子

```python
>>> d = {'a': 1, 'b': 2, 'c': 3}
>>> for i in d:
...     print(i)
...
a
c
b
```

这是迭代dict，拿到的只是key，因为dict是无序的，每次结果可能都不一样。

如果要迭代value，可以用for v in d.values()；如果要同时迭代key和value，可以用for k, v in d.items()

```python
>>> for v in d.values():
...     print(v)
...
1
3
2
>>> for k,v in d.items():
...     print('%s: %s' % (k,v))
...
a: 1
c: 3
b: 2
```

字符串也可以迭代

```python
>>> for i in 'ABC':
...     print(i)
...
A
B
C
```

那什么样的对象可以被迭代呢？可以判断对象是collections模块的Iterable类型

```python
>>> from collections import Iterable
>>> isinstance('abc', Iterable)
True
>>> isinstance([1, 2, 3], Iterable)
True
>>> isinstance(123, Iterable)
False
```
