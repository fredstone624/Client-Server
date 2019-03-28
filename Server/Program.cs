using Core;
using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var connection = new Connection(ConnectionSettings.Host, ConnectionSettings.Port);
                var socket = connection.GetSocket();
                var backlog = 1;

                socket.Bind(connection.EndPoint);
                socket.Listen(backlog);

                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", connection.EndPoint);

                    // Программа приостанавливается, ожидая входящее соединение
                    var handler = socket.Accept();
                    
                    // Принимаем данные от клиента
                    var bytes = new byte[1024];
                    var size = handler.Receive(bytes);
                    var data = Encoding.UTF8.GetString(bytes, 0, size);

                    // Показываем данные на консоли
                    Console.WriteLine("Полученный текст: {0}", data);
                    Console.WriteLine("\n{0}\n", new string('-', 50));

                    // Отправляем ответ клиенту
                    string reply = "Спасибо за запрос в " + data.Length.ToString() + " символов";

                    var buffer = Encoding.UTF8.GetBytes(reply);
                    handler.Send(buffer);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
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