using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using CourseProjectCodingBase.Model;
using System.Text;

namespace CourseProjectCodingBase.Lab4
{
    public partial class Lab4Page : Page
    {
        private string loadedFilePath;
        private byte[] compressedData;
        private HuffmanCoding huffmanCoding = new HuffmanCoding();
        private LZW lzw = new LZW();

        public Lab4Page()
        {
            InitializeComponent();
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                loadedFilePath = openFileDialog.FileName;
                OutputTextBox.Text = $"Файл загружен: {loadedFilePath}";
            }
        }

        private void CompressButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loadedFilePath))
            {
                MessageBox.Show("Пожалуйста, загрузите файл перед сжатием.");
                return;
            }

            string method = ((ComboBoxItem)CompressionMethodComboBox.SelectedItem)?.Content.ToString();
            string inputText = File.ReadAllText(loadedFilePath);

            if (method == "Huffman")
            {
                string encodedText = huffmanCoding.Encode(inputText);
                compressedData = ConvertToByteArray(encodedText);
            }
            else if (method == "LZW")
            {
                List<int> compressed = lzw.Compress(inputText);
                compressedData = compressed.SelectMany(BitConverter.GetBytes).ToArray();
            }

            OutputTextBox.Text += $"\nРазмер файла до сжатия: {new FileInfo(loadedFilePath).Length} байт ({new FileInfo(loadedFilePath).Length / 1024.0:F2} КБ)";
            OutputTextBox.Text += $"\nРазмер файла после сжатия: {compressedData.Length} байт ({compressedData.Length / 1024.0:F2} КБ)";

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (compressedData == null)
            {
                MessageBox.Show("Пожалуйста, сначала сожмите файл.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Binary files (*.bin)|*.bin|Text files (*.txt)|*.txt|ZIP files (*.zip)|*.zip";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName, compressedData);
                OutputTextBox.Text += $"\nФайл сохранен: {saveFileDialog.FileName}";
            }
        }

        private void DecodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (compressedData == null)
            {
                MessageBox.Show("Пожалуйста, сожмите файл перед декодированием.");
                return;
            }

            string method = ((ComboBoxItem)CompressionMethodComboBox.SelectedItem)?.Content.ToString();
            string decodedText = string.Empty;

            if (method == "Huffman")
            {
                string encodedText = ConvertToString(compressedData);
                string inputText = File.ReadAllText(loadedFilePath); // Получаем исходный текст
                decodedText = huffmanCoding.Decode(encodedText, inputText.Length); // Передаем длину исходного текста
            }
            else if (method == "LZW")
            {
                List<int> compressed = new List<int>();
                for (int i = 0; i < compressedData.Length; i += 4)
                {
                    compressed.Add(BitConverter.ToInt32(compressedData, i));
                }
                decodedText = lzw.Decompress(compressed);
            }

            OutputTextBox.Text += $"\nДекодированный текст: {decodedText}";
        }

        private byte[] ConvertToByteArray(string input)
        {
            int numOfBytes = (input.Length / 8) + (input.Length % 8 == 0 ? 0 : 1);
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '1')
                {
                    bytes[i / 8] |= (byte)(1 << (7 - (i % 8)));
                }
            }
            return bytes;
        }

        private string ConvertToString(byte[] input)
        {
            StringBuilder sb = new StringBuilder(input.Length * 8);
            foreach (byte b in input)
            {
                for (int i = 0; i < 8; i++)
                {
                    sb.Append((b & (1 << (7 - i))) != 0 ? '1' : '0');
                }
            }
            return sb.ToString();
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            loadedFilePath = null;
            compressedData = null;
            OutputTextBox.Clear();
            CompressionMethodComboBox.SelectedIndex = -1;
            MessageBox.Show("Все данные очищены.");
        }
    }
}