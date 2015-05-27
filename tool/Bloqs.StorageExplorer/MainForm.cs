using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bloqs.Http.Net;
using Container = Bloqs.Http.Net.Container;

namespace Bloqs.StorageExplorer
{
    public partial class MainForm : Form
    {
        private Account _account;
        private Client _client;

        public MainForm()
        {
            InitializeComponent();
        }

        private async Task<bool> Autholize(EventArgs e)
        {
            using (var authForm = new AuthenticationForm())
            {
                authForm.ShowDialog();
                if (authForm.IsCancelled)
                {
                    Close();
                    return true;
                }
                _account = authForm.Account;
            }
            _client = _account.CreateClient();

            try
            {
                var containers = await _client.ListContainersAsync();
                listBoxContainers.DataSource = containers;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }

            return true;
        }

        private async Task GetAndDisplayContainersAsync()
        {
            listBoxContainers.DataSource = await _client.ListContainersAsync();
        }

        private async Task GetAndDisplayBlobsAsync(Container container)
        {
            var blobs = await container.ListBlobsAsync();
            listBoxBlobs.DataSource = blobs;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            bool auth;
            do
            {
                auth = await Autholize(e);
            } while (auth == false);
        }

        private async void listBoxContainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var container = (Container)listBoxContainers.SelectedItem;
            try
            {
                await GetAndDisplayBlobsAsync(container);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void listBoxContainers_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var index = listBoxContainers.IndexFromPoint(e.Location);
            var position = listBoxContainers.PointToScreen(e.Location);

            if (index < 0)
            {
                contextMenuStripContainerList.Show(position);
            }
            else
            {
                listBoxContainers.SelectedIndex = index;
                contextMenuStripContainer.Show(position);
            }
        }

        private async void listBoxContainers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var index = listBoxContainers.IndexFromPoint(e.Location);
            if (index < 0) return;

            var container = (Container)listBoxContainers.Items[index];
            using (var containerEditDialog = new ContainerDialog(_client, container))
            {
                if (containerEditDialog.ShowDialog() != DialogResult.OK) return;
                try
                {
                    await GetAndDisplayContainersAsync();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void listBoxBlobs_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var index = listBoxBlobs.IndexFromPoint(e.Location);
            var position = listBoxBlobs.PointToScreen(e.Location);

            if (index < 0)
            {
                if (listBoxContainers.SelectedItem == null) return;
                contextMenuStripBlobList.Show(position);
            }
            else
            {
                listBoxBlobs.SelectedIndex = index;
                contextMenuStripBlob.Show(position);
            }
        }

        private void listBoxBlobs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var index = listBoxBlobs.IndexFromPoint(e.Location);
            if (index < 0) return;

            var blob = (Blob) listBoxBlobs.Items[index];
            using (var dialog = new BlobDialog(blob))
            {
                dialog.ShowDialog();
            }
        }

        private async void propertyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var container = (Container) listBoxContainers.SelectedItem;

            using (var containerEditDialog = new ContainerDialog(_client, container))
            {
                if (containerEditDialog.ShowDialog() != DialogResult.OK) return;
                try
                {
                    await GetAndDisplayContainersAsync();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private async void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var containerEditDialog = new ContainerDialog(_client))
            {
                if (containerEditDialog.ShowDialog() != DialogResult.OK) return;
                try
                {
                    await GetAndDisplayContainersAsync();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private async void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await GetAndDisplayContainersAsync();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void propertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var blob = (Blob)listBoxBlobs.SelectedItem;
            using (var dialog = new BlobDialog(blob))
            {
                dialog.ShowDialog();
            }
        }

        private async void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var blob = (Blob) listBoxBlobs.SelectedItem;
            using (var ms = new MemoryStream())
            {
                try
                {
                    await blob.DownloadToStreamAsync(ms);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    return;
                }

                saveFileDialog.FileName = blob.Name;
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    File.WriteAllBytes(saveFileDialog.FileName, ms.ToArray());
                    MessageBox.Show(@"Download complete.");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private async void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var container = (Container)listBoxContainers.SelectedItem;
            using (var uploadDialog = new BlobUploadDialog(container))
            {
                if (uploadDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    await GetAndDisplayBlobsAsync(container);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private async void refreshToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var container = (Container)listBoxContainers.SelectedItem;
            try
            {
                await GetAndDisplayBlobsAsync(container);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
