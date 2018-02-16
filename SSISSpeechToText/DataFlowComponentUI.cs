namespace Martin.SQLServer.Dts
{
    using Microsoft.SqlServer.Dts.Design;
    using Microsoft.SqlServer.Dts.Pipeline.Design;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Windows.Forms;

    public abstract class DataFlowComponentUI : IDtsComponentUI
    {
        #region Data members
        // entire communication with the components goes through these three interfaces

        /// <summary>
        /// The meta data related to the component
        /// </summary>
        private IDTSComponentMetaData100 componentMetadata;

        /// <summary>
        /// The design time component
        /// </summary>
        private IDTSDesigntimeComponent100 designtimeComponent;

        /// <summary>
        /// The virtual input from SSIS
        /// </summary>
        private IDTSVirtualInput100 virtualInput;

        // handy design-time services in case we need them

        /// <summary>
        /// The SSIS Service Provide
        /// </summary>
        private IServiceProvider serviceProvider;

        /// <summary>
        /// The SSIS Error collector
        /// </summary>
        private IErrorCollectionService errorCollector;

        // some transforms are dealing with connections and/or variables

        /// <summary>
        /// The connections that this SSIS component has
        /// </summary>
        private Connections connections;

        /// <summary>
        /// The variables that this SSIS component has
        /// </summary>
        private Variables variables;

#endregion

        #region Properties

        /// <summary>
        /// Gets the components metadata
        /// </summary>
        protected IDTSComponentMetaData100 ComponentMetadata
        {
            get
            {
                return this.componentMetadata;
            }
        }

        /// <summary>
        /// Gets the design time component
        /// </summary>
        protected IDTSDesigntimeComponent100 DesigntimeComponent
        {
            get
            {
                return this.designtimeComponent;
            }
        }

        /// <summary>
        /// Gets the virtual input
        /// </summary>
        protected IDTSVirtualInput100 VirtualInput
        {
            get
            {
                return this.virtualInput;
            }
        }

        /// <summary>
        /// Gets the service provider
        /// </summary>
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this.serviceProvider;
            }
        }

        /// <summary>
        /// Gets the connections
        /// </summary>
        protected Connections Connections
        {
            get
            {
                return this.connections;
            }
        }

        /// <summary>
        /// Gets the variables
        /// </summary>
        protected Variables Variables
        {
            get
            {
                return this.variables;
            }
        }

        #endregion
        #region Helper methods

        /// <summary>
        /// Getting tooltip text to be displayed for the given data flow column.
        /// </summary>
        /// <param name="dataflowColumn">The SSIS Virtual Column</param>
        /// <returns>The text to display on the hover window</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public static string GetTooltipString(object dataflowColumn)
        {
            if (dataflowColumn is IDTSVirtualInputColumn100)
            {
                IDTSVirtualInputColumn100 column = dataflowColumn as IDTSVirtualInputColumn100;
                return FormatTooltipText(
                    column.Name,
                    column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentCulture),  // Changed from CurrentUICulture
                    column.Scale.ToString(CultureInfo.CurrentCulture),
                    column.Precision.ToString(CultureInfo.CurrentCulture),
                    column.CodePage.ToString(CultureInfo.CurrentCulture),
                    column.SourceComponent);
            }
            else if (dataflowColumn is IDTSInputColumn100)
            {
                IDTSInputColumn100 column = dataflowColumn as IDTSInputColumn100;
                return FormatTooltipText(
                    column.Name,
                    column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentCulture),
                    column.Scale.ToString(CultureInfo.CurrentCulture),
                    column.Precision.ToString(CultureInfo.CurrentCulture),
                    column.CodePage.ToString(CultureInfo.CurrentCulture));
            }
            else if (dataflowColumn is IDTSOutputColumn100)
            {
                IDTSOutputColumn100 column = dataflowColumn as IDTSOutputColumn100;
                return FormatTooltipText(
                    column.Name,
                    column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentCulture),
                    column.Scale.ToString(CultureInfo.CurrentCulture),
                    column.Precision.ToString(CultureInfo.CurrentCulture),
                    column.CodePage.ToString(CultureInfo.CurrentCulture));
            }
            else if (dataflowColumn is IDTSExternalMetadataColumn100)
            {
                IDTSExternalMetadataColumn100 column = dataflowColumn as IDTSExternalMetadataColumn100;
                return FormatTooltipText(
                    column.Name,
                    column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentCulture),
                    column.Scale.ToString(CultureInfo.CurrentCulture),
                    column.Precision.ToString(CultureInfo.CurrentCulture),
                    column.CodePage.ToString(CultureInfo.CurrentCulture));
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns the text to show in the tooltip
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="dataType">The column datatype</param>
        /// <param name="length">The column length</param>
        /// <param name="scale">The column scale</param>
        /// <param name="precision">The column precision</param>
        /// <param name="codePage">The columns codepage</param>
        /// <param name="sourceComponent">The name of the source</param>
        /// <returns>The formatted tool tip</returns>
        public static string FormatTooltipText(string name, string dataType, string length, string scale, string precision, string codePage, string sourceComponent)
        {
            string tooltip = FormatTooltipText(name, dataType, length, scale, precision, codePage);
            tooltip += "\nSource: " + sourceComponent;

            return tooltip;
        }

        /// <summary>
        /// Returns the text to show in the tooltip
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="dataType">The column datatype</param>
        /// <param name="length">The column length</param>
        /// <param name="scale">The column scale</param>
        /// <param name="precision">The column precision</param>
        /// <param name="codePage">The columns codepage</param>
        /// <returns>The formatted tool tip</returns>
        public static string FormatTooltipText(string name, string dataType, string length, string scale, string precision, string codePage)
        {
            System.Text.StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("Name: ");
            strBuilder.Append(name);
            strBuilder.Append('\n');
            strBuilder.Append("Data type: ");
            strBuilder.Append(dataType);
            strBuilder.Append('\n');
            strBuilder.Append("Length: ");
            strBuilder.Append(length);
            strBuilder.Append('\n');
            strBuilder.Append("Scale: ");
            strBuilder.Append(scale);
            strBuilder.Append('\n');
            strBuilder.Append("Precision: ");
            strBuilder.Append(precision);
            strBuilder.Append('\n');
            strBuilder.Append("Code page: ");
            strBuilder.Append(codePage);

            return strBuilder.ToString();
        }

        #endregion

        #region IDtsComponentUI Members

        /// <summary>
        /// Called before Edit, New and Delete to pass in the necessary parameters.  
        /// </summary>
        /// <param name="dtsComponentMetadata">The components metadata</param>
        /// <param name="serviceProvider">The SSIS service provider</param>
        void IDtsComponentUI.Initialize(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            this.componentMetadata = dtsComponentMetadata;
            this.serviceProvider = serviceProvider;

            Debug.Assert(this.serviceProvider != null, "The service provider was null!");

            this.errorCollector = this.serviceProvider.GetService(
                typeof(IErrorCollectionService)) as IErrorCollectionService;
            Debug.Assert(this.errorCollector != null, "The errorCollector was null!");

            if (this.errorCollector == null)
            {
                Exception ex = new System.ApplicationException("Not all editing services are available.");
                throw ex;
            }
        }

        /// <summary>
        /// Called to invoke the UI. 
        /// </summary>
        /// <param name="parentWindow">The calling window</param>
        /// <param name="variables">The SSIS variables</param>
        /// <param name="connections">The SSIS connections</param>
        /// <returns>True all works</returns>
        bool IDtsComponentUI.Edit(IWin32Window parentWindow, Microsoft.SqlServer.Dts.Runtime.Variables variables, Microsoft.SqlServer.Dts.Runtime.Connections connections)
        {
            this.ClearErrors();

            try
            {
                Debug.Assert(this.componentMetadata != null, "Original Component Metadata is not OK.");

                this.designtimeComponent = this.componentMetadata.Instantiate();

                Debug.Assert(this.designtimeComponent != null, "Design-time component object is not OK.");

                // Cache the virtual input so the available columns are easily accessible.
                this.LoadVirtualInput();

                // Cache variables and connections.
                this.variables = variables;
                this.connections = connections;

                // Here comes the UI that will be invoked in EditImpl virtual method.
                return this.EditImpl(parentWindow);
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
                return false;
            }
        }

        /// <summary>
        /// Called before adding the component to the diagram. 
        /// </summary>
        /// <param name="parentWindow">The calling window</param>
        void IDtsComponentUI.New(IWin32Window parentWindow)
        {
        }

        /// <summary>
        /// Called before deleting the component from the diagram. 
        /// </summary>
        /// <param name="parentWindow">The calling window</param>
        void IDtsComponentUI.Delete(IWin32Window parentWindow)
        {
        }

        /// <summary>
        /// Display the component help
        /// </summary>
        /// <param name="parentWindow">The calling window</param>
        void IDtsComponentUI.Help(IWin32Window parentWindow)
        {
        }

        #endregion

        #region Handling errors
        /// <summary>
        /// Clear the collection of errors collected by handling the pipeline events.
        /// </summary>
        protected void ClearErrors()
        {
            this.errorCollector.ClearErrors();
        }

        /// <summary>
        /// Get the text of error message that consist of all errors captured from pipeline events (OnError and OnWarning). 
        /// </summary>
        /// <returns>The error message</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected string GetErrorMessage()
        {
            return this.errorCollector.GetErrorMessage();
        }

        /// <summary>
        /// Reports errors occurred in the components by retrieving 
        /// error messages reported through pipeline events
        /// </summary>
        /// <param name="ex">passes in the exception to display</param>
        protected void ReportErrors(Exception ex)
        {
            if (this.errorCollector.GetErrors().Count > 0)
            {
                MessageBox.Show(
                    this.errorCollector.GetErrorMessage(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    0);
            }
            else
            {
                if (ex != null)
                {
                    MessageBox.Show(
                        ex.Message + "\r\nSource: " + ex.Source + "\r\n" + ex.TargetSite + "\r\n" + ex.StackTrace,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        0);
                }
                else
                {
                    MessageBox.Show(
                        "Somehow we got an error without an exception",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        0);

                }
            }
        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// Bring up the form by implementing this method in subclasses. 
        /// </summary>
        /// <param name="parentControl">The caller's window id</param>
        /// <returns>True if all ok.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Impl")]
        protected abstract bool EditImpl(IWin32Window parentControl);

        #endregion

        #region Handling virtual inputs

        /// <summary>
        /// Loads all virtual inputs and makes their columns easily accessible.
        /// </summary>
        protected void LoadVirtualInput()
        {
            Debug.Assert(this.componentMetadata != null, "The passed in component metadata was null!");

            IDTSInputCollection100 inputCollection = this.componentMetadata.InputCollection;

            if (inputCollection.Count > 0)
            {
                IDTSInput100 input = inputCollection[0];
                this.virtualInput = input.GetVirtualInput();
            }
        }

        #endregion
    }
}