using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Martin.SQLServer.Dts.Tests;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace Martin.SQLServer.Dts.Tests
{
    [TestClass]
    public class SSISSpeechToTextTest
    {
        [TestMethod]
        public void TestProvideComponentProperties()
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

            int intExpected = 5;  // Add 1 to the number that the component added, as there is a custom MS one as well.
            int intActual = speechToText.CustomPropertyCollection.Count;
            Assert.AreEqual(intExpected, intActual, "Custom Property Collection Count is wrong");

            for (int i = 0; i < intActual; i++)
            {
                string value = speechToText.CustomPropertyCollection[i].Name;
                Assert.IsNotNull(value);
            }

            // ToDo: Complete this unit test.

        }
    }
}
