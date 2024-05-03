using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;

namespace SimpleMessenger;

/// <summary>
/// **********************The main entry point for the application**********************
/// </summary>
public static class Program
{
    // To handle Some Race Condition these Lockobject has introduced.
    public static readonly object AliveLocker = new();
    public static readonly object MyLocker = new();

    public static readonly ApplicationData App = new();
    
    public static string OwnIP;
   
   
    [STAThread]
    
    public static void Main()
    {
        // For Getting Own IP. 
        var host = Dns.GetHostEntry(Dns.GetHostName());
        var ips = new List<string>();
        foreach (IPAddress ip in host.AddressList.OrderByDescending(ip=>ip.ToString()))
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ips.Add(ip.ToString());
            }
        }
        if (ips.Count > 0) 
        {
            OwnIP = ips.FirstOrDefault(ip => ip.StartsWith("192.168."));
            OwnIP ??= ips[0];
        }

        if(OwnIP is null)
        {
            MessageBox.Show("未能找到有效的本机IP!");
            OwnIP = "127.0.0.1";
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new FormWelcome());
        Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
    }

    public static void Application_ApplicationExit(object sender, EventArgs e)
    {
        foreach (var form in App.Forms.Values)
        {
            form.Close();

            form.Dispose();

        }
        App.Forms.Clear();
    }
}
