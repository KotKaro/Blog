using System;

namespace Blog.Auth.Abstractions
{
    public class JwtToken
    {
        public string Value { get; private set; }

        public JwtToken(string value)
        {
            Value = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentNullException(nameof(value));
        }
    }
}
