using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CourseProjectCodingBase.Model;

namespace CourseProjectCodingBase.Lab1
{
    public partial class Lab1Page : Page
    {
        public Lab1Page()
        {
            InitializeComponent();
        }

        private void CalculateEntropy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Удаляем пробелы и разбиваем алфавит по запятой
                var symbols = AlphabetInput.Text
                    .Split(',')
                    .Select(s => s.Trim())
                    .ToArray();

                // Удаляем пробелы и разбиваем вероятности по запятой,
                // с учетом точки как десятичного разделителя
                var probabilities = ProbabilitiesInput.Text
                    .Split(',')
                    .Select(p => p.Trim())
                    .Select(p => double.Parse(p, System.Globalization.CultureInfo.InvariantCulture))
                    .ToArray();

                // Проверка: кол-во символов и вероятностей должно совпадать
                if (symbols.Length != probabilities.Length)
                {
                    MessageBox.Show("Количество символов и вероятностей должно совпадать.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка: сумма вероятностей ≈ 1
                if (Math.Abs(probabilities.Sum() - 1.0) > 0.001)
                {
                    MessageBox.Show("Сумма вероятностей должна быть равна 1.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Вычисление энтропии
                double entropy = -probabilities.Sum(p => p * Math.Log(p, 2));
                EntropyResult.Text = $"Энтропия: {entropy:F4}";
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат вероятностей. Используйте **точку** как десятичный разделитель и запятые между числами.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EncodeHuffman_Click(object sender, RoutedEventArgs e)
        {
            string input = HuffmanInput.Text;
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Введите текст для кодирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            HuffmanCoding huffman = new HuffmanCoding();
            string encodedText = huffman.Encode(input);
            double efficiency = huffman.GetEfficiency(input, encodedText);

            HuffmanResult.Text = $"Закодированный текст: {encodedText}\nЭффективность: {efficiency:P2}";
        }
    }
}