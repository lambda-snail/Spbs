using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Spbs.Shared.Data;

public class ReaderRepositoryBase<TDto, TDbCOntext> : IAsyncDisposable,
    IReaderRepositoryBase<TDto> where TDbCOntext : DbContext
    where TDto : class
{
    protected TDbCOntext _db { get => _contextFactory.CreateDbContext(); }

    private IDbContextFactory<TDbCOntext> _contextFactory;
    
    public ReaderRepositoryBase(IDbContextFactory<TDbCOntext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public virtual async Task<TDto?> GetByIdAsync(Guid id)
    {
        var result = await _db.Set<TDto>().FindAsync(id);
        return result;
    }

    public ValueTask DisposeAsync()
    {
        return _db.DisposeAsync();
    }
}