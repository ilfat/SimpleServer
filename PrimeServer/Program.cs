using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace PrimeServer
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			try
			{
				int port = int.Parse (args [0]);
				new Server().Run(port);
			} catch (IndexOutOfRangeException)
			{
				Console.Write ("Ошибка. Не введен номер порта.");
			}
			catch (Exception e) {
				Console.WriteLine (e.Message);
				//Если например введен отрицательный порт, порт занят или другие ошибки, просто выходим  
			}
		}
	}
}
