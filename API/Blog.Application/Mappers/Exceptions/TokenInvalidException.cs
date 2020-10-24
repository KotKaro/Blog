using System;

namespace Blog.Application.Mappers.Exceptions
{
    public class TokenInvalidException : Exception
    {
        public TokenInvalidException() : base("Incorrect token.")
        {

        }
    }
}
