namespace ChartGraphQLAPI.Logic
{
    public class DataSource
    {
        public string type { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public List<string> label_x { get; set; }
        public string label_y { get; set; }
        public List<string> data { get; set; }
        public string colour { get; set; }

    }
}
