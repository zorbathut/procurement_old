namespace POEApi.Model
{
    public class Tab
    {
        public string Name { get; set; }
        public int i { get; set; }
        public Colour Colour { get; set; }
        public string src { get; set; }
    }

    public class Colour
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
    }
}
