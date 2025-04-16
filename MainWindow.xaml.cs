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
            MainFrame.Source = new System.Uri("Lab2/Lab2Page.xaml", System.UriKind.Relative);
        }

        private void OpenLab3(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new System.Uri("Lab3/Lab3Page.xaml", System.UriKind.Relative);
        }

        private void OpenLab4(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new System.Uri("Lab4/Lab4Page.xaml", System.UriKind.Relative);
        }

        private void OpenLab5(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new System.Uri("Lab5/Lab5Page.xaml", System.UriKind.Relative);
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            // Завершение работы приложения
            Application.Current.Shutdown();
        }
    }
}