using System;
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
            try
            {
                var fromCatDir = "./_site/_cat";
                var fromTagDir = "./_site/_tag";
                var destCatDir = "./cat";
                var destTagDir = "./tag";
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

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
