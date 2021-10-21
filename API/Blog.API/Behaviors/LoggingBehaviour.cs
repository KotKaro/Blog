using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Domain.DataAccess;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Blog.API.Behaviors
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("Starting request of type: {requestType}", typeof(TRequest).Name);
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                _logger.LogError("Processing: {requestType} - throws exception: {message}", typeof(TRequest).Name, ex.Message);
                throw;
            }
            finally
            {
                _logger.LogInformation("Processing: {requestType} - ended", typeof(TRequest).Name);
            }
        }
    }
}
