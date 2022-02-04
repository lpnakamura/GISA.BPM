using GISA.BPM.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GISA.BPM.Api.Controllers
{
    [Produces("application/json")]
    public abstract class ApiBaseController : ControllerBase
    {
        private readonly INotificationContext _notificationContext;

        public ApiBaseController(INotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        protected ActionResult CustomResponse<TModel>(TModel model)
        {
            if (!_notificationContext.HasNotifications) return Ok(model);
            return BadRequest(_notificationContext.Notifications);
        }

        protected ActionResult CustomResponse()
        {
            return CustomResponse(Ok());
        }
    }
}
