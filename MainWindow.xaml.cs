using CourseProjectCodingBase.Lab2;
using CourseProjectCodingBase.Lab3;
using CourseProjectCodingBase.Lab4;
using CourseProjectCodingBase.Lab5; // Добавляем пространство имен для Лабораторной работы 5
using System.Windows;

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

        private void OpenLab4(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Lab4Page());
        }

        private void OpenLab5(object sender, RoutedEventArgs e) // Добавлен метод для Лабораторной работы 5
        {
            MainFrame.Navigate(new Lab5Page());
        }
    }
}
