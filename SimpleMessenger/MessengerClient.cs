using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace SimpleMessenger;

public delegate void SERVER_CONNECTION_DELIGATE(string serverIP, bool success);
public delegate void SERVER_JOIN(ClientInfo info);
public delegate void SERVER_LEAVE(ClientInfo info);
public delegate void SERVER_MSG(ClientInfo info, string msg, int remoteID, int line);
public delegate void SERVER_STATUS(ClientInfo info, string sts, ClientMessageType type);
public delegate void SERVER_List(List<ClientInfo> l);
public delegate void SERVER_BUZZ(int senderID);
public delegate void SERVER_DISCONNECTED_BY_SERVER();

public class MessengerClient
{
    
    /// <summary>
    /// Inroducing custom Events
    /// </summary>
    /// 
    public event SERVER_CONNECTION_DELIGATE ConnectionStatus;
    public event SERVER_LEAVE ClientLeaved;
    public event SERVER_MSG NewMsg;
    public event SERVER_STATUS NewStatus;
    public event SERVER_List NewList;
    public event SERVER_BUZZ NewBuzz;
    public event SERVER_DISCONNECTED_BY_SERVER DisconnectByServer;
    

    private Dictionary<int, ClientInfo> _ClientDic;
    //public List<int> NumberoOfMessage = [];
    //public List<string> NumberoOfMessageString = [];
    public bool MessageSound = true;
    private ClientInfo Me;
    private SocketListener _Listener;
    public string ServerIP;
    public int ServerPort;
    public string SelfIP;
    readonly Timer Timer = new(3000);
    readonly Timer TimerForAlive = new(3000);




    /// <summary>
    /// Sharing Dictionary instance with other class
    /// </summary>
    public Dictionary<int, ClientInfo> ClientDic => _ClientDic;


    /// <summary>
    /// Shareing SocketListener instance with other class.
    /// </summary>
    public SocketListener Listener => _Listener;


    /// <summary>
    /// Defalt Constructor.
    /// </summary>
    public MessengerClient()
    {

    }

    
    /// <summary>
    /// Start listening in a port, send joining message to server,
    /// </summary>
    /// <param name="serverIP"></param>
    /// <param name="name"></param>
    public void Start(string serverIP,int serverPort, string name)
    {
        this.ServerIP = serverIP;
        this.ServerPort = serverPort;
        Me = new ClientInfo
        {
            IP = Program.OwnIP,
            Name = name
        };

        _ClientDic = [];
        _Listener = new SocketListener(0, GotClientMessage);
        Program.App.Info.ListenPort = Listener.Port;
        Me.ListenPort = Listener.Port;
        // Sending joining message to server 
        ClientMessage msg = new()
        {
            Info = Me,
            Type = (int)ClientMessageType.Join
        };

        Listener.Send(this.ServerIP, this.ServerPort, msg.Serialize());

        Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
        Timer.Start();
        TimerForAlive.Elapsed += new ElapsedEventHandler(TimerForAlive_Elapsed);
        TimerForAlive.Start();
    }

    /// <summary>
    /// It will send a alive message within 3 seconds.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void TimerForAlive_Elapsed(object sender, ElapsedEventArgs e)
    {
        TimerForAlive.Stop();
        //throw new NotImplementedException();
        ClientMessage m = new()
        {
            Type = (int)ClientMessageType.Alive,
            Info = Program.App.Info
        };
        Listener.Send(Program.App.ServerIP,this.ServerPort, m.Serialize());
        TimerForAlive.Start();
    }



    /// <summary>
    /// if client become unable to connect with server within 3 seconds.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        ConnectionStatus(ServerIP, false);
        Timer.Stop();
    }



    /// <summary>
    /// When Client get any message. Here Message type will be checked. 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="dataAvailable"></param>
    private void GotClientMessage(byte[] data, int dataAvailable)
    {
        var message = ClientMessage.DeSerialize(data);
        //Checking Which Type of Message Client got.
        switch ((ClientMessageType)message.Type)
        {
            case ClientMessageType.ClientList:

                Timer.Stop();
                Program.App.Info.ClientID = message.Info.ClientID;        
                foreach (ClientInfo info in message.CurrentClients)
                {
                    if (ClientDic.ContainsKey(info.ClientID) == false)
                        ClientDic.Add(info.ClientID, info);
                }
                ConnectionStatus?.Invoke(ServerIP, true);
                break;



            case ClientMessageType.ClientListForALL:

                message.CurrentClients.Add(message.Info);
                foreach (ClientInfo info in message.CurrentClients)
                {
                    if (ClientDic.ContainsKey(info.ClientID) == false)
                        ClientDic.Add(info.ClientID, info);
                }
                NewList?.Invoke(message.CurrentClients);
                NewStatus?.Invoke(message.Info, message.Status, ClientMessageType.Join);
                break;
           


            case ClientMessageType.Msg:
                NewMsg?.Invoke(message.Info, message.Msg, message.From, message.LineNumb);
                break;



            case ClientMessageType.Disconnect:
                NewStatus?.Invoke(message.Info, message.Status, ClientMessageType.Disconnect);
                if (ClientDic.ContainsKey(message.Info.ClientID) == true)
                    ClientDic.Remove(message.Info.ClientID);
                ClientLeaved?.Invoke(message.Info);
                break;

            case ClientMessageType.Status:
                NewStatus?.Invoke(message.Info, message.Status, ClientMessageType.Status);
                break;

            case ClientMessageType.Buzz:

                NewBuzz?.Invoke(message.From);
                break;

            case ClientMessageType.DisconnectedByServer:
                DisconnectByServer?.Invoke();
                break;
        }
    }



    /// <summary>
    /// This will stop all client thread and dispose.
    /// </summary>
    public void Dispose()
    {
        Listener.RunServer = false;

        foreach (var form in Program.App.Forms.Values)
        {
            form.Close();
            form.Dispose();

        }
        Program.App.Forms.Clear();
        Program.App.Client.ClientDic.Clear();
    }
}


