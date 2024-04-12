//using System;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;

//public class CryptoStreamDecorator : Stream
//{
//    private readonly Stream _baseStream;
//    private readonly CryptoStream _cryptoStream;

//    public CryptoStreamDecorator(Stream baseStream, ICryptoTransform transform)
//    {
//        _baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
//        _cryptoStream = new CryptoStream(_baseStream, transform, CryptoStreamMode.Read);
//    }

//    public override bool CanRead => _cryptoStream.CanRead;
//    public override bool CanSeek => false;
//    public override bool CanWrite => false;
//    public override long Length => _baseStream.Length;
//    public override long Position
//    {
//        get => _baseStream.Position;
//        set => throw new NotSupportedException();
//    }

//    public override void Flush()
//    {
//        _cryptoStream.Flush();
//    }

//    public override int Read(byte[] buffer, int offset, int count)
//    {
//        return _cryptoStream.Read(buffer, offset, count);
//    }

//    public override long Seek(long offset, SeekOrigin origin)
//    {
//        throw new NotSupportedException();
//    }

//    public override void SetLength(long value)
//    {
//        throw new NotSupportedException();
//    }

//    public override void Write(byte[] buffer, int offset, int count)
//    {
//        throw new NotSupportedException();
//    }

//    public string ReadText()
//    {
//        byte[] buffer = new byte[_baseStream.Length];
//        _baseStream.Position = 0;
//        _baseStream.Read(buffer, 0, buffer.Length);
//        return Encoding.UTF8.GetString(buffer);
//    }
//}
