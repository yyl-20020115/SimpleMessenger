using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


namespace SimpleMessenger;

public partial class FormWelcome : Form
{
    public FormWelcome()
    {
        InitializeComponent();
        this.FormClosing += new FormClosingEventHandler(FormWelcome_FormClosing); 
    }


    /// <summary>
    /// From Loading...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FormWelcome_Load(object sender, EventArgs e)
    {
        // temporary work, 
        txtIP.Text = Program.OwnIP;
        this.textBoxName.Text = "新用户@" + txtIP.Text;
    }

    /// <summary>
    /// Form Closing...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void FormWelcome_FormClosing(object sender, FormClosingEventArgs e)
    {
        this.Dispose();
        this.Close();
    }
    



    /// <summary>
    /// This will Block execute if Client start a server.
    /// </summary>
    /// <param name="sender"></param> 
    /// <param name="e"></param>
    private void StartServerButtonClick(object sender, EventArgs e)
    {
        if (textBoxName.Text != "")
        {
            Program.App.IsServer = true;
            Program.App.Info.Name = textBoxName.Text;
            Program.App.Info.IP = Program.OwnIP;
            Program.App.ServerIP = txtIP.Text;
            //Creating SERVER
            Program.App.Server = new MessengerServer(Program.App.Info.IP);
            //Server is also a Client. So creating a client.
            Program.App.Client = new MessengerClient();
            Program.App.Client.ConnectionStatus += new SERVER_CONNECTION_DELIGATE(Client_ConnectionStatus);
            Program.App.Client.Start("127.0.0.1", textBoxName.Text);
        }
        else
            MessageBox.Show("请首先输入你的名字!");
    }



    /// <summary>
    /// This will execiute if client only give input the server's ip and join
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void JoinButton_Click(object sender, EventArgs e)
    {
        if (textBoxName.Text != "" && txtIP.Text != "")
        {
            Program.App.Info.Name = textBoxName.Text;
            Program.App.ServerIP = txtIP.Text;
            Program.App.Info.IP = Program.OwnIP;
            // Creating Client...
            Program.App.Client = new MessengerClient();
            Program.App.Client.ConnectionStatus += new SERVER_CONNECTION_DELIGATE(Client_ConnectionStatus);
            Program.App.Client.Start(txtIP.Text, Program.App.Info.Name);
        }
        else
        {
            MessageBox.Show("请首先输入你的名字和服务器IP，然后再点击加入!");
        }
    }




    delegate void CONNECTION_STATUS(string ip, bool success);
   /// <summary>
    /// Confirmoing If Client is connected with Server
   /// </summary>
   /// <param name="serverIP"></param>
   /// <param name="success"></param>    
    void Client_ConnectionStatus(string serverIP, bool success)
    {    
        if (txtIP.InvokeRequired)
        {
            txtIP.BeginInvoke(new CONNECTION_STATUS(Client_ConnectionStatus), [serverIP, success]);
            return;
        }
        else if (success)
        {
            //It is confirmed that Client is Connected with Server, So Now Showing their own Client Window.
            FormMessenger messenger = new(this);
            messenger.Show();
            // Form1 hideing...
            this.Hide();
        }
        else
        {
            // Client is not Connected with Server, So showing this msg...
            MessageBox.Show("链接到 IP:" + serverIP +"失败!");

            //Closing All thread that I started For connecting.
            if (Program.App.IsServer)
            {
                Program.App.Server.Dispose();
                Program.App.Server = null;
            }
            Program.App.Client?.Dispose();
            Program.App.Client = null;
        }
    }
    //Leave
    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.Close();
    }
    //Manual
    private void ManualToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (File.Exists("Project__Chat_Messenger__Report.pdf"))
            Process.Start("Project__Chat_Messenger__Report.pdf");
        else
            MessageBox.Show("File is not found");
    }
    //about
    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var about = new AboutBox();
        about.ShowDialog();
    }
}
