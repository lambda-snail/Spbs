using System;
using System.Collections.Generic;

namespace Spbs.Ui.Features.BankIntegration.Models;

public class NordigenEula
{
    public Guid Id { get; init; }

    /// <summary>
    /// The date &amp; time at which the end user agreement was created.
    /// </summary>
    public DateTime Created { get; init; }

    /// <summary>
    /// Maximum number of days of transaction data to retrieve.
    /// </summary>
    public int MaxHistoricalDays { get; set; }

    /// <summary>
    /// Number of days from acceptance that the access can be used.
    /// </summary>
    public int AccessValidForDays { get; set; }

    /// <summary>
    /// Array containing one or several values of [&#39;balances&#39;, &#39;details&#39;, &#39;transactions&#39;]
    /// </summary>
    public string[] AccessScope { get; set; } = new string[] { "balances" };

    /// <summary>
    /// The date &amp; time at which the end user accepted the agreement.
    /// </summary>
    public DateTime? Accepted { get; init; }

    /// <summary>
    /// An Institution ID for this EUA
    /// </summary>
    public string InstitutionId { get; set; }
}