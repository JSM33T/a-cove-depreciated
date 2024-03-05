namespace almondcove.Models.DTO.Media.Gallery
{
    public class AlbumItemsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Slug { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class AlbumCollection{
        public string Name { get; set; }
        public string Year { get; set; }
        public string Desc { get; set; }
        public List<AlbumItemsDTO> Images { get; set; }
        
    }
}
