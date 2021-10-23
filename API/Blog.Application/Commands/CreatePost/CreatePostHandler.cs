using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;
using MediatR;

namespace Blog.Application.Commands.CreatePost
{
    public class CreatePostHandler : IRequestHandler<CreatePost, Unit>
    {
        private readonly IPostRepository _repository;

        public CreatePostHandler(IPostRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(CreatePost request, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(
            new Post
                (
                    request.Id,
                    new Title(request.Title),
                    new Content(request.Content)
                )
            );

            return Unit.Value;
        }
    }
}
