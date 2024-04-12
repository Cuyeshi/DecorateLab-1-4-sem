
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace WpfLibrary
//{
//    public class SeekComboDecorator : FileStreamDecorator
//    {
//        public SeekComboDecorator(FileStream fileStream) : base(fileStream)
//        {
//        }

//        public List<byte[]>? Action(byte[] data ,byte[] combination, int m)
//        {
//            List<byte[]> res = new List<byte[]>();
//            byte[] bufCopy = new byte[data.Length];
//            Array.Copy(data, bufCopy, data.Length);
//            long pos = 0;

//            while (pos < data.Length - combination.Length)
//            {
//                pos = FindPos(bufCopy, combination);
//                if (pos == -1)
//                {
//                    break;
//                }
//                else
//                {
//                    bufCopy[pos] = 0;
//                    pos += combination.Length;
//                    byte[] resPart = new byte[m];
//                    for (long i = 0; i < m; i++)
//                    {
//                        if (bufCopy.Length > pos + i)
//                        {
//                            resPart[i] = bufCopy[pos + i];
//                        }
//                        else
//                        {
//                            break;
//                        }
//                    }
                    

//                    res.Add(resPart);
//                    //MessageBox.Show("Bylo");
//                }
//            }
//            return res;
//        }

//        private long FindPos(byte[] data, byte[] combination)
//        {
//            for (long i = 0; i <= data.Length - combination.Length; i++)
//            {
//                bool found = true;

//                for (long j = 0; j < combination.Length; j++)
//                {
//                    if (data[i + j] != combination[j])
//                    {
//                        found = false;
//                        break;
//                    }
//                }

//                if (found)
//                    return i;
//            }

//            return -1;
//        }
//    }
//}
