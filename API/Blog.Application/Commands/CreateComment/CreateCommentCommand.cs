using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Blog.Application.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Creator { get; set; }
        public string Content { get; set; }
    }
}
