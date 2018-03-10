// ----------------------------------------------------------------
// ExceptionBrowser
// Version 1.0.0
// Copyright 2008, Irénée HOTTIER,
// 
// This is free software licensed under the NUnit license, You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ----------------------------------------------------------------

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
