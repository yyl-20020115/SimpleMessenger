using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace SimpleMessenger;


public delegate void SocketListenerMsg(byte[] data,int dataAvailable);
public delegate void ServerNotExist();

public class SocketListener
{

    //Introducing a custom event
    public event ServerNotExist ServerGone;

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
        var dataAvailable = 0;
        var msg = "";
        var remoteIP="";
        while (serverRunning)
        {
            
                if (listener.Pending())
                {
                    var c = listener.AcceptTcpClient();

                    // read data if available
                    if (c.Available > 0)
                    {
                        var ns = c.GetStream();
                        if (ns.CanRead)
                        {
                            byte[] data = new byte[1024 * 8];
                            dataAvailable = ns.Read(data, 0, data.Length);
                            msg = Encoding.ASCII.GetString(data, 0, dataAvailable);
                            remoteIP = ((IPEndPoint)c.Client.RemoteEndPoint).ToString();
                            // call delegate
                            _handler(data, dataAvailable);
                            
                        }
                    }
                    // close socket
                    c.Close();

                }
                else Thread.Sleep(10);
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
    public void Send(string ip, int port, byte[] data,bool broadCast=false)
    {
        var s = new Thread(new ParameterizedThreadStart(SendingThread));
        s.Start(new object[] { ip, port, data, broadCast });      
    }



    /// <summary>
    /// Start a thread for send meaage, AS it can take time.
    /// </summary>
    /// <param name="param"></param>
    private void SendingThread(object param)
    {
        object[] p=(object[]) param;
        string ip = (string)p[0];
        int port = (int)p[1];
        byte[] data = (byte[])p[2];
        //bool broadCast = (bool)p[3];
        try
        {
           
            var c = new TcpClient(ip,port);
            c.GetStream().Write(data, 0, data.Length);
            c.Close();
       }
        catch
        {
            if (RunServer == false)
                return;
            if (Program.App.IsServer == false)
            {
                ServerGone?.Invoke();
                Thread.Sleep(300);
            }
        }
    }
}
