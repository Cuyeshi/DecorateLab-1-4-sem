using System;
using System.IO;
using System.Text;
using System.Windows;
using StreamDecoratorLibrary;

namespace StreamDecoratorDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateTotalBytesWritten(long totalBytesWritten)
        {
            txtTotalBytesWritten.Text = $"Total Bytes: {totalBytesWritten}";
        }

        private void UpdateFileContent(string content)
        {
            txtFileContent.Text = content;
        }

        private void btnFileStream_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "testfile.txt";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            using (CountingStreamDecorator countingStream = new CountingStreamDecorator(fs))
            {
                byte[] data = Encoding.UTF8.GetBytes("Это FileStream!");
                countingStream.Write(data, 0, data.Length);
                UpdateTotalBytesWritten(countingStream.TotalBytesWritten);
            }
            UpdateFileContent(File.ReadAllText(filePath));
        }

        private void btnMemoryStream_Click(object sender, RoutedEventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            using (CountingStreamDecorator countingStream = new CountingStreamDecorator(ms))
            {
                byte[] data = Encoding.UTF8.GetBytes("Это MemoryStream!");
                countingStream.Write(data, 0, data.Length);
                UpdateTotalBytesWritten(countingStream.TotalBytesWritten);
                UpdateFileContent(Encoding.UTF8.GetString(ms.ToArray()));
            }
        }

        private void btnBufferedStream_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "testfile.txt";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            using (BufferedStream bs = new BufferedStream(fs))
            using (CountingStreamDecorator countingStream = new CountingStreamDecorator(bs))
            {
                byte[] data = Encoding.UTF8.GetBytes("Это BufferedStream!");
                countingStream.Write(data, 0, data.Length);
                UpdateTotalBytesWritten(countingStream.TotalBytesWritten);
            }
            UpdateFileContent(File.ReadAllText(filePath));
        }
    }
}
