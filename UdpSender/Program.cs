using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
	class DLine
	{
		public long TimeStamp { get; set; }
		public static long Offset = 0;
		public byte[] Data { get; set; }

		public string tag { get; set; }

		public DLine( string line )
		{
			var components = line.Split( '@' );
			Data = Encoding.UTF8.GetBytes( components[1] );
			tag = components[1].Substring( 0, Math.Min( 10, components[1].Length ) );
			TimeStamp = Int64.Parse( components[0] );
		}
	}
	class Program
	{
		static int port = 10001;
		static IPEndPoint endpoint;
		static void Main( string[] args )
		{
			endpoint = new IPEndPoint( new IPAddress( new byte[] { 127, 0, 0, 1 } ), port );
			string infile = "testdata.bin";
			var slist = File.ReadAllLines( infile ).Select( l => new DLine( l ) ).ToArray();


			using( UdpClient c = new UdpClient( 45232 ) )
			{

				while( SendList( slist, c ) )
					Thread.Sleep( 4000 );
			}
		}

		static bool SendList( DLine[] list, UdpClient c )
		{
			var firststamp = list[0].TimeStamp;
			var timestarted = DateTime.Now.Ticks;
			DLine.Offset = timestarted - firststamp;

			foreach( var line in list )
			{
				while( true )
				{
					if( DateTime.Now.Ticks > line.TimeStamp + DLine.Offset )
					{
						c.Send( line.Data, line.Data.Length, endpoint );
						break;
					}

					//if( Abort() )
					//	return false;
					//else
					Thread.Yield();
				}
			}
			return true;
		}
	}
}
