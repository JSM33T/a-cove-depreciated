namespace almondcove.Models.DTO.Media.Studio
{
    public class StudioItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded{ get; set; }
        public bool IsActive{ get; set; }
    }
}
