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

            IDTSCustomProperty100 cpActual = speechToText.CustomPropertyCollection[Utility.SubscriptionKeyPropName];
            Assert.AreEqual(cpActual.Name, Utility.SubscriptionKeyPropName, "Property name is wrong");
            Assert.AreEqual(cpActual.ExpressionType, DTSCustomPropertyExpressionType.CPET_NOTIFY, "Expression type on SubscriptionKeyPropName is wrong");

            cpActual = speechToText.CustomPropertyCollection[Utility.OperationModePropName];
            Assert.AreEqual(cpActual.Name, Utility.OperationModePropName, "Property name is wrong");
            Assert.AreEqual(cpActual.ExpressionType, DTSCustomPropertyExpressionType.CPET_NONE, "Expression type on OperationModePropName is wrong");
            Assert.AreEqual(cpActual.TypeConverter, typeof(SSISSpeechToText.OperationModeEnum).AssemblyQualifiedName, "Type Converter on OperationModePropName is wrong");

            cpActual = speechToText.CustomPropertyCollection[Utility.LanguagePropName];
            Assert.AreEqual(cpActual.Name, Utility.LanguagePropName, "Property name is wrong");
            Assert.AreEqual(cpActual.ExpressionType, DTSCustomPropertyExpressionType.CPET_NONE, "Expression type on LanguagePropName is wrong");
            Assert.AreEqual(cpActual.TypeConverter, typeof(SSISSpeechToText.SpeechLanguageEnum).AssemblyQualifiedName, "Type Converter on LanguagePropName is wrong");

            cpActual = speechToText.CustomPropertyCollection[Utility.ChannelSeparationPropName];
            Assert.AreEqual(cpActual.Name, Utility.ChannelSeparationPropName, "Property name is wrong");
            Assert.AreEqual(cpActual.ExpressionType, DTSCustomPropertyExpressionType.CPET_NONE, "Expression type on ChannelSeparationPropName is wrong");
            Assert.AreEqual(cpActual.TypeConverter, typeof(SSISSpeechToText.ChannelSeparationEnum).AssemblyQualifiedName, "Type Converter on ChannelSeparationPropName is wrong");

            intExpected = 1;
            intActual = speechToText.InputCollection.Count;
            Assert.AreEqual(intExpected, intActual, "Input Collection Count is wrong");
            Assert.AreEqual(speechToText.InputCollection[0].Name, "Input", "Input Name is Wrong");
            Assert.AreEqual(speechToText.InputCollection[0].InputColumnCollection.Count, 0, "There are input columns");

            intExpected = 1;
            intActual = speechToText.OutputCollection.Count;
            Assert.AreEqual(intExpected, intActual, "Output Collection Count is wrong");
            Assert.AreEqual(speechToText.OutputCollection[0].Name, "SpeechOutput", "Output Name is Wrong");
            Assert.AreEqual(speechToText.OutputCollection[0].OutputColumnCollection.Count, 3, "The number of output columns is wrong");

            Assert.AreEqual(speechToText.OutputCollection[0].OutputColumnCollection[Utility.OutputChannelColumnName].Name, Utility.OutputChannelColumnName, "Output column name is wrong");
            Assert.AreEqual(speechToText.OutputCollection[0].OutputColumnCollection[Utility.OutputChannelColumnName].DataType, DataType.DT_STR, "Output column OutputSpeechColumnName datatype is wrong");
            Assert.AreEqual((SSISSpeechToText.OutputTypeEnum) speechToText.OutputCollection[0].OutputColumnCollection[Utility.OutputChannelColumnName].CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value, SSISSpeechToText.OutputTypeEnum.Channel, "Output column OutputChannelColumnName purpose is wrong");

            Assert.AreEqual(speechToText.OutputCollection[0].OutputColumnCollection[Utility.OutputSpeechColumnName].Name, Utility.OutputSpeechColumnName, "Output column name is wrong");
            Assert.AreEqual(speechToText.OutputCollection[0].OutputColumnCollection[Utility.OutputSpeechColumnName].DataType, DataType.DT_TEXT, "Output column OutputSpeechColumnName datatype is wrong");
            Assert.AreEqual((SSISSpeechToText.OutputTypeEnum)speechToText.OutputCollection[0].OutputColumnCollection[Utility.OutputSpeechColumnName].CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value, SSISSpeechToText.OutputTypeEnum.Speech, "Output column OutputSpeechColumnName purpose is wrong");

            Assert.AreEqual(speechToText.OutputCollection[0].OutputColumnCollection[Utility.OutputTimecodeColumnName].Name, Utility.OutputTimecodeColumnName, "Output column name is wrong");
            Assert.AreEqual(speechToText.OutputCollection[0].OutputColumnCollection[Utility.OutputTimecodeColumnName].DataType, DataType.DT_DBTIME2, "Output column OutputTimecodeColumnName datatype is wrong");
            Assert.AreEqual((SSISSpeechToText.OutputTypeEnum)speechToText.OutputCollection[0].OutputColumnCollection[Utility.OutputTimecodeColumnName].CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value, SSISSpeechToText.OutputTypeEnum.Timecode, "Output column OutputTimecodeColumnName purpose is wrong");

        }


        [TestMethod]
        public void TestValidateOK()
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

            speechToText.CustomPropertyCollection[Utility.SubscriptionKeyPropName].Value = "NotTheDefault";

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISVALID;

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestValidateMissingInput()
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

            speechToTextInstance.DeleteInput(speechToText.InputCollection[0].ID);

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISCORRUPT;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidateExtraInput()
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

            speechToTextInstance.InsertInput(DTSInsertPlacement.IP_AFTER, speechToText.InputCollection[0].ID);

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISCORRUPT;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidateMissingOutput()
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

            speechToTextInstance.DeleteOutput(speechToText.OutputCollection[0].ID);

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISCORRUPT;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidateExtraOutput()
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

            speechToTextInstance.InsertOutput(DTSInsertPlacement.IP_AFTER, speechToText.OutputCollection[0].ID);

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISCORRUPT;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidateMissingCustomProperty()
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

            speechToText.CustomPropertyCollection.RemoveObjectByIndex(0);

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISCORRUPT;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidateExtraCustomProperty()
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

            speechToText.CustomPropertyCollection.New();

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISCORRUPT;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidateCustomProperty_SubscriptionKey_Missing()
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
            speechToText.CustomPropertyCollection.RemoveObjectByID(speechToText.CustomPropertyCollection[Utility.SubscriptionKeyPropName].ID);
            speechToText.CustomPropertyCollection.New();

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISCORRUPT;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidateCustomProperty_OperationMode_Missing()
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
            speechToText.CustomPropertyCollection[Utility.SubscriptionKeyPropName].Value = "NotTheDefault";
            speechToText.CustomPropertyCollection.RemoveObjectByID(speechToText.CustomPropertyCollection[Utility.OperationModePropName].ID);
            speechToText.CustomPropertyCollection.New();

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISCORRUPT;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidateCustomProperty_Language_Missing()
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
            speechToText.CustomPropertyCollection[Utility.SubscriptionKeyPropName].Value = "NotTheDefault";
            speechToText.CustomPropertyCollection.RemoveObjectByID(speechToText.CustomPropertyCollection[Utility.LanguagePropName].ID);
            speechToText.CustomPropertyCollection.New();

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISCORRUPT;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidateCustomProperty_ChannelSeparation_Missing()
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
            speechToText.CustomPropertyCollection[Utility.SubscriptionKeyPropName].Value = "NotTheDefault";
            speechToText.CustomPropertyCollection.RemoveObjectByID(speechToText.CustomPropertyCollection[Utility.ChannelSeparationPropName].ID);
            speechToText.CustomPropertyCollection.New();

            DTSValidationStatus actual = speechToTextInstance.Validate();
            DTSValidationStatus expected = DTSValidationStatus.VS_ISCORRUPT;

            Assert.AreEqual(expected, actual);
        }

    }
}
