using System;

namespace Blog.Domain.Models.Aggregates.Post
{
    public class Post : Entity, IAggregateRoot
    {
        public Title Title { get; private set; }
        public Content Content { get; private set; }

        private Post() { }
        public Post(Title title, Content content)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public Post(Guid id, Title title, Content content) : base(id)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }
}
