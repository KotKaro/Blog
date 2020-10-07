using System;

namespace Blog.Domain.DataAccess
{
    public interface ITransaction : IDisposable
    {
        Guid Id { get; }

        void Commit();

        void Rollback();
    }
}
