using Blog.IntegrationTests.Common;
using Xunit;

namespace Blog.IntegrationTests
{
    [CollectionDefinition(nameof(BlogTestCollection))]
    public class BlogTestCollection : ICollectionFixture<BlogApplicationFactory>
    {
        
    }
}