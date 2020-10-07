using System;

namespace Blog.Application.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException() : base("Incorrect username/password.")
        {

        }
    }
}
