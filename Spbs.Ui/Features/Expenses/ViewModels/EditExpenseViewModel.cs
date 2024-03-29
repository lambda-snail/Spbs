using System;
using System.ComponentModel.DataAnnotations;

namespace Spbs.Ui.Features.Expenses;

/// <summary>
/// Model used to validate expenses that are edited or created in the UI.
/// </summary>
public class EditExpenseViewModel
{
    public Guid? Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }

    public bool Recurring { get; set; } = false;

    public Guid OwningUserId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public string? Venue { get; set; }
    public string? Currency { get; set; }

    public string? Tags { get; set; }
}