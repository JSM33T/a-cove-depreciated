namespace almondcove.Models.Props
{
    public class MetaProps
    {
        public MetaProps()
        {
            Title = "laymaann";
            Description = "A webspace for photos, videos by laymaann";
            Tags = "laymaann, photography,portfolio";
            Image = "https://laymaann.in/assets/meta/banner.jpg";
            Url = "https://laymaann.in";
            Type = "Website";
            Author = "Laymaann | Karan Singh";
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; } 
        public string Image { get; set; }
        public string Url { get; set; } 
        public string Type { get; set; }
        public string Author { get; set; }
    }
}
