using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebSocket_API.WebSocket
{
    public class WebSocketHandler : WebSocketHandlerBase
    {
        public WebSocketHandler(WebSocket socket) : base(socket) { }

        public override async Task OnOpen()
        {
            await SendStringAsync("Connection established.");
        }

        public override async Task OnMessage(string message)
        {
            await SendStringAsync($"Received: {message}");
        }

        public override async Task OnClose()
        {
            await SendStringAsync("Connection closed.");
        }
    }
}
