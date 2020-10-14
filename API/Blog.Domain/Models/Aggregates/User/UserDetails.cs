using System;
using System.Collections.Generic;

namespace Blog.Domain.Models.Aggregates.User
{
    public class UserDetails : ValueObject
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        private UserDetails() { }

        public UserDetails(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
            }

            Username = username;
            Password = password;
        }

        protected override IEnumerable<object> GetAtomicVales()
        {
            yield return Username;
            yield return Password;
        }
    }
}