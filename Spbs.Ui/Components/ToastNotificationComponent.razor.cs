using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

    private string GetIconFromOnNotificationLevel(NotificationLevel level)
    {
        string openingTag = """<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-info-circle-fill" viewBox="0 0 16 16">""";
        string closingTag = "</svg>";
        string svgPath = level switch
        {
            NotificationLevel.Info =>
                """<path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z"/>""",
            NotificationLevel.Success => 
                """<path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>""",
            NotificationLevel.Warning => 
                """<path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8 4a.905.905 0 0 0-.9.995l.35 3.507a.552.552 0 0 0 1.1 0l.35-3.507A.905.905 0 0 0 8 4zm.002 6a1 1 0 1 0 0 2 1 1 0 0 0 0-2z"/>""",
            NotificationLevel.Error => 
                """<path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293 5.354 4.646z"/>""",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        return openingTag + svgPath + closingTag;
    }
}