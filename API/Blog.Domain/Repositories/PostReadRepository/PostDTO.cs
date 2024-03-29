﻿using System;

namespace Blog.Domain.Repositories.PostReadRepository
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public CommentDTO[] Comments { get; set; }
    }
}
