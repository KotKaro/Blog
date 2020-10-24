using System;

namespace Blog.Application.Mappers.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException() : base("Incorrect username/password.")
        {

        }
    }
}
