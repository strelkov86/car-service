using System;

namespace SibintekTask.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }

    public class NotFoundException<T> : NotFoundException
    {
        public NotFoundException(object id)
            : base($"{typeof(T).Name} with id {id} was not found")
        {
        }

        public NotFoundException()
            : base($"{typeof(T).Name} was not found")
        {
        }
    }
}
