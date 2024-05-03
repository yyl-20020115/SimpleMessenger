using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace SimpleMessenger;

public partial class FormMessenger : Form
{
    FormWelcome welcome;
    private int originalHight;
    private int StatusNumber = 0; 

    public FormMessenger(FormWelcome welcome)
    {
        InitializeComponent();
        this.welcome = welcome;
        listClients.DisplayMember = "名称";
        this.labelSelfIP.Text = Program.OwnIP;

        //Giving New Client already existing client list
        foreach (var info in Program.App.Client.ClientDic.Values)
        {
            if (listClients.Items.Contains(info) == false)
            {
                listClients.Items.Add(info);
                Program.App.Client.NumberoOfMessage[info.ClientID]= 0;
                Program.App.Client.NumberoOfMessageString[info.ClientID] ="";
                FormChat newForm = new FormChat(info);
                Program.App.Forms.Add(info.ClientID, newForm);
                newForm.Hide();
            }

        }

        listClients.SelectedIndexChanged += new EventHandler(ListClients_SelectedIndexChanged);
        this.FormClosing += new FormClosingEventHandler(FormMessenger_FormClosing);
        Program.App.Client.NewList += new SERVER_List(GetList);
        Program.App.Client.ClientLeaved += new SERVER_LEAVE(RemoveFromList);
        Program.App.Client.NewMsg += new SERVER_MSG(Client_NewMsg);
        Program.App.Client.NewStatus += new SERVER_STATUS(Client_NewMStatus);
        Program.App.Client.NewBuzz += new SERVER_BUZZ(Client_NewBuzz);
        Program.App.Client.DisconnectByServer += new SERVER_DISCONNECTED_BY_SERVER(Client_DisconnectByServer);
        Program.App.Client.Listener.ServerGone += new ServerNotExist(L_ServerGone);
    }




    /// <summary>
    /// Form2 loading..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FormMessenger_Load(object sender, EventArgs e)
    {
        btnHide.Visible = false; // this button lies in expanded area, so it is hiding.
        lblBuddyList.Visible = true; // this button lies in expanded area, so it is hiding.
        richTxtNewsFeed.Visible = false; // this textbox lies in expanded area, so it is hiding.
        originalHight = this.Width;
        this.Text = Program.App.Info.Name; // Setting Form's name with CLient Name.
    }





    delegate void ServerIsGone();      
    /// <summary>
    /// If server is not found, client window will be disappeared and show user a message.
    /// </summary>
    public void L_ServerGone()
    {
        if (this.InvokeRequired)
            this.Invoke(new ServerIsGone(L_ServerGone), new object[] { });
        else
        {
            Program.App.Client.Dispose();
            Program.App.Client=null;
            this.Hide();
            welcome.Show();
            MessageBox.Show("未找到服务器!");
            this.Dispose();
            this.Close();
        }

    }



    delegate void disBySer();
    /// <summary>
    /// if client is disconnected by server.client window will be disappeared and show user a message.
    /// </summary>
    void Client_DisconnectByServer()
    {
        if (this.InvokeRequired)
            this.Invoke(new disBySer(Client_DisconnectByServer), new object[] { });
        else 
        {
            Program.App.Client.Dispose();
            Program.App.Client = null;
            welcome.Show();
            this.Hide();
            MessageBox.Show("您已经和服务器断开链接!");
            this.Dispose();
            this.Close();
        }
    }




