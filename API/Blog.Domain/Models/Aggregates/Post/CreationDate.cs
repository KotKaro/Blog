using System;
using System.Collections.Generic;

namespace Blog.Domain.Models.Aggregates.Post
{
    public class CreationDate : ValueObject
    {
        public DateTime Value { get; private set; }
        public CreationDate()
        {
            Value = DateTime.Today;
        }

        protected override IEnumerable<object> GetAtomicVales()
        {
            yield return Value;
        }
    }
}