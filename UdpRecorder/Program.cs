
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;

namespace ConsoleApplication4
{

	class Program
	{
		static bool halt = false;
		static string[] packets;
		static void Main( string[] args )
		{
			var UdpRecv = new Thread( new ThreadStart( Loop ) );
			UdpRecv.IsBackground = true;
			UdpRecv.Start();
			Console.ReadKey();
			halt = true;
			UdpRecv.Join();
			File.WriteAllLines( "testdata.bin", packets );
		}

		Thread UdpRecv;
		UdpClient listener;

		// listen for udp packets in a separate thread.
		static void Loop()
		{
			var l = new List<string>();
			var listener = new UdpClient( 10001 ); // 10001 is the port, obv. remember to open the firewall!
			IPEndPoint groupEP = new IPEndPoint( IPAddress.Any, 10001 );
			try
			{
				while( !halt )
				{
					byte[] bytes = listener.Receive( ref groupEP );
					var str = System.Text.Encoding.UTF8.GetString( bytes );
					l.Add( DateTime.Now.Ticks + "@" + str );
					Thread.Sleep( 1 );
				}

			}
			catch( System.Exception e )
			{
			}
			finally
			{
				listener.Close();
			}
			packets = l.ToArray();
		}
	}
}