using System;
using System.Threading.Tasks;

namespace Spbs.Ui.Data;

public interface IReaderRepositoryBase<TDto> where TDto : class
{
    public Task<TDto> GetByIdAsync(Guid id);
}