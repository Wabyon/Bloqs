namespace Bloqs.StorageExplorer
{
    partial class MainForm
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
            this.listBoxContainers = new System.Windows.Forms.ListBox();
            this.listBoxBlobs = new System.Windows.Forms.ListBox();
            this.contextMenuStripContainer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripContainerList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripBlob = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripBlobList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.uploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStripContainer.SuspendLayout();
            this.contextMenuStripContainerList.SuspendLayout();
            this.contextMenuStripBlob.SuspendLayout();
            this.contextMenuStripBlobList.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxContainers
            // 
            this.listBoxContainers.DisplayMember = "Name";
            this.listBoxContainers.FormattingEnabled = true;
            this.listBoxContainers.ItemHeight = 15;
            this.listBoxContainers.Location = new System.Drawing.Point(12, 12);
            this.listBoxContainers.Name = "listBoxContainers";
            this.listBoxContainers.Size = new System.Drawing.Size(296, 424);
            this.listBoxContainers.TabIndex = 0;
            this.listBoxContainers.SelectedIndexChanged += new System.EventHandler(this.listBoxContainers_SelectedIndexChanged);
            this.listBoxContainers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxContainers_MouseDoubleClick);
            this.listBoxContainers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listBoxContainers_MouseUp);
            // 
            // listBoxBlobs
            // 
            this.listBoxBlobs.DisplayMember = "Name";
            this.listBoxBlobs.FormattingEnabled = true;
            this.listBoxBlobs.ItemHeight = 15;
            this.listBoxBlobs.Location = new System.Drawing.Point(314, 12);
            this.listBoxBlobs.Name = "listBoxBlobs";
            this.listBoxBlobs.Size = new System.Drawing.Size(296, 424);
            this.listBoxBlobs.TabIndex = 1;
            this.listBoxBlobs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxBlobs_MouseDoubleClick);
            this.listBoxBlobs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listBoxBlobs_MouseUp);
            // 
            // contextMenuStripContainer
            // 
            this.contextMenuStripContainer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertyToolStripMenuItem1});
            this.contextMenuStripContainer.Name = "contextMenuStripContainer";
            this.contextMenuStripContainer.Size = new System.Drawing.Size(125, 26);
            // 
            // propertyToolStripMenuItem1
            // 
            this.propertyToolStripMenuItem1.Name = "propertyToolStripMenuItem1";
            this.propertyToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.propertyToolStripMenuItem1.Text = "Property";
            this.propertyToolStripMenuItem1.Click += new System.EventHandler(this.propertyToolStripMenuItem1_Click);
            // 
            // contextMenuStripContainerList
            // 
            this.contextMenuStripContainerList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.contextMenuStripContainerList.Name = "contextMenuStripContainerList";
            this.contextMenuStripContainerList.Size = new System.Drawing.Size(127, 48);
            // 
            // addNewToolStripMenuItem
            // 
            this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            this.addNewToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.addNewToolStripMenuItem.Text = "Add New";
            this.addNewToolStripMenuItem.Click += new System.EventHandler(this.addNewToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // contextMenuStripBlob
            // 
            this.contextMenuStripBlob.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertyToolStripMenuItem,
            this.downloadToolStripMenuItem});
            this.contextMenuStripBlob.Name = "contextMenuStripBlob";
            this.contextMenuStripBlob.Size = new System.Drawing.Size(132, 48);
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            this.propertyToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.propertyToolStripMenuItem.Text = "Property";
            this.propertyToolStripMenuItem.Click += new System.EventHandler(this.propertyToolStripMenuItem_Click);
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.downloadToolStripMenuItem.Text = "Download";
            this.downloadToolStripMenuItem.Click += new System.EventHandler(this.downloadToolStripMenuItem_Click);
            // 
            // contextMenuStripBlobList
            // 
            this.contextMenuStripBlobList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadToolStripMenuItem,
            this.refreshToolStripMenuItem1});
            this.contextMenuStripBlobList.Name = "contextMenuStripBlobList";
            this.contextMenuStripBlobList.Size = new System.Drawing.Size(119, 48);
            // 
            // uploadToolStripMenuItem
            // 
            this.uploadToolStripMenuItem.Name = "uploadToolStripMenuItem";
            this.uploadToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.uploadToolStripMenuItem.Text = "Upload";
            this.uploadToolStripMenuItem.Click += new System.EventHandler(this.uploadToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem1
            // 
            this.refreshToolStripMenuItem1.Name = "refreshToolStripMenuItem1";
            this.refreshToolStripMenuItem1.Size = new System.Drawing.Size(118, 22);
            this.refreshToolStripMenuItem1.Text = "Refresh";
            this.refreshToolStripMenuItem1.Click += new System.EventHandler(this.refreshToolStripMenuItem1_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "All Files|*.*";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "All Files|*.*";
            this.openFileDialog.Title = "Upload File";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.listBoxBlobs);
            this.Controls.Add(this.listBoxContainers);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bloqs Storage Explorer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.contextMenuStripContainer.ResumeLayout(false);
            this.contextMenuStripContainerList.ResumeLayout(false);
            this.contextMenuStripBlob.ResumeLayout(false);
            this.contextMenuStripBlobList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxContainers;
        private System.Windows.Forms.ListBox listBoxBlobs;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripContainer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripContainerList;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBlob;
        private System.Windows.Forms.ToolStripMenuItem propertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBlobList;
        private System.Windows.Forms.ToolStripMenuItem uploadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem propertyToolStripMenuItem1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}