using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Spbs.Shared.Data;

public class WriterRepositoryBase<TDto, TDbCOntext> : IAsyncDisposable, IWriterRepositoryBase<TDto> where TDbCOntext : DbContext
    where TDto : class
{
    //protected TDbCOntext _db { get => _contextFactory.CreateDbContext(); }

    protected IDbContextFactory<TDbCOntext> _contextFactory;

    public WriterRepositoryBase(IDbContextFactory<TDbCOntext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<TDto> InsertAsync(TDto row)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();

        var result = db.Set<TDto>().Add(row);
        await db.SaveChangesAsync();
        return result.Entity;
    }
    
    public async Task UpdateAsync(TDto row)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        
        db.Set<TDto>().Update(row);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(TDto row)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        
        db.Set<TDto>().Remove(row);
        await db.SaveChangesAsync();
    }

    public ValueTask DisposeAsync()
    {
        //return _db.DisposeAsync();
        return ValueTask.CompletedTask;
    }
}
