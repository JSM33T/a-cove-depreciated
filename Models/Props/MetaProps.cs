namespace almondcove.Models.Props
{
    public class MetaProps
    {
        public MetaProps()
        {
            Title = "AlmondCove";
            Description = "A webspace for blogs , apps, music and resources";
            Tags = "AlmondCove, photography,portfolio,apps,blogs,lightroom,presets";
            Image = "https://almondcove.in/assets/meta/banner.jpg";
            Url = "https://almondcove.in";
            Type = "Website";
            Author = "jsm33t | Jass";
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
