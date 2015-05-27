using System.Security.Cryptography;
using System.Text;

namespace Bloqs.Data.Models
{
    internal class BlobImageDataModel
    {
        public string AccountId { get; set; }
        public string ContainerId { get; set; }
        public string BlobId { get; set; }
        public byte[] Image { get; set; }

        public BlobImageDataModel()
        {
        }

        public BlobImageDataModel(BlobAttributes attributes)
        {
            AccountId = Hash(attributes.Container.Account.Id, attributes.Container.Account.CryptKey);
            ContainerId = Hash(attributes.Container.Id, attributes.Container.Account.CryptKey);
            BlobId = Hash(attributes.Container.Id, attributes.Container.Account.CryptKey);            
        }

        public BlobImageDataModel(Blob blob) : this((BlobAttributes)blob)
        {
            Image = EncryptImage(blob);
        }

        public byte[] ToByteArray(Account account)
        {
            return DecryptImage(account);
        }

        private static string Hash(string source, string salt)
        {
            var data = Encoding.UTF8.GetBytes(source + salt);
            using (var crypt = MD5.Create())
            {
                var hashValue = crypt.ComputeHash(data);
                var result = new StringBuilder();
                foreach (var b in hashValue)
                {
                    result.Append(b.ToString("x2"));
                }
                return result.ToString();
            }
        }

        private static byte[] EncryptImage(Blob blob)
        {
            using (var rijndael = new RijndaelManaged())
            {
                byte[] key, iv;
                GenerateKeyFromPassword(blob.Container.Account.CryptKey, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
                rijndael.Key = key;
                rijndael.IV = iv;

                using (var encryptor = rijndael.CreateEncryptor())
                {
                    var encrypted = encryptor.TransformFinalBlock(blob.Image, 0, blob.Image.Length);
                    return encrypted;
                }
            }
        }

        private byte[] DecryptImage(Account account)
        {
            using (var rijndael = new RijndaelManaged())
            {
                byte[] key, iv;
                GenerateKeyFromPassword(account.CryptKey, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
                rijndael.Key = key;
                rijndael.IV = iv;

                using (var decryptor = rijndael.CreateDecryptor())
                {
                    var encrypted = decryptor.TransformFinalBlock(Image, 0, Image.Length);
                    return encrypted;
                }
            }
        }

        private static void GenerateKeyFromPassword(string password, int keySize, out byte[] key, int blockSize,
            out byte[] iv)
        {
            var salt = Encoding.UTF8.GetBytes(@"R\PT;4K8jY4YUdXxEbkj");
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt) { IterationCount = 1000 })
            {
                key = deriveBytes.GetBytes(keySize / 8);
                iv = deriveBytes.GetBytes(blockSize / 8);
            }
        }
    }
}
