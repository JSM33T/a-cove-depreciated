namespace almondcove.Models.Props
{
    public class MetaProps
    {
        public MetaProps()
        {
            Title = "almondcove";
            Description = "A webspace for photos, videos by almondcove";
            Tags = "almondcove,blog, blogs,movies,series,morbid,urbex,urbanexploration,portfolio";
            Image = "https://almondcove.in/assets/meta/banner.jpg";
            Url = "https://almondcove.in";
            Type = "Website";
            Author = "almondcove | Karan Singh";
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
