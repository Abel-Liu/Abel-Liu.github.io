---
layout: learnpython
title:  "Python学习笔记（二） 常用类型"
date:   2016-03-16 15:30:00
categories: python
tags: [python, pylesson]
description: 
---
<!--more-->

* list

Python内置的列表类型，好玩的是可以用负的索引倒着取值（索引0是第一个元素，-1是最后一个元素，-2是倒数第二个）

```python
items = ['a', 'b', 'c']
print('all items: %s' % items)
print('length is %d' % len(items))
print('first: %s' % items[0])
print('last: %s' % items[-1])
```

```
D:\>python hello.py
all items: ['a', 'b', 'c']
length is 3
first: a
last: c
```

使用append追加元素，insert插入到指定位置

```python
>>> items = ['a', 'b', 'c']
>>> items.append('d')
>>> print(items)
['a', 'b', 'c', 'd']
>>> items.insert(1, 'x')
>>> print(items)
['a', 'x', 'b', 'c', 'd']
```

使用pop()取出末尾元素（将从列表中移除），pop(i)取指定位置的元素

```python
>>> one = items.pop()
>>> one
'd'
>>> print(items)
['a', 'x', 'b', 'c']
>>> one = items.pop(1)
>>> one
'x'
>>> print(items)
['a', 'b', 'c']
```

list中的元素类型可以不同，甚至可以包含另一个list

```python
>>> items = ['abc', 123, True]
>>> items
['abc', 123, True]
>>> items = ['a', ['b', 'c'], 'd']
>>> len(items)
3
>>> items[0]
'a'
>>> items[1][0]
'b'
```

* tuple

Python中的另一种列表，和list相似，唯一不同的是tuple一旦初始化就不能改变，但可以用索引取值，tuple使用`小括号()`来包裹元素。下方例子给items赋值会报错

```python
>>> items = ('a', 'b', 'c')
>>> items[0] = 'x'
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
TypeError: 'tuple' object does not support item assignment
```

需要注意，若定义的tuple中只有一个元素，会有问题

```python
>>> items = (1)
>>> items
1
```

items实际存的是数字1，而不是列表。所以要用逗号来消除歧义

```python
>>> items = (1,)
>>> items
(1,)
```

另一个需要注意的是“可变”的tuple，看以下例子

```python
>>> items = (1, [2, 3], 4)
>>> items[1][0] = 5
>>> items
(1, [5, 3], 4)
```

tuple的第二个元素是一个list，可以改变list的值，因为tuple的第二个元素实际指向的内存地址没有变

* dict

dict顾名思义就是字典，存储的是key-value（键值对）

```python
>>> ages = {'Tom': 28, 'Jack': 22, 'Amy': 25}
>>> ages['Jack']
22
```

如果key不存在会报错

```python
>>> ages['Mike']
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
KeyError: 'Mike'
```

为了防止出错可以判断key是否存在

```python
>>> 'Mike' in ages
False
```

或者通过get()来取值，如果key不存在返回None，或者自己指定返回值

```python
>>> ages.get('Mike')
>>> ages.get('Mike', 0)
0
```

和list一样，dict也有pop()方法

```python
>>> a = ages.pop('Tom')
>>> a
28
>>> ages
{'Jack': 22, 'Amy': 25}
```

可以给指定的key赋新值，若key不存在则添加

```python
>>> ages
{'Jack': 22, 'Amy': 25}
>>> ages['Jack'] = 33
>>> ages
{'Jack': 33, 'Amy': 25}
>>> ages['Lucy'] = 44
>>> ages
{'Jack': 33, 'Lucy': 44, 'Amy': 25}
>>>
```

>dict中的元素是无序的

* set

set和dict类似，但set只有key没有value，set中的key是不重复的

使用list来初始化一个set，重复元素会被过滤

```python
>>> s = set([1, 2, 2, 3])
>>> s
{1, 2, 3}
```

由于set只有key，所以通过add()来添加元素，remove()来移除元素

```python
>>> s = set([1, 2, 3])
>>> s.add(4)
>>> s
{1, 2, 3, 4}
>>> s.remove(1)
>>> s
{2, 3, 4}
```

另外，set可以做并集、交集操作

```python
>>> s1 = set([1, 2, 3])
>>> s2 = set([2, 3, 4])
>>> s1 &s2
{2, 3}
>>> s1 | s2
{1, 2, 3, 4}
```

dict和set的key都只能是不可变对象

```python
>>> a = {'a': 1, [2, 3]: 2}
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
TypeError: unhashable type: 'list'
>>> a = {'a': 1, (2, 3): 2}
>>> a
{'a': 1, (2, 3): 2}
>>> a[(2, 3)]
2
```