using System;
using System.Threading.Tasks;

namespace Spbs.Shared.Data;

public interface IReaderRepositoryBase<TDto> where TDto : class
{
    public Task<TDto> GetByIdAsync(Guid id);
}