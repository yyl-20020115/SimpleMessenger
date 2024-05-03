using System;
using System.Collections.Generic;
using System.Timers;

namespace SimpleMessenger;


public class MessengerServer
{
    public static int ListenerPort = 12345;

    int lastClientID = 0;
    private readonly Dictionary<int, ClientInfo> MyList = [];
    private readonly Dictionary<int, ClientInfo> AliveList =[];
    private readonly SocketListener listener;
    private readonly string ownIP;
    private readonly Timer serverTimer = new(6000);
    private int ownPort = 0;

    /// <summary>
    /// Defalt Constructor.
    /// </summary>
    /// <param name="ownIP"></param>
    public MessengerServer(string ownIP, int ownPort)
    {
        this.ownIP = ownIP;
        this.ownPort = ownPort;
        listener = new SocketListener(ListenerPort, MsgFromClient);
        serverTimer.Elapsed += new ElapsedEventHandler(ServerTimer_Elapsed);
        serverTimer.Start();
    }




    /// <summary>
    /// Server Checks Within 6 seconds weather every Clients send alive message or not.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ServerTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        serverTimer.Stop();
        DateTime now = DateTime.Now;
        TimeSpan diff;
        string res;

        lock (Program.AliveLocker)
        {
            foreach (var info in AliveList.Values)
            {
                diff = now - info.LastAliveMessage;
                res = diff.Seconds.ToString();
                int x = int.Parse(res);
                if (x >= 6)
                {
                    lock (Program.MyLocker)
                    {
                        MyList.Remove(info.ClientID);
                        ClientMessage M = new()
                        {
                            Type = (int)ClientMessageType.Disconnect,
                            Info = info
                        };
                        byte[] data3 = M.Serialize();
                        foreach (var clients in MyList.Values)
                        {
                            listener.SendData(clients.IP, clients.ListenPort, data3);
                        }
                        ClientMessage m = new()
                        {
                            Type = (int)ClientMessageType.DisconnectedByServer
                        };
                        listener.SendData(info.IP, info.ListenPort, m.Serialize());
                    }
                }
                serverTimer.Start();
            }
            //throw new NotImplementedException();
        }
    }




    /// <summary>
    /// Checking Which Type of message Server got for further Processing  
    /// </summary>
    /// <param name="data"></param>
    /// <param name="dataAvailable"></param>
    private void MsgFromClient(byte[] data,int dataAvailable)
    {
        var msg = ClientMessage.DeSerialize(data);

        switch ((ClientMessageType)msg.Type)
        { 
            case ClientMessageType.Join:

                ClientMessage m = new()
                {
                    Type = (int)ClientMessageType.ClientList
                }; // this m will sent to new comer.
                m.Info.ClientID = ++lastClientID;

                msg.Info.ClientID = m.Info.ClientID; // this msg will sent to other clients for notify about new comer.
                msg.Type =(int)ClientMessageType.ClientListForALL;
                var data2 = msg.Serialize();
                lock (Program.MyLocker)
                {
                    foreach (var clients in MyList.Values)
                    {
                        m.CurrentClients.Add(clients);
                        // Send all clients the join msg
                        listener.SendData(clients.IP, clients.ListenPort, data2);
                    }

                    // Sending new client server's client list
                    listener.SendData(msg.Info.IP, msg.Info.ListenPort, m.Serialize());
                    //// Add this client to server's own client list
                    MyList.Add(msg.Info.ClientID, msg.Info);
                }
                break;



            case ClientMessageType.Msg:

                listener.SendData(msg.Info.IP, msg.Info.ListenPort,data);
                break;


            case ClientMessageType.Disconnect:
                lock (Program.MyLocker)
                {
                    MyList.Remove(msg.Info.ClientID);
                    ClientMessage M = new()
                    {
                        Type = (int)ClientMessageType.Disconnect,
                        Info = msg.Info
                    };
                    byte[] data3 = M.Serialize();
                    foreach (ClientInfo c in MyList.Values)
                    {
                        listener.SendData(c.IP, c.ListenPort, data3);
                    }
                }
                if (Program.App.Info.ClientID == msg.Info.ClientID)
                {
                    this.Dispose();
                }
                
                break;


            case ClientMessageType.Status:

                foreach (var c in MyList.Values)
                {
                    listener.SendData(c.IP, c.ListenPort, msg.Serialize());
                }
                break;



            case ClientMessageType.Buzz:

                listener.SendData(msg.Info.IP, msg.Info.ListenPort,data);
                break;

            case ClientMessageType.Alive:

                msg.Info.LastAliveMessage = DateTime.Now;

                lock(Program.AliveLocker)
                {
                   // AliveList[msg.Info.ClientID] = new DateTime();                  
                    AliveList[msg.Info.ClientID] = msg.Info;
                }
                break;

          }

    }



    /// <summary>
    /// Stopping All threads and dispose.
    /// </summary>
    public void Dispose()
    {
        listener.RunServer = false;
        Program.App.Server.MyList.Clear();
    }
}
