using BLockReviewsAPI.Models;
using Microsoft.Extensions.Configuration;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Nethereum.Web3;
using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace BLockReviewsAPI.BlockChainDI
{
	public interface IEtherConn
	{
		public Task GetBlockNumber();
		public Task<BlockReviewAccount> CreateAccount();

		public Task ReviewContract(Review review, ReviewActions actions);
	}

	public class EtherConn : IEtherConn
	{
		private Web3 web3;
		private string web3URL = "";
		private IConfiguration configuration;
		public EtherConn(IConfiguration _configuration)
		{
			configuration = _configuration;
			web3URL = configuration["Ether:BlockURL"];
			web3 = new Web3(web3URL);
		}

		public async Task GetBlockNumber()
		{
			var balance = await web3.Eth.GetBalance.SendRequestAsync("0xF168C8E33782dB08771d337a7E2111cf8ea1a5F7");

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
				PrivateKey = privateKey,
				PublicKey = account.Address
			};
		}

		private string ReviewContractAddress = "0xdCa52Fd1334dF9a69a2d3416590eB95Ec9f2CB6b";
		private string abi = @"[
	{
		""inputs"": [
			{
				""internalType"": ""string"",
				""name"": ""_title"",
				""type"": ""string""
			},
			{
				""internalType"": ""string"",
				""name"": ""_des"",
				""type"": ""string""
			},
			{
				""internalType"": ""string"",
				""name"": ""_writer"",
				""type"": ""string""
			}
		],
		""name"": ""create"",
		""outputs"": [],
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""uint256"",
				""name"": ""_idx"",
				""type"": ""uint256""
			}
		],
		""name"": ""get"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			},
			{
				""internalType"": ""string"",
				""name"": """",
				""type"": ""string""
			},
			{
				""internalType"": ""string"",
				""name"": """",
				""type"": ""string""
			},
			{
				""internalType"": ""string"",
				""name"": """",
				""type"": ""string""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""inputs"": [],
		""name"": ""getIdx"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""name"": ""Review_mapping"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": ""id"",
				""type"": ""uint256""
			},
			{
				""internalType"": ""string"",
				""name"": ""title"",
				""type"": ""string""
			},
			{
				""internalType"": ""string"",
				""name"": ""description"",
				""type"": ""string""
			},
			{
				""internalType"": ""string"",
				""name"": ""writer"",
				""type"": ""string""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	}
]";
		public async Task ReviewContract(Review review, ReviewActions actions)
		{
			var privateKey = "9965659ffce1eb91e09f9675fea70ac49b2e0ecaa275a35406d53cf094d85932";
			var senderAddress = "0xF168C8E33782dB08771d337a7E2111cf8ea1a5F7";

			var account = new Nethereum.Web3.Accounts.Account(privateKey);
			web3 = new Web3(account, web3URL);
			web3.TransactionManager.UseLegacyAsDefault = true;


			var contract = web3.Eth.GetContract(abi, ReviewContractAddress);


			var function = contract.GetFunction(actions.ToString());

			if (actions == ReviewActions.getIdx)
			{
				var result = await function.CallAsync<int>();
			}
			else if (actions == ReviewActions.create)
			{
				HexBigInteger GasPrice = Nethereum.Web3.Web3.Convert.ToWei(2.5, UnitConversion.EthUnit.Gwei).ToHexBigInteger();
				HexBigInteger Gas = 200000.ToHexBigInteger();


				// 이것도 되는거임
				//	var transfer = new object[]
				//	{
				//		"text","text","test"					
				//	};
				//	var result = await function.SendTransactionAndWaitForReceiptAsync(senderAddress, Gas, GasPrice, null, transfer);

				var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
				var transfer = new TransferFunction()
				{
					title = "text",
					des = "test",
					writer = "test"
				};

				transfer.GasPrice = GasPrice;
				transfer.Gas = Gas;
				//transfer.Nonce = 2;

				var transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(ReviewContractAddress, transfer);
				//var result = transactionReceipt.ContractAddress;
				//var result = await transferHandler.SignTransactionAsync(ReviewContractAddress, transfer);


			}

		}
	}

	public enum ReviewActions
	{
		create,
		get,
		getIdx,
	}

	[Function("create")]
	public class TransferFunction : FunctionMessage
	{
		[Parameter("string", "_title", 1)]
		public string title { get; set; }

		[Parameter("string", "_des", 2)]
		public string des { get; set; }

		[Parameter("string", "_writer", 3)]
		public string writer { get; set; }
	}
}
