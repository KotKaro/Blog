using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Domain.DataAccess;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IMediator _mediator;
        public ITransaction CurrentTransaction { get; private set; }
        public bool HasActiveTransaction => CurrentTransaction != null;

        public EfUnitOfWork(DbContext context, IMediator mediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ITransaction> BeginTransactionAsync()
        {
            if (HasActiveTransaction)
            {
                return CurrentTransaction;
            }

            var newDbContextTransaction = await _context
                .Database
                .BeginTransactionAsync(IsolationLevel.ReadCommitted);

            var newTransactionDecorator = new DbContextTransactionDecorator(newDbContextTransaction);

            CurrentTransaction = newTransactionDecorator;

            return CurrentTransaction;
        }

        public async Task<int> SaveEntitiesAsync()
        {
            _context.ChangeTracker.DetectChanges();
            var entityEntries = _context.ChangeTracker
                .Entries<Entity>()
                .ToArray();

            var domainEvents = entityEntries
                .SelectMany(x => x.Entity?.DomainEvents ?? Enumerable.Empty<IDomainEvent>())
                .ToArray();

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

            return await _context.SaveChangesAsync();

        }

        public async Task CommitTransactionAsync(ITransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction != CurrentTransaction)
            {
                throw new InvalidOperationException($"Transaction: {transaction.Id} is not current");
            }

            try
            {
                await SaveEntitiesAsync();
                CurrentTransaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }

            finally
            {
                ClearTransaction();
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                CurrentTransaction?.Rollback();
            }
            finally
            {
                ClearTransaction();
            }
        }

        private void ClearTransaction()
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }
        }
    }
}
