using NetMQ;
using NetMQ.Sockets;

namespace WeatherApp.Services
{
    public class ZeroMqClient
    {
        private readonly string _address;

        public ZeroMqClient(string address = "tcp://localhost:5555")
        {
            _address = address;
        }

        public string SendMessage(string message)
        {
            using (var client = new RequestSocket(_address))
            {
                Console.WriteLine("Sending request to weather image generator: " + message);
                client.SendFrame(message);

                // Get response from microservice
                string response = client.ReceiveFrameString();
                return response;
            }
        }
    }
}
