using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using AutoWeg.API.Hubs;
using System;
using System.Threading.Tasks;

namespace AutoWeg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GarageController : ControllerBase
    {
        private readonly IHubContext<GarageHub> _hubContext;

        // Constructor ke zariye SignalR Hub ka access lena
        public GarageController(IHubContext<GarageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // WinForm software (AfzalAuto) is URL par data bhejega:
        // POST: https://localhost:XXXX/api/garage/update-status
        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] CarStatusRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.CarNumber))
            {
                return BadRequest(new { message = "Invalid data: Car number missing." });
            }

            try
            {
                // Yahan par live log print hoga console window mein
                Console.WriteLine($"[Cloud API Log]: Car {request.CarNumber} status changed to {request.NewStatus}");

                // SignalR ke zariye sabhi connected Android apps (.NET MAUI) ko instant push notification bhejna
                await _hubContext.Clients.All.SendAsync("ReceiveStatusUpdate", request.CarNumber, request.NewStatus);

                return Ok(new { message = "Data broadcasted to mobile devices in real-time!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Server Error: {ex.Message}" });
            }
        }
    }

    // Data packaging model
    public class CarStatusRequest
    {
        public string CarNumber { get; set; }
        public string NewStatus { get; set; }
    }
}
