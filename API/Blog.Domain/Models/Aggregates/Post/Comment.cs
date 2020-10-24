using System;

namespace Blog.Domain.Models.Aggregates.Post
{
    public class Comment : Entity
    {
        public Creator Creator { get; private set; }
        public Content Content { get; private set; }

        // ReSharper disable once UnusedMember.Local
        private Comment() { }

        public Comment(Guid id, Creator creator, Content content): base(id)
        {
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }
}
