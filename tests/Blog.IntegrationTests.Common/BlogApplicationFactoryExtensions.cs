using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blog.Application.Commands.CreatePost;
using Blog.Application.Commands.UpdatePost;
using Blog.Application.DTO;
using Blog.Auth.Abstractions;
using Microsoft.Extensions.Primitives;

namespace Blog.IntegrationTests.Common;

public static class BlogApplicationFactoryExtensions
{
    public static async Task<PostDTO[]> GetPosts(this BlogApplicationFactory factory, int pageNumber = 1,
        int pageSize = 10)
    {
        return await (await factory.Server.CreateClient()
                .GetAsync($"/posts?{nameof(pageNumber)}={pageNumber}&{nameof(pageSize)}={pageSize}"))
            .Content
            .ReadFromJsonAsync<PostDTO[]>();
    }

    public static async Task<PostDTO> GetPostById(this BlogApplicationFactory factory, Guid id)
    {
        return await (await factory.Server.CreateClient().GetAsync($"/posts/{id}"))
            .Content
            .ReadFromJsonAsync<PostDTO>();
    }
    
    public static async Task<HttpResponseMessage> GetPostByIdNoParse(this BlogApplicationFactory factory, Guid id)
    {
        return await factory.Server.CreateClient().GetAsync($"/posts/{id}");
    }

    public static async Task<PostDTO> CreatePost(this BlogApplicationFactory factory, CreatePost createPost)
    {
        return await (await factory.Server.CreateClient().PostAsync("/posts",
                new StringContent(System.Text.Json.JsonSerializer.Serialize(createPost)))).Content
            .ReadFromJsonAsync<PostDTO>();
    }

    public static async Task<HttpResponseMessage> UpdatePostWithoutHeaders(this BlogApplicationFactory factory,
        UpdatePostCommand updatePost)
    {
        return await factory.Server.CreateClient().PatchAsync("/posts",
            new StringContent(System.Text.Json.JsonSerializer.Serialize(updatePost), Encoding.UTF8,
                "application/json"));
    }

    public static async Task<HttpResponseMessage> UpdatePost(this BlogApplicationFactory factory,
        UpdatePostCommand updatePost)
    {
        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(updatePost),
            Encoding.UTF8,
            "application/json");

        foreach (var (key, value) in factory.GetHeadersWithToken())
        {
            content.Headers.Add(key, value.ToString());
        }

        return await factory.Server.CreateClient().PatchAsync("/posts", content);
    }

    private static Dictionary<string, StringValues> GetHeadersWithToken(this BlogApplicationFactory factory)
    {
        var jwtService = (IJwtService)factory.Services.GetService(typeof(IJwtService));
        var token = jwtService!
            .GenerateToken(new Claim("username", "John"))
            .Value;

        return new Dictionary<string, StringValues> { { "jwt-token", token } };
    }
    
    public static async Task<HttpResponseMessage> DeletePost(this BlogApplicationFactory factory, Guid id)
    {
        var request = factory.Server.CreateRequest($"/posts/{id}");

        foreach (var (key, value) in factory.GetHeadersWithToken())
        {
            request.AddHeader(key, value.ToString());
        }

        return await request.SendAsync(HttpMethod.Delete.Method);
    }
}