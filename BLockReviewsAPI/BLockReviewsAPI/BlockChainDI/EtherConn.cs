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

        private string ReviewContractAddress = "0xA75A0f01Fb51B0628F2da4edDc8498AfD3812EE3";
        private string abi = @"[ {""inputs"": [ { ""internalType"": ""address payable"", ""name"": ""_tokenAddress"", ""type"": ""address"" } ], ""stateMutability"": ""payable"", ""type"": ""constructor"" }, { ""anonymous"": false, ""inputs"": [ { ""indexed"": false, ""internalType"": ""uint256"", ""name"": ""id"", ""type"": ""uint256"" }, { ""indexed"": false, ""internalType"": ""string"", ""name"": ""title"", ""type"": ""string"" }, { ""indexed"": false, ""internalType"": ""string"", ""name"": ""description"", ""type"": ""string"" }, { ""indexed"": false, ""internalType"": ""address"", ""name"": ""creator"", ""type"": ""address"" }, { ""indexed"": false, ""internalType"": ""uint256"", ""name"": ""liked"", ""type"": ""uint256"" } ], ""name"": ""ReviewInfo"", ""type"": ""event"" }, { ""inputs"": [ { ""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256"" } ], ""name"": ""Review_Mapping"", ""outputs"": [ { ""internalType"": ""uint256"", ""name"": ""id"", ""type"": ""uint256"" }, { ""internalType"": ""string"", ""name"": ""title"", ""type"": ""string"" }, { ""internalType"": ""string"", ""name"": ""description"", ""type"": ""string"" }, { ""internalType"": ""address"", ""name"": ""creator"", ""type"": ""address"" }, { ""internalType"": ""uint256"", ""name"": ""liked"", ""type"": ""uint256"" } ], ""stateMutability"": ""view"", ""type"": ""function"" }, { ""inputs"": [ { ""internalType"": ""address"", ""name"": ""_owner"", ""type"": ""address"" } ], ""name"": ""getBalanceOf"", ""outputs"": [ { ""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256"" } ], ""stateMutability"": ""view"", ""type"": ""function"" }, { ""inputs"": [], ""name"": ""getIdx"", ""outputs"": [ { ""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256"" } ], ""stateMutability"": ""view"", ""type"": ""function"" }, { ""inputs"": [ { ""internalType"": ""uint256"", ""name"": ""_id"", ""type"": ""uint256"" }, { ""internalType"": ""address"", ""name"": ""_to"", ""type"": ""address"" }, { ""internalType"": ""uint256"", ""name"": ""_amount"", ""type"": ""uint256"" } ], ""name"": ""likeReview"", ""outputs"": [ { ""internalType"": ""bool"", ""name"": """", ""type"": ""bool"" }, { ""internalType"": ""bool"", ""name"": """", ""type"": ""bool"" } ],""stateMutability"": ""payable"",""type"": ""function""},{""inputs"": [],""name"": ""reviewTokenAddress"",""outputs"": [{""internalType"": ""contract ReviewToken"",""name"": """",""type"": ""address""}],""stateMutability"": ""view"",""type"": ""function""},{""inputs"": [{""internalType"": ""string"",""name"": ""_title"",""type"": ""string""},{""internalType"": ""string"",""name"": ""_descripiton"",""type"": ""string""},{""internalType"": ""address"",""name"": ""_to"",""type"": ""address""},{""internalType"": ""uint256"",""name"": ""_amount"",""type"": ""uint256""}],""name"": ""writeReview"",""outputs"": [{""internalType"": ""bool"",""name"": """",""type"": ""bool""},{""internalType"": ""bool"",""name"": """",""type"": ""bool""}],""stateMutability"": ""payable"",""type"": ""function""}]";
        public async Task SignTx(UserInfo user, string message)
        {
            

        }
    }    
}
