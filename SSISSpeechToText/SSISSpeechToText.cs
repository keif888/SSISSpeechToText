﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Bing.Speech;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System.Windows;
using System.Windows.Threading;

namespace Martin.SQLServer.Dts
{
    [DtsPipelineComponent(ComponentType = ComponentType.Transform
        , CurrentVersion = 0
        , DisplayName = "SSIS Speech To Text"
        , Description = "Uses Microsoft Cognitive Services to translate Speech to text"
        , IconResource = "Martin.SQLServer.Dts.if_microphone_497229.ico"
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
        /// Used when an Event needs to be canceled.
        /// </summary>
        private bool cancelEvent;

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

        /// <summary>
        /// Stores the actual async output buffer that will receive all the output data
        /// </summary>
        private PipelineBuffer outputBuffer;

        /// <summary>
        /// The speech recognition client
        /// </summary>
        private SpeechClient speechClient;

        /// <summary>
        /// Stores the URI to connect to the Speech service.
        /// </summary>
        private Uri SpeechServiceUri;

        #endregion


        #region Background Tasks

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
            SSISSpeechToText.AddShortPhraseUrlProperty(this.ComponentMetaData);
            SSISSpeechToText.AddLongPhraseUrlProperty(this.ComponentMetaData);

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
            outputTimecodesColumn.SetDataTypeProperties(DataType.DT_DBTIME2, 0, 0, 7, 0);
            AddOutputTypeProperty(outputTimecodesColumn, OutputTypeEnum.Timecode);

        }

        #endregion

        #region PerformUpgrade
        /// <summary>
        /// If this component needs to add new properties, that aren't handled in the initial version, then this code will be updated.
        /// </summary>
        /// <param name="pipelineVersion"></param>
        public override void PerformUpgrade(int pipelineVersion)
        {
            base.PerformUpgrade(pipelineVersion);
        }
        #endregion

        #region PerformDowngrade
        /// <summary>
        /// If you have a different "version" of your component in the version of SSIS then this is where you
        /// tweak it.  https://docs.microsoft.com/en-us/sql/integration-services/extending-packages-custom-objects/support-multi-targeting-in-your-custom-components
        /// </summary>
        /// <param name="pipelineVersion"></param>
        /// <param name="targetServerVersion"></param>
        public override void PerformDowngrade(int pipelineVersion, DTSTargetServerVersion targetServerVersion)
        {
            base.PerformDowngrade(pipelineVersion, targetServerVersion);
        }
        #endregion

        #region Validate

        /// <summary>
        /// Called repeatedly when the component is edited in the designer, and once at the beginning of execution.
        /// Verifies the following:
        /// 1. Check that there is only one output
        /// 2. Check that there is only one input
        /// 3. Check that the required CustomProperties exist (and are valid)
        /// 4. Check that the required output columns are present and correct
        /// 5. The base class validation succeeds.
        /// </summary>
        /// <returns>The status of the validation</returns>
        public override DTSValidationStatus Validate()
        {
            if (ComponentMetaData.InputCollection.Count < 1)
            {
                this.InternalFireError("The required input is missing from the collection.");
                return DTSValidationStatus.VS_ISCORRUPT;
            }
            if (ComponentMetaData.InputCollection.Count > 1)
            {
                this.InternalFireError("There is more than one input in the collection.");
                return DTSValidationStatus.VS_ISCORRUPT;
            }
            if (ComponentMetaData.OutputCollection.Count < 1)
            {
                this.InternalFireError("The required output is missing from the collection.");
                return DTSValidationStatus.VS_ISCORRUPT;
            }
            if (ComponentMetaData.OutputCollection.Count > 1)
            {
                this.InternalFireError("There is more than one output in the collection.");
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (ComponentMetaData.CustomPropertyCollection.Count != 7)
            {
                this.InternalFireError("There is either to many or not enough custom properties.");
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            bool foundSubscriptionKey = false;
            bool foundChannelSeparation = false;
            bool foundLanguage = false;
            bool foundOperationMode = false;
            bool foundShortPhrase = false;
            bool foundLongPhrase = false;

            // Search for all the property names.
            foreach (IDTSCustomProperty100 cp in ComponentMetaData.CustomPropertyCollection)
            {
                switch (cp.Name)
                {
                    case Utility.ConstSubscriptionKeyPropName:
                        if (string.Compare(cp.Value.ToString(), "SubscriptionKeyRequired", StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            this.InternalFireError("Subscription Key must be set to real value");
                            return DTSValidationStatus.VS_ISBROKEN;
                        }
                        foundSubscriptionKey = true;
                        break;
                    case Utility.ConstChannelSeparationPropName:
                        if (cp.TypeConverter != typeof(ChannelSeparationEnum).AssemblyQualifiedName)
                        {
                            this.InternalFireError("Channel Separation data type is incorrect.");
                            return DTSValidationStatus.VS_ISCORRUPT;
                        }
                        foundChannelSeparation = true;
                        break;
                    case Utility.ConstLanguagePropName:
                        if (cp.TypeConverter != typeof(SpeechLanguageEnum).AssemblyQualifiedName)
                        {
                            this.InternalFireError("API Language data type is incorrect.");
                            return DTSValidationStatus.VS_ISCORRUPT;
                        }
                        foundLanguage = true;
                        break;
                    case Utility.ConstOperationModePropName:
                        if (cp.TypeConverter != typeof(OperationModeEnum).AssemblyQualifiedName)
                        {
                            this.InternalFireError("Operation Mode data type is incorrect.");
                            return DTSValidationStatus.VS_ISCORRUPT;
                        }
                        foundOperationMode = true;
                        break;
                    case Utility.ConstShortPhraseUrlPropName:
                        if (cp.TypeConverter != string.Empty)
                        {
                            this.InternalFireError("Short Phrase Url data type is incorrect.");
                            return DTSValidationStatus.VS_ISCORRUPT;
                        }
                        foundShortPhrase = true;
                        break;
                    case Utility.ConstLongPhraseUrlPropName:
                        if (cp.TypeConverter != string.Empty)
                        {
                            this.InternalFireError("Long Phrase Url data type is incorrect.");
                            return DTSValidationStatus.VS_ISCORRUPT;
                        }
                        foundLongPhrase = true;
                        break;
                    default:
                        break;
                }
            }

            if (!foundSubscriptionKey)
            {
                this.InternalFireError(string.Format("Custom Property {0} is missing.", Utility.SubscriptionKeyPropName));
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (!foundOperationMode)
            {
                this.InternalFireError(string.Format("Custom Property {0} is missing.", Utility.OperationModePropName));
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (!foundLanguage)
            {
                this.InternalFireError(string.Format("Custom Property {0} is missing.", Utility.LanguagePropName));
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (!foundChannelSeparation)
            {
                this.InternalFireError(string.Format("Custom Property {0} is missing.", Utility.ChannelSeparationPropName));
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (!foundShortPhrase)
            {
                this.InternalFireError(string.Format("Custom Property {0} is missing.", Utility.ShortPhraseUrlPropName));
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (!foundLongPhrase)
            {
                this.InternalFireError(string.Format("Custom Property {0} is missing.", Utility.LongPhraseUrlPropName));
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            // Test that the required output columns are their.
            bool foundInputChannel = false;
            bool foundSpeechText = false;
            bool foundTimeCode = false;
            foreach(IDTSOutputColumn100 oc in ComponentMetaData.OutputCollection[0].OutputColumnCollection)
            {
                foreach (IDTSCustomProperty100 cp in oc.CustomPropertyCollection)
                {
                    if (cp.Name == Utility.OutputColumnOutputTypePropName)
                    {
                        switch ((OutputTypeEnum)oc.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value)
                        {
                            case OutputTypeEnum.Channel:
                                foundInputChannel = true;
                                break;
                            case OutputTypeEnum.Speech:
                                foundSpeechText = true;
                                break;
                            case OutputTypeEnum.Timecode:
                                foundTimeCode = true;
                                break;
                            default:
                                // ToDo: Check passthrough columns are connected upstream.
                                break;
                        }
                    }
                    else
                    {
                        this.InternalFireError(string.Format("Output Column {0} has invalid property {1}.", oc.Name, cp.Name));
                        return DTSValidationStatus.VS_ISCORRUPT;
                    }
                }
            }
            if (!foundInputChannel)
            {
                this.InternalFireError(string.Format("Required Output Column {0} is missing.", Utility.OutputChannelColumnName));
                return DTSValidationStatus.VS_ISCORRUPT;
            }
            if (!foundSpeechText)
            {
                this.InternalFireError(string.Format("Required Output Column {0} is missing.", Utility.OutputSpeechColumnName));
                return DTSValidationStatus.VS_ISCORRUPT;
            }
            if (!foundTimeCode)
            {
                this.InternalFireError(string.Format("Required Output Column {0} is missing.", Utility.OutputTimecodeColumnName));
                return DTSValidationStatus.VS_ISCORRUPT;
            }



            return base.Validate();
        }


        #endregion

        #region ReinitializeMetaData

        /// <summary>
        /// Called when VS_NEEDSNEWMETADATA is returned from Validate. 
        /// Reset all of the output columns.
        /// </summary>
        public override void ReinitializeMetaData()
        {
            if (ComponentMetaData.InputCollection.Count == 0)
            {
                base.ProvideComponentProperties();
            }

            if (ComponentMetaData.OutputCollection.Count == 0)
            {
                base.ProvideComponentProperties();
            }

            base.ReinitializeMetaData();
        }

        #endregion

        #endregion

        #region Design Time
        #endregion

        #region Run Time

        #region PreExecute
        /// <summary>
        /// Sets up all the in memory column information cache and objects that are reused in ProcessInput
        /// ToDo:
        /// Cache input column information
        /// Cache output column information
        /// Setup NAudio objects
        /// Setup SpeechClient objects
        /// </summary>
        public override void PreExecute()
        {
            bool fireAgain = true;
            string DefaultLocale = string.Empty;
            this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Pre-Execute phase is beginning.", string.Empty, 0, ref fireAgain);
            // Load up the custom properties.
            SubscriptionKey = (string)ComponentMetaData.CustomPropertyCollection[Utility.SubscriptionKeyPropName].Value;
            OperationMode = (OperationModeEnum)ComponentMetaData.CustomPropertyCollection[Utility.OperationModePropName].Value;
            Language = (SpeechLanguageEnum)ComponentMetaData.CustomPropertyCollection[Utility.LanguagePropName].Value;
            ChannelSeparation = (ChannelSeparationEnum)ComponentMetaData.CustomPropertyCollection[Utility.ChannelSeparationPropName].Value;
            // ToDo: Implement this!
            string ShortPhaseUrl = (string)ComponentMetaData.CustomPropertyCollection[Utility.ShortPhraseUrlPropName].Value;
            string LongPhaseUrl = (string)ComponentMetaData.CustomPropertyCollection[Utility.LongPhraseUrlPropName].Value;
            switch (OperationMode)
            {
                case OperationModeEnum.ShortDictation:
                    SpeechServiceUri = new Uri(ShortPhaseUrl);
                    break;
                case OperationModeEnum.LongDictation:
                    SpeechServiceUri = new Uri(LongPhaseUrl);
                    break;
                default:
                    SpeechServiceUri = new Uri(LongPhaseUrl);
                    break;
            }

            switch (Language)
            {
                case SpeechLanguageEnum.AmericanEnglish:
                    DefaultLocale = "en-us";
                    break;
                case SpeechLanguageEnum.BritishEnglish:
                    DefaultLocale = "en-gb";
                    break;
                case SpeechLanguageEnum.German:
                    DefaultLocale = "de-de";
                    break;
                case SpeechLanguageEnum.Spanish:
                    DefaultLocale = "es-es";
                    break;
                case SpeechLanguageEnum.French:
                    DefaultLocale = "fr-fr";
                    break;
                case SpeechLanguageEnum.Italian:
                    DefaultLocale = "it-it";
                    break;
                case SpeechLanguageEnum.Mandarin:
                    DefaultLocale = "zh-cn";
                    break;
                default:
                    DefaultLocale = "en-us";
                    break;
            }

            //RM             this.dataClient = SpeechRecognitionServiceFactory.CreateDataClient(
            //RM             this.speechRecognitionMode,
            //RM             DefaultLocale,
            //RM             this.SubscriptionKey);
            //RM             this.dataClient.ShortPhraseUrl = this.ShortPhraseUrl;

            // Event handlers for speech recognition results
            //RM             if (this.speechRecognitionMode == SpeechRecognitionMode.ShortPhrase)
            //RM             {
            //RM             this.dataClient.OnResponseReceived += this.OnDataShortPhraseResponseReceivedHandler;
            //RM         }
            //RM             else
            //RM             {
            //RM             this.dataClient.OnResponseReceived += this.OnDataDictationResponseReceivedHandler;
            //RM         }

            //this.dataClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;
            //RM             this.dataClient.OnConversationError += this.OnConversationErrorHandler;
            base.PreExecute();
        }
        #endregion

        #region PrimeOutput

        /// <summary>
        /// Sets up the output buffer for use within the component
        /// </summary>
        /// <param name="outputs"></param>
        /// <param name="outputIDs"></param>
        /// <param name="buffers"></param>
        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            if (buffers.Length > 0)
            {
                outputBuffer = buffers[0];
            }
            else
            {
                throw new Exception("buffers not configured in PrimeOutput.");
            }
        }
        #endregion

        #region ProcessInput

        /// <summary>
        /// Called at run time when a PipelineBuffer from an upstream component is available to the component to let the component process the incoming rows.
        /// ToDo:
        /// Loop through all the data in the buffer
        /// As each record is read
        ///     Open the audio file
        ///     Split channels (if required)
        ///     For each audio stream
        ///         Look for pauses in audio
        ///             Send PCM data to SpeechClient for each segment of speach (up to pause)
        ///             Send text from SpeechClient to output
        /// Close outputBuffer on buffer.EndOfRowset
        /// </summary>
        /// <param name="inputID">The ID of the input of the component.</param>
        /// <param name="buffer">The PipelineBuffer object.</param>
        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {
            base.ProcessInput(inputID, buffer);
        }
        #endregion

        #region PostExecute
        /// <summary>
        /// Closes down any in memory column caches and objects from PreExecute.
        /// Also lets the 
        /// </summary>
        public override void PostExecute()
        {
            base.PostExecute();
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

        private static void AddShortPhraseUrlProperty(IDTSComponentMetaData100 componentMetaData)
        {
            IDTSCustomProperty100 shortPhraseUrlProperty = componentMetaData.CustomPropertyCollection.New();
            shortPhraseUrlProperty.Name = Utility.ShortPhraseUrlPropName;
            shortPhraseUrlProperty.Description = "Enter the Uri for the Speech API Short Phrase endpoint.";
            shortPhraseUrlProperty.ContainsID = false;
            shortPhraseUrlProperty.EncryptionRequired = false;
            shortPhraseUrlProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NOTIFY;
            shortPhraseUrlProperty.Value = @"wss://speech.platform.bing.com/api/service/recognition";
        }

        private static void AddLongPhraseUrlProperty(IDTSComponentMetaData100 componentMetaData)
        {
            IDTSCustomProperty100 longPhraseUrlProperty = componentMetaData.CustomPropertyCollection.New();
            longPhraseUrlProperty.Name = Utility.LongPhraseUrlPropName;
            longPhraseUrlProperty.Description = "Enter the Uri for the Speech API Long Phrase endpoint.";
            longPhraseUrlProperty.ContainsID = false;
            longPhraseUrlProperty.EncryptionRequired = false;
            longPhraseUrlProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NOTIFY;
            longPhraseUrlProperty.Value = @"wss://speech.platform.bing.com/api/service/recognition/continuous";
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

        #region InternalFireError
        /// <summary>
        /// Sticks an Error message out to the Error Log.
        /// </summary>
        /// <param name="message">The error message to fire</param>
        private void InternalFireError(string message)
        {
            ComponentMetaData.FireError(0, ComponentMetaData.Name, message, string.Empty, 0, out this.cancelEvent);
        }
        #endregion


//#if asdfasdf
#region Speech Client Interaction



#endregion
//#endif
    }
}
