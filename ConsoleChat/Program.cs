using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRChatClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                 .WithUrl("http://localhost:5000/chatHub")
                 .Build();

            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

            await connection.StartAsync();

            Console.WriteLine("Enter your name:");
            var user = Console.ReadLine();

            Console.WriteLine("Enter room name to join:");
            var room = Console.ReadLine();

            await connection.InvokeAsync("JoinRoom", room);

            Console.WriteLine($"Joined room {room}. You can start chatting now.");

            while (true)
            {
                var message = Console.ReadLine();
                await connection.InvokeAsync("SendMessageToRoom", room, user, message);
            }
        }
    
    }
}