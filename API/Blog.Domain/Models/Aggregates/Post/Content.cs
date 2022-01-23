using System.Collections.Generic;
using Blog.Domain.Exceptions;

namespace Blog.Domain.Models.Aggregates.Post
{
    public class Content : ValueObject
    {
        public string Value { get; private set; }

        public Content(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidValueException("Value cannot be null or whitespace.", nameof(value));
            }

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicVales()
        {
            yield return Value;
        }
    }
}