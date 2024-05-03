using System.Collections.Generic;
using System.Drawing;

namespace SimpleMessenger;

public class ApplicationData
{
    public Dictionary<int, FormChat> Forms = [];

    public string ServerIP = "";

    public Image Userimage;

    // Storing my info
    public ClientInfo Info = new ();

    public bool IsServer = false;

    public MessengerClient Client;

    public MessengerServer Server;

    public SocketListener Listener;

}