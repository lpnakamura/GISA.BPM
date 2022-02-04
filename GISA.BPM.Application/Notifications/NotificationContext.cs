using FluentValidation.Results;
using GISA.BPM.Application.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace GISA.BPM.Application.Notifications
{
    public class NotificationContext : INotificationContext
    {
        private readonly List<Notification> _notifications;
        IReadOnlyCollection<Notification> INotificationContext.Notifications => _notifications;
        bool INotificationContext.HasNotifications => _notifications.Any();

        public NotificationContext()
        {
            _notifications = new List<Notification>();
        }


        public void AddNotification(string key, string message)
        {
            _notifications.Add(new Notification(key, message));
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotifications(IEnumerable<Notification> notificationList)
        {
            _notifications.AddRange(notificationList);
        }

        public void AddNotifications(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                AddNotification(error.ErrorCode, error.ErrorMessage);
        }
    }
}
