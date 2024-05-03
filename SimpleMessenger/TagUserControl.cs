using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleMessenger;

public partial class TagUserControl : UserControl
{

    private ClientInfo info;
    private string msg;
    private int getline;


    public TagUserControl(ClientInfo info, string msg, int line)
    {
        this.info = info;
        this.msg = msg;
        getline = line;
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
        if (info.ClientID == Program.App.Info.ClientID)
        {
            lblName.Text = "你";
            txtRichMsg.BackColor = Color.CornflowerBlue;
            this.BackColor = Color.CornflowerBlue;

        }
        else
        {
            lblName.Text = info.Name;
        }
        ChangeHeight();
    }



    /// <summary>
    /// The RichTextbox (message showing field) will change its height Dynamically with the height of messgae.
    /// </summary>
    private void ChangeHeight()
    {
        txtRichMsg.Visible = false;
        if (msg != "BuZZ!!!!!!!!")
            txtRichMsg.Text = msg;
        else
        {
            Font font1 = new Font(txtRichMsg.Font, FontStyle.Bold);
            txtRichMsg.SelectionFont = font1;
            txtRichMsg.SelectionColor = Color.Red;
            txtRichMsg.SelectedText = msg;
        }
        int H = getline * txtRichMsg.Font.Height + txtRichMsg.Margin.Vertical;
        txtRichMsg.Height = H;
        txtRichMsg.Visible = true;
        string myDate;
        myDate = DateTime.Now.ToString("yyyy.MM.dd") + " at " + DateTime.Now.ToString("HH:mm:ss") + " says,";
        lblDate.Text = myDate;
    }
}
