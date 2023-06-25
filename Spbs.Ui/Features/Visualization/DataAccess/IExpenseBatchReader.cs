using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spbs.Ui.Features.Visualization.Models;

namespace Spbs.Ui.Features.Visualization.DataAccess;

public interface IExpenseBatchReader
{
    Task<List<ExpenseVisualizationModel>> GetAllExpensesByUserForMonth(Guid userId, DateOnly month);

    Task<List<ExpenseVisualizationModel>> GetAllExpensesByUserBetweenDates(Guid userId, DateTime fromDate,
        DateTime? toDate);
}