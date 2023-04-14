using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tigernet.Hosting.DataAccess.Brokers;
using Tigernet.Hosting.DataAccess.Models.Entity;
using Tigernet.Hosting.DataAccess.Models.Query;
using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Brokers;

public class EfCoreDataAccessBroker : DbContext, IDataSourceBroker
{
    public DbSet<User> Users => Set<User>();

    private static DbContextOptions<EfCoreDataAccessBroker> GetOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<EfCoreDataAccessBroker>();
        optionsBuilder.UseInMemoryDatabase("Tigernet");
        return optionsBuilder.Options;
    }

    [Obsolete("Only for testing purposes, use the constructor with options passed")]
    public EfCoreDataAccessBroker() : this(GetOptions())
    {
        Database.EnsureCreated();
    }

    public EfCoreDataAccessBroker(DbContextOptions<EfCoreDataAccessBroker> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasData(new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Mukhammadkarim",
                    Age = 12
                },
                new User
                {
                    Id = 2,
                    Name = "Samandar",
                    Age = 32
                },
                new User
                {
                    Id = 3,
                    Name = "Djakhongir",
                    Age = 35
                },
                new User
                {
                    Id = 4,
                    Name = "Ixtiyor",
                    Age = 56
                },
                new User
                {
                    Id = 5,
                    Name = "Yunusjon",
                    Age = 34
                },
                new User
                {
                    Id = 6,
                    Name = "Sabohat",
                    Age = 23
                }
            });
    }

    public IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IEntity
    {
        return Set<TEntity>().Where(expression);
    }

    public async ValueTask<TEntity> CreateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity
    {
        var entry = await Set<TEntity>().AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public ValueTask<TEntity> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity
    {
        var entry = Set<TEntity>().Update(entity);
        return new ValueTask<TEntity>(entry.Entity);
    }

    public ValueTask<TEntity> DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity
    {
        var entry = Set<TEntity>().Remove(entity);
        return new ValueTask<TEntity>(entry.Entity);
    }

    public async ValueTask<bool> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken) > 0;
    }

    public IReadOnlyList<Type> GetEntityTypes()
    {
        return this.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.PropertyType.InheritsOrImplements(typeof(DbSet<>)))
            .Select(x => x.PropertyType.GenericTypeArguments.First())
            .ToList();
    }
}