using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Spbs.Ui.Features.BankIntegration.Models;

public class User
{
    [JsonProperty("id")] 
    public Guid Id { get; set; }

    [JsonProperty("nordigenLinks")] 
    public List<Guid> NordigenLinks { get; set; } = new ();
}