using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;


namespace SimpleMessenger;

public enum ClientMessageType : int
{
    Msg = 0,
    Join,
    Connect,
    Disconnect,
    Alive,
    ClientList,
    ClientListForALL,
    Buzz,
    DisconnectedByServer,
    Client_Image,
    Status
}


public class ClientMessage
{
    [YamlMember]
    public int Type;
    [YamlMember]
    public int Port;
    [YamlMember]
    public string Msg = "";
    [YamlMember]
    public string Status;
    [YamlMember]
    public string To = "";
    [YamlMember]
    public int LineNumb;
    [YamlMember]
    public List<ClientInfo> CurrentClients = [];
    [YamlMember]
    public string SourceIP = "";
    [YamlMember]
    public ClientInfo Info = new();
    [YamlMember]
    public int From;
    [YamlMember]
    public byte[] ProfilePic = [];

    /// <summary>
    /// Defalt constructor.
    /// </summary>
    public ClientMessage()
    {
    }

    /// <summary>
    /// Serializing Object.
    /// </summary>
    /// <returns></returns>
    public byte[] Serialize()
    {
        var serializer = new SerializerBuilder().Build();
        var result = serializer.Serialize(this);
        return Encoding.UTF8.GetBytes(result);
    }

    /// <summary>
    /// Overload DeSerialize.
    /// </summary>
    /// <param name="asciiBytes"></param>
    /// <param name="offset"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static ClientMessage DeSerialize(byte[] bytes)
    {
        var deserializer = new DeserializerBuilder().Build();
        return deserializer.Deserialize<ClientMessage>(Encoding.UTF8.GetString(bytes));
    }
}
