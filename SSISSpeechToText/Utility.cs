using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Pipeline;


namespace Martin.SQLServer.Dts
{
    public sealed class Utility
    {

        private const string ConstOutputTypePropName = "OutputColumnType";
        /// <summary>
        /// Gets the output columns output type property name
        /// </summary>
        public static string OutputColumnOutputTypePropName
        {
            get
            {
                return ConstOutputTypePropName;
            }
        }

        private const string ConstOutputChannelColumnName = "InputChannel";
        public static string OutputChannelColumnName
        { get
            {
                return ConstOutputChannelColumnName;
            }
        }

        private const string ConstOutputSpeechColumnName = "SpeechTextResult";
        public static string OutputSpeechColumnName
        {
            get
            {
                return ConstOutputSpeechColumnName;
            }
        }

        private const string ConstOutputTimecodeColumnName = "Timecode";
        public static string OutputTimecodeColumnName
        {
            get
            {
                return ConstOutputTimecodeColumnName;
            }
        }

        private const string ConstSubscriptionKeyPropName = "SubscriptionKey";
        public static string SubscriptionKeyPropName
        {
            get
            {
                return ConstSubscriptionKeyPropName;
            }
        }

        private const string ConstOperationModePropName = "OperationMode";
        public static string OperationModePropName
        {
            get
            {
                return ConstOperationModePropName;
            }
        }

        private const string ConstLanguagePropName = "APILanguage";
        public static string LanguagePropName
        {
            get
            {
                return ConstLanguagePropName;
            }
        }

        private const string ConstChannelSeparationPropName = "ChannelSeparation";
        public static string ChannelSeparationPropName
        {
            get
            {
                return ConstChannelSeparationPropName;
            }
        }
    }
}
