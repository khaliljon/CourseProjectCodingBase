using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CourseProjectCodingBase.Lab5
{
    public partial class Lab5Page : Page
    {
        private string originalFilePath;
        private string compressedFilePath;

        public Lab5Page()
        {
            InitializeComponent();
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                originalFilePath = openFileDialog.FileName;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                using (FileStream stream = new FileStream(originalFilePath, FileMode.Open, FileAccess.Read))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    bitmap.Freeze(); 
                    OriginalImage.Source = bitmap;
                }

                long fileSize = new FileInfo(originalFilePath).Length;
                OriginalSizeText.Text = $"Размер: {fileSize / 1024} KB";
            }
        }

        private void CompressImage_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(originalFilePath))
            {
                MessageBox.Show("Сначала загрузите изображение.");
                return;
            }

            int quality = (int)QualitySlider.Value;
            compressedFilePath = Path.Combine(Path.GetTempPath(), "compressed.jpg");

            using (Bitmap bitmap = new Bitmap(originalFilePath))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                bitmap.Save(compressedFilePath, jpgEncoder, encoderParams);
            }

            CompressedImage.Source = new BitmapImage(new Uri(compressedFilePath));

            long compressedSize = new FileInfo(compressedFilePath).Length;
            CompressedSizeText.Text = $"Размер: {compressedSize / 1024} KB";

            MessageBox.Show($"Сжатие завершено. Размер: {compressedSize / 1024} KB");
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            if (compressedFilePath == null)
            {
                MessageBox.Show("Сначала выполните сжатие изображения.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JPEG Image (*.jpg)|*.jpg"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.Copy(compressedFilePath, saveFileDialog.FileName, true);
                MessageBox.Show("Изображение сохранено.");
            }
        }

        private void ClearImages_Click(object sender, RoutedEventArgs e)
        {
            OriginalImage.Source = null;
            CompressedImage.Source = null;

            originalFilePath = null;
            compressedFilePath = null;

            OriginalSizeText.Text = "Размер: -";
            CompressedSizeText.Text = "Размер: -";

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
