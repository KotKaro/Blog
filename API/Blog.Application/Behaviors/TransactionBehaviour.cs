using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Domain.DataAccess;
using MediatR;

namespace Blog.Application.Behaviors
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionBehaviour(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is not IRequest)
            {
                return await next();
            }
            
            if (_unitOfWork.HasActiveTransaction)
            {
                return await next();
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            var response = await next();
            await _unitOfWork.CommitTransactionAsync(transaction);

            return response;
        }
    }
}
