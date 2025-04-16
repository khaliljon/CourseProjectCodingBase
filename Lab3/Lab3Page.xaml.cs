using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace CourseProjectCodingBase.Lab3
{
    public partial class Lab3Page : Page
    {
        private List<double> signal = new List<double>(); 
        private List<double> noisySignal = new List<double>(); 
        private List<double> processedSignal = new List<double>(); 
        private const int CanvasWidth = 600;
        private const int CanvasHeight = 100;

        public Lab3Page()
        {
            InitializeComponent();
        }

        private void GenerateSignal_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            signal = Enumerable.Range(0, 100).Select(_ => rnd.NextDouble() * 2 - 1).ToList();
            noisySignal = new List<double>(signal); 
            processedSignal = new List<double>(signal); 
            DrawChart(signal, OriginalCanvas);
        }

        private void AddNoise_Click(object sender, RoutedEventArgs e)
        {
            if (signal.Count == 0) return;
            Random rnd = new Random();
            noisySignal = new List<double>(signal); 
            int count = (int)(noisySignal.Count * 0.1);
            for (int i = 0; i < count; i++)
            {
                int index = rnd.Next(noisySignal.Count);
                noisySignal[index] += (rnd.NextDouble() * 2 - 1); 
            }
            DrawChart(noisySignal, NoisyCanvas);
        }

        private void RemoveValues_Click(object sender, RoutedEventArgs e)
        {
            if (noisySignal.Count == 0) return;
            Random rnd = new Random();
            processedSignal = new List<double>(noisySignal); 
            int count = (int)(processedSignal.Count * 0.05);
            for (int i = 0; i < count; i++)
            {
                int index = rnd.Next(processedSignal.Count);
                processedSignal[index] = double.NaN; 
            }
            DrawChart(processedSignal, ProcessedCanvas);
        }

        private void DrawChart(List<double> signal, Canvas canvas)
        {
            canvas.Children.Clear();

            if (signal.Count == 0) return;

            double actualWidth = canvas.ActualWidth > 0 ? canvas.ActualWidth : canvas.Width;
            double actualHeight = canvas.ActualHeight > 0 ? canvas.ActualHeight : canvas.Height;

            double xStep = actualWidth / signal.Count;
            double yMid = actualHeight / 2;

            double maxAmplitude = signal.Where(v => !double.IsNaN(v)).Select(Math.Abs).DefaultIfEmpty(1).Max();
            double scale = (yMid - 5) / maxAmplitude; // -5 для небольшого отступа от границ

            Polyline polyline = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };

            for (int i = 0; i < signal.Count; i++)
            {
                if (double.IsNaN(signal[i])) continue;

                double x = i * xStep;
                double y = yMid - (signal[i] * scale);

                polyline.Points.Add(new Point(x, y));
            }

            canvas.Children.Add(polyline);
        }
        private void ClearMemory_Click(object sender, RoutedEventArgs e)
        {
            signal.Clear();
            noisySignal.Clear();
            processedSignal.Clear();
            OriginalCanvas.Children.Clear();
            NoisyCanvas.Children.Clear();
            ProcessedCanvas.Children.Clear();
        }
    }
}
