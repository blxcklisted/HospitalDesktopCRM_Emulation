using System.Security.Cryptography;
using System.Text;

namespace Project.UI.Infrastructure
{
    internal class PasswordEncryptor
    {
        private string password = null;
        public string Password { get => password; set => password = value; }

        public PasswordEncryptor(string password) => this.Password = password;

        public string EncryptPassword()
        {
            return EncryptPassword(Password);
        }
        public string EncryptPassword(string password)
        {
            if (password == null)
                return null;
            MD5 md5 = MD5.Create();

            byte[] inputPasswordBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputPasswordBytes);
            StringBuilder sb = new StringBuilder();
            foreach (var h in hash)
            {
                sb.Append(h.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
