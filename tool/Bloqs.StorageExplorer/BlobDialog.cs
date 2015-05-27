using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Bloqs.Http.Net;

namespace Bloqs.StorageExplorer
{
    public partial class BlobDialog : Form
    {
        private readonly Blob _blob;

        public BlobDialog(Blob blob)
        {
            _blob = blob;
            InitializeComponent();
            Text = _blob.Name;
            Bind(_blob);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Bind(Blob blob)
        {
            textBoxFile.Text = blob.Name;

            textBoxCacheControl.Text = blob.Properties.CacheControl;
            textBoxContentDisposition.Text = blob.Properties.ContentDisposition;
            textBoxContentEncoding.Text = blob.Properties.ContentEncoding;
            textBoxContentLanguage.Text = blob.Properties.ContentLanguage;
            textBoxContentMD5.Text = blob.Properties.ContentMD5;
            textBoxContentType.Text = blob.Properties.ContentType;
            textBoxETag.Text = blob.Properties.ETag;
            textBoxLastModified.Text = blob.Properties.LastModifiedUtcDateTime.ToString(CultureInfo.InvariantCulture);

            dataGridViewMetadata.DataSource = blob.Metadata.ToList();
        }
    }
}
