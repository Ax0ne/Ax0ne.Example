/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/8/22 10:02:05
 *  FileName:CloneDemo.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Example.ConsoleApp
{
    [Serializable]
    public class Employee : ICloneable
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }

        public object Clone()
        {
            using (Stream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
            //return this.MemberwiseClone();
        }
    }

    [Serializable]
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}