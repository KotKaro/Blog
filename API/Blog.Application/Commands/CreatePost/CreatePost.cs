using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Blog.Application.Commands.CreatePost
{
    public class CreatePost : IRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
