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
            TotalBytesWrittenText.Text = $"Total Bytes Written: {totalBytesWritten}";
        }

        private void WriteToFile_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "testfile.txt";
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            using (CountingStreamDecorator countingStream = new CountingStreamDecorator(fileStream))
            {
                byte[] data = Encoding.UTF8.GetBytes("Hello, FileStream!");
                countingStream.Write(data, 0, data.Length);
                UpdateTotalBytesWritten(countingStream.TotalBytesWritten);
            }
        }

        private void WriteToMemory_Click(object sender, RoutedEventArgs e)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (CountingStreamDecorator countingStream = new CountingStreamDecorator(memoryStream))
            {
                byte[] data = Encoding.UTF8.GetBytes("Hello, MemoryStream!");
                countingStream.Write(data, 0, data.Length);
                UpdateTotalBytesWritten(countingStream.TotalBytesWritten);

                // Прочитать данные из MemoryStream и отобразить их в текстовом блоке
                memoryStream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(memoryStream);
                string content = reader.ReadToEnd();
                MessageBox.Show(content, "Data Written to Memory");
            }
        }

        private void WriteToBuffered_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "testfile.txt";
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            using (BufferedStream bufferedStream = new BufferedStream(fileStream))
            using (CountingStreamDecorator countingStream = new CountingStreamDecorator(bufferedStream))
            {
                byte[] data = Encoding.UTF8.GetBytes("Hello, BufferedStream!");
                countingStream.Write(data, 0, data.Length);
                UpdateTotalBytesWritten(countingStream.TotalBytesWritten);
            }
        }
    }
}
