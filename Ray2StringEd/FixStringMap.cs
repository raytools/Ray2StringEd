using CsvHelper.Configuration;

namespace Ray2StringEd
{
    public class FixStringMap : ClassMap<FixString>
    {
        public FixStringMap()
        {
            Map(x => x.Offset).Name("Offset");
            Map(x => x.DwordLength).Name("Length");
            Map(x => x.MaxTextLength).Name("MaxTextLength");
            Map(x => x.Text).Name("Text");
        }
    }
}