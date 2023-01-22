using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Azure;

namespace Spbs.Ui.Features.Expenses;

public class EditExpenseItemViewModel
{
    public Guid? Id { get; set; }

    [Required, MaxLength(128)]
    public string Name { get; set; }
    
    [Required, Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    
    [Required, Range(0, double.MaxValue)]
    public double Price { get; set; }
}