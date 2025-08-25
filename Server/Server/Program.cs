using System.Net;
using System.Net.Sockets;

namespace _Server;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Socket listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipEndPoint = new IPEndPoint(ipAdd, 8888);
        listenfd.Bind(ipEndPoint);
        
        listenfd.Listen(0);
        Console.WriteLine("[服务器]启动成功");
        while (true)
        {
            Socket connfd = listenfd.Accept();
            Console.WriteLine("[服务器]Accept");
            
            byte[] buffer = new byte[1024];
            int count = connfd.Receive(buffer);
            string readStr = System.Text.Encoding.Default.GetString(buffer, 0, count);
            Console.WriteLine("[服务器接收]" + readStr);
            
            byte[] sendBuffer = System.Text.Encoding.Default.GetBytes(readStr);
            connfd.Send(sendBuffer);
        }
    }
}