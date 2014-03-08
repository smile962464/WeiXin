using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WeiXin.Models;

namespace WeiXin.Test
{
    [TestClass]
    public class RMessageTest
    {
        [TestMethod]
        public void TextMessageTest()
        {
            RTextMessage t = new RTextMessage("from", "to", "test");

            var json = t.ToJsonString();
            //Assert.AreEqual( ((dynamic)t.ToJson()).touser, "to");
        }
    }
}
