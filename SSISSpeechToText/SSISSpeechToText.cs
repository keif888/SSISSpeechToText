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
    }
}
