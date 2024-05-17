using System;

namespace Art_gall.Exceptions
{
    public class ExceptionHandling : Exception
    {
        public ExceptionHandling(string message)
        {
        }

        // Custom exception for ArtWork not found  (use only related to aet works)
        public class ArtWorkNotFoundException : ExceptionHandling
        {
            public ArtWorkNotFoundException(string message) : base(message) { }
        }

        // Custom exception for user not found  (use only related to users entities)
        public class UserNotFoundException : ExceptionHandling
        {
            public UserNotFoundException(string message) : base(message) { }
        }
    }
}
