using BLockReviewsAPI.Models;
using Microsoft.Extensions.Configuration;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI.BlockChainDI
{
    public interface IEtherConn
    {
        public Task GetBlockNumber();
        public Task<BlockReviewAccount> CreateAccount();
    }

    public class EtherConn : IEtherConn
    {
        private string web3URL = "";
        private IConfiguration configuration;
        public EtherConn(IConfiguration _configuration)
        {
            configuration = _configuration;
            web3URL = configuration["Ether:BlockURL"];
        }

        public async Task GetBlockNumber()
        {
            var web3 = new Web3(web3URL);
            var latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();            
            Console.WriteLine($"Latest Block Number is: {latestBlockNumber}");
        }

        public async Task<BlockReviewAccount> CreateAccount()
        {           
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            var account = new Nethereum.Web3.Accounts.Account(privateKey);

            return new BlockReviewAccount
            {
                 address = account.Address,
                 PrivateKey = account.PrivateKey,
                 PublicKey = account.PublicKey
            };
        }

        public async Task SignTx(UserInfo user, string message)
        {
            var signer1 = new EthereumMessageSigner();

            var signature1 = signer1.EncodeUTF8AndSign(message, new EthECKey(user.AccountPrivateKey));

            var addressRec1 = signer1.EncodeUTF8AndEcRecover(message, signature1);

            var msg2 = "test";
            var signer2 = new EthereumMessageSigner();
            var signature2 = signer2.HashAndSign(msg2,
                            "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7");

        }
    }    
}
