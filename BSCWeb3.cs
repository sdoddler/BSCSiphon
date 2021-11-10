using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Signer;
using Nethereum.JsonRpc.Client;

namespace BSCSiphon
{
    public class BSCWeb3
    {

        public Web3 web3;
        private string publicAddress { get; set; } = "";

        public string PublicAddress { get => publicAddress; }

       

        public BSCWeb3(string privKey)
        {
            
         //   publicAddress = pubAddy;
            Account account = new Account(privKey, 56);

            web3 = new Web3(account, "https://bsc-dataseed.binance.org/");
            web3.TransactionManager.UseLegacyAsDefault = true;


            publicAddress = Web3.GetAddressFromPrivateKey(privKey);

        }

        public async Task GetAddress()
        {
            var accounts = await web3.Eth.Accounts.SendRequestAsync();

            foreach(string s in accounts)
            {
                Console.WriteLine(s);
                
            }
        }

      public  async Task<decimal> GetBalance(string publicKey)
        {

            var balance = await web3.Eth.GetBalance.SendRequestAsync(publicKey);
            var etherAmount = Web3.Convert.FromWei(balance.Value);

            Console.WriteLine($"Balance of {publicKey} is: {etherAmount}");
            return etherAmount;
        }
       

      public  async Task<string> SendTransaction(string destination, decimal amount)
        {

            try
            {
                var transaction = await web3.Eth.GetEtherTransferService()
                     .TransferEtherAndWaitForReceiptAsync(destination, amount);

                Console.WriteLine(transaction.TransactionHash);
                return transaction.TransactionHash;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Transaction Error!\n"+e.Message ;
            }
        }
    }
}