    delegate void ServerGetMsg(ClientInfo info, string msg, int remoteID,int line);
    /// <summary>
    /// Getting msg From other Clients through Server
    /// </summary>
    /// <param name="info"></param>
    /// <param name="msg"></param>
    /// <param name="remoteID"></param>
    /// <param name="line"></param>
    void Client_NewMsg(ClientInfo info, string msg, int remoteID, int line)
    {
        if (this.InvokeRequired)
        {
            this.BeginInvoke(new ServerGetMsg(Client_NewMsg), new object[4] { info, msg, remoteID,line });
        }
        else
        {
            if (Program.App.Client.ClientDic.ContainsKey(remoteID))
            {
                Program.App.Forms[remoteID].GetMessag(Program.App.Client.ClientDic[remoteID], msg,line);
                if (Program.App.Forms[remoteID].Visible == false)
                {
                    ClientInfo tempinfo = new();
                    tempinfo = Program.App.Client.ClientDic[remoteID];
                    int x = listClients.Items.IndexOf(tempinfo);
                    Program.App.Client.NumberoOfMessage[tempinfo.ClientID]++;
                    Program.App.Client.NumberoOfMessageString[tempinfo.ClientID] = "(" + Program.App.Client.NumberoOfMessage[tempinfo.ClientID].ToString() + ")";
                    listClients.Items.RemoveAt(x);
                    listClients.Items.Add(tempinfo);
                    listClients.BackColor = Color.Pink;
                }
            }
        }
        
     //throw new NotImplementedException();
    }





