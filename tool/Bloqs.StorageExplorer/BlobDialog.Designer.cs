namespace Bloqs.StorageExplorer
{
    partial class BlobDialog
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
            this.labelFile = new System.Windows.Forms.Label();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.labelCacheControl = new System.Windows.Forms.Label();
            this.textBoxCacheControl = new System.Windows.Forms.TextBox();
            this.labelContentDisposition = new System.Windows.Forms.Label();
            this.textBoxContentDisposition = new System.Windows.Forms.TextBox();
            this.labelContentEncoding = new System.Windows.Forms.Label();
            this.textBoxContentEncoding = new System.Windows.Forms.TextBox();
            this.labelContentLanguage = new System.Windows.Forms.Label();
            this.textBoxContentLanguage = new System.Windows.Forms.TextBox();
            this.labelContentMD5 = new System.Windows.Forms.Label();
            this.textBoxContentMD5 = new System.Windows.Forms.TextBox();
            this.labelContentType = new System.Windows.Forms.Label();
            this.textBoxContentType = new System.Windows.Forms.TextBox();
            this.textBoxETag = new System.Windows.Forms.TextBox();
            this.labelETag = new System.Windows.Forms.Label();
            this.labelLastModified = new System.Windows.Forms.Label();
            this.textBoxLastModified = new System.Windows.Forms.TextBox();
            this.dataGridViewMetadata = new System.Windows.Forms.DataGridView();
            this.ColumnKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelMetadata = new System.Windows.Forms.Label();
            this.groupBoxProperties = new System.Windows.Forms.GroupBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMetadata)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(13, 9);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(27, 15);
            this.labelFile.TabIndex = 0;
            this.labelFile.Text = "File";
            // 
            // textBoxFile
            // 
            this.textBoxFile.Location = new System.Drawing.Point(16, 27);
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.ReadOnly = true;
            this.textBoxFile.Size = new System.Drawing.Size(315, 21);
            this.textBoxFile.TabIndex = 1;
            // 
            // labelCacheControl
            // 
            this.labelCacheControl.AutoSize = true;
            this.labelCacheControl.Location = new System.Drawing.Point(53, 84);
            this.labelCacheControl.Name = "labelCacheControl";
            this.labelCacheControl.Size = new System.Drawing.Size(84, 15);
            this.labelCacheControl.TabIndex = 0;
            this.labelCacheControl.Text = "Cache Control";
            // 
            // textBoxCacheControl
            // 
            this.textBoxCacheControl.Location = new System.Drawing.Point(139, 81);
            this.textBoxCacheControl.Name = "textBoxCacheControl";
            this.textBoxCacheControl.ReadOnly = true;
            this.textBoxCacheControl.Size = new System.Drawing.Size(180, 21);
            this.textBoxCacheControl.TabIndex = 1;
            // 
            // labelContentDisposition
            // 
            this.labelContentDisposition.AutoSize = true;
            this.labelContentDisposition.Location = new System.Drawing.Point(24, 111);
            this.labelContentDisposition.Name = "labelContentDisposition";
            this.labelContentDisposition.Size = new System.Drawing.Size(113, 15);
            this.labelContentDisposition.TabIndex = 0;
            this.labelContentDisposition.Text = "Content Disposition";
            // 
            // textBoxContentDisposition
            // 
            this.textBoxContentDisposition.Location = new System.Drawing.Point(139, 108);
            this.textBoxContentDisposition.Name = "textBoxContentDisposition";
            this.textBoxContentDisposition.ReadOnly = true;
            this.textBoxContentDisposition.Size = new System.Drawing.Size(180, 21);
            this.textBoxContentDisposition.TabIndex = 1;
            // 
            // labelContentEncoding
            // 
            this.labelContentEncoding.AutoSize = true;
            this.labelContentEncoding.Location = new System.Drawing.Point(33, 138);
            this.labelContentEncoding.Name = "labelContentEncoding";
            this.labelContentEncoding.Size = new System.Drawing.Size(104, 15);
            this.labelContentEncoding.TabIndex = 0;
            this.labelContentEncoding.Text = "Content Encoding";
            // 
            // textBoxContentEncoding
            // 
            this.textBoxContentEncoding.Location = new System.Drawing.Point(139, 135);
            this.textBoxContentEncoding.Name = "textBoxContentEncoding";
            this.textBoxContentEncoding.ReadOnly = true;
            this.textBoxContentEncoding.Size = new System.Drawing.Size(180, 21);
            this.textBoxContentEncoding.TabIndex = 1;
            // 
            // labelContentLanguage
            // 
            this.labelContentLanguage.AutoSize = true;
            this.labelContentLanguage.Location = new System.Drawing.Point(29, 165);
            this.labelContentLanguage.Name = "labelContentLanguage";
            this.labelContentLanguage.Size = new System.Drawing.Size(108, 15);
            this.labelContentLanguage.TabIndex = 0;
            this.labelContentLanguage.Text = "Content Language";
            // 
            // textBoxContentLanguage
            // 
            this.textBoxContentLanguage.Location = new System.Drawing.Point(139, 162);
            this.textBoxContentLanguage.Name = "textBoxContentLanguage";
            this.textBoxContentLanguage.ReadOnly = true;
            this.textBoxContentLanguage.Size = new System.Drawing.Size(180, 21);
            this.textBoxContentLanguage.TabIndex = 1;
            // 
            // labelContentMD5
            // 
            this.labelContentMD5.AutoSize = true;
            this.labelContentMD5.Location = new System.Drawing.Point(58, 192);
            this.labelContentMD5.Name = "labelContentMD5";
            this.labelContentMD5.Size = new System.Drawing.Size(79, 15);
            this.labelContentMD5.TabIndex = 0;
            this.labelContentMD5.Text = "Content MD5";
            // 
            // textBoxContentMD5
            // 
            this.textBoxContentMD5.Location = new System.Drawing.Point(139, 189);
            this.textBoxContentMD5.Name = "textBoxContentMD5";
            this.textBoxContentMD5.ReadOnly = true;
            this.textBoxContentMD5.Size = new System.Drawing.Size(180, 21);
            this.textBoxContentMD5.TabIndex = 1;
            // 
            // labelContentType
            // 
            this.labelContentType.AutoSize = true;
            this.labelContentType.Location = new System.Drawing.Point(59, 219);
            this.labelContentType.Name = "labelContentType";
            this.labelContentType.Size = new System.Drawing.Size(78, 15);
            this.labelContentType.TabIndex = 0;
            this.labelContentType.Text = "Content Type";
            // 
            // textBoxContentType
            // 
            this.textBoxContentType.Location = new System.Drawing.Point(139, 216);
            this.textBoxContentType.Name = "textBoxContentType";
            this.textBoxContentType.ReadOnly = true;
            this.textBoxContentType.Size = new System.Drawing.Size(180, 21);
            this.textBoxContentType.TabIndex = 1;
            // 
            // textBoxETag
            // 
            this.textBoxETag.Location = new System.Drawing.Point(139, 243);
            this.textBoxETag.Name = "textBoxETag";
            this.textBoxETag.ReadOnly = true;
            this.textBoxETag.Size = new System.Drawing.Size(180, 21);
            this.textBoxETag.TabIndex = 4;
            // 
            // labelETag
            // 
            this.labelETag.AutoSize = true;
            this.labelETag.Location = new System.Drawing.Point(101, 246);
            this.labelETag.Name = "labelETag";
            this.labelETag.Size = new System.Drawing.Size(36, 15);
            this.labelETag.TabIndex = 3;
            this.labelETag.Text = "ETag";
            // 
            // labelLastModified
            // 
            this.labelLastModified.AutoSize = true;
            this.labelLastModified.Location = new System.Drawing.Point(56, 273);
            this.labelLastModified.Name = "labelLastModified";
            this.labelLastModified.Size = new System.Drawing.Size(81, 15);
            this.labelLastModified.TabIndex = 3;
            this.labelLastModified.Text = "Last Modified";
            // 
            // textBoxLastModified
            // 
            this.textBoxLastModified.Location = new System.Drawing.Point(139, 270);
            this.textBoxLastModified.Name = "textBoxLastModified";
            this.textBoxLastModified.ReadOnly = true;
            this.textBoxLastModified.Size = new System.Drawing.Size(180, 21);
            this.textBoxLastModified.TabIndex = 4;
            // 
            // dataGridViewMetadata
            // 
            this.dataGridViewMetadata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewMetadata.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnKey,
            this.ColumnValue});
            this.dataGridViewMetadata.Location = new System.Drawing.Point(15, 332);
            this.dataGridViewMetadata.Name = "dataGridViewMetadata";
            this.dataGridViewMetadata.RowTemplate.Height = 21;
            this.dataGridViewMetadata.Size = new System.Drawing.Size(316, 149);
            this.dataGridViewMetadata.TabIndex = 6;
            // 
            // ColumnKey
            // 
            this.ColumnKey.DataPropertyName = "Key";
            this.ColumnKey.HeaderText = "Key";
            this.ColumnKey.MaxInputLength = 256;
            this.ColumnKey.Name = "ColumnKey";
            this.ColumnKey.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnKey.Width = 128;
            // 
            // ColumnValue
            // 
            this.ColumnValue.DataPropertyName = "Value";
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.MaxInputLength = 256;
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnValue.Width = 128;
            // 
            // labelMetadata
            // 
            this.labelMetadata.AutoSize = true;
            this.labelMetadata.Location = new System.Drawing.Point(13, 314);
            this.labelMetadata.Name = "labelMetadata";
            this.labelMetadata.Size = new System.Drawing.Size(59, 15);
            this.labelMetadata.TabIndex = 7;
            this.labelMetadata.Text = "Metadata";
            // 
            // groupBoxProperties
            // 
            this.groupBoxProperties.Location = new System.Drawing.Point(15, 54);
            this.groupBoxProperties.Name = "groupBoxProperties";
            this.groupBoxProperties.Size = new System.Drawing.Size(317, 255);
            this.groupBoxProperties.TabIndex = 8;
            this.groupBoxProperties.TabStop = false;
            this.groupBoxProperties.Text = "Properties";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(257, 487);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 22);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Close";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // BlobDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 521);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelMetadata);
            this.Controls.Add(this.dataGridViewMetadata);
            this.Controls.Add(this.textBoxLastModified);
            this.Controls.Add(this.labelLastModified);
            this.Controls.Add(this.textBoxETag);
            this.Controls.Add(this.labelETag);
            this.Controls.Add(this.textBoxContentType);
            this.Controls.Add(this.textBoxContentMD5);
            this.Controls.Add(this.textBoxContentLanguage);
            this.Controls.Add(this.textBoxContentEncoding);
            this.Controls.Add(this.textBoxContentDisposition);
            this.Controls.Add(this.textBoxCacheControl);
            this.Controls.Add(this.textBoxFile);
            this.Controls.Add(this.labelContentType);
            this.Controls.Add(this.labelContentMD5);
            this.Controls.Add(this.labelContentLanguage);
            this.Controls.Add(this.labelContentEncoding);
            this.Controls.Add(this.labelContentDisposition);
            this.Controls.Add(this.labelCacheControl);
            this.Controls.Add(this.labelFile);
            this.Controls.Add(this.groupBoxProperties);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlobDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BlobUploadDialog";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMetadata)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.Label labelCacheControl;
        private System.Windows.Forms.TextBox textBoxCacheControl;
        private System.Windows.Forms.Label labelContentDisposition;
        private System.Windows.Forms.TextBox textBoxContentDisposition;
        private System.Windows.Forms.Label labelContentEncoding;
        private System.Windows.Forms.TextBox textBoxContentEncoding;
        private System.Windows.Forms.Label labelContentLanguage;
        private System.Windows.Forms.TextBox textBoxContentLanguage;
        private System.Windows.Forms.Label labelContentMD5;
        private System.Windows.Forms.TextBox textBoxContentMD5;
        private System.Windows.Forms.Label labelContentType;
        private System.Windows.Forms.TextBox textBoxContentType;
        private System.Windows.Forms.TextBox textBoxETag;
        private System.Windows.Forms.Label labelETag;
        private System.Windows.Forms.Label labelLastModified;
        private System.Windows.Forms.TextBox textBoxLastModified;
        private System.Windows.Forms.DataGridView dataGridViewMetadata;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.Label labelMetadata;
        private System.Windows.Forms.GroupBox groupBoxProperties;
        private System.Windows.Forms.Button buttonCancel;
    }
}