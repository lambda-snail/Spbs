using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spbs.Data.Cosmos;
using Spbs.Ui.Features.Visualization.Models;

namespace Spbs.Ui.Features.Visualization.DataAccess;

public interface IExpenseBatchReader<TModel> where TModel : class, ICosmosData
{
    Task<List<TModel>> GetAllExpensesByUserForMonth(Guid userId, DateOnly month);

    Task<List<TModel>> GetAllExpensesByUserBetweenDates(Guid userId, DateTime fromDate,
        DateTime? toDate);
}