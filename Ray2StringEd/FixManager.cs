using System.Collections.Generic;
using System.IO;
using System.Text;
using R2MagicDecoder;

namespace Ray2StringEd
{
    public class FixManager
    {
        public FixManager(string path, int offset = 0x7051F4)
        {
            Path = path;
            Offset = offset;
        }

        private string Path { get; }
        private int Offset { get; }
        private byte[] FixBytes { get; set; }

        public IEnumerable<FixString> ReadFix()
        {
            FixBytes = MagicDecoder.DecodeBytes(File.ReadAllBytes(Path));
            using (MemoryStream fix = new MemoryStream(FixBytes))
            {
                fix.Seek(Offset, SeekOrigin.Begin);

                using (BinaryReader reader = new BinaryReader(fix))
                {
                    long fixLength = fix.Length;
                    long position;

                    while ((position = reader.BaseStream.Position) != fixLength)
                    {
                        int length = reader.ReadInt32();

                        if (length > 0x200 || length <= 0)
                            continue;
                        
                        byte[] bytes = reader.ReadBytes((length - 1) * 4);
                        string text = Encoding.GetEncoding(1252).GetString(bytes).Trim('\0');

                        yield return new FixString(text, length, position);
                    }
                }
            }
        }

        public void WriteFix(IEnumerable<FixString> strings)
        {
            using (MemoryStream fix = new MemoryStream(FixBytes))
            using (BinaryWriter writer = new BinaryWriter(fix))
            {
                foreach (FixString s in strings)
                {
                    fix.Seek(s.Offset, SeekOrigin.Begin);

                    string text = s.Text + '\0';
                    byte[] bytes = Encoding.GetEncoding(1252).GetBytes(text);

                    writer.Write(s.DwordLength);
                    writer.Write(bytes);

                    int freeLength = s.ByteLength - bytes.Length - 4;
                    if (freeLength > 0)
                    {
                        byte[] freeBytes = new byte[freeLength];
                        writer.Write(freeBytes);
                    }
                }
            }

            File.WriteAllBytes(Path, MagicDecoder.DecodeBytes(FixBytes));
        }

        public void BackupFix()
        {
            string bakPath = Path + ".bak";

            if (!File.Exists(bakPath))
                File.Copy(Path, bakPath);
        }
    }
}