﻿using LiteDB.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace LiteDB.Tests.Engine
{
    [TestClass]
    public class Create_Database_Tests
    {
        [TestMethod]
        public void Create_Database_With_Initial_Size()
        {
            var initial = 163840; // initial size: 20 x 8192 = 163.840 bytes
            var minimal = 8192 * 4; // 1 header + 1 collection + 1 data + 1 index = 4 pages minimal

            using (var file = new TempFile())
            using (var db = new LiteEngine(new EngineSettings { FileName = file.FileName, InitialSize = initial }))
            {
                // test if file has 40kb
                Assert.AreEqual(initial, file.Size);

                // insert minimal data
                db.Execute("insert into col1 values {a:1}");

                Assert.AreEqual(initial, file.Size);

                // ok, now shrink and test if file are minimal size
                db.Shrink();
                
                Assert.AreEqual(minimal, file.Size);
            }
        }
    }
}