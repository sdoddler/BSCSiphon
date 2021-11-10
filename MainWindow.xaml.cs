using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BSCSiphon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BSCWeb3 userWeb3 = null;

        private Timer _timer = null;

        private bool siphoning = false;

        string privKey = "";

        string newWallet = "";

        decimal threshold = 0;

        public MainWindow()
        {
            InitializeComponent();

            //userweb3 send trnsaction /// get balance

            
        }


        public async void TimerCallback(Object o)
        {
            if (!siphoning)
            {

                privKey = "";

                newWallet = "";

                _timer.Dispose();
                _timer = null;
                return;
            }

            if (userWeb3 != null)
            {
                decimal balance = await userWeb3.GetBalance(userWeb3.PublicAddress);
                this.Dispatcher.Invoke(() =>
                {
                    Log("Current Balance:" + balance.ToString() + " BNB");
                });
                

                if (balance > threshold)
                { string reply = "Balance greater than Threshold - Attempting to Send BNB!";
                    try
                    {
                        reply = await userWeb3.SendTransaction(newWallet, balance * 0.95m);
                    }
                    catch (Exception e)
                    {
                        reply = "ERROR!\n"+e.Message;
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        Log(reply);
                    });
                 }
            }
            
        }

        public void Log(string message)
        {
            LogBox.AppendText("\n[" + DateTime.Now.ToString("HH:mm") + "]" + message);
            LogScrollViewer.ScrollToEnd();
        }

        private void Btn_Twitter1_Click(object sender, RoutedEventArgs e)
        {
            WebsiteOpen("https://twitter.com/theshillverse");
        }


        private void Btn_Altura_Click(object sender, RoutedEventArgs e)
        {
            WebsiteOpen("https://app.alturanft.com/collection/0x27970a7fa322bbfefe208dbca7f8130a964c2b12");
        }



        private void WebsiteOpen(string url)
        {
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void Btn_Github1_Click(object sender, RoutedEventArgs e)
        {
            WebsiteOpen("https://github.com/sdoddler");
        }

        private void Btn_BSC_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText("0xDDA06A9D45D28a5aC74D5Cfbe66c53d3cdf804Cd");
        }

        private void btn_StartSiphoning_Click(object sender, RoutedEventArgs e)
        {
            siphoning = !siphoning;

            txt_newWallet.IsEnabled = !siphoning;
            txt_privKey.IsEnabled = !siphoning;
            ud_Threshold.IsEnabled = !siphoning;

            if (siphoning)
            {
                threshold = decimal.Parse(ud_Threshold.Text);

                _timer = new Timer(TimerCallback, null, 5000, 60000);

                userWeb3 = new BSCWeb3(txt_privKey.Text);

                Log($"Siphoning any BNB over {threshold}\nFrom: {userWeb3.PublicAddress}\nTo:{txt_newWallet.Text}");

                newWallet = txt_newWallet.Text;

              //  Console.WriteLine("Addy:" +userWeb3.PublicAddress);
            }
            else
            {

                 privKey = "";

                 newWallet = "";

                _timer.Dispose();
                _timer = null;
            }
           
        }
    }
}
