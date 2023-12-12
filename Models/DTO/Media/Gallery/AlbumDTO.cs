namespace almondcove.Models.DTO.Media.Gallery
{
    public class AlbumDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Category { get; set; }
        public string Year { get; set; }
        public bool IsActive { get; set; }
        public string Slug { get; set; }
    }
}
