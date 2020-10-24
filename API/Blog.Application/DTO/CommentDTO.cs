using System;

namespace Blog.Application.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Creator { get; set; }
        public string Content { get; set; }
    }
}
