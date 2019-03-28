using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Core
{
    public class Connection
    {
        public IPHostEntry Host { get; }
        public IPAddress Address { get; }
        public IPEndPoint EndPoint { get; }

        public Connection(string host, int port)
        {
            Host = Dns.GetHostEntry(host);
            Address = Host.AddressList.First();
            EndPoint = new IPEndPoint(Address, port);
        }

        public Socket GetSocket() =>
            GetSocket(Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        public Socket GetSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) =>
            new Socket(addressFamily, socketType, protocolType);
    }
}