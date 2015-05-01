﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;

namespace GenTagTpl
{
    class Program
    {
        static void Main(string[] args)
        {
            var fromCatDir = ConfigurationManager.AppSettings["fromCatDir"];
            var fromTagDir = ConfigurationManager.AppSettings["fromTagDir"];
            var destCatDir = ConfigurationManager.AppSettings["destCatDir"];
            var destTagDir = ConfigurationManager.AppSettings["destTagDir"];
            var catTpl =
@"---
layout: catpage
tag: {0}
---";
            var tagTpl =
@"---
layout: tagpage
tag: {0}
---";
            Gen(fromCatDir, destCatDir, catTpl);
            Gen(fromTagDir, destTagDir, tagTpl);

            Console.WriteLine("OK. Press any key to exit...");
            Console.ReadKey();
        }

        static void Gen(string fromDir, string destDir, string tpl)
        {
            if (Directory.Exists(destDir))
            {
                Directory.Delete(destDir, true);
            }
            Directory.CreateDirectory(destDir);

            if (Directory.Exists(fromDir))
            {
                var files = Directory.GetFiles(fromDir, "*", SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    var cat = File.ReadAllText(f);
                    var md = Path.Combine(destDir, cat + ".md");
                    File.WriteAllText(md, string.Format(tpl, cat));
                    Console.WriteLine(new FileInfo(md).FullName);
                }
            }
        }
    }
}
