using System;

namespace Spbs.Ui.Features.BankIntegration.Models;

public class TransactionsRequestParameters
{
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
};