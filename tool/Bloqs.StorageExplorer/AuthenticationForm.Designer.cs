namespace Bloqs.StorageExplorer
{
    partial class AuthenticationForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelAccountName = new System.Windows.Forms.Label();
            this.textBoxAccountName = new System.Windows.Forms.TextBox();
            this.labelAccessKey = new System.Windows.Forms.Label();
            this.textBoxAccessKey = new System.Windows.Forms.TextBox();
            this.buttonAccess = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelAccountName
            // 
            this.labelAccountName.AutoSize = true;
            this.labelAccountName.Location = new System.Drawing.Point(12, 9);
            this.labelAccountName.Name = "labelAccountName";
            this.labelAccountName.Size = new System.Drawing.Size(87, 15);
            this.labelAccountName.TabIndex = 0;
            this.labelAccountName.Text = "Account Name";
            // 
            // textBoxAccountName
            // 
            this.textBoxAccountName.Location = new System.Drawing.Point(15, 27);
            this.textBoxAccountName.MaxLength = 256;
            this.textBoxAccountName.Name = "textBoxAccountName";
            this.textBoxAccountName.Size = new System.Drawing.Size(597, 21);
            this.textBoxAccountName.TabIndex = 1;
            // 
            // labelAccessKey
            // 
            this.labelAccessKey.AutoSize = true;
            this.labelAccessKey.Location = new System.Drawing.Point(12, 51);
            this.labelAccessKey.Name = "labelAccessKey";
            this.labelAccessKey.Size = new System.Drawing.Size(68, 15);
            this.labelAccessKey.TabIndex = 2;
            this.labelAccessKey.Text = "Access Key";
            // 
            // textBoxAccessKey
            // 
            this.textBoxAccessKey.Location = new System.Drawing.Point(15, 69);
            this.textBoxAccessKey.MaxLength = 256;
            this.textBoxAccessKey.Name = "textBoxAccessKey";
            this.textBoxAccessKey.Size = new System.Drawing.Size(597, 21);
            this.textBoxAccessKey.TabIndex = 3;
            // 
            // buttonAccess
            // 
            this.buttonAccess.Location = new System.Drawing.Point(537, 106);
            this.buttonAccess.Name = "buttonAccess";
            this.buttonAccess.Size = new System.Drawing.Size(75, 23);
            this.buttonAccess.TabIndex = 4;
            this.buttonAccess.Text = "Access";
            this.buttonAccess.UseVisualStyleBackColor = true;
            this.buttonAccess.Click += new System.EventHandler(this.buttonAccess_Click);
            // 
            // AuthenticationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 141);
            this.Controls.Add(this.buttonAccess);
            this.Controls.Add(this.textBoxAccessKey);
            this.Controls.Add(this.labelAccessKey);
            this.Controls.Add(this.textBoxAccountName);
            this.Controls.Add(this.labelAccountName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuthenticationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bloqs Storage Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AuthenticationForm_FormClosing);
            this.Load += new System.EventHandler(this.AuthenticationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAccountName;
        private System.Windows.Forms.TextBox textBoxAccountName;
        private System.Windows.Forms.Label labelAccessKey;
        private System.Windows.Forms.TextBox textBoxAccessKey;
        private System.Windows.Forms.Button buttonAccess;
    }
}

