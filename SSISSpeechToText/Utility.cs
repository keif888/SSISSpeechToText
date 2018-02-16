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

        internal const string ConstOutputTypePropName = "OutputColumnType";
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

        internal const string ConstOutputChannelColumnName = "InputChannel";
        public static string OutputChannelColumnName
        { get
            {
                return ConstOutputChannelColumnName;
            }
        }

        internal const string ConstOutputSpeechColumnName = "SpeechTextResult";
        public static string OutputSpeechColumnName
        {
            get
            {
                return ConstOutputSpeechColumnName;
            }
        }

        internal const string ConstOutputTimecodeColumnName = "Timecode";
        public static string OutputTimecodeColumnName
        {
            get
            {
                return ConstOutputTimecodeColumnName;
            }
        }

        internal const string ConstSubscriptionKeyPropName = "SubscriptionKey";
        public static string SubscriptionKeyPropName
        {
            get
            {
                return ConstSubscriptionKeyPropName;
            }
        }

        internal const string ConstOperationModePropName = "OperationMode";
        public static string OperationModePropName
        {
            get
            {
                return ConstOperationModePropName;
            }
        }

        internal const string ConstLanguagePropName = "APILanguage";
        public static string LanguagePropName
        {
            get
            {
                return ConstLanguagePropName;
            }
        }

        internal const string ConstChannelSeparationPropName = "ChannelSeparation";
        public static string ChannelSeparationPropName
        {
            get
            {
                return ConstChannelSeparationPropName;
            }
        }
    }
}
