using System;

namespace SimpleMessenger;

/// <summary>
///  ************************ This class is for store Client Information**************************
/// </summary>

[Serializable]
public class ClientInfo
{
    public int ClientID;
    public string Name;
    public string IP;
    public int ListenPort;
    public DateTime lastAlivemsg;

    public override bool Equals(object obj) => obj is ClientInfo info && this.ClientID == info.ClientID;

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => (Name + Program.App.Client.NumberoOfMessageString[ClientID]);
}