    /// <summary>
    ///  When a name will select from the client List, a chat box for that client will be appeared.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void ListClients_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listClients.SelectedIndex!=-1)
        {
            var c = (ClientInfo)listClients.SelectedItem;
            if (c == null) return;
            ShowChatWindow(c);
            if (Program.App.Client.NumberoOfMessageString[c.ClientID] != "")
            {
                listClients.ClearSelected();
                listClients.BackColor = Color.MediumOrchid;
                int x = listClients.Items.IndexOf(c);
                Program.App.Client.NumberoOfMessage[c.ClientID] = 0;
                Program.App.Client.NumberoOfMessageString[c.ClientID] = "";
                listClients.Items.RemoveAt(x);
                listClients.Items.Add(c);
            }
        }       
    }

 


   
    /// <summary>
    /// chat window show.
    /// </summary>
    /// <param name="c"></param>
    private void ShowChatWindow(ClientInfo c)
    {
        if(Program.App.Forms[c.ClientID].Visible==false)
        Program.App.Forms[c.ClientID].Visible=true;
        Program.App.Forms[c.ClientID].Activate();
    }


    
    /// <summary>
    /// Form closing event...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void FormMessenger_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (Program.App.IsServer)
        {
            Program.App.Server.Dispose();
            Program.App.Server = null;
        }
        Program.App.Client.Dispose();
        Program.App.Client = null;
        welcome.Dispose();
        welcome.Close();
        this.Dispose();
        this.Close();
    }
   





    delegate void ServerList(List<ClientInfo> l);
    /// <summary>
    /// When a new client is signed in. Server sent their info to other clients. here already existing clients 
    /// are getting newly updated client list from server
    /// </summary>
    /// <param name="l"></param>
    void GetList(List<ClientInfo> l)
    {
        if (listClients.InvokeRequired)
        {
            listClients.BeginInvoke(new ServerList(GetList), [l]);
            return;
        }
        else
        {
            foreach(ClientInfo info in l)
            {
                if (listClients.Items.Contains(info)==false)
                {
                    listClients.Items.Add(info);
                    Program.App.Client.NumberoOfMessage[info.ClientID] = 0;
                    Program.App.Client.NumberoOfMessageString[info.ClientID] = "";
                    FormChat newForm = new(info);
                    Program.App.Forms.Add(info.ClientID, newForm);
                    newForm.Hide();
                }
               
            }
        }
    }






    delegate void ServerLeaved(ClientInfo info);
    /// <summary>
    /// if someone leaved from messenger, other clients will remove him/her from their list. 
    /// </summary>
    /// <param name="info"></param>
    void RemoveFromList(ClientInfo info)
    {
        if (listClients.InvokeRequired)
        {
            listClients.Invoke(new ServerLeaved(RemoveFromList), [info]);
        }
        else
        {
            listClients.Items.Remove(info);
            listClients.Refresh();
            Program.App.Forms.Remove(info.ClientID);
        }
    }




   
    /// <summary>
    /// Form2(Client window) is expendable. By clicking this button window will expand.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnNewsFeed_Click(object sender, EventArgs e)
    {
        if (this.Width == originalHight)
        {
            this.Width = originalHight + 288;
            btnHide.Visible = true;
            lblBuddyList.Visible = true;
            richTxtNewsFeed.Visible = true;
            btnNewsFeed.Font = new Font(btnNewsFeed.Font, FontStyle.Regular);
            btnNewsFeed.BackColor = Color.White;
            btnNewsFeed.Text = "消息订阅";
            StatusNumber = 0;
        }
    }



    /// <summary>
    /// As Form2(Client window) is expendable. By clicking this button window will hide the expanded area.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnHide_Click(object sender, EventArgs e)
    {
        richTxtNewsFeed.Visible = false;
        btnHide.Visible = false;
        lblBuddyList.Visible = false;
        this.Width = originalHight;
    }




   
    /// <summary>
    /// setting font for writing Status/public message in Testbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtStatus_TextChanged(object sender, EventArgs e)
    {
        txtStatus.Font = new Font(txtStatus.Font, FontStyle.Regular);
        txtStatus.ForeColor = Color.Black;
    }




   
    /// <summary>
    /// This button send Public Message/Status to all other clients.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSendStatus_Click(object sender, EventArgs e)
    {
        ClientMessage msg = new ClientMessage();
        msg.Type = (int)ClientMsgType.Status;
        msg.Status = txtStatus.Text;
        msg.Info.ClientID = Program.App.Info.ClientID;
        msg.Info.Name = Program.App.Info.Name;
        Program.App.Client.Listener.Send(Program.App.ServerIP,12345,msg.Serialize());
        txtStatus.Font = new Font(txtStatus.Font, FontStyle.Italic);
        txtStatus.ForeColor = Color.LightGray;
        txtStatus.Text = "";
    }




    delegate void ServerStatus(ClientInfo info, string sts, ClientMsgType type);
    /// <summary>
    /// When Other clients Update their status/Public Message, NewsFeed or Buddy news textbox will be added that news
    /// </summary>
    /// <param name="info"></param>
    /// <param name="sts"></param>
    void Client_NewMStatus(ClientInfo info, string sts, ClientMsgType type)
    {
        if (richTxtNewsFeed.InvokeRequired)
        {
            richTxtNewsFeed.BeginInvoke(new ServerStatus(Client_NewMStatus), new object[3] { info, sts, type });
        }
        else 
        {

            Font font1 = new Font( richTxtNewsFeed.Font, FontStyle.Bold);

            richTxtNewsFeed.SelectionFont = font1;
            richTxtNewsFeed.SelectionColor = Color.Red;
            if(info.Name!=Program.App.Info.Name)
            richTxtNewsFeed.SelectedText = Environment.NewLine + info.Name+":";
            else richTxtNewsFeed.SelectedText = Environment.NewLine + "你" + ":";
            
            if (type == ClientMsgType.Status)
            {
                Font font2 = new Font(richTxtNewsFeed.Font, FontStyle.Italic);
                richTxtNewsFeed.SelectionFont = font2;
                richTxtNewsFeed.SelectionColor = Color.DimGray;
                richTxtNewsFeed.SelectedText = " 状态更新于 " + DateTime.Now.ToString("HH:mm:ss");
                Font font3 = new Font(richTxtNewsFeed.Font, FontStyle.Regular);
                richTxtNewsFeed.SelectionFont = font3;
                richTxtNewsFeed.SelectionColor = Color.Black;
                richTxtNewsFeed.SelectedText = Environment.NewLine + sts + Environment.NewLine;
            }
            else if (type == ClientMsgType.Join)
            {
                Font font2 = new Font(richTxtNewsFeed.Font, FontStyle.Italic);
                richTxtNewsFeed.SelectionFont = font2;
                richTxtNewsFeed.SelectionColor = Color.DimGray;
                richTxtNewsFeed.SelectedText = " 加入于 " + DateTime.Now.ToString("HH:mm:ss");
                Font font3 = new Font(richTxtNewsFeed.Font, FontStyle.Regular);
                richTxtNewsFeed.SelectionFont = font3;
                richTxtNewsFeed.SelectionColor = Color.Black;
                richTxtNewsFeed.SelectedText = Environment.NewLine + Environment.NewLine;
            }
            else if (type == ClientMsgType.Disconnect)
            {
                Font font2 = new Font(richTxtNewsFeed.Font, FontStyle.Italic);
                richTxtNewsFeed.SelectionFont = font2;
                richTxtNewsFeed.SelectionColor = Color.DimGray;
                richTxtNewsFeed.SelectedText = " 离开于 " + DateTime.Now.ToString("HH:mm:ss");
                Font font3 = new Font(richTxtNewsFeed.Font, FontStyle.Regular);
                richTxtNewsFeed.SelectionFont = font3;
                richTxtNewsFeed.SelectionColor = Color.Black;
                richTxtNewsFeed.SelectedText = Environment.NewLine + Environment.NewLine;
            }
            if (info.ClientID != Program.App.Info.ClientID && this.Width==originalHight)
            {
                Font font2 = new Font(richTxtNewsFeed.Font, FontStyle.Italic);
                richTxtNewsFeed.SelectionFont = font2;
                richTxtNewsFeed.SelectionColor = Color.DimGray;
                StatusNumber++;
                btnNewsFeed.Font = new Font(btnNewsFeed.Font, FontStyle.Bold);
                btnNewsFeed.BackColor = Color.Yellow;
                btnNewsFeed.Text = "消息订阅(" + StatusNumber + ")";
            }
        }
    }





   
    delegate void ServerBuzz(int senderID);
    /// <summary>
    /// When a client get Buzz from other, than this news will make change in Form3(Chat box).
    /// </summary>
    /// <param name="senderID"></param>
    void Client_NewBuzz(int senderID)
    {
        if (this.InvokeRequired)
        {
            this.BeginInvoke(new ServerBuzz(Client_NewBuzz), new object[1] { senderID });
        }
        else
        {
            if (Program.App.Client.ClientDic.ContainsKey(senderID))
            {
                Program.App.Forms[senderID].GetBuzz(senderID);
                if (Program.App.Client.NumberoOfMessageString[senderID] != "")
                {
                    listClients.BackColor = Color.MediumOrchid;
                    int x = listClients.Items.IndexOf(Program.App.Client.ClientDic[senderID]);
                    Program.App.Client.NumberoOfMessage[senderID] = 0;
                    Program.App.Client.NumberoOfMessageString[senderID] = "";
                    listClients.Items.RemoveAt(x);
                    listClients.Items.Add(Program.App.Client.ClientDic[senderID]);
                }

            }
        }

    }




    
    /// <summary>
    /// This is the menu at the top of Client Window. If client  click Leave option, there will send a leave
    /// message to server and close all window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LeaveToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
        var m = new ClientMessage
        {
            Type = (int)ClientMsgType.Disconnect
        };
        m.Info.ClientID = Program.App.Info.ClientID;
        m.Info.Name = Program.App.Info.Name;
        m.From = Program.App.Info.ClientID;
        Program.App.Client.Listener.Send(Program.App.ServerIP, 12345, m.Serialize());
        if (Program.App.IsServer)
        {
            Program.App.Server.Dispose();
            Program.App.Server = null;
        }
        Program.App.Client.Dispose();
        Program.App.Client = null;
        welcome.Show();
        this.Dispose();
        this.Close();
    }




   /// <summary>
    /// This is the menu at the top of Client Window. Client can ON the Message tone,
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    private void ONToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Program.App.Client.MessageSound = true;
    }




    /// <summary>
    /// This is the menu at the top of Client Window. Client can OFF the Message tone,
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OFFToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Program.App.Client.MessageSound = false;
    }

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var about = new AboutBox();
        about.ShowDialog();
    }

    private void ManualToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (File.Exists("Project__Chat_Messenger__Report.pdf"))
            Process.Start("Project__Chat_Messenger__Report.pdf");
        else
            MessageBox.Show("未找到帮助文件!");
    }
}
