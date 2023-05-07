using System.Transactions;
using Newtonsoft.Json;

namespace Integrations.Nordigen.Models;

public class TransactionAmount
{
    public string Currency { get; set; } = String.Empty;
    public decimal Amount { get; set; } = 0.0m;
}

public class DebtorAccount
{
    [JsonProperty("iban")]
    public string Iban { get; set; } = String.Empty;
}

public class NordigenTransaction
{
    [JsonProperty("transactionId")]
    public string TransactionId { get; set; } = String.Empty;
    
    [JsonProperty("debtorName")]
    public string DebtorName { get; set; } = String.Empty;

    [JsonProperty("debtorAccount")]
    public DebtorAccount DebtorAccount { get; set; } = new();
    
    [JsonProperty("transactionAmount")]
    public TransactionAmount TransactionAmount { get; set; } = new();
    
    [JsonProperty("bookingDate")]
    public DateOnly BookingDate { get; set; }
    
    [JsonProperty("valueDate")]
    public DateOnly ValueDate { get; set; }

    [JsonProperty("remittanceInformationUnstructured")]
    public string RemittanceInformationUnstructured { get; set; } = string.Empty;
}

public class TransactionsPair
{
    [JsonProperty("booked")]
    public List<Transaction> Booked { get; set; } = new();

    [JsonProperty("pending")]
    public List<Transaction> Pending { get; set; } = new();
}

public class ListTransactionsResponse
{
    [JsonProperty("transactions")]
    public TransactionsPair Transactions { get; set; } = new();
}