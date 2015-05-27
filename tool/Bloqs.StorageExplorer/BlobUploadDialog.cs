using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Bloqs.Http.Net;

namespace Bloqs.StorageExplorer
{
    public partial class BlobUploadDialog : Form
    {
        private readonly Container _container;

        public BlobUploadDialog(Container container)
        {
            _container = container;
            InitializeComponent();
        }

        private async void buttonOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFile.Text)) return;
            var file = new FileInfo(textBoxFile.Text);

            try
            {
                var blob = await _container.GetBlobReferenceAsync(file.Name);
                blob.Properties.CacheControl = textBoxCacheControl.Text;
                blob.Properties.ContentDisposition = textBoxContentDisposition.Text;
                blob.Properties.ContentEncoding = textBoxContentEncoding.Text;
                blob.Properties.ContentType = textBoxContentType.Text;
                foreach (var row in dataGridViewMetadata.Rows.Cast<DataGridViewRow>())
                {
                    var key = row.Cells["ColumnKey"].Value;
                    var value = row.Cells["ColumnValue"].Value;
                    if (key != null && value != null)
                    {
                        blob.Metadata[key.ToString()] = value.ToString();
                    }
                }

                using (var fs = file.OpenRead())
                {
                    var data = new byte[fs.Length];
                    await fs.ReadAsync(data, 0, (int) fs.Length);
                    await blob.UploadFromByteArrayAsync(data, 0, data.Length);
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonFileDialog_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                Filter = @"All Files|*.*",
                Title = @"Upload File",
                Multiselect = false,
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                textBoxFile.Text = openFileDialog.FileName;
            }
        }
    }
}
