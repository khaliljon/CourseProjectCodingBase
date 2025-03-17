using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CourseProjectCodingBase.Model;
using CourseProject.Lab2;


namespace CourseProjectCodingBase.Lab2
{
    public partial class Lab2Page : Page
    {
        public Lab2Page()
        {
            InitializeComponent();
        }

        private void ClearResults()
        {
            HuffmanCodesGrid.ItemsSource = null;
            OutputTextBox.Text = string.Empty;
        }

        private void EncodeHuffman(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Enter text before encoding.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            HuffmanCoding huffman = new HuffmanCoding();
            string encodedText = huffman.Encode(input);
            double efficiency = huffman.GetEfficiency(input, encodedText);
            Dictionary<char, string> huffmanCodes = huffman.GetHuffmanCodes();

            OutputTextBox.Text = $"Encoded: {encodedText}\nEfficiency: {efficiency:P2}";

            HuffmanCodesGrid.ItemsSource = huffmanCodes.Select(kv => new { Symbol = kv.Key, Code = kv.Value }).ToList();
        }

        private void EncodeShannonFano(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Enter text before encoding.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ClearHuffmanCodes();

            ShannonFanoCoding shannonFano = new ShannonFanoCoding();
            string encodedText = shannonFano.Encode(input);
            double compressionRatio = shannonFano.GetCompressionRatio(input, encodedText);

            OutputTextBox.Text = $"Encoded: {encodedText}\nCompression Ratio: {compressionRatio:P2}";
        }


        private void EncodeReedSolomon(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Enter text before encoding.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ClearHuffmanCodes();

            ReedSolomonCoding reedSolomon = new ReedSolomonCoding();
            string encodedText = reedSolomon.Encode(input);
            string corruptedText = reedSolomon.IntroduceErrors(encodedText);

            string correctedText;
            int errorsFixed = 0;

            try
            {
                correctedText = reedSolomon.Decode(corruptedText, ref errorsFixed);

                for (int i = 0; i < correctedText.Length; i++)
                {
                    if (correctedText[i] != input[i])
                    {
                        Console.WriteLine($"Ошибка исправлена в позиции {i}: {input[i]} -> {correctedText[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                correctedText = $"Decoding Error: {ex.Message}";
            }

            Console.WriteLine($"Исправлено ошибок: {errorsFixed}");

            OutputTextBox.Text = $"Encoded: {encodedText}\nCorrupted: {corruptedText}\nCorrected: {(correctedText == input ? correctedText : "Correction Failed")}";
        }

        private void ClearHuffmanCodes()
        {
            HuffmanCodesGrid.ItemsSource = null;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
            InputTextBox.Text = string.Empty;
        }
    }
}
