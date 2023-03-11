using System;
using System.Diagnostics;
using System.Linq;
using Azure.Data.AppConfiguration;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Spbs.Ui.ComponentServices;

namespace Spbs.Ui.Components;

public partial class ToastNotificationComponent : IDisposable
{
    [Inject] private NotificationService _notificationService { get; set; }

    protected override void OnInitialized()
    {
        _notificationService.OnToastAdded += ToastAddedToast;
        _notificationService.OnToastRemoved += ToastRemovedToast;
    }

    private void ToastAddedToast()
    {
        StateHasChanged();
    }

    private void ToastRemovedToast()
    {
        Console.WriteLine($"Toast removed. {_notificationService.GetToasts().Count()} toasts remaining!");
        StateHasChanged();
    }
    
    private (string, string) BuildToastSettings(ToastMessage toast)
    {
        string backgroundCssClass = String.Empty;
        string iconCssClass = String.Empty;
        
        switch (toast.Level)
        {
            case NotificationLevel.Info:
                backgroundCssClass = $"bg-info";
                iconCssClass = "info";
                break;
            case NotificationLevel.Success:
                backgroundCssClass = $"bg-success";
                iconCssClass = "check";
                break;
            case NotificationLevel.Warning:
                backgroundCssClass = $"bg-warning";
                iconCssClass = "exclamation";
                break;
            case NotificationLevel.Error:
                backgroundCssClass = "bg-danger";
                iconCssClass = "times";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(toast.Level), toast.Level, null);
        }

        return (backgroundCssClass, iconCssClass);
    }

    void IDisposable.Dispose()
    {
        _notificationService.OnToastAdded -= ToastAddedToast;
        _notificationService.OnToastRemoved -= ToastRemovedToast;
    }

    private string GetTimeSinceCreatedText(TimeOnly time)
    {
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
        TimeSpan interval = now - time;

        return interval.Seconds switch
        {
            <10 => "A few seconds ago",
            >=10 and <60 => "Less than a minute ago",
            >=60 and <300 => "Less than five minutes ago",
            _ => interval.Minutes.ToString() + " minutes ago"
        };
    }
}