using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System.Windows.Forms;

namespace Martin.SQLServer.Dts
{
    class SSISSpeechToTextUI : DataFlowComponentUI
    {
        #region Virtual methods

        /// <summary>
        /// Implementation of the method resposible for displaying the form.
        /// This one is abstract in the base class.
        /// </summary>
        /// <param name="parentControl">The owner window</param>
        /// <returns>true when the form is shown ok</returns>
        protected override bool EditImpl(IWin32Window parentControl)
        {
            using (SpeechToTextForm form = new SpeechToTextForm())
            {
                this.HookupEvents(form);

                if (form.ShowDialog(parentControl) == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
        /// <summary>
        /// Communication with UI form goes through these events. The UI will raise events when some data/action
        /// is needed and this class is responsible to answer those requests. This way we create separation between UI 
        /// specific logic and interactions with SSIS data flow object model.
        /// There are n required events:
        /// GetInputColumns - Retrieves all the input columns from the SSIS data flow object
        /// </summary>
        /// <param name="form">The form that called this</param>
        private void HookupEvents(SpeechToTextForm form)
        {
            //form.GetInputColumns += new GetInputColumnsEventHandler(this.form_GetInputColumns);
            //form.SetInputColumn += new ChangeInputColumnEventHandler(this.form_SetInputColumn);
        }

    }
}
