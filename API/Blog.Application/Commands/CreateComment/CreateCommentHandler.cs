using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Domain.Exceptions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;
using MediatR;

namespace Blog.Application.Commands.CreateComment
{
    public class CreateCommentHandler : IRequestHandler<CreateComment, Unit>
    {
        private readonly IPostRepository _repository;

        public CreateCommentHandler(IPostRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(CreateComment request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetByIdAsync(request.PostId);

            if (post == null)
            {
                throw new RecordNotFoundException(typeof(Post), request.Id);
            }

            post.AddComment(new Comment(
                request.Id,
                new Creator(request.Creator),
                new Content(request.Content)
            ));

            return Unit.Value;
        }
    }
}
