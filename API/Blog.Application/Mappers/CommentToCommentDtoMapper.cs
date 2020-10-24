using AutoMapper;
using Blog.Application.DTO;
using Blog.Domain.Models.Aggregates.Post;

namespace Blog.Application.Mappers
{
    public class CommentToCommentDtoMapper : Profile
    {
        public CommentToCommentDtoMapper()
        {
            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.Id,
                    act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Creator,
                    act => act.MapFrom(src => src.Creator.Value))
                .ForMember(dest => dest.Content,
                    act => act.MapFrom(src => src.Content.Value));
        }
    }
}
