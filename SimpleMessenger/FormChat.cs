using System;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Text;

namespace SimpleMessenger;


/// <summary>
/// **************************** This Form(3) is CHAT WINDOW************************
/// *************Every Client in the list has a own chat box***************
/// ********Normally this chat box is hidden. When User start a chat with someone, than it appers***
/// </summary>


public partial class FormChat : Form
{
    private readonly ClientInfo client;
    private int lineNumber;
    private bool ShiftEnter = true;

    readonly SoundPlayer BuzzSound = new (Properties.Resources.BUZZER);
    readonly SoundPlayer MessageSound = new (@"C:\WINDOWS\Media\chimes.wav");


    public FormChat(ClientInfo client)
    {
        this.client = client;
        InitializeComponent();
        this.FormClosing += new FormClosingEventHandler(FormChat_FormClosing);
        SendMessageBox.KeyDown += new KeyEventHandler(SendMsgBox_KeyDown);
        SendMessageBox.KeyPress += new KeyPressEventHandler(SendMsgBox_KeyPress);

    }



    /// <summary>
    /// Form Loading. Set the Form's name with Buddy's(with whome user is chatting) name. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FormChat_Load(object sender, EventArgs e)
    {
        this.Text = client.Name;
        this.SendMessageBox.AllowDrop = true;
    }



    /// <summary>
    /// Form Closing...Here Form is not actually closed, if User click Form's close option. it simply Hide. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void FormChat_FormClosing(object sender, FormClosingEventArgs e)
    {
        this.Hide();
        e.Cancel = true;
    }



    /// <summary>
    /// Checkbox true/false. Change, how message will send. By pressing Enter or clicking button.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CheckBoxEnter_CheckedChanged(object sender, EventArgs e)
    {
        SendMessageBox.Focus();
        ShiftEnter = ShiftEnter != true;
    }



    /// <summary>
    /// if User Send message by Pressing Enter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void SendMsgBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (checkBoxEnter.Checked == false) return;
        ShiftEnter = false;
        if (Control.ModifierKeys == Keys.Shift  && e.KeyCode==Keys.Enter)
        {
            ShiftEnter = true;
        }
    }



    /// <summary>
    /// If User Send Message By clicking SEND Button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void SendMsgBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (checkBoxEnter.Checked == false) return;
        if (e.KeyChar == 13 && ShiftEnter==false)
        {
            e.Handled = true;
            btnSend.PerformClick();
        }
        //throw new NotImplementedException();
    }




    /// <summary>
    /// When SEND Button is clicked for messageg sending, this block will execute.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SendButton_Click(object sender, EventArgs e)
    {
        if (SendMessageBox.Text != "")
        {
           
            var m = new ClientMessage
            {
                Type = (int)ClientMessageType.Msg,
                Info = client,
                Msg = SendMessageBox.Rtf
            };
            var lines = SendMessageBox.Lines;
            int y;
            lineNumber = lines.Length;
            for (int i = 0; i < lines.Length; i++)
            {
                y = lines[i].Length;
                lineNumber += ((y /20));
            }
            m.LineNumb = lineNumber;
            m.From = Program.App.Info.ClientID;
            var data = m.Serialize();

            Program.App.Client.Listener.Send(Program.App.Client.ServerIP, MessengerServer.ListenerPort,data);
            TagUserControl myUsercon = new TagUserControl(Program.App.Info, SendMessageBox.Text, lineNumber);
            flowLayoutPanel1.Controls.Add(myUsercon);
            flowLayoutPanel1.VerticalScroll.Value = flowLayoutPanel1.VerticalScroll.Maximum;
            SendMessageBox.Rtf = "";
        }
    }
    



    delegate void GET_MSG(ClientInfo info, string m,int line);
    /// <summary>
    /// Here User Get messgae from Buddy and show it.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="m"></param>
    /// <param name="line"></param>
    public void GetMessag(ClientInfo info, string m,int line)
    {
        if (this.InvokeRequired)
        {
            this.BeginInvoke(new GET_MSG(GetMessag), [info, m, line]);
        }
        else
        {
            //this.Show();
            if (this.Visible == true)
                WindowState = FormWindowState.Normal;
            if(Program.App.Client.MessageSound==true)
            this.MessageSound.Play();
            TagUserControl myUsercon = new TagUserControl(info,m,line);
            flowLayoutPanel1.Controls.Add(myUsercon);
            flowLayoutPanel1.VerticalScroll.Value = flowLayoutPanel1.VerticalScroll.Maximum;
        }
    }


    
    /// <summary>
    /// Chat Window will Vibrate/Shake when user get a BuZZ!!!.
    /// </summary>
    public void ShakeMe()
    {
        int X = this.Left;
        int Y = this.Top;

        int position = 0;

        var RandomClass = new Random();

        for (int i = 0; i <= 25;i++ )
        {
            position = RandomClass.Next(X + 1, X + 15);

            this.Left = position;

            position = RandomClass.Next(Y + 1, Y + 15);
            this.Top = position;

            this.Left = X;
            this.Top = Y;
        }

    }



    delegate void GET_BUZZ(int senderID);
    /// <summary>
    /// Here User get Buzz from Buddy.
    /// </summary>
    /// <param name="senderID"></param>
    public void GetBuzz(int senderID)
    {
        if (this.InvokeRequired)
        {
            this.BeginInvoke(new GET_BUZZ(GetBuzz), new object[1] { senderID });
        }
        else
        {
            this.Show();
            this.Activate();
            ShakeMe();
            this.TopMost = true;
            this.SendMessageBox.Select();
            lineNumber = 0;
            var x = "震动!";
            lineNumber++;
            var myUsercon = new TagUserControl(client,x, lineNumber);
            flowLayoutPanel1.Controls.Add(myUsercon);
            flowLayoutPanel1.VerticalScroll.Value = flowLayoutPanel1.VerticalScroll.Maximum;
            this.BuzzSound.Play();
            lineNumber = 0;
        }
    }



    /// <summary>
    /// When User Give Buzz to abuddy.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnBuZZ_Click(object sender, EventArgs e)
    {
        ClientMessage m = new()
        {
            Type = (int)ClientMessageType.Buzz,
            Info = client,
            From = Program.App.Info.ClientID
        };

        Program.App.Client.Listener.Send(Program.App.Client.ServerIP, MessengerServer.ListenerPort, m.Serialize());
    }

}