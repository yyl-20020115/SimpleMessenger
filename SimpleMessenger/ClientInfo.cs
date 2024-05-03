using System;
using YamlDotNet.Serialization;

namespace SimpleMessenger;

public class ClientInfo
{
    [YamlMember]
    public int ClientID;
    [YamlMember]
    public string Name;
    [YamlMember]
    public string IP;
    [YamlMember]
    public int ListenPort;
    [YamlMember]
    public DateTime LastAliveMessage;

    public override bool Equals(object obj) => obj is ClientInfo info && this.ClientID == info.ClientID;

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => (Name /*+ Program.App.Client.NumberoOfMessageString[ClientID]*/);
}
