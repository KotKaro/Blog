using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Domain.Exceptions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;
using MediatR;

namespace Blog.Application.Commands.CreatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Unit>
    {
        private readonly IPostRepository _repository;

        public UpdatePostCommandHandler(IPostRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetByIdAsync(request.Id);

            if (post == null)
            {
                throw new RecordNotFoundException(typeof(Post), request.Id);
            }

            post.UpdateContent(new Content(request.Content));
            post.UpdateTitle(new Title(request.Title));
            
            return Unit.Value;
        }
    }
}
