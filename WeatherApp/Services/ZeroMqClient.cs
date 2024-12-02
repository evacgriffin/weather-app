using NetMQ;
using NetMQ.Sockets;

namespace WeatherApp.Services
{
    public class ZeroMqClient
    {
        private string _address;

        public ZeroMqClient(string address = null)
        {
            _address = address;
        }

        public void SetAddress(string address)
        {
            _address = address;
        }

        public string SendMessage(string message)
        {
            if (string.IsNullOrEmpty(_address))
                throw new InvalidOperationException("Address is not set.");

            using (var client = new RequestSocket(_address))
            {
                Console.WriteLine($"Sending request to {_address}: {message}");
                client.SendFrame(message);

                // Get response from microservice
                string response = client.ReceiveFrameString();
                return response;
            }
        }
    }
}
