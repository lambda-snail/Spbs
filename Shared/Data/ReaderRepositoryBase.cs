using Microsoft.EntityFrameworkCore;

namespace Spbs.Shared.Data;

public class ReaderRepositoryBase<TDto, TDbCOntext> : IAsyncDisposable,
    IReaderRepositoryBase<TDto> where TDbCOntext : DbContext
    where TDto : class
{
    //protected TDbCOntext _db { get => _contextFactory.CreateDbContext(); }

    protected IDbContextFactory<TDbCOntext> _contextFactory;
    
    public ReaderRepositoryBase(IDbContextFactory<TDbCOntext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public virtual async Task<TDto?> GetByIdAsync(Guid id)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        var result = await db.Set<TDto>().FindAsync(id);
        return result;
    }

    public ValueTask DisposeAsync()
    {
        //return _db.DisposeAsync();
        return ValueTask.CompletedTask;
    }
}