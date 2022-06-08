using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Project.UI.Infrastructure
{
    public static class ByteArrayConverter
    {
        public static BitmapImage GetBitmapImage(byte[] imageBytes)
        {
            try
            {
                if (imageBytes == null || imageBytes.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new MemoryStream(imageBytes))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                return image;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        public static byte[] GetByteArray(string filepath)
        {
            try
            {
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);

                byte[] ImageData = new byte[fs.Length];

                fs.Read(ImageData, 0, Convert.ToInt32(fs.Length));

                fs.Close();
                return ImageData;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }
}
