using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martin.SQLServer.Dts;

namespace Martin.SQLServer.Dts.Tests
{
    [TestClass]
    public class UtilityTest
    {
        /// <summary>
        ///A test for OutputColumnOutputTypePropName
        ///</summary>
        [TestMethod()]
        public void OutputColumnOutputTypePropNameTest()
        {
            string actual;
            actual = Utility.OutputColumnOutputTypePropName;
            Assert.AreEqual("OutputColumnType", actual);
        }

        [TestMethod()]
        public void OutputChannelColumnNameTest()
        {
            string actual;
            actual = Utility.OutputChannelColumnName;
            Assert.AreEqual("InputChannel", actual);
        }

        [TestMethod()]
        public void OutputSpeechColumnNameTest()
        {
            string actual;
            actual = Utility.OutputSpeechColumnName;
            Assert.AreEqual("SpeechTextResult", actual);
        }

        [TestMethod()]
        public void OutputTimecodeColumnNameTest()
        {
            string actual;
            actual = Utility.OutputTimecodeColumnName;
            Assert.AreEqual("Timecode", actual);
        }

        [TestMethod()]
        public void SubscriptionKeyPropNameTest()
        {
            string actual;
            actual = Utility.SubscriptionKeyPropName;
            Assert.AreEqual("SubscriptionKey", actual);
        }


        [TestMethod()]
        public void OperationModePropNameTest()
        {
            string actual;
            actual = Utility.OperationModePropName;
            Assert.AreEqual("OperationMode", actual);
        }
        

        [TestMethod()]
        public void LanguagePropNameTest()
        {
            string actual;
            actual = Utility.LanguagePropName;
            Assert.AreEqual("APILanguage", actual);
        }

        [TestMethod()]
        public void ChannelSeparationPropNameTest()
        {
            string actual;
            actual = Utility.ChannelSeparationPropName;
            Assert.AreEqual("ChannelSeparation", actual);
        }
    }
}
