using AutoMapper;
using Blog.Application.DTO;
using Blog.Domain.Models.Aggregates.Post;

namespace Blog.Application.Mappers
{
    public class PostToPostDtoMapper : Profile
    {
        public PostToPostDtoMapper()
        {
            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.Id,
                    act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title,
                    act => act.MapFrom(src => src.Title.Value))
                .ForMember(dest => dest.Content,
                    act => act.MapFrom(src => src.Content.Value));
        }
    }
}
