using AutoMapper;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories.PostReadRepository;

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
                    act => act.MapFrom(src => src.Content.Value))
                .ForMember(dest => dest.CreationDate,
                    act => act.MapFrom(src => src.CreationDate.Value))
                .ForMember(dest => dest.Comments,
                    act => act.MapFrom(src => src.Comments));
        }
    }
}
