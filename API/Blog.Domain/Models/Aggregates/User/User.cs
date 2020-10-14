using System;

namespace Blog.Domain.Models.Aggregates.User
{
    public class User : Entity, IAggregateRoot
    {
        public UserDetails UserDetails { get; private set; }

        //For EF Core
        protected User() { }

        public User(UserDetails userDetails)
        {
            UserDetails = userDetails ?? throw new ArgumentNullException(nameof(userDetails));
        }
    }
}
