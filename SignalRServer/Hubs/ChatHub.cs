using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace SignaRServer

{
    public class ChatHub :  Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("-->connection established " + Context.ConnectionId); 
            Clients.Client(Context.ConnectionId).SendAsync("RecieveConnID", Context.ConnectionId);
            return base.OnConnectedAsync();
        }       

        public async Task SendMessageAsync(string message) 
        {
            var routeOb = JsonConvert.DeserializeObject<dynamic>(message);
            string toClient = routeOb.To;
            Console.WriteLine("Message Recieved on:" + Context.ConnectionId);

            if (toClient == string.Empty)
            {
                await Clients.All.SendAsync("ReceiveMessage",message);
                Console.WriteLine("toClient empty, BROADCAST TO ALL CLIENTS:" + "Message" + message);
            }

            else
            {
                await Clients.Client(toClient).SendAsync("ReceiveMessage",message);
                Console.WriteLine("toClient specified, sending to"+  Context.ConnectionId + message);
            }
        }
    }
}