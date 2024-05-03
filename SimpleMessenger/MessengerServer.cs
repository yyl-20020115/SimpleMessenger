using System;
using System.Collections.Generic;
using System.Timers;

namespace SimpleMessenger;

/// <summary>
///   ****************************** This Class is  For SERVER****************************
///     ****Here Strat Server's SocketListening thread, Message Analysis happens, and hold other neccesary info for Server*********
/// </summary>
/// 

public class MessengerServer
{
    int lastClientID = 0;
    private readonly Dictionary<int, ClientInfo> MyList = [];
    private readonly Dictionary<int, ClientInfo> AliveList =[];
    private readonly SocketListener listener;
    private readonly string ownIP;
    private readonly Timer serverTimer = new(6000);


    /// <summary>
    /// Defalt Constructor.
    /// </summary>
    /// <param name="ownIP"></param>
    public MessengerServer(string ownIP)
    {
        this.ownIP = ownIP;
        listener = new SocketListener(12345,MsgFromClient);
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
                diff = now - info.lastAlivemsg;
                res = diff.Seconds.ToString();
                int x = int.Parse(res);
                if (x >= 6)
                {
                    lock (Program.MyLocker)
                    {
                        MyList.Remove(info.ClientID);
                        ClientMessage M = new()
                        {
                            Type = (int)ClientMsgType.Disconnect,
                            Info = info
                        };
                        byte[] data3 = M.Serialize();
                        foreach (var c in MyList.Values)
                        {
                            listener.Send(c.IP, c.ListenPort, data3);
                        }
                        ClientMessage m = new()
                        {
                            Type = (int)ClientMsgType.disconnectedByServer
                        };
                        listener.Send(info.IP, info.ListenPort, m.Serialize());
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
        var msg = ClientMessage.DeSerialize(data,0,dataAvailable);

        switch ((ClientMsgType)msg.Type)
        { 

            case ClientMsgType.Join:

                ClientMessage m = new()
                {
                    Type = (int)ClientMsgType.ClientList
                }; // this m will sent to new comer.
                m.Info.ClientID = ++lastClientID;

                msg.Info.ClientID = m.Info.ClientID; // this msg will sent to other clients for notify about new comer.
                msg.Type =(int)ClientMsgType.clientListForALL;
                byte[] data2 = msg.Serialize();
                lock (Program.MyLocker)
                {
                    foreach (var c in MyList.Values)
                    {
                        m.CurrentClients.Add(c);
                        // Send all clients the join msg
                        listener.Send(c.IP, c.ListenPort, data2);
                    }

                    // Sending new client server's client list
                    listener.Send(msg.Info.IP, msg.Info.ListenPort, m.Serialize());
                    //// Add this client to server's own client list
                    MyList.Add(msg.Info.ClientID, msg.Info);
                }
                break;



            case ClientMsgType.Msg:

                listener.Send(msg.Info.IP, msg.Info.ListenPort,data);
                break;


            case ClientMsgType.Disconnect:
                lock (Program.MyLocker)
                {
                    MyList.Remove(msg.Info.ClientID);
                    ClientMessage M = new()
                    {
                        Type = (int)ClientMsgType.Disconnect,
                        Info = msg.Info
                    };
                    byte[] data3 = M.Serialize();
                    foreach (ClientInfo c in MyList.Values)
                    {
                        listener.Send(c.IP, c.ListenPort, data3);
                    }
                }
                if (Program.App.Info.ClientID == msg.Info.ClientID)
                {
                    this.Dispose();
                }
                
                break;


            case ClientMsgType.Status:

                foreach (var c in MyList.Values)
                {
                    listener.Send(c.IP, c.ListenPort, msg.Serialize());
                }
                break;



            case ClientMsgType.Buzz:

                listener.Send(msg.Info.IP, msg.Info.ListenPort,data);
                break;

            case ClientMsgType.Alive:

                msg.Info.lastAlivemsg = DateTime.Now;

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
