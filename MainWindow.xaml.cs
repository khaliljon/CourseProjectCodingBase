using CourseProjectCodingBase.Lab2;
using CourseProjectCodingBase.Lab3;
using CourseProjectCodingBase.Lab4;
using System.Windows;
using System.Windows.Controls;

namespace CourseProjectCodingBase
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenLab2(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Lab2Page());
        }

        private void OpenLab3(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Lab3Page());
        }

        private void OpenLab4(object sender, RoutedEventArgs e) // Добавлен метод для Лабораторной работы 4
        {
            MainFrame.Navigate(new Lab4Page());
        }
    }
}