namespace Ray2StringEd
{
    public class FixString
    {
        internal FixString() { }

        public FixString(string text, int dwordLength, long offset)
        {
            Text = text;
            DwordLength = dwordLength;
            Offset = offset;
        }

        public string Text { get; set; }
        public int DwordLength { get; set; }
        public long Offset { get; set; }
        
        public int ByteLength => DwordLength * 4;
        public int MaxTextLength => ByteLength - 5;
    }
}