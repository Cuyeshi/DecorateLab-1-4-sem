using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfLibrary;

namespace LR2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Stream memoryStream = new MemoryStream();
        public Stream stream;

        public MainWindow()
        {
            InitializeComponent();
            FileTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            memoryStream.Seek(0, SeekOrigin.Begin);
        }

        private void Choose_Doc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            bool? success = fileDialog.ShowDialog();
            if (success == true)
            {
                filePathBox.Text = fileDialog.FileName;
                FileTextBox.Text = "Прочитай файл\n";
            }
            else
            {
                return;
            }
        }

        private void Reed_file_Click(object sender, RoutedEventArgs e)
        {
            string path = filePathBox.Text;

            if (!File.Exists(path)) { MessageBox.Show("Такого файла не существует."); return; }

            string decoratorType = Decorator.Text;
            switch (decoratorType)
            {
                case "FileStream":
                    stream = new FileStream(path, FileMode.Open);
                    using (DecoratedStream decoratedFileStream = new DecoratedStream(stream))
                    {
                        byte[] buffer = new byte[decoratedFileStream.Length];
                        byte[] catched;
                        byte[] combination = GetCombination();

                        decoratedFileStream.ReadWithCombination(buffer, 0, buffer.Length, combination, Int32.Parse(mValBox.Text));

                        catched = decoratedFileStream.savedBytes;
                        CatchedBytesBox.Clear();
                        if (catched != null) foreach (byte b in catched) CatchedBytesBox.Text += b + " ";

                        StringBuilder sb = new StringBuilder();
                        foreach (byte b in buffer)
                        {
                            sb.Append(b.ToString()).Append(' ');
                        }
                        FileTextBox.Text = sb.ToString();

                        string text = Encoding.UTF8.GetString(buffer);

                        FileTextBox.Text = "FileStream: Файл успешно прочитан!\n";
                        FileTextBox.Text += text;

                        stream.Close();
                    }
                    break;
                case "BufferStream":
                    stream = new BufferedStream(new FileStream(path, FileMode.Open));
                    using (DecoratedStream decoratedBufferStream = new DecoratedStream(stream))
                    {
                        byte[] buffer = new byte[decoratedBufferStream.Length];
                        byte[] catched;
                        byte[] combination = GetCombination();

                        decoratedBufferStream.ReadWithCombination(buffer, 0, buffer.Length, combination, Int32.Parse(mValBox.Text));

                        catched = decoratedBufferStream.savedBytes;
                        CatchedBytesBox.Clear();
                        if (catched != null) foreach (byte b in catched) CatchedBytesBox.Text += b + " ";

                        StringBuilder sb = new StringBuilder();
                        foreach (byte b in buffer)
                        {
                            sb.Append(b.ToString()).Append(' ');
                        }
                        FileTextBox.Text = sb.ToString();

                        string text = Encoding.UTF8.GetString(buffer);

                        FileTextBox.Text = "BufferStream: Файл успешно прочитан!\n";
                        FileTextBox.Text += text;

                        stream.Close();
                    }
                    break;
                case "MemoryStream":
                    using (MemoryStream stream = new MemoryStream())
                    {

                        using (FileStream fs = new FileStream(path, FileMode.Open))
                        {
                            fs.CopyTo(stream);
                        }

                        stream.Seek(0, SeekOrigin.Begin);

                        using (DecoratedStream decoratedBufferStream = new DecoratedStream(stream))
                        {
                            byte[] buffer = new byte[decoratedBufferStream.Length];
                            byte[] catched;
                            byte[] combination = GetCombination();

                            decoratedBufferStream.ReadWithCombination(buffer, 0, buffer.Length, combination, Int32.Parse(mValBox.Text));

                            catched = decoratedBufferStream.savedBytes;
                            CatchedBytesBox.Clear();
                            if (catched != null) foreach (byte b in catched) CatchedBytesBox.Text += b + " ";

                            StringBuilder sb = new StringBuilder();
                            foreach (byte b in buffer)
                            {
                                sb.Append(b.ToString()).Append(' ');
                            }
                            FileTextBox.Text = sb.ToString();

                            string text = Encoding.UTF8.GetString(buffer);

                            FileTextBox.Text = "MemoryStream: Файл успешно прочитан!\n";
                            FileTextBox.Text += text;

                            stream.Close();
                        }
                    }
                    break;
            }


        }

        public byte[] GetCombination()
        {
            List<byte> result = new List<byte>();

            string comboLine = CombiBox.Text;
            string[] slicedCombo = comboLine.Split(' ');
            foreach (string s in slicedCombo)
            {
                try
                {
                    result.Add(Byte.Parse(s));
                }
                catch (Exception e)
                {
                    throw new FormatException("Parse Error");
                }
            }

            return result.ToArray();
        }
    }
}