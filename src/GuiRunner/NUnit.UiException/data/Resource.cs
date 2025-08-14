using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.UiException.Tests.data
{
    public class Resource
    {
        public const string HelloWorld_txt = "HelloWorld.txt";
        public const string TextCode_cs = "TextCode.cs";
        public const string Image_png = "Image.png";
        public const string OneLine_txt = "OneLine.txt";

        public static string GetFilename(string filename)
        {
            string path;

            path = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..\\..\\data\\");

            path = System.IO.Path.GetFullPath(path);

            path += filename;

            return (path);
        }        
    }
}
