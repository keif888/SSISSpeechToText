namespace Martin.SQLServer.Dts
{
    partial class SpeechToTextForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpeechToTextForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TBSubscriptionKey = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CBChannelSeparation = new System.Windows.Forms.ComboBox();
            this.CBLanguage = new System.Windows.Forms.ComboBox();
            this.CBOperationMode = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CBFileNames = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.TBChannel = new System.Windows.Forms.TextBox();
            this.RTBAbout = new System.Windows.Forms.RichTextBox();
            this.BTNCancel = new System.Windows.Forms.Button();
            this.BTNOk = new System.Windows.Forms.Button();
            this.ToolTipSpeechToText = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subscription Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Operation Mode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Language";
            // 
            // TBSubscriptionKey
            // 
            this.TBSubscriptionKey.Location = new System.Drawing.Point(112, 19);
            this.TBSubscriptionKey.Name = "TBSubscriptionKey";
            this.TBSubscriptionKey.Size = new System.Drawing.Size(178, 20);
            this.TBSubscriptionKey.TabIndex = 2;
            this.ToolTipSpeechToText.SetToolTip(this.TBSubscriptionKey, "Enter your subscription key for \r\nAzure Cognitive Services\r\nBing Speech API");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.CBChannelSeparation);
            this.groupBox1.Controls.Add(this.CBLanguage);
            this.groupBox1.Controls.Add(this.CBOperationMode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TBSubscriptionKey);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 164);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Speech Recognition Settings";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Channel Separation";
            // 
            // CBChannelSeparation
            // 
            this.CBChannelSeparation.FormattingEnabled = true;
            this.CBChannelSeparation.Items.AddRange(new object[] {
            "Mono/Ignore",
            "Separate Left and Right"});
            this.CBChannelSeparation.Location = new System.Drawing.Point(112, 102);
            this.CBChannelSeparation.Name = "CBChannelSeparation";
            this.CBChannelSeparation.Size = new System.Drawing.Size(178, 21);
            this.CBChannelSeparation.TabIndex = 6;
            this.ToolTipSpeechToText.SetToolTip(this.CBChannelSeparation, "If you have stereo input, the decide if you want each channel processed separatel" +
        "y");
            // 
            // CBLanguage
            // 
            this.CBLanguage.FormattingEnabled = true;
            this.CBLanguage.Items.AddRange(new object[] {
            "American English",
            "British English",
            "German",
            "Spanish",
            "French",
            "Italian",
            "Mandarin"});
            this.CBLanguage.Location = new System.Drawing.Point(112, 74);
            this.CBLanguage.Name = "CBLanguage";
            this.CBLanguage.Size = new System.Drawing.Size(178, 21);
            this.CBLanguage.TabIndex = 5;
            this.ToolTipSpeechToText.SetToolTip(this.CBLanguage, "Select the language that is used in the speech");
            // 
            // CBOperationMode
            // 
            this.CBOperationMode.FormattingEnabled = true;
            this.CBOperationMode.Items.AddRange(new object[] {
            "Short Dictation",
            "Long Dictation"});
            this.CBOperationMode.Location = new System.Drawing.Point(112, 46);
            this.CBOperationMode.Name = "CBOperationMode";
            this.CBOperationMode.Size = new System.Drawing.Size(178, 21);
            this.CBOperationMode.TabIndex = 4;
            this.ToolTipSpeechToText.SetToolTip(this.CBOperationMode, "Select the dictation mode that you wish to use.");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CBFileNames);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(318, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 164);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input Settings";
            // 
            // CBFileNames
            // 
            this.CBFileNames.FormattingEnabled = true;
            this.CBFileNames.Location = new System.Drawing.Point(73, 13);
            this.CBFileNames.Name = "CBFileNames";
            this.CBFileNames.Size = new System.Drawing.Size(178, 21);
            this.CBFileNames.TabIndex = 1;
            this.ToolTipSpeechToText.SetToolTip(this.CBFileNames, "Select the column that has the speech recording file names to process");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "File Names";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.TBChannel);
            this.groupBox3.Location = new System.Drawing.Point(584, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(262, 163);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output Column Names";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Timecodes";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Speech";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Channel";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(78, 71);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(178, 20);
            this.textBox4.TabIndex = 5;
            this.ToolTipSpeechToText.SetToolTip(this.textBox4, "Enter the column name for the time codes.\r\nUsed when channel separation is active" +
        ".");
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(78, 45);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(178, 20);
            this.textBox3.TabIndex = 4;
            this.ToolTipSpeechToText.SetToolTip(this.textBox3, "Enter the name of the column that will have all the text returned");
            // 
            // TBChannel
            // 
            this.TBChannel.Location = new System.Drawing.Point(78, 19);
            this.TBChannel.Name = "TBChannel";
            this.TBChannel.Size = new System.Drawing.Size(178, 20);
            this.TBChannel.TabIndex = 3;
            this.ToolTipSpeechToText.SetToolTip(this.TBChannel, "Enter the name of the output column that has the channel name in it");
            // 
            // RTBAbout
            // 
            this.RTBAbout.Enabled = false;
            this.RTBAbout.Location = new System.Drawing.Point(13, 183);
            this.RTBAbout.Name = "RTBAbout";
            this.RTBAbout.Size = new System.Drawing.Size(751, 174);
            this.RTBAbout.TabIndex = 8;
            this.RTBAbout.Text = resources.GetString("RTBAbout.Text");
            // 
            // BTNCancel
            // 
            this.BTNCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTNCancel.Location = new System.Drawing.Point(771, 334);
            this.BTNCancel.Name = "BTNCancel";
            this.BTNCancel.Size = new System.Drawing.Size(75, 23);
            this.BTNCancel.TabIndex = 9;
            this.BTNCancel.Text = "Cancel";
            this.BTNCancel.UseVisualStyleBackColor = true;
            // 
            // BTNOk
            // 
            this.BTNOk.Location = new System.Drawing.Point(771, 305);
            this.BTNOk.Name = "BTNOk";
            this.BTNOk.Size = new System.Drawing.Size(75, 23);
            this.BTNOk.TabIndex = 10;
            this.BTNOk.Text = "Ok";
            this.BTNOk.UseVisualStyleBackColor = true;
            // 
            // SpeechToTextForm
            // 
            this.AcceptButton = this.BTNOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BTNCancel;
            this.ClientSize = new System.Drawing.Size(858, 369);
            this.Controls.Add(this.BTNOk);
            this.Controls.Add(this.BTNCancel);
            this.Controls.Add(this.RTBAbout);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(874, 408);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(874, 408);
            this.Name = "SpeechToTextForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Speech To Text";
            this.Load += new System.EventHandler(this.SpeechToTextForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBSubscriptionKey;
        private System.Windows.Forms.ToolTip ToolTipSpeechToText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CBChannelSeparation;
        private System.Windows.Forms.ComboBox CBLanguage;
        private System.Windows.Forms.ComboBox CBOperationMode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox CBFileNames;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox TBChannel;
        private System.Windows.Forms.RichTextBox RTBAbout;
        private System.Windows.Forms.Button BTNCancel;
        private System.Windows.Forms.Button BTNOk;
    }
}