using System;
using System.Collections.Generic;

namespace Blog.Domain.Models.Aggregates.Post
{
    public class Post : Entity, IAggregateRoot
    {
        public Title Title { get; private set; }
        public Content Content { get; private set; }
        public CreationDate CreationDate { get; private set; }
        public IList<Comment> Comments { get; private set; }

        // ReSharper disable once UnusedMember.Local
        private Post() { }

        public Post(Guid id, Title title, Content content) : base(id)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            CreationDate = new CreationDate();
            Comments = new List<Comment>();
        }

        public void UpdateContent(Content content)
        {
            Content = content;
        }

        public void UpdateTitle(Title title)
        {
            Title = title;
        }

        public void AddComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            Comments.Add(comment);
        }
    }
}
