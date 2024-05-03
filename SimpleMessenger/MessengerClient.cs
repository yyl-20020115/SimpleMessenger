using System.Collections.Generic;
using System.Timers;

namespace SimpleMessenger;

public delegate void SERVER_CONNECTION_DELIGATE(string serverIP, bool success);
public delegate void SERVER_JOIN(ClientInfo info);
public delegate void SERVER_LEAVE(ClientInfo info);
public delegate void SERVER_MSG(ClientInfo info, string msg, int remoteID, int line);
public delegate void SERVER_STATUS(ClientInfo info, string sts, ClientMsgType type);
public delegate void SERVER_List(List<ClientInfo> l);
public delegate void SERVER_BUZZ(int senderID);
public delegate void SERVER_DISCONNECTED_BY_SERVER();

/// <summary>
///  ************************** This Class is  For CLIENT****************************
///  ****Here Strat client's SocketListening thread, Message Analysis happens,and hold other neccesary info for client*********.
/// </summary>


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
    public int[] NumberoOfMessage = new int[1000];
    public string[] NumberoOfMessageString = new string[1000];
    public bool MessageSound = true;
    private ClientInfo Me;
    private SocketListener _Listener;
    public string ServerIP;
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
    public void Start(string serverIP,string name)
    {
        this.ServerIP = serverIP;
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
            Type = (int)ClientMsgType.Join
        };
        Listener.Send(serverIP,12345,msg.Serialize());
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
        ClientMessage m = new ClientMessage();
        m.Type = (int)ClientMsgType.Alive;
        m.Info = Program.App.Info;
        Listener.Send(Program.App.ServerIP,12345,m.Serialize());
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

        var msg = ClientMessage.DeSerialize(data, 0, dataAvailable);

        //Checking Which Type of Message Client got.
        switch ((ClientMsgType)msg.Type)
        {

            case ClientMsgType.ClientList:

                Timer.Stop();
                Program.App.Info.ClientID = msg.Info.ClientID;        
                foreach (ClientInfo info in msg.CurrentClients)
                {
                    if (ClientDic.ContainsKey(info.ClientID) == false)
                        ClientDic.Add(info.ClientID, info);
                }
                if (ConnectionStatus!=null)
                ConnectionStatus(ServerIP, true);
                break;



            case ClientMsgType.clientListForALL:

                msg.CurrentClients.Add(msg.Info);
                foreach (ClientInfo info in msg.CurrentClients)
                {
                    if (ClientDic.ContainsKey(info.ClientID) == false)
                        ClientDic.Add(info.ClientID, info);
                }
                if(NewList!=null)
                NewList(msg.CurrentClients);
                if (NewStatus != null)
                    NewStatus(msg.Info, msg.Status, ClientMsgType.Join);
                break;
           


            case ClientMsgType.Msg:

                if(NewMsg!=null) 
                    NewMsg(msg.Info, msg.Msg, msg.From,msg.LineNumb);
                break;



            case ClientMsgType.Disconnect:
                if (NewStatus != null)
                    NewStatus(msg.Info, msg.Status, ClientMsgType.Disconnect);
                if (ClientDic.ContainsKey(msg.Info.ClientID) == true)
                    ClientDic.Remove(msg.Info.ClientID);
                if (ClientLeaved!=null)
                ClientLeaved(msg.Info);
                break;

            case ClientMsgType.Status:

                if(NewStatus!=null)
                    NewStatus(msg.Info,msg.Status, ClientMsgType.Status);
                break;

            case ClientMsgType.Buzz:

                if (NewBuzz != null)
                    NewBuzz(msg.From);
                break;

            case ClientMsgType.disconnectedByServer:
                if (DisconnectByServer!=null)
                DisconnectByServer();
                break;
        }
    }



    /// <summary>
    /// This will stop all client thread and dispose.
    /// </summary>
    public void Dispose()
    {
        Listener.RunServer = false;

        foreach (var f in Program.App.Forms.Values)
        {
            f.Close();
            f.Dispose();

        }
        Program.App.Forms.Clear();
        Program.App.Client.ClientDic.Clear();
    }
}


