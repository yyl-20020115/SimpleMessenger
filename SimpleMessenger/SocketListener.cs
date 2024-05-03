﻿using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;

namespace SimpleMessenger;


public delegate void SocketListenerMsg(byte[] data, int dataAvailable);
public delegate void ServerNotExist();

public class SocketListener
{
    public static string SMMP_MAGIC = "SMMP";
    public static int MaxMessageLength = int.MaxValue;

    //Introducing a custom event
    public event ServerNotExist ServerLost;

    public int Port;
    private bool serverRunning = true;
    private readonly SocketListenerMsg _handler;
    private readonly Thread listenerThread = null;
    private readonly TcpListener listener;


    public SocketListener(int port, SocketListenerMsg handler)
    {
        _handler = handler;
        Port = port;

        listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();
        Port = ((IPEndPoint)listener.LocalEndpoint).Port;

        listenerThread = new Thread(new ThreadStart(ServerThread));
        listenerThread.Start();
    }


    /// <summary>
    /// Creat a New thread for Continuously listening in a port. 
    /// </summary>
    public void ServerThread()
    {
        while (serverRunning)
        {
            if (!listener.Pending())
                Thread.Sleep(10);
            else
            {
                var client = listener.AcceptTcpClient();
                // read data if available
                if (client.Available > 0)
                {
                    var stream = client.GetStream();
                    if (stream.CanRead)
                    {
                        var buffer = new byte[SMMP_MAGIC.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        var magic = Encoding.ASCII.GetString(buffer);
                        if (magic == SMMP_MAGIC)
                        {
                            stream.Read(buffer, 0, buffer.Length);
                            var length = BitConverter.ToInt32(buffer, 0);
                            if (length <= MaxMessageLength)
                            {
                                buffer = new byte[length];
                                var dataAvailable = stream.Read(buffer, 0, buffer.Length);
                                if (dataAvailable == length)
                                {
                                    var msg = Encoding.ASCII.GetString(buffer, 0, dataAvailable);
                                    var remoteIP = ((IPEndPoint)client.Client.RemoteEndPoint).ToString();
                                    // call delegate
                                    _handler(buffer, dataAvailable);
                                }
                            }
                        }
                    }
                }
                // close socket
                client.Close();
            }
        }
        listener.Stop();
    }



    /// <summary>
    /// Passing information about weather Socket listening or not, to other class.
    /// </summary>
    public bool RunServer
    {
        get => serverRunning;
        set
        {
            if (serverRunning != value)
            {
                serverRunning = value;
            }
        }
    }



    /// <summary>
    /// Sending message in specific ip and port.
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <param name="data"></param>
    /// <param name="broadCast"></param>
    public void SendData(string ip, int port, byte[] data)
    {
        Task.Run(() => {
            try
            {
                using var client = new TcpClient(ip, port);
                using var stream = client.GetStream();
                //header:
                //SimpleMessage Message Packet : 4 Bytes
                //Content Length : 4 Bytes
                //Content Text : N Bytes 
                stream.Write(Encoding.ASCII.GetBytes(SMMP_MAGIC), 0, SMMP_MAGIC.Length);
                stream.Write(BitConverter.GetBytes(data.Length), 0, sizeof(int));
                stream.Write(data, 0, data.Length);
            }
            catch
            {
                if (!RunServer)
                    return;
                if (!Program.App.IsServer)
                {
                    ServerLost?.Invoke();
                    Thread.Sleep(300);
                }
            }
        });
    }
}
