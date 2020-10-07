using System;

namespace Blog.Application.Exceptions
{
    public class TokenInvalidException : Exception
    {
        public TokenInvalidException() : base("Incorrect token.")
        {

        }
    }
}
