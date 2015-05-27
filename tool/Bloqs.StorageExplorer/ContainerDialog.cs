using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Bloqs.Http.Net;

namespace Bloqs.StorageExplorer
{
    public partial class ContainerDialog : Form
    {
        private readonly Client _client;
        private readonly Container _container;
        private readonly Mode _mode;

        private enum Mode
        {
            Add,
            Refer,
        }

        public ContainerDialog(Client client)
            : this(client, null, Mode.Add)
        {
        }

        public ContainerDialog(Client client, Container container)
            : this(client, container, Mode.Refer)
        {
            
        }

        private ContainerDialog(Client client, Container container, Mode mode)
        {
            _client = client;
            _container = container;
            _mode = mode;

            InitializeComponent();

            labelCreatedDateTimeUtc.Text = "";
            labelLastModifiedDateTimeUtc.Text = "";

            if (mode == Mode.Add)
            {
                base.Text = @"Add new";
                textBoxName.ReadOnly = false;
            }
            else
            {
                base.Text = container.Name;
                BindControl();
                textBoxName.ReadOnly = true;
                checkBoxIsPublic.Enabled = false;
                buttonOk.Visible = false;
                buttonCancel.Text = @"Close";
            }
        }

        private void BindControl()
        {
            textBoxName.Text = _container.Name;
            checkBoxIsPublic.Checked = _container.IsPublic;
            dataGridViewMetadata.DataSource = _container.Metadata.ToList();

            if (_container.CreatedUtcDateTime.HasValue)
            {
                labelCreatedDateTimeUtc.Text = _container.CreatedUtcDateTime.Value.ToString(CultureInfo.InvariantCulture);
            }

            if (_container.LastModifiedUtcDateTime.HasValue)
            {
                labelLastModifiedDateTimeUtc.Text = _container.LastModifiedUtcDateTime.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        private async void buttonOk_Click(object sender, EventArgs e)
        {
            if (_mode == Mode.Refer)
            {
                MessageBox.Show(@"sorry, not implemented.");
                DialogResult = DialogResult.OK;
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxName.Text)) return;

            try
            {
                var container = await _client.GetContainerReferenceAsync(textBoxName.Text);
                if (container.CreatedUtcDateTime != null)
                {
                    MessageBox.Show(@"this container name is already exists.");
                    return;
                }
                container.IsPublic = checkBoxIsPublic.Checked;
                foreach (var row in dataGridViewMetadata.Rows.Cast<DataGridViewRow>())
                {
                    var key = row.Cells["ColumnKey"].Value;
                    var value = row.Cells["ColumnValue"].Value;
                    if (key != null && value != null)
                    {
                        container.Metadata[key.ToString()] = value.ToString();
                    }
                }

                await container.CreateIfNotExistsAsync();
                
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
    }
}
