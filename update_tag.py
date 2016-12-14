#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import os
import shutil
import time
import threading

def update_all():
    fromCatDir = "./_site/_cat"
    fromTagDir = "./_site/_tag"
    destCatDir = "./cat"
    destTagDir = "./tag"
    catTpl = '''---
layout: catpage
tag: %s
---'''

    tagTpl = '''---
layout: tagpage
tag: %s
---'''

    gen(fromCatDir, destCatDir, catTpl)
    gen(fromTagDir, destTagDir, tagTpl)


def gen(from_dir, to_dir, tpl):
    if os.path.exists(to_dir):
        shutil.rmtree(to_dir)
    
    time.sleep(1)
    os.mkdir(to_dir)

    if os.path.exists(from_dir):
        for root,dirs,files in os.walk(from_dir):
            for f in files:
                fs = open(os.path.join(root, f), 'r')
                text = fs.read()
                fs.close()

                md = os.path.join(to_dir, text + '.md')
                ws = open(md, 'w')
                ws.write(tpl % (text))
                ws.close()
                print(os.path.abspath(md))
       
if __name__ == '__main__':
    update_all()