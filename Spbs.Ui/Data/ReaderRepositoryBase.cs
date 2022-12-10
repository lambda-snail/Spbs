using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Spbs.Ui.Data;

public class ReaderRepositoryBase<TDto, TDbCOntext> : IAsyncDisposable,
    IReaderRepositoryBase<TDto> where TDbCOntext : DbContext
    where TDto : class
{
    protected TDbCOntext _db { get; set; }

    public ReaderRepositoryBase(IDbContextFactory<TDbCOntext> factory)
    {
        _db = factory.CreateDbContext();
    }

    public async Task<TDto> GetByIdAsync(Guid id)
    {
        var result = await _db.Set<TDto>().FindAsync(id);
        return result;
    }

    public ValueTask DisposeAsync()
    {
        return _db.DisposeAsync();
    }
}