using System;
using Newtonsoft.Json;

namespace Spbs.Ui.Features.BankIntegration.ImportExpenses;

public class ImportExpensesViewModel
{
    public bool IncludeInImport { get; set; } = false;
    public bool IsPending { get; set; }
    
    public string TransactionId { get; set; } = String.Empty;
    public string DebtorName { get; set; } = String.Empty;
    public DebtorAccountViewModel DebtorAccount { get; set; } = new();
    public TransactionAmountViewModel TransactionAmount { get; set; } = new();
    public DateOnly BookingDate { get; set; }
    public DateOnly ValueDate { get; set; }
    public string RemittanceInformationUnstructured { get; set; } = string.Empty;    
}

public class TransactionAmountViewModel
{
    public string Currency { get; set; } = String.Empty;
    public double Amount { get; set; } = 0.0d;
}

public class DebtorAccountViewModel
{
    [JsonProperty("iban")]
    public string Iban { get; set; } = String.Empty;
}
