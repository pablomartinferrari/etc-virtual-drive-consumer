namespace ETCStorageHelper.WinFormsDemo
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpEnvironment = new System.Windows.Forms.GroupBox();
            this.rbCommercial = new System.Windows.Forms.RadioButton();
            this.rbGCCHigh = new System.Windows.Forms.RadioButton();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grpOperations = new System.Windows.Forms.GroupBox();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.lblFolderPath = new System.Windows.Forms.Label();
            this.btnCreateDirectory = new System.Windows.Forms.Button();
            this.btnGetDirectoryUrl = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtFileContent = new System.Windows.Forms.TextBox();
            this.lblFileContent = new System.Windows.Forms.Label();
            this.btnWriteFile = new System.Windows.Forms.Button();
            this.btnReadFile = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.btnClearOutput = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.grpEnvironment.SuspendLayout();
            this.grpOperations.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpEnvironment
            // 
            this.grpEnvironment.Controls.Add(this.rbCommercial);
            this.grpEnvironment.Controls.Add(this.rbGCCHigh);
            this.grpEnvironment.Location = new System.Drawing.Point(12, 12);
            this.grpEnvironment.Name = "grpEnvironment";
            this.grpEnvironment.Size = new System.Drawing.Size(560, 60);
            this.grpEnvironment.TabIndex = 0;
            this.grpEnvironment.TabStop = false;
            this.grpEnvironment.Text = "Select Cloud Environment";
            // 
            // rbCommercial
            // 
            this.rbCommercial.AutoSize = true;
            this.rbCommercial.Location = new System.Drawing.Point(200, 25);
            this.rbCommercial.Name = "rbCommercial";
            this.rbCommercial.Size = new System.Drawing.Size(220, 21);
            this.rbCommercial.TabIndex = 1;
            this.rbCommercial.Text = "Commercial (graph.microsoft.com)";
            this.rbCommercial.UseVisualStyleBackColor = true;
            this.rbCommercial.CheckedChanged += new System.EventHandler(this.Environment_CheckedChanged);
            // 
            // rbGCCHigh
            // 
            this.rbGCCHigh.AutoSize = true;
            this.rbGCCHigh.Checked = true;
            this.rbGCCHigh.Location = new System.Drawing.Point(20, 25);
            this.rbGCCHigh.Name = "rbGCCHigh";
            this.rbGCCHigh.Size = new System.Drawing.Size(174, 21);
            this.rbGCCHigh.TabIndex = 0;
            this.rbGCCHigh.TabStop = true;
            this.rbGCCHigh.Text = "GCC High (graph.microsoft.us)";
            this.rbGCCHigh.UseVisualStyleBackColor = true;
            this.rbGCCHigh.CheckedChanged += new System.EventHandler(this.Environment_CheckedChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblStatus.Location = new System.Drawing.Point(12, 80);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(200, 17);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Environment: GCC High";
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(450, 78);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(122, 25);
            this.btnTestConnection.TabIndex = 6;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // grpOperations
            // 
            this.grpOperations.Controls.Add(this.txtFolderPath);
            this.grpOperations.Controls.Add(this.lblFolderPath);
            this.grpOperations.Controls.Add(this.btnCreateDirectory);
            this.grpOperations.Controls.Add(this.btnGetDirectoryUrl);
            this.grpOperations.Controls.Add(this.txtFileName);
            this.grpOperations.Controls.Add(this.lblFileName);
            this.grpOperations.Controls.Add(this.txtFileContent);
            this.grpOperations.Controls.Add(this.lblFileContent);
            this.grpOperations.Controls.Add(this.btnWriteFile);
            this.grpOperations.Controls.Add(this.btnReadFile);
            this.grpOperations.Location = new System.Drawing.Point(12, 105);
            this.grpOperations.Name = "grpOperations";
            this.grpOperations.Size = new System.Drawing.Size(560, 200);
            this.grpOperations.TabIndex = 2;
            this.grpOperations.TabStop = false;
            this.grpOperations.Text = "SharePoint Operations";
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Location = new System.Drawing.Point(110, 25);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(300, 22);
            this.txtFolderPath.TabIndex = 0;
            this.txtFolderPath.Text = "WinFormsDemo/TestFolder";
            // 
            // lblFolderPath
            // 
            this.lblFolderPath.AutoSize = true;
            this.lblFolderPath.Location = new System.Drawing.Point(20, 28);
            this.lblFolderPath.Name = "lblFolderPath";
            this.lblFolderPath.Size = new System.Drawing.Size(80, 17);
            this.lblFolderPath.TabIndex = 1;
            this.lblFolderPath.Text = "Folder Path:";
            // 
            // btnCreateDirectory
            // 
            this.btnCreateDirectory.Location = new System.Drawing.Point(420, 23);
            this.btnCreateDirectory.Name = "btnCreateDirectory";
            this.btnCreateDirectory.Size = new System.Drawing.Size(120, 28);
            this.btnCreateDirectory.TabIndex = 2;
            this.btnCreateDirectory.Text = "Create Directory";
            this.btnCreateDirectory.UseVisualStyleBackColor = true;
            this.btnCreateDirectory.Click += new System.EventHandler(this.btnCreateDirectory_Click);
            // 
            // btnGetDirectoryUrl
            // 
            this.btnGetDirectoryUrl.Location = new System.Drawing.Point(420, 57);
            this.btnGetDirectoryUrl.Name = "btnGetDirectoryUrl";
            this.btnGetDirectoryUrl.Size = new System.Drawing.Size(120, 28);
            this.btnGetDirectoryUrl.TabIndex = 3;
            this.btnGetDirectoryUrl.Text = "Get Directory URL";
            this.btnGetDirectoryUrl.UseVisualStyleBackColor = true;
            this.btnGetDirectoryUrl.Click += new System.EventHandler(this.btnGetDirectoryUrl_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(110, 95);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(300, 22);
            this.txtFileName.TabIndex = 4;
            this.txtFileName.Text = "WinFormsDemo/TestFolder/test-file.txt";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(20, 98);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(66, 17);
            this.lblFileName.TabIndex = 5;
            this.lblFileName.Text = "File Path:";
            // 
            // txtFileContent
            // 
            this.txtFileContent.Location = new System.Drawing.Point(110, 125);
            this.txtFileContent.Multiline = true;
            this.txtFileContent.Name = "txtFileContent";
            this.txtFileContent.Size = new System.Drawing.Size(300, 60);
            this.txtFileContent.TabIndex = 6;
            this.txtFileContent.Text = "Hello from WinForms Demo!\r\nWritten at: {timestamp}";
            // 
            // lblFileContent
            // 
            this.lblFileContent.AutoSize = true;
            this.lblFileContent.Location = new System.Drawing.Point(20, 128);
            this.lblFileContent.Name = "lblFileContent";
            this.lblFileContent.Size = new System.Drawing.Size(85, 17);
            this.lblFileContent.TabIndex = 7;
            this.lblFileContent.Text = "File Content:";
            // 
            // btnWriteFile
            // 
            this.btnWriteFile.Location = new System.Drawing.Point(420, 93);
            this.btnWriteFile.Name = "btnWriteFile";
            this.btnWriteFile.Size = new System.Drawing.Size(120, 28);
            this.btnWriteFile.TabIndex = 8;
            this.btnWriteFile.Text = "Write File";
            this.btnWriteFile.UseVisualStyleBackColor = true;
            this.btnWriteFile.Click += new System.EventHandler(this.btnWriteFile_Click);
            // 
            // btnReadFile
            // 
            this.btnReadFile.Location = new System.Drawing.Point(420, 127);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(120, 28);
            this.btnReadFile.TabIndex = 9;
            this.btnReadFile.Text = "Read File";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.BackColor = System.Drawing.Color.Black;
            this.txtOutput.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtOutput.ForeColor = System.Drawing.Color.LightGreen;
            this.txtOutput.Location = new System.Drawing.Point(12, 330);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(560, 200);
            this.txtOutput.TabIndex = 3;
            this.txtOutput.WordWrap = false;
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(12, 310);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(55, 17);
            this.lblOutput.TabIndex = 4;
            this.lblOutput.Text = "Output:";
            // 
            // btnClearOutput
            // 
            this.btnClearOutput.Location = new System.Drawing.Point(492, 305);
            this.btnClearOutput.Name = "btnClearOutput";
            this.btnClearOutput.Size = new System.Drawing.Size(80, 23);
            this.btnClearOutput.TabIndex = 5;
            this.btnClearOutput.Text = "Clear";
            this.btnClearOutput.UseVisualStyleBackColor = true;
            this.btnClearOutput.Click += new System.EventHandler(this.btnClearOutput_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 542);
            this.Controls.Add(this.btnClearOutput);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.grpOperations);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.grpEnvironment);
            this.MinimumSize = new System.Drawing.Size(600, 580);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ETC Storage Helper - WinForms Demo (GCC High / Commercial)";
            this.grpEnvironment.ResumeLayout(false);
            this.grpEnvironment.PerformLayout();
            this.grpOperations.ResumeLayout(false);
            this.grpOperations.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpEnvironment;
        private System.Windows.Forms.RadioButton rbCommercial;
        private System.Windows.Forms.RadioButton rbGCCHigh;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpOperations;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Label lblFolderPath;
        private System.Windows.Forms.Button btnCreateDirectory;
        private System.Windows.Forms.Button btnGetDirectoryUrl;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtFileContent;
        private System.Windows.Forms.Label lblFileContent;
        private System.Windows.Forms.Button btnWriteFile;
        private System.Windows.Forms.Button btnReadFile;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Button btnClearOutput;
        private System.Windows.Forms.Button btnTestConnection;
    }
}
