/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2015/1/12 17:44:42
 *  FileName:TestFileClass.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/

using System.IO;

using Xunit;

using Example.Infrastructure;

namespace Example.UnitTest
{
    public class TestFileClass
    {
        [Fact]
        public void TestFileWrite()
        {
            var p1 = @"F:\Ax0ne\Example\Ax0ne.Example\test.txt";

            Utils.WriteFile(p1, "Unit Test \r\n", null);
            Assert.True(File.Exists(p1));
        }
        [Fact]
        public void TestFileWriteAppend()
        {
            var p1 = @"F:\Ax0ne\Example\Ax0ne.Example\test.txt";
            var content = string.Empty;
            for (int i = 10; i < 100; i++)
            {
                content += string.Format("我是第{0}行\r\n", i);
            }
            Utils.WriteFileAppend(p1, content, null);
            Assert.True(File.Exists(p1));
        }
        [Fact]
        public void TestGenerateSqlEnum()
        {
            var assemblyPath = @"F:\Ax0neWork\BusinessExpress2.0\trunk\BExpress.Model\bin\Debug\BExpress.Model.dll";
            var filePath = @"C:\Users\Ax0ne\Desktop\EnumSql.txt";
            EnumUtils.GenerateSqlByAssembly(assemblyPath, filePath);
            Assert.True(File.Exists(filePath));
        }
    }
}
