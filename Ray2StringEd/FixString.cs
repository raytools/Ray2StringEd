namespace Ray2StringEd
{
    public class FixString
    {
        public FixString(string text, int dwordLength, long offset)
        {
            Text = text;
            DwordLength = dwordLength;
            Offset = offset;
        }

        public string Text { get; set; }
        public int DwordLength { get; }
        public long Offset { get; }

        public int ByteLength => DwordLength * 4;
        public int MaxTextLength => ByteLength - 5;
    }
}