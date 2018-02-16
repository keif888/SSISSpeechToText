using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Martin.SQLServer.Dts
{
    public partial class SpeechToTextForm : Form
    {
        public SpeechToTextForm()
        {
            InitializeComponent();
        }


        private void SpeechToTextForm_Load(object sender, EventArgs e)
        {
            RTBAbout.SelectAll();
            RTBAbout.SelectedRtf = @"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3081\deflangfe3081{\fonttbl{\f0\fswiss\fprq2\fcharset0 Calibri;}}
{\*\generator Riched20 10.0.16299}
            {\*\mmathPr\mnaryLim0\mdispDef1\mwrapIndent1440 }\viewkind4\uc1
\pard\widctlpar\sa160\sl252\slmult1\b\f0\fs24 Speech to Text\par
\b0\fs22\'a9 Keith Martin 2018\par
Provides the ability to read recorded sound files from the file system, and send them to Microsoft Cognitive Services, Bing Speech API.\par
If you have stereo recordings, with one channel per person, then these can be processed separately, with timecodes added to indicate the start time of each persons speech.\par
}";
        }
    }
}
