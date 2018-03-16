using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Martin.SQLServer.Dts.Tests
{
    [TestClass]
    public class SpeechToTextImplementTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap();
            exeConfigurationFileMap.ExeConfigFilename = "SpeechLicense.config";
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
            Assert.IsNotNull(config.AppSettings.Settings["subscriptionKey"]);
            Assert.AreEqual("87DFG8S7G9S8G98SDFGHSDFG978DA", config.AppSettings.Settings["subscriptionKey"].Value);
            /*
             * Thank you for calling Lorem Ipsum, Australia's best widgets. We're attending to other customers right now, but please leave your name and phone number and we'll get back to you as soon as we can. Thanks again for your call.
             */
            return;
        }
    }
}
