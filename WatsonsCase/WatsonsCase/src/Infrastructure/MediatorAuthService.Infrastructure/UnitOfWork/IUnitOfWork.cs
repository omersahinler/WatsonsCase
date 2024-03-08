using WatsonsCase.Domain.Core.Base.Abstract;
using WatsonsCase.Domain.Core.Base.Concrete;
using WatsonsCase.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WatsonsCase.Infrastructure.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, IEntity;

    Task<int> SaveChangesAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task CommitAsync(bool isSaveChanges = true);

    Task RollBackAsync();
}

public interface IUnitOfWork<TContext> : IUnitOfWork, IAsyncDisposable where TContext : DbContext
{
    TContext Context { get; }
}