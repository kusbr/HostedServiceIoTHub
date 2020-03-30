using System;
using System.Text;
using System.Threading.Tasks;
using D2C.Services;
using D2C.Services.Hosted;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;

namespace D2C.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendController : ControllerBase
    {

        readonly DeviceManagerHostedService deviceService;

        public SendController(DeviceManagerHostedService iotService)
        {
            this.deviceService = iotService;
        }

        [HttpGet("/")]
        public async Task<ActionResult<String>> Get()
        {
            return "D2C is ready";
        }

        [HttpPost("/")]
        public async Task<StatusCodeResult> ReceiveDeviceMessage([FromHeader] String deviceId,   [FromBody] String message)
        {
            await this.deviceService.SendMessageAsync(deviceId, message);
            return new StatusCodeResult(201);
        }

      
    }
}