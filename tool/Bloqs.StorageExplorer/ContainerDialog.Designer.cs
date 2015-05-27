namespace Bloqs.StorageExplorer
{
    partial class ContainerDialog
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
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelAccessibility = new System.Windows.Forms.Label();
            this.checkBoxIsPublic = new System.Windows.Forms.CheckBox();
            this.labelMetadata = new System.Windows.Forms.Label();
            this.dataGridViewMetadata = new System.Windows.Forms.DataGridView();
            this.ColumnKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelTitleCreatedDateTimeUtc = new System.Windows.Forms.Label();
            this.labelCreatedDateTimeUtc = new System.Windows.Forms.Label();
            this.labelTitleLastModifiedDateTimeUtc = new System.Windows.Forms.Label();
            this.labelLastModifiedDateTimeUtc = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMetadata)).BeginInit();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(13, 13);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 15);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxName.Location = new System.Drawing.Point(16, 31);
            this.textBoxName.MaxLength = 256;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(316, 21);
            this.textBoxName.TabIndex = 1;
            // 
            // labelAccessibility
            // 
            this.labelAccessibility.AutoSize = true;
            this.labelAccessibility.Location = new System.Drawing.Point(13, 62);
            this.labelAccessibility.Name = "labelAccessibility";
            this.labelAccessibility.Size = new System.Drawing.Size(72, 15);
            this.labelAccessibility.TabIndex = 2;
            this.labelAccessibility.Text = "Accessibility";
            // 
            // checkBoxIsPublic
            // 
            this.checkBoxIsPublic.AutoSize = true;
            this.checkBoxIsPublic.Font = new System.Drawing.Font("Consolas", 9F);
            this.checkBoxIsPublic.Location = new System.Drawing.Point(16, 80);
            this.checkBoxIsPublic.Name = "checkBoxIsPublic";
            this.checkBoxIsPublic.Size = new System.Drawing.Size(264, 18);
            this.checkBoxIsPublic.TabIndex = 3;
            this.checkBoxIsPublic.Text = "Anyone can download blobs in this.";
            this.checkBoxIsPublic.UseVisualStyleBackColor = true;
            // 
            // labelMetadata
            // 
            this.labelMetadata.AutoSize = true;
            this.labelMetadata.Location = new System.Drawing.Point(13, 107);
            this.labelMetadata.Name = "labelMetadata";
            this.labelMetadata.Size = new System.Drawing.Size(59, 15);
            this.labelMetadata.TabIndex = 4;
            this.labelMetadata.Text = "Metadata";
            // 
            // dataGridViewMetadata
            // 
            this.dataGridViewMetadata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewMetadata.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnKey,
            this.ColumnValue});
            this.dataGridViewMetadata.Location = new System.Drawing.Point(16, 125);
            this.dataGridViewMetadata.Name = "dataGridViewMetadata";
            this.dataGridViewMetadata.RowTemplate.Height = 21;
            this.dataGridViewMetadata.Size = new System.Drawing.Size(316, 196);
            this.dataGridViewMetadata.TabIndex = 5;
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
            // labelTitleCreatedDateTimeUtc
            // 
            this.labelTitleCreatedDateTimeUtc.AutoSize = true;
            this.labelTitleCreatedDateTimeUtc.Location = new System.Drawing.Point(13, 334);
            this.labelTitleCreatedDateTimeUtc.Name = "labelTitleCreatedDateTimeUtc";
            this.labelTitleCreatedDateTimeUtc.Size = new System.Drawing.Size(85, 15);
            this.labelTitleCreatedDateTimeUtc.TabIndex = 6;
            this.labelTitleCreatedDateTimeUtc.Text = "Created (UTC)";
            // 
            // labelCreatedDateTimeUtc
            // 
            this.labelCreatedDateTimeUtc.AutoSize = true;
            this.labelCreatedDateTimeUtc.Location = new System.Drawing.Point(16, 349);
            this.labelCreatedDateTimeUtc.Name = "labelCreatedDateTimeUtc";
            this.labelCreatedDateTimeUtc.Size = new System.Drawing.Size(69, 15);
            this.labelCreatedDateTimeUtc.TabIndex = 7;
            this.labelCreatedDateTimeUtc.Text = "yyyy/MM/dd";
            // 
            // labelTitleLastModifiedDateTimeUtc
            // 
            this.labelTitleLastModifiedDateTimeUtc.AutoSize = true;
            this.labelTitleLastModifiedDateTimeUtc.Location = new System.Drawing.Point(13, 373);
            this.labelTitleLastModifiedDateTimeUtc.Name = "labelTitleLastModifiedDateTimeUtc";
            this.labelTitleLastModifiedDateTimeUtc.Size = new System.Drawing.Size(116, 15);
            this.labelTitleLastModifiedDateTimeUtc.TabIndex = 6;
            this.labelTitleLastModifiedDateTimeUtc.Text = "Last Modified (UTC)";
            // 
            // labelLastModifiedDateTimeUtc
            // 
            this.labelLastModifiedDateTimeUtc.AutoSize = true;
            this.labelLastModifiedDateTimeUtc.Location = new System.Drawing.Point(16, 391);
            this.labelLastModifiedDateTimeUtc.Name = "labelLastModifiedDateTimeUtc";
            this.labelLastModifiedDateTimeUtc.Size = new System.Drawing.Size(69, 15);
            this.labelLastModifiedDateTimeUtc.TabIndex = 7;
            this.labelLastModifiedDateTimeUtc.Text = "yyyy/MM/dd";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(171, 427);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 22);
            this.buttonOk.TabIndex = 8;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(257, 427);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 22);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // ContainerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 461);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelLastModifiedDateTimeUtc);
            this.Controls.Add(this.labelCreatedDateTimeUtc);
            this.Controls.Add(this.labelTitleLastModifiedDateTimeUtc);
            this.Controls.Add(this.labelTitleCreatedDateTimeUtc);
            this.Controls.Add(this.dataGridViewMetadata);
            this.Controls.Add(this.labelMetadata);
            this.Controls.Add(this.checkBoxIsPublic);
            this.Controls.Add(this.labelAccessibility);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ContainerDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ContainerEditForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMetadata)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelAccessibility;
        private System.Windows.Forms.CheckBox checkBoxIsPublic;
        private System.Windows.Forms.Label labelMetadata;
        private System.Windows.Forms.DataGridView dataGridViewMetadata;
        private System.Windows.Forms.Label labelTitleCreatedDateTimeUtc;
        private System.Windows.Forms.Label labelCreatedDateTimeUtc;
        private System.Windows.Forms.Label labelTitleLastModifiedDateTimeUtc;
        private System.Windows.Forms.Label labelLastModifiedDateTimeUtc;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
    }
}