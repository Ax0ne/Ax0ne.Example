using System;
using System.IO;
using Example.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Example.UnitTest
{
    [TestClass]
    public class FileTest
    {
        [TestMethod]
        public void TestFileWrite()
        {
            var p1 = @"F:\Ax0ne\Example\Ax0ne.Example\test.txt";

            Utils.WriteFile(p1, "Unit Test \r\n", null);
            Assert.IsTrue(File.Exists(p1));
        }
        [TestMethod]
        public void TestFileWriteAppend()
        {
            var p1 = @"F:\Ax0ne\Example\Ax0ne.Example\test.txt";
            var content = string.Empty;
            for (int i = 10; i < 100; i++)
            {
                content += string.Format("我是第{0}行\r\n", i);
            }
            Utils.WriteFileAppend(p1, content, null);
            Assert.IsTrue(File.Exists(p1));
        }
        [TestMethod]
        public void TestGenerateSqlEnum()
        {
            var assemblyPath = @"F:\Ax0neWork\BusinessExpress2.0\trunk\BExpress.Model\bin\Debug\BExpress.Model.dll";
            var filePath = @"C:\Users\Ax0ne\Desktop\EnumSql.txt";
            EnumUtils.GenerateSqlByAssembly(assemblyPath, filePath);
            Assert.IsTrue(File.Exists(filePath));
        }
    }
}
