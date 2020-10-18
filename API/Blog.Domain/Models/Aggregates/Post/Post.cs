using System;

namespace Blog.Domain.Models.Aggregates.Post
{
    public class Post : Entity, IAggregateRoot
    {
        public Title Title { get; private set; }
        public Content Content { get; private set; }
        public CreationDate CreationDate { get; private set; }

        private Post() { }

        public Post(Guid id, Title title, Content content) : base(id)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            CreationDate = new CreationDate();
        }

        public void UpdateContent(Content content)
        {
            Content = content;
        }

        public void UpdateTitle(Title title)
        {
            Title = title;
        }
    }
}
