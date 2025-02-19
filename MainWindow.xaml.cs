using CourseProjectCodingBase.Lab2;
using CourseProjectCodingBase.Lab3;
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
    }
}
