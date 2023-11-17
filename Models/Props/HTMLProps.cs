namespace almondCove.Models.Props
{
    public class HTMLProps
    {
        public HTMLProps()
        {
            DataBsTheme = "";
            HeaderClass = "navbar navbar-expand-lg fixed-top bg-light";
            NavClass = "";
            BodyClass = "";
            IsLoaderActive = "active";
            HasCode = false;
        }
        public string DataBsTheme { get; set; }
        public string HeaderClass { get; set; }
        public string NavClass { get; set; }
        public string BodyClass { get; set; }
        public string IsLoaderActive { get; set; }
        public bool HasCode { get; set; }

       
    }
}
