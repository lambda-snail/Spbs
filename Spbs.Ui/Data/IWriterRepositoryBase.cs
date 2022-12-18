using System.Threading.Tasks;

namespace Spbs.Ui.Data;

public interface IWriterRepositoryBase<TDto> where TDto : class
{
    Task<TDto> InsertAsync(TDto row);
    Task UpdateAsync(TDto row);
    Task DeleteAsync(TDto row);
}