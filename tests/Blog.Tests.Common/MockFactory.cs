using System;
using Blog.Application.Commands.CreatePost;
using Blog.Application.Commands.UpdatePost;
using Blog.Application.Queries.GetPostById;
using Blog.Application.Queries.GetPosts;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Models.Aggregates.User;

namespace Blog.Tests.Common
{
    public static class MockFactory
    {
        public static GetPostsQuery CreateGetPostsQuery(int pageNumber = 1, int pageSize = 10)
            => new GetPostsQuery
            {
                PageSize = pageSize,
                PageNumber = pageNumber
            };

        public static Post CreatePost(Guid? id = null, Title title = null, Content content = null)
        {
            return new Post(
                id ?? Guid.NewGuid(),
                title ?? CreateTitle(),
                content ?? CreateContent()
            );
        }

        private static Content CreateContent(string content = "content")
        {
            return new Content(content);
        }

        private static Title CreateTitle(string title = "title")
        {
            return new Title(title);
        }

        public static CreatePostCommand CreateCreatePostCommand(string title = "title", string content = "content")
        {
            return new CreatePostCommand
            {
                Id = Guid.NewGuid(),
                Title = title,
                Content = content
            };
        }

        public static GetPostByIdQuery CreateGetByIdQuery(Guid? id = null)
        {
            return new GetPostByIdQuery
            {
                Id = id ?? Guid.NewGuid()
            };
        }

        public static User CreateUser(UserDetails userDetails = null)
        {
            return new User(userDetails ?? CreateUserDetails());
        }

        private static UserDetails CreateUserDetails(string username = "test", string password = "test")
        {
            return new UserDetails(username, password);
        }

        public static UpdatePostCommand CreateUpdatePostCommand(Guid? postId = null, string title = "title", string content = "content")
        {
            return new UpdatePostCommand
            {
                Id = postId ?? Guid.NewGuid(),
                Title = title,
                Content = content,
            };
        }
    }
}
