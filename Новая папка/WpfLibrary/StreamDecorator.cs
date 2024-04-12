using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLibrary
{
    public abstract class StreamDecorator : Stream
    {

        protected Stream st;
        public byte[] savedBytes;

        public StreamDecorator(Stream st)
        {
            this.st = st;
        }

        public override bool CanRead => st.CanRead;

        public override bool CanSeek => st.CanSeek;

        public override bool CanWrite => st.CanWrite;

        public override long Length => st.Length;

        public override long Position { get => st.Position; set => st.Position = value; }

        public override void Flush()
        {
            st.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return st.Read(buffer, offset, count);
        }

        public int ReadWithCombination(byte[] buffer, int offset, int count, byte[] combination, int m)
        {

            int bytesRead;
            if (st is MemoryStream)
            {
                // Если это MemoryStream, используйте ReadByte для чтения байтов
                MemoryStream memoryStream = (MemoryStream)st;
                bytesRead = 0;
                for (int i = 0; i < count; i++)
                {
                    int nextByte = memoryStream.ReadByte();
                    if (nextByte == -1)
                        break; // Достигнут конец потока
                    buffer[offset + i] = (byte)nextByte;
                    bytesRead++;
                }
            }
            else
            {
                // First, read from the base stream
                bytesRead = st.Read(buffer, offset, count);
            }

            // Check if the combination is present in the read bytes
            if (CheckCombination(buffer, bytesRead, combination))
            {
                // If the combination is found, save the next m bytes
                SaveNextBytes(buffer, offset, bytesRead, m);
            }

            return bytesRead;
        }

        private bool CheckCombination(byte[] buffer, int bytesRead, byte[] combination)
        {
            // Check if the combination exists in the buffer
            for (int i = 0; i <= bytesRead - combination.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < combination.Length; j++)
                {
                    if (buffer[i + j] != combination[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return true;
            }
            return false;
        }

        private void SaveNextBytes(byte[] buffer, int offset, int bytesRead, int m)
        {
            // Calculate how many bytes to save
            int bytesToSave = Math.Min(m, bytesRead - offset);

            // Initialize or resize the savedBytes array
            if (savedBytes == null)
                savedBytes = new byte[bytesToSave];
            else if (savedBytes.Length != bytesToSave)
                Array.Resize(ref savedBytes, bytesToSave);

            // Copy the bytes to savedBytes
            Array.Copy(buffer, offset, savedBytes, 0, bytesToSave);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return st.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            st.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            st.Write(buffer, offset, count);
        }
    }
}
