using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AutoWeg.API.Hubs
{
    // Hub class SignalR ki base Hub class se inherit hoti hai
    public class GarageHub : Hub
    {
        // Jab WinForm se car status update hoga, toh woh is method ko call karega
        public async Task UpdateCarStatus(string carNumber, string newStatus)
        {
            // Yeh server se jude sabhi clients (Android apps) ko 'ReceiveStatusUpdate' event bhejega
            await Clients.All.SendAsync("ReceiveStatusUpdate", carNumber, newStatus);
        }
    }
}
