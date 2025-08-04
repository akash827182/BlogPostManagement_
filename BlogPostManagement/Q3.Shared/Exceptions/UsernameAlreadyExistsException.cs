using System;

namespace Q3.Shared.Exceptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException() : base("Username already exists.") { }
    }
}
