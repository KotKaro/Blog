using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Domain.Exceptions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;
using MediatR;

namespace Blog.Application.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly ICommentRepository _repository;

        public DeleteCommentCommandHandler(ICommentRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetByIdAsync(request.Id);
            if (post == null)
            {
                throw new RecordNotFoundException(typeof(Comment), request.Id);
            }

            _repository.Delete(post);

            return Unit.Value;
        }
    }
}
