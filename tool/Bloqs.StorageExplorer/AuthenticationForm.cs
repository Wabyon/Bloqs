using System;
using System.Configuration;
using System.Windows.Forms;
using Bloqs.Http.Net;

namespace Bloqs.StorageExplorer
{
    public partial class AuthenticationForm : Form
    {
        private readonly Uri _baseAddress;
        private Credentials _credentials;

        internal bool IsCancelled { get; private set; }

        internal Account Account { get; private set; }

        internal AuthenticationForm()
        {
            _baseAddress = new Uri(ConfigurationManager.AppSettings["BaseAddress"]);
            InitializeComponent();
        }

        private void buttonAccess_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxAccountName.Text)) return;
            if (string.IsNullOrWhiteSpace(textBoxAccessKey.Text)) return;

            _credentials = new Credentials(textBoxAccountName.Text, textBoxAccessKey.Text);

            Account = new Account(_credentials, _baseAddress);
            Close();
        }

        private void AuthenticationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.AccountName = textBoxAccountName.Text;
            Properties.Settings.Default.AccessKey = textBoxAccessKey.Text;
            Properties.Settings.Default.Save();

            if (_credentials == null) IsCancelled = true;
        }

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {
            textBoxAccountName.Text = Properties.Settings.Default.AccountName;
            textBoxAccessKey.Text = Properties.Settings.Default.AccessKey;
        }
    }
}
