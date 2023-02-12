using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Spbs.Shared.Data;

public class WriterRepositoryBase<TDto, TDbCOntext> : IAsyncDisposable, IWriterRepositoryBase<TDto> where TDbCOntext : DbContext
    where TDto : class
{
    protected TDbCOntext _db { get => _contextFactory.CreateDbContext(); }

    private IDbContextFactory<TDbCOntext> _contextFactory;

    public WriterRepositoryBase(IDbContextFactory<TDbCOntext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<TDto> InsertAsync(TDto row)
    {
        var result = _db.Set<TDto>().Add(row);
        await _db.SaveChangesAsync();
        return result.Entity;
    }
    
    public Task UpdateAsync(TDto row)
    {
        _db.Set<TDto>().Update(row);
        return _db.SaveChangesAsync();
    }

    public Task DeleteAsync(TDto row)
    {
        _db.Set<TDto>().Remove(row);
        return _db.SaveChangesAsync();
    }

    public ValueTask DisposeAsync()
    {
        return _db.DisposeAsync();
    }
}
