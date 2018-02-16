﻿using System;
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
        , DisplayName = "SSIS Speach To Text"
        , Description = "Uses Microsoft Cognitive Services to translate speach to text"
        , IconResource = "Martin.SQLServer.Dts.key.ico"
#if SQL2017
        , UITypeName = "Martin.SQLServer.Dts.SSISSpeachToTextUI, SSISSpeachToText, Version=14.0.0.0, Culture=neutral, PublicKeyToken=51c551904274ab44"
#endif
#if SQL2016
        , UITypeName = "Martin.SQLServer.Dts.SSISSpeachToTextUI, SSISSpeachToText, Version=13.0.0.0, Culture=neutral, PublicKeyToken=51c551904274ab44"
#endif
#if SQL2014
        , UITypeName = "Martin.SQLServer.Dts.SSISSpeachToTextUI, SSISSpeachToText, Version=12.0.0.0, Culture=neutral, PublicKeyToken=51c551904274ab44"
#endif
#if SQL2012
        , UITypeName = "Martin.SQLServer.Dts.SSISSpeachToTextUI, SSISSpeachToText, Version=11.0.0.0, Culture=neutral, PublicKeyToken=51c551904274ab44"
#endif
        )]
    public class SSISSpeachToText : PipelineComponent
    {
    }
}