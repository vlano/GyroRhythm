using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;

public class SocketClient : MonoBehaviour
{
	[SerializeField]
	private TMP_InputField _ipInput;
	private Socket _sender;
	private Gyroscope _gyro;
	public GameObject referenceObject;

	public int speed = 1000;
	private void Start()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		_gyro = Input.gyro;
		_gyro.enabled = true;
	}
	private void Update()
	{
		SendOrientation(referenceObject.transform.rotation);
	}

	/// <summary>
	/// Sends the rotation data as a quaternion to the server.
	/// </summary>
	/// <param name="quat">Quaternion rotation data to send.</param>
	private void SendOrientation(Quaternion quat)
	{
		if (_sender == null)
			return;
		int size = sizeof(float);
		byte[] buffer = new byte[size * 4];

		Array.Copy(BitConverter.GetBytes(quat.x), 0, buffer, 0, size);
		Array.Copy(BitConverter.GetBytes(quat.y), 0, buffer, size, size);
		Array.Copy(BitConverter.GetBytes(quat.z), 0, buffer, size * 2, size);
		Array.Copy(BitConverter.GetBytes(quat.w), 0, buffer, size * 3, size);


		int byteSent = _sender.Send(buffer);
	}

	/// <summary>
	/// Connects the client to the server.
	/// </summary>
	public void Connect()
	{		
		IPAddress ipAddr = System.Net.IPAddress.Parse(_ipInput.text);
		IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 8100);

		_sender = new Socket(ipAddr.AddressFamily,
				  SocketType.Stream, ProtocolType.Tcp);

		_sender.Connect(localEndPoint);

		referenceObject.GetComponent<GyroMover>().ResetOrientation();

	}
	private void OnApplicationQuit()
	{
		_sender.Shutdown(SocketShutdown.Both);
		_sender.Close();
	}
}
