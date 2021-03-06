﻿using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class UdpRecv : MonoBehaviour {
	int port = 10001;// 10001 is the port, obv. remember to open the firewall!

	// these fields become accesable to other scripts
	public float latestX =0;
	public float latestY=0;
	public float latestAngle=0;
	public float latestLength=0;
	
	Thread UdpThread;
	UdpClient listener;
	// Use this for initialization
	void Start () {
		UdpThread = new Thread(new ThreadStart(Loop));
		UdpThread.IsBackground = true;
		UdpThread.Start ();
	}
	
	// listen for udp packets in a separate thread.
	void Loop(){
		listener = new UdpClient(port);
		IPEndPoint groupEP = new IPEndPoint(IPAddress.Any,port);
		try 
		{
			while (true) 
			{
				byte[] bytes = listener.Receive( ref groupEP);
				var str = System.Text.Encoding.UTF8.GetString(bytes);
				var datasplit = str.Split('#');
				if( datasplit.Length > 1 )
				{
					var datapoint = datasplit[1].Split(' ');
					
					//foreach( var point in data)
					if( datapoint.Length > 0)
					{
						var point = datapoint[0];
						Debug.Log (point);
						var dataset = point.Split(',');
						if( dataset.Length >= 5)
						{
							float[] fs = new []{0.0f,0.0f,0.0f,0.0f,0.0f};
							var resX = float.TryParse(dataset[0],out fs[0]);
							var resY = float.TryParse(dataset[1],out fs[1]);
							var resD = float.TryParse(dataset[2],out fs[2]);
							var resA = float.TryParse(dataset[3],out fs[3]);
							if( resX && resY && resD && resA )
							{
								var t = new Transposer(640,480,Screen.width,Screen.height);
								latestX = t.X(fs[0]);
								latestY = t.Y (fs[1]);
								latestLength = fs[2];
								latestAngle = Transposer.Angl(fs[3]);
							}
							float latestId;
							var resID = float.TryParse(dataset[4],out latestId);
						}
					}
				}
				Thread.Sleep(1);			
			}
			
		} 
		catch (System.Exception e) 
		{
			Debug.Log(e.ToString());
		}
		finally
		{
			listener.Close();
		}
	}
	// Update is called once per frame
	void Update () {}
	
	void OnDisable() 
	{ 
		if ( UdpThread!= null) 
			UdpThread.Abort(); 
	} 
}

public class Transposer
{
	public Transposer( float cx, float cy, float screenx, float screeny )
	{
		CameraXRes = cx;
		CameraYRes = cy;
		ScreenXRes = screenx;
		ScreenYRes = screeny;
	}
	float CameraXRes;
	float CameraYRes;
	float ScreenXRes;
	float ScreenYRes;

	public float X( float invalue )
	{
		return  ScreenXRes * (invalue/CameraXRes);
	}
	
	public float Y( float invalue )
	{
		return (-ScreenYRes /CameraYRes) * invalue + ScreenYRes;
	}
	
	public static float Angl( float invalue )
	{
		return invalue * -1f;
	}
}