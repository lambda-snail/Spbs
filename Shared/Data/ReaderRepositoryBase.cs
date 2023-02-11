using Microsoft.EntityFrameworkCore;

namespace Spbs.Shared.Data;

public class ReaderRepositoryBase<TDto, TDbCOntext> : IAsyncDisposable,
    IReaderRepositoryBase<TDto> where TDbCOntext : DbContext
    where TDto : class
{
    protected TDbCOntext _db { get; set; }

    public ReaderRepositoryBase(TDbCOntext context)
    {
        _db = context;//.CreateDbContext();
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