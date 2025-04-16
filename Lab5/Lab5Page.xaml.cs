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
        private Bitmap _originalBitmap;
        private Bitmap _compressedBitmap;
        private MemoryStream _jpegMemoryStream;

        public Lab5Page()
        {
            InitializeComponent();
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog { Filter = "Image files (*.bmp;*.png)|*.bmp;*.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                OriginalImage.Source = null;
                CompressedImage.Source = null;

                _originalBitmap = new Bitmap(openFileDialog.FileName);
                OriginalImage.Source = BitmapToImageSource(_originalBitmap);
                _compressedBitmap = null;
                _jpegMemoryStream = null;

                // Вывод объема исходного изображения
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                ImageInfoText.Text = $"Объем исходного изображения: {fileInfo.Length / 1024.0:F2} КБ";
            }
        }

        private void CompressButton_Click(object sender, RoutedEventArgs e)
        {
            if (_originalBitmap == null)
            {
                MessageBox.Show("Сначала загрузите изображение.");
                return;
            }

            try
            {
                ProgressBar.Visibility = Visibility.Visible;
                var quality = 100 - (int)CompressionSlider.Value;

                _jpegMemoryStream = new MemoryStream();
                var encoder = GetEncoder(ImageFormat.Jpeg);
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                _originalBitmap.Save(_jpegMemoryStream, encoder, encoderParameters);
                _jpegMemoryStream.Seek(0, SeekOrigin.Begin);
                _compressedBitmap = new Bitmap(_jpegMemoryStream);
                CompressedImage.Source = BitmapToImageSource(_compressedBitmap);

                // Вывод объема сжатого изображения
                ImageInfoText.Text += $"\nОбъем сжатого изображения: {_jpegMemoryStream.Length / 1024.0:F2} КБ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сжатии изображения: {ex.Message}");
            }
            finally
            {
                ProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_jpegMemoryStream == null)
            {
                MessageBox.Show("Сначала выполните сжатие изображения.");
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JPEG Image|*.jpg",
                FileName = "Compressed_image.jpg"
            };

            try
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, _jpegMemoryStream.ToArray());
                    MessageBox.Show($"Изображение сохранено по пути: {saveFileDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка всех данных
            _originalBitmap = null;
            _compressedBitmap = null;
            _jpegMemoryStream = null;

            OriginalImage.Source = null;
            CompressedImage.Source = null;
            ImageInfoText.Text = "Объем исходного и сжатого изображения будет отображаться здесь.";
            CompressionSlider.Value = 50;
            CompressionQualityText.Text = "50";

            MessageBox.Show("Все данные были очищены.");
        }

        private void CompressionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CompressionQualityText != null)
            {
                CompressionQualityText.Text = $"{(int)CompressionSlider.Value}%";
            }
        }

        private static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            throw new InvalidOperationException("JPEG encoder not found.");
        }
    }
}