using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.Bing.Speech;
using NAudio.Wave;
using Microsoft.VisualStudio.Threading;

namespace Martin.SQLServer.Dts.Tests
{
    [TestClass]
    public class SpeechToTextImplementTest
    {
        private System.Configuration.Configuration config;

        [TestInitialize]
        [DeploymentItem("SpeechLicense.config", ".")]
        public void SetupSpeechToTextImplementTest()
        {
            ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap();
            exeConfigurationFileMap.ExeConfigFilename = "SpeechLicense.config";
            this.config = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
            DeviceMetadata dummy = new DeviceMetadata(DeviceType.Far, DeviceFamily.Automotive, NetworkType.Cell1xRTT, OsName.Windows, "10", "whocares", "123");
            var dummy2 = NAudio.Wave.WaveCallbackStrategy.Event;
            var dummy3 = Microsoft.VisualStudio.Threading.JoinableTaskCreationOptions.LongRunning;
        }

        [TestMethod]
        [DeploymentItem("SpeechLicense.config", ".")]
        public void TestConfigFileIsAvailable()
        {
            Assert.IsNotNull(config.AppSettings.Settings["subscriptionKey"]);
            Assert.AreNotEqual("87DFG8S7G9S8G98SDFGHSDFG978DA", config.AppSettings.Settings["subscriptionKey"].Value);
            /*
             * Thank you for calling Lorem Ipsum, Australia's best widgets. We're attending to other customers right now, but please leave your name and phone number and we'll get back to you as soon as we can. Thanks again for your call.
             */
            return;
        }


        [TestMethod]
        [DeploymentItem("Ex_Pro_1.mp3", ".")]
        [DeploymentItem("SpeechLicense.config", ".")]
        public void TestExecuteRecogniseAsyncBasic()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;
            ComponentEventHandler events = new ComponentEventHandler();
            dataFlowTask.Events = DtsConvert.GetExtendedInterface(events as IDTSComponentEvents);

            IDTSComponentMetaData100 speechToText = dataFlowTask.ComponentMetaDataCollection.New();
            speechToText.ComponentClassID = typeof(Martin.SQLServer.Dts.SSISSpeechToText).AssemblyQualifiedName;
            CManagedComponentWrapper speechToTextInstance = speechToText.Instantiate();

            speechToTextInstance.ProvideComponentProperties();
            speechToText.CustomPropertyCollection[Utility.SubscriptionKeyPropName].Value = config.AppSettings.Settings["subscriptionKey"].Value;

            Uri targetURI = new Uri(@"wss://speech.platform.bing.com/api/service/recognition/continuous");
            SpeechToTextImplement testMe = new SpeechToTextImplement(speechToText, "en-us", targetURI, config.AppSettings.Settings["subscriptionKey"].Value);
            string filename = "Ex_Pro_1.mp3";
            testMe.ExecuteRecogniseAsync(filename).Wait();
            Assert.IsTrue(testMe.Results.Count > 0);
        }
    }
}
