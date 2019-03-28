using Core;
using System;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Устанавливаем удаленную точку для сокета
                var connection = new Connection(ConnectionSettings.Host, ConnectionSettings.Port);

                while (true)
                {
                    var sender = connection.GetSocket();

                    // Соединяем сокет с удаленной точкой
                    sender.Connect(connection.EndPoint);

                    Console.Write("Введите сообщение: ");
                    var message = Console.ReadLine();

                    Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
                    var buffer = Encoding.UTF8.GetBytes(message);

                    // Отправляем данные через сокет
                    sender.Send(buffer);

                    // Получаем ответ от сервера
                    var bytes = new byte[1024];
                    var size = sender.Receive(bytes);

                    Console.WriteLine("\nОтвет от сервера: {0}", Encoding.UTF8.GetString(bytes, 0, size));
                    Console.WriteLine("\n{0}\n", new string('-', 50));

                    // Освобождаем сокет
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}