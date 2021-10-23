using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application.Commands.CheckToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    public class BlogControllerBase : ControllerBase
    {
        protected readonly IMediator mediator;

        public BlogControllerBase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected Task CheckTokenAsync()
        {
            return mediator.Send(new CheckToken { Token = GetTokenHeaderValue() });
        }

        private string GetTokenHeaderValue()
        {
            var val = HttpContext?.Request?.Headers["jwt-token"];

            return val.HasValue ? val.Value.FirstOrDefault() : "no-token";
        }
    }
}