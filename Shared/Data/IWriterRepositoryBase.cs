using System.Threading.Tasks;

namespace Spbs.Shared.Data;

public interface IWriterRepositoryBase<TDto> where TDto : class
{
    Task<TDto> InsertAsync(TDto row);
    Task UpdateAsync(TDto row);
    Task DeleteAsync(TDto row);
}