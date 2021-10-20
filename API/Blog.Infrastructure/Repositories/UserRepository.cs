using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataAccess;
using Blog.Domain.DataAccess;
using Blog.Domain.Models.Aggregates.User;
using Blog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BlogDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        protected override IQueryable<User> GetQueryWithIncludes()
        {
            return DbContext.Set<User>();
        }

        public Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            
            return GetQueryWithIncludes()
                .FirstOrDefaultAsync(x =>
                    x.UserDetails.Username.ToLower() == username.ToLower());
        }
    }
}
