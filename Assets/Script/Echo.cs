using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Echo : MonoBehaviour
{
    private Socket socket;
    
    public InputField inputField;
    public Text text;
    
    public Button connectButton;
    public Button sendButton;
    
    //接受缓冲区
    byte[] readBuff = new byte[1024];
    private string recvStr = "";

    private void Awake()
    {
        connectButton.onClick.AddListener(Connection);
        sendButton.onClick.AddListener(Send);
    }

    private void Connection()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.BeginConnect("127.0.0.1", 8888, ConnectCallBack, socket);
    }

    private void ConnectCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Connect Success");
            socket.BeginReceive(readBuff,0,1024,0,ReceiveCallBack, socket);
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Connect Fail:" + e);
        }
    }

    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndReceive(ar);
            recvStr = System.Text.Encoding.Default.GetString(readBuff);
            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallBack, socket);
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void Send()
    {
        string sendStr = inputField.text;
        byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
        socket.Send(sendBytes);
    }

    private void Update()
    {
        text.text = recvStr;
    }
}
