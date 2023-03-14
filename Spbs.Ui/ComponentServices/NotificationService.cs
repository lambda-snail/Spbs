using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Timer = System.Threading.Timer;

namespace Spbs.Ui.ComponentServices;

public enum NotificationLevel
{
    Info,
    Success,
    Warning,
    Error
}

public struct ToastMessage
{
    public string Heading { get; set; }
    public string Message { get; set; }
    
    public NotificationLevel Level { get; set; }
    public bool IsVIsible { get; set; }
    public TimeOnly Created { get; init; }
}

public class NotificationService : INotificationService
{
    public event Action? OnToastAdded;
    public event Action? OnToastRemoved;
    private Timer? Countdown;

    private List<ToastMessage> _toastMessages = new();

    public void ShowToast(string heading, string message, NotificationLevel level)
    {
        _toastMessages.Add(new ToastMessage
        {
            Heading = heading,
            Message = message,
            Level = level,
            IsVIsible = true,
            Created = TimeOnly.FromDateTime(DateTime.Now)
        });
        
        OnToastAdded?.Invoke();
        //StartCountdown();
    }

    public IEnumerable<ToastMessage> GetToasts() => _toastMessages;
    public bool HasToasts => _toastMessages is { Count: > 0 };

    public void RemoveToast(ToastMessage toast)
    {
        Console.WriteLine($"Remove toast {toast.Heading}");
        _toastMessages.Remove(toast);
        OnToastRemoved?.Invoke();
    }
}