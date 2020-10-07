using System.Linq;
using System.Reflection;
using AutoMapper;
using Blog.Application.Mappers;

namespace Blog.Tests.Common
{
    public class Mapper
    {
        private static IMapper mapper;
        public static IMapper GetInstance()
        {
            mapper ??= CreateMapper();
            return mapper;
        }

        private static IMapper CreateMapper()
        {
            var assembly = Assembly.GetAssembly(typeof(PostToPostDtoMapper));
            var profiles = assembly
                .GetTypes()
                .Where(t => t.BaseType == typeof(Profile) && !t.IsAbstract && t.IsPublic);
            var configuration = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

            return configuration.CreateMapper();
        }
    }
}
