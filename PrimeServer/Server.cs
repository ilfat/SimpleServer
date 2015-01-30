using System;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace PrimeServer
{
	public class Server
	{
		const long NUMBER_PARAMETER_MAX = 5000000000;

		public void Run(int port)
		{
			try
			{
				Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.Bind (new IPEndPoint (IPAddress.Any, port));
				socket.Listen (port);

				while (true)
				{
					Socket connection = socket.Accept ();
					try
					{
						string gotParam = readRequest (connection);
						long gotLongParam = long.Parse (gotParam);
						if (gotLongParam > 0 && gotLongParam < NUMBER_PARAMETER_MAX)
							connection.Send (createResponse ((++gotLongParam).ToString ()));
					} catch
					{
						//Не обрабатываем ошибки, неправильные протоколы,неправильные имена параметров. Просто если запрос не соответствует представленному в задании = игнорируем.
					}
					try
					{
						connection.Shutdown (SocketShutdown.Both);
						connection.Close ();
					} catch
					{
						//Если соединение было прервано или другие проблемы
					}
				}
			} catch (IndexOutOfRangeException)
			{
				Console.Write ("Ошибка. Не введен номер порта.");
			} catch
			{
			}
		}

		string readRequest(Socket socket)
		{
			using (var networkStream = new NetworkStream (socket))
			{
				using (var reader = new StreamReader (networkStream))
				{
					return parceGetParams (reader.ReadLine ());
				}
			}
		}

		string[] paramsSplit = { " ", "&" };

		string parceGetParams(string line)
		{
			Console.WriteLine (line);
			return Regex.Split (line, "GET /nearestPrime\\?.*number=") [1].Split (paramsSplit, StringSplitOptions.None) [0];
		}

		byte[] createResponse(string s)
		{
			byte[] responseBytes = Encoding.Default.GetBytes (s);
			string response = "HTTP/1.1 200 OK\r\n" + "Server: Prime server\r\n"
			                  + "Content-Type: text/html\r\n" + "Content-Length: "
			                  + responseBytes.Length + "\r\n" + "Connection: close\r\n\r\n";
			string result = response + s;
			return Encoding.Default.GetBytes (result);
		}
	}
}
