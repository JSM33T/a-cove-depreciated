namespace almondCove.Models.Props
{
    public class HTMLProps
    {
        public HTMLProps()
        {
            DataBsTheme = "";
            HeaderClass = "navbar mx-md-4 navbar-expand-lg fixed-top bg-light navbar-frost bt-rad-1";
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
