using System;
using System.Runtime.CompilerServices;
#pragma warning disable CS8618

namespace Spbs.Ui.Features.BankIntegration.Models;

public class Institution
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Bic { get; set; }
    public string TransactionTotalDays { get; set; }
    public string Logo { get; set; }
    
    public override string ToString()
    {
        return Name;
    }
    
    public override bool Equals(object? input)
    {
        if (input?.GetType() != typeof(Institution))
        {
            return false;
        }
        
        Institution? institution = input as Institution;
        return Id == institution?.Id;
    }
}