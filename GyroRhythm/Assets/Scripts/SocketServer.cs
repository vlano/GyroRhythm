using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;

public class SocketServer : MonoBehaviour
{
	[SerializeField]
	private GameObject _controllableGameObject;
	[SerializeField]
	private TMP_Text _ipText;
	private Thread _tcpListenerThread;
	private Socket _clientSocket;
	private Quaternion _rot;
	private string _data = null;

	private string _localIP;

	string received;
	void Start()
	{
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
		socket.Connect("192.168.0.1", 8100);
		IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
		_localIP = endPoint.Address.ToString();
		
		_ipText.text = "IP address:\n"+_localIP;

		_tcpListenerThread = new Thread(new ThreadStart(Listen));
		_tcpListenerThread.IsBackground = true;
		_tcpListenerThread.Start();
	}

	private void Update()
	{
		_controllableGameObject.transform.localRotation = _rot;
	}

	/// <summary>
	/// Listens for incoming data and converts it to a Quaternion.
	/// </summary>
	private void Listen()
	{
		IPAddress ipAddr = System.Net.IPAddress.Parse(_localIP);
		IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 8100);

		Socket listener = new Socket(ipAddr.AddressFamily,
				SocketType.Stream, ProtocolType.Tcp);

		listener.Bind(localEndPoint);
		listener.Listen(1);

		_clientSocket = listener.Accept();
		
		int size = sizeof(float);
		byte[] bytes = new byte[size * 4]; ;

		_rot = new Quaternion();
		while (true)
		{
			int numByte = _clientSocket.Receive(bytes);
			_rot.x = System.BitConverter.ToSingle(bytes, 0);
			_rot.y = System.BitConverter.ToSingle(bytes, size);
			_rot.z = System.BitConverter.ToSingle(bytes, size * 2);
			_rot.w = System.BitConverter.ToSingle(bytes, size * 3);

			received = _rot.eulerAngles.ToString();
		}
	}

	private void OnApplicationQuit()
	{
		_clientSocket.Shutdown(SocketShutdown.Both);
		_clientSocket.Close();
	}
}
