using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Spbs.Ui.Data;

public class WriterRepositoryBase<TDto, TDbCOntext> : IAsyncDisposable, IWriterRepositoryBase<TDto> where TDbCOntext : DbContext
    where TDto : class
{
    protected TDbCOntext _db { get; set; }

    public WriterRepositoryBase(IDbContextFactory<TDbCOntext> factory)
    {
        _db = factory.CreateDbContext();
    }

    public virtual async Task<TDto> InsertAsync(TDto row)
    {
        var result = _db.Set<TDto>().Add(row);
        await _db.SaveChangesAsync();
        return result.Entity;
    }
    
    public virtual Task UpdateAsync(TDto row)
    {
        _db.Set<TDto>().Update(row);
        return _db.SaveChangesAsync();
    }

    public virtual Task DeleteAsync(TDto row)
    {
        _db.Set<TDto>().Remove(row);
        return _db.SaveChangesAsync();
    }

    public ValueTask DisposeAsync()
    {
        return _db.DisposeAsync();
    }
}
