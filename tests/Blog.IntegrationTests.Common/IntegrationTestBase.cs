using System.Threading.Tasks;
using Autofac;
using Blog.Domain.DataAccess;
using Blog.Infrastructure;
using NUnit.Framework;

namespace Blog.IntegrationTests.Common
{
    [SetUpFixture]
    [Parallelizable(ParallelScope.None)]
    public class IntegrationTestBase
    {
        private AppContainerStarter _containerStarter;
        public BlogDbContext BlogContext { get; private set; }

        protected IContainer Container;
        protected IUnitOfWork UnitOfWork;

        [OneTimeSetUp]
        public async Task OneTimeSetUpBase()
        {
            _containerStarter = new AppContainerStarter();
            Container = await _containerStarter.StartTestContainer();

            UnitOfWork = Container.Resolve<IUnitOfWork>();
            BlogContext = Container.Resolve<BlogDbContext>();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _containerStarter.DockerContainerStarter.PruneContainers();
        }
    }
}