using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;


namespace SimpleMessenger;

/// <summary>
/// **************************************This Class define Message Protocol*****************************
/// </summary>

[Serializable]
public enum ClientMsgType : int
{
    Msg = 0,
    Join,
    Connect,
    Disconnect,
    Alive,
    ClientList,
    clientListForALL,
    Buzz,
    disconnectedByServer,
    Client_Image,
    Status
}

[Serializable]
public class ClientMessage
{
    public int Type;
    public int Port;
    public string Msg;
    public string Status;
    public string To;
    public int LineNumb;
    public List<ClientInfo> CurrentClients;
    public string SourceIP;
    public ClientInfo Info;
    public int From;
    public byte[] ProfilePic;

    /// <summary>
    /// Defalt constructor.
    /// </summary>
    public ClientMessage()
    {
        CurrentClients = [];
        ProfilePic = new byte[1];
        Info = new();
        Msg = "";
        SourceIP = "";
        To = "";
    }



    /// <summary>
    /// Serializing Object.
    /// </summary>
    /// <returns></returns>
    public byte[] Serialize()
    {
        var x = new XmlSerializer(this.GetType());
        using var ms = new MemoryStream();
        x.Serialize(ms, this);
        return ms.GetBuffer();
    }


    /// <summary>
    /// DeSerialize Object
    /// </summary>
    /// <param name="rawString"></param>
    /// <returns></returns>
    public static ClientMessage DeSerialize(string rawString)
    {
        var x = new XmlSerializer(typeof(ClientMessage));
        using var ms = new MemoryStream();
        var data = Encoding.ASCII.GetBytes(rawString);
        ms.Write(data, 0, data.Length);
        return (ClientMessage)x.Deserialize(ms);
    }


    /// <summary>
    /// Overload DeSerialize.
    /// </summary>
    /// <param name="asciiBytes"></param>
    /// <param name="offset"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static ClientMessage DeSerialize(byte[] asciiBytes, int offset, int length)
    {
        var x = new XmlSerializer(typeof(ClientMessage));
        //string data = Encoding.ASCII.GetString(asciiBytes, offset, length);
        //byte[] asciiData=
        using var ms = new MemoryStream(asciiBytes, false);
        //ms.Write(asciiBytes, offset, length);
        return (ClientMessage)x.Deserialize(ms);
    }
}
