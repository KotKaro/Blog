using Blog.IntegrationTests.Common;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Blog.IntegrationTests
{
    [CollectionDefinition(nameof(BlogTestCollection), DisableParallelization = true)]
    public class BlogTestCollection : ICollectionFixture<BlogApplicationFactory>
    {
        
    }
}