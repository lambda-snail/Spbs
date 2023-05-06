using System;
using System.Collections.Generic;

namespace Spbs.Ui.ComponentServices;

public interface INotificationService
{
    event Action? OnToastAdded;
    event Action? OnToastRemoved;
    bool HasToasts { get; }
    void ShowToast(string heading, string message, NotificationLevel level);
    IEnumerable<ToastMessage> GetToasts();
    void RemoveToast(ToastMessage toast);
}