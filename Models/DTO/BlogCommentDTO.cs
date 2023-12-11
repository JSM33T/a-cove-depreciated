namespace almondcove.Models.DTO
{
    public class BlogCommentDTO
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public string DateCommented { get; set; }

        public int UserId { get; set; }

        public string UsersName { get; set; }

        public string UserName { get; set; }

        public string Slug { get; set; }
    }
}
