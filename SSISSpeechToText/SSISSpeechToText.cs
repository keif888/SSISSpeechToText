using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace Martin.SQLServer.Dts
{
    [DtsPipelineComponent(ComponentType = ComponentType.Transform
        , CurrentVersion = 0
        , DisplayName = "SSIS Speech To Text"
        , Description = "Uses Microsoft Cognitive Services to translate Speech to text"
        , IconResource = "Martin.SQLServer.Dts.key.ico"
#if SQL2017
        , UITypeName = "Martin.SQLServer.Dts.SSISSpeechToTextUI, SSISSpeechToText, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b4fe731b01e0026b"
#endif
#if SQL2016
        , UITypeName = "Martin.SQLServer.Dts.SSISSpeechToTextUI, SSISSpeechToText, Version=13.0.0.0, Culture=neutral, PublicKeyToken=b4fe731b01e0026b"
#endif
#if SQL2014
        , UITypeName = "Martin.SQLServer.Dts.SSISSpeechToTextUI, SSISSpeechToText, Version=12.0.0.0, Culture=neutral, PublicKeyToken=b4fe731b01e0026b"
#endif
#if SQL2012
        , UITypeName = "Martin.SQLServer.Dts.SSISSpeechToTextUI, SSISSpeechToText, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b4fe731b01e0026b"
#endif
        )]
    public class SSISSpeechToText : PipelineComponent
    {
        #region Enumerators
        public enum OperationModeEnum
        {
            ShortDictation,
            LongDictation
        }

        public enum SpeechLanguageEnum
        {
            AmericanEnglish,
            BritishEnglish,
            German,
            Spanish,
            French,
            Italian,
            Mandarin
        }

        public enum ChannelSeparationEnum
        {
            MonoIgnore,
            SeparateLeftAndRight
        }

        public enum OutputTypeEnum
        {
            PassThrough,
            Channel,
            Speech,
            Timecode
        }

        #endregion


        #region private variables
        /// <summary>
        /// Stores the Bing API subscription key
        /// </summary>
        private string SubscriptionKey;

        /// <summary>
        /// Stores the API operatio mode
        /// </summary>
        private OperationModeEnum OperationMode;

        /// <summary>
        /// Stores the language of the speech to be processed
        /// </summary>
        private SpeechLanguageEnum Language;

        /// <summary>
        /// Stores what audio processing is required.
        /// </summary>
        private ChannelSeparationEnum ChannelSeparation;

        #endregion


        #region Design Time
        #region ProvideComponentProperties
        /// <summary>
        /// Sets up all the base properties for the component to utilise
        /// </summary>
        public override void ProvideComponentProperties()
        {
            // Support resettability.
            this.RemoveAllInputsOutputsAndCustomProperties();
            // Let the base component add the input and output.
            base.ProvideComponentProperties();
            // Add Contact information etc.
            ComponentMetaData.ContactInfo = "https://github.com/keif888/SSISSpeechToText/";

            // Add all the required properties.
            SSISSpeechToText.AddSubscriptionKeyProperty(this.ComponentMetaData);
            SSISSpeechToText.AddOperationModeProperty(this.ComponentMetaData);
            SSISSpeechToText.AddLanguageProperty(this.ComponentMetaData);
            SSISSpeechToText.AddChannelSeparationProperty(this.ComponentMetaData);

            // Name the input and output, and make the output asynchronous.
            ComponentMetaData.InputCollection[0].Name = "Input";

            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
            output.Name = "SpeechOutput";
            output.Description = "Speech analysis rows are directed to this output.";
            output.SynchronousInputID = 0;


            // Add the output channel column
            IDTSOutputColumn100 outputChannelColumn = output.OutputColumnCollection.New();
            outputChannelColumn.Name = Utility.OutputChannelColumnName;
            outputChannelColumn.Description = "Contains which channel this speech came from";
            outputChannelColumn.SetDataTypeProperties(DataType.DT_STR, 30, 0, 0, 1252);
            AddOutputTypeProperty(outputChannelColumn, OutputTypeEnum.Channel);

            // Add the output speech column
            IDTSOutputColumn100 outputSpeechColumn = output.OutputColumnCollection.New();
            outputSpeechColumn.Name = Utility.OutputSpeechColumnName;
            outputSpeechColumn.Description = "Contains which channel this speech came from";
            outputSpeechColumn.SetDataTypeProperties(DataType.DT_TEXT, 0, 0, 0, 1252);
            AddOutputTypeProperty(outputSpeechColumn, OutputTypeEnum.Speech);

            //Add the output timecodes column
            IDTSOutputColumn100 outputTimecodesColumn = output.OutputColumnCollection.New();
            outputTimecodesColumn.Name = Utility.OutputTimecodeColumnName;
            outputTimecodesColumn.Description = "Contains the time that this fragment of speech started at";
            outputTimecodesColumn.SetDataTypeProperties(DataType.DT_DBTIME2, 0, 0, 0, 0);
            AddOutputTypeProperty(outputTimecodesColumn, OutputTypeEnum.Timecode);

        }




        #endregion

        #endregion


        #region Property interaction

        private static void AddSubscriptionKeyProperty(IDTSComponentMetaData100 componentMetaData)
        {
            IDTSCustomProperty100 subscriptionKeyProperty = componentMetaData.CustomPropertyCollection.New();
            subscriptionKeyProperty.Name = Utility.SubscriptionKeyPropName;
            subscriptionKeyProperty.Description = "Enter your subscription key for Bing Speech API.";
            subscriptionKeyProperty.ContainsID = false;
            subscriptionKeyProperty.EncryptionRequired = true;
            subscriptionKeyProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NOTIFY;
            subscriptionKeyProperty.Value = "SubscriptionKeyRequired";
        }

        private static void AddOperationModeProperty(IDTSComponentMetaData100 componentMetaData)
        {
            IDTSCustomProperty100 operationModeProperty = componentMetaData.CustomPropertyCollection.New();
            operationModeProperty.Name = Utility.OperationModePropName;
            operationModeProperty.Description = "Select the mode for operation of the Speech API.";
            operationModeProperty.ContainsID = false;
            operationModeProperty.EncryptionRequired = false;
            operationModeProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            operationModeProperty.TypeConverter = typeof(OperationModeEnum).AssemblyQualifiedName;
            operationModeProperty.Value = OperationModeEnum.ShortDictation;
        }

        private static void AddLanguageProperty(IDTSComponentMetaData100 componentMetaData)
        {
            IDTSCustomProperty100 languageProperty = componentMetaData.CustomPropertyCollection.New();
            languageProperty.Name = Utility.LanguagePropName;
            languageProperty.Description = "Select the language for operation of the Speech API.";
            languageProperty.ContainsID = false;
            languageProperty.EncryptionRequired = false;
            languageProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            languageProperty.TypeConverter = typeof(SpeechLanguageEnum).AssemblyQualifiedName;
            languageProperty.Value = SpeechLanguageEnum.BritishEnglish;
        }

        private static void AddChannelSeparationProperty(IDTSComponentMetaData100 componentMetaData)
        {
            IDTSCustomProperty100 channelSeparationProperty = componentMetaData.CustomPropertyCollection.New();
            channelSeparationProperty.Name = Utility.ChannelSeparationPropName;
            channelSeparationProperty.Description = "Select whether to split stereo into separate channels, and process them separately.";
            channelSeparationProperty.ContainsID = false;
            channelSeparationProperty.EncryptionRequired = false;
            channelSeparationProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            channelSeparationProperty.TypeConverter = typeof(ChannelSeparationEnum).AssemblyQualifiedName;
            channelSeparationProperty.Value = ChannelSeparationEnum.MonoIgnore;
        }

        #region AddOutputTypeProperty
        /// <summary>
        /// Creates the new custom property to hold the output type.
        /// </summary>
        /// <param name="outputColumn">The output column to add the property to.</param>
        /// <param name="outputType">provides the type of the output column</param>
        static private void AddOutputTypeProperty(IDTSOutputColumn100 outputColumn, OutputTypeEnum outputType)
        {
            // Add the Output Type property
            IDTSCustomProperty100 outputTypeProperty = outputColumn.CustomPropertyCollection.New();
            outputTypeProperty.Description = "Stores the type of the output column";
            outputTypeProperty.Name = Utility.OutputColumnOutputTypePropName;
            outputTypeProperty.ContainsID = false;
            outputTypeProperty.EncryptionRequired = false;
            outputTypeProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            outputTypeProperty.TypeConverter = typeof(OutputTypeEnum).AssemblyQualifiedName;
            outputTypeProperty.Value = outputType;
        }
        #endregion


        #endregion
    }
}
