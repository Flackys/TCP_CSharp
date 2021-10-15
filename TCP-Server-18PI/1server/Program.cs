using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketTcpServer
{
    class Program
    {
        static int port = 8005; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    int number = Convert.ToInt32(builder.ToString());
                    var str = String.Empty;
                    if (number.ToString().Length > 1) Console.WriteLine("Введено некорректное число");
                    else
                    {
                        if (number == 1) str += "Автомобиль ";
                        if (number == 2) str += "Автомобиль, Погода ";
                        if (number == 3) str += "Автомобиль, Погода, Дерево ";
                        if (number == 4) str += "Автомобиль, Погода, Дерево, Дом ";
                        if (number == 5) str += "Автомобиль, Погода, Дерево, Дом, Запись ";
                        if (number == 6) str += "Автомобиль, Погода, Дерево, Дом, Запись, Пример ";
                        if (number == 7) str += "Автомобиль, Погода, Дерево, Дом, Запись, Пример, Программа ";
                        if (number == 8) str += "Автомобиль, Погода, Дерево, Дом, Запись, Пример, Программа, Дублер ";
                        if (number == 9) str += "Автомобиль, Погода, Дерево, Дом, Запись, Пример, Программа, Дублер, Лимит ";

                        Console.WriteLine(str);
                        Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
                        Console.ReadLine();
                    }

                        //Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                        // отправляем ответ
                        string message = "информация доставлена";
                        data = Encoding.Unicode.GetBytes(message);
                        handler.Send(data);
                        // закрываем сокет
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}