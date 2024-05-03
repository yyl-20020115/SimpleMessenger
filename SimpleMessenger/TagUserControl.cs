using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleMessenger;

public partial class TagUserControl : UserControl
{
    private readonly ClientInfo Info;
    private readonly string Message;
    private readonly int GotLine;

    public TagUserControl(ClientInfo info, string msg, int line)
    {
        this.Info = info;
        this.Message = msg;
        GotLine = line;
        InitializeComponent();
    }

    /// <summary>
    /// UserControl Loading.... Setting colors and name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TagUserControl_Load(object sender, EventArgs e)
    {
        txtRichMsg.ForeColor = Color.Black;
        lblName.ForeColor = Color.Black;
        lblDate.ForeColor = Color.Black;
        if (Info.ClientID == Program.App.Info.ClientID)
        {
            lblName.Text = "你";
            txtRichMsg.BackColor = Color.CornflowerBlue;
            this.BackColor = Color.CornflowerBlue;
        }
        else
        {
            lblName.Text = Info.Name;
        }
        ChangeHeight();
    }



    /// <summary>
    /// The RichTextbox (message showing field) will change its height Dynamically with the height of messgae.
    /// </summary>
    private void ChangeHeight()
    {
        txtRichMsg.Visible = false;
        //if (Message != "BuZZ!!!!!!!!")
        //{
        //}
        //else
        //{
        //    Font font1 = new Font(txtRichMsg.Font, FontStyle.Bold);
        //    txtRichMsg.SelectionFont = font1;
        //    txtRichMsg.SelectionColor = Color.Red;
        //    txtRichMsg.SelectedText = Message;
        //}
        txtRichMsg.Rtf = Message;
        int H = GotLine * txtRichMsg.Font.Height + txtRichMsg.Margin.Vertical;
        txtRichMsg.Height = H;
        txtRichMsg.Visible = true;
        string myDate;
        myDate = DateTime.Now.ToString("yyyy.MM.dd") + " at " + DateTime.Now.ToString("HH:mm:ss") + " says,";
        lblDate.Text = myDate;
    }
}
