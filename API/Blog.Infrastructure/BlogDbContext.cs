﻿using Blog.Domain.Models.Aggregates.Post;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);
        }
    }
}