using System;
using Blog.Domain.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.DataAccess
{
    public class DbContextTransactionDecorator : ITransaction
    {
        public Guid Id { get; }

        public IDbContextTransaction DbContextTransaction { get; private set; }

        public DbContextTransactionDecorator(IDbContextTransaction transaction)
        {
            DbContextTransaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            Id = Guid.NewGuid();
        }

        public void Dispose()
        {
            DbContextTransaction.Dispose();
        }

        public void Commit()
        {
            DbContextTransaction.Commit();
        }

        public void Rollback()
        {
            DbContextTransaction.Rollback();
        }
    }
}