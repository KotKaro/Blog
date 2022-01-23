using System;

namespace Blog.Domain.Repositories.PostReadRepository
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Creator { get; set; }
        public string Content { get; set; }
    }
}
