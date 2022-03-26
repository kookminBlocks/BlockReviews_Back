using BLockReviewsAPI.Models;
using Microsoft.Extensions.Configuration;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Util;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI.BlockChainDI
{
    public interface IRegisterContract
    {
        public Task RegisterAccount(UserInfo user);
    }

    public class RegisterContract : IRegisterContract
    {
        private Web3 web3;
        private RpcClient web3URL;
        private IConfiguration configuration;
        public RegisterContract(IConfiguration _configuration)
        {
            configuration = _configuration;
            var url = configuration["Ether:BlockURL"];
            web3URL = new RpcClient(new Uri(url));                           
        }


        private const string ContractAddr = "0x1AB655C9396c4D8416Fa9fabDeAd33E76fa0E24C";
        private const string ContractAbi = @"[
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_amount"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""approveForGSN"",
            ""outputs"": [],
            ""stateMutability"": ""payable"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""address"",
                    ""name"": ""_tokenOwner"",
                    ""type"": ""address""
                }
            ],
            ""name"": ""approveForNFT"",
            ""outputs"": [],
            ""stateMutability"": ""payable"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_reviewId"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""address"",
                    ""name"": ""_admin"",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_amount"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""createLiked"",
            ""outputs"": [],
            ""stateMutability"": ""payable"",
            ""type"": ""function""
        },
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
                    ""internalType"": ""address"",
                    ""name"": ""_admin"",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_amount"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""address"",
                    ""name"": ""_nftOwner"",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""string"",
                    ""name"": ""_nftUri"",
                    ""type"": ""string""
                },
                {
                    ""internalType"": ""string"",
                    ""name"": ""_category"",
                    ""type"": ""string""
                }
            ],
            ""name"": ""createReview"",
            ""outputs"": [],
            ""stateMutability"": ""payable"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_tokenId"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_price"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""registerForSale"",
            ""outputs"": [],
            ""stateMutability"": ""payable"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""address"",
                    ""name"": ""_owner"",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""address"",
                    ""name"": ""_buyer"",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_tokenId"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_amount"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""saleNFT"",
            ""outputs"": [],
            ""stateMutability"": ""payable"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""address payable"",
                    ""name"": ""_tokenContractAddress"",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""address payable"",
                    ""name"": ""_nftContractAddress"",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""address"",
                    ""name"": ""_forwarderAddress"",
                    ""type"": ""address""
                }
            ],
            ""stateMutability"": ""payable"",
            ""type"": ""constructor""
        },
        {
            ""anonymous"": false,
            ""inputs"": [
                {
                    ""indexed"": false,
                    ""internalType"": ""string"",
                    ""name"": ""title"",
                    ""type"": ""string""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""string"",
                    ""name"": ""description"",
                    ""type"": ""string""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""address"",
                    ""name"": ""writer"",
                    ""type"": ""address""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""address[]"",
                    ""name"": ""likedUser"",
                    ""type"": ""address[]""
                }
            ],
            ""name"": ""like_event"",
            ""type"": ""event""
        },
        {
            ""anonymous"": false,
            ""inputs"": [
                {
                    ""indexed"": false,
                    ""internalType"": ""string"",
                    ""name"": ""title"",
                    ""type"": ""string""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""string"",
                    ""name"": ""description"",
                    ""type"": ""string""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""address"",
                    ""name"": ""writer"",
                    ""type"": ""address""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""uint256"",
                    ""name"": ""nftId"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""review_event"",
            ""type"": ""event""
        },
        {
            ""anonymous"": false,
            ""inputs"": [
                {
                    ""indexed"": false,
                    ""internalType"": ""address"",
                    ""name"": ""_from"",
                    ""type"": ""address""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""address"",
                    ""name"": ""_to"",
                    ""type"": ""address""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""uint256"",
                    ""name"": ""_tokenId"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""tradeNft_event"",
            ""type"": ""event""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_tokenId"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""withdrawFromSale"",
            ""outputs"": [],
            ""stateMutability"": ""payable"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""address"",
                    ""name"": ""_owner"",
                    ""type"": ""address""
                }
            ],
            ""name"": ""getNftBalanceOf"",
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
                    ""name"": ""_nftId"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""getNftOwnerOf"",
            ""outputs"": [
                {
                    ""internalType"": ""address"",
                    ""name"": """",
                    ""type"": ""address""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_nftId"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""getNftTokenUri"",
            ""outputs"": [
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
            ""inputs"": [
                {
                    ""internalType"": ""string"",
                    ""name"": ""_category"",
                    ""type"": ""string""
                }
            ],
            ""name"": ""getReviewByCategory"",
            ""outputs"": [
                {
                    ""components"": [
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
                            ""internalType"": ""address"",
                            ""name"": ""writer"",
                            ""type"": ""address""
                        },
                        {
                            ""internalType"": ""address[]"",
                            ""name"": ""likedUser"",
                            ""type"": ""address[]""
                        },
                        {
                            ""internalType"": ""uint256"",
                            ""name"": ""nftId"",
                            ""type"": ""uint256""
                        },
                        {
                            ""internalType"": ""uint256"",
                            ""name"": ""price"",
                            ""type"": ""uint256""
                        },
                        {
                            ""internalType"": ""string"",
                            ""name"": ""category"",
                            ""type"": ""string""
                        },
                        {
                            ""internalType"": ""uint256"",
                            ""name"": ""createdAt"",
                            ""type"": ""uint256""
                        }
                    ],
                    ""internalType"": ""struct BlockReview.Review[]"",
                    ""name"": """",
                    ""type"": ""tuple[]""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_reviewId"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""getReviewById"",
            ""outputs"": [
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
                    ""internalType"": ""address"",
                    ""name"": """",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""address[]"",
                    ""name"": """",
                    ""type"": ""address[]""
                },
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
                    ""internalType"": ""address"",
                    ""name"": ""_writer"",
                    ""type"": ""address""
                }
            ],
            ""name"": ""getReviewByWriter"",
            ""outputs"": [
                {
                    ""components"": [
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
                            ""internalType"": ""address"",
                            ""name"": ""writer"",
                            ""type"": ""address""
                        },
                        {
                            ""internalType"": ""address[]"",
                            ""name"": ""likedUser"",
                            ""type"": ""address[]""
                        },
                        {
                            ""internalType"": ""uint256"",
                            ""name"": ""nftId"",
                            ""type"": ""uint256""
                        },
                        {
                            ""internalType"": ""uint256"",
                            ""name"": ""price"",
                            ""type"": ""uint256""
                        },
                        {
                            ""internalType"": ""string"",
                            ""name"": ""category"",
                            ""type"": ""string""
                        },
                        {
                            ""internalType"": ""uint256"",
                            ""name"": ""createdAt"",
                            ""type"": ""uint256""
                        }
                    ],
                    ""internalType"": ""struct BlockReview.Review[]"",
                    ""name"": """",
                    ""type"": ""tuple[]""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""address"",
                    ""name"": ""forwarder"",
                    ""type"": ""address""
                }
            ],
            ""name"": ""isTrustedForwarder"",
            ""outputs"": [
                {
                    ""internalType"": ""bool"",
                    ""name"": """",
                    ""type"": ""bool""
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
                    ""internalType"": ""address"",
                    ""name"": ""writer"",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""nftId"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""price"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""string"",
                    ""name"": ""category"",
                    ""type"": ""string""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""createdAt"",
                    ""type"": ""uint256""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""string"",
                    ""name"": """",
                    ""type"": ""string""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": """",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""reviewByCategory_mapping"",
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
                    ""internalType"": ""address"",
                    ""name"": """",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": """",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""reviewByWriter_mapping"",
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
            ""inputs"": [],
            ""name"": ""reviewCoinAddress"",
            ""outputs"": [
                {
                    ""internalType"": ""contract ReviewCoin"",
                    ""name"": """",
                    ""type"": ""address""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function""
        },
        {
            ""inputs"": [],
            ""name"": ""reviewNftAddress"",
            ""outputs"": [
                {
                    ""internalType"": ""contract ReviewNFT"",
                    ""name"": """",
                    ""type"": ""address""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function""
        },
        {
            ""inputs"": [],
            ""name"": ""TotalSupply"",
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
            ""inputs"": [],
            ""name"": ""trustedForwarder"",
            ""outputs"": [
                {
                    ""internalType"": ""address"",
                    ""name"": """",
                    ""type"": ""address""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function""
        },
        {
            ""inputs"": [],
            ""name"": ""versionRecipient"",
            ""outputs"": [
                {
                    ""internalType"": ""string"",
                    ""name"": """",
                    ""type"": ""string""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function""
        }
    ]";
        public async Task RegisterAccount(UserInfo user)
        {
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            

            web3 = new Web3(account, web3URL);    

            web3.TransactionManager.UseLegacyAsDefault = true;
            var contract = web3.Eth.GetContract(ContractAbi, ContractAddr);
            //var function = contract.GetFunction(actions.ToString());

            HexBigInteger GasPrice = Nethereum.Web3.Web3.Convert.ToWei(2.5, UnitConversion.EthUnit.Gwei).ToHexBigInteger();
            HexBigInteger Gas = 200000.ToHexBigInteger();

            var gsnHandle = web3.Eth.GetContractTransactionHandler<GsnApprove>();
            var gsnParam = new GsnApprove() { amount = 10000000 };
            gsnParam.GasPrice = GasPrice;
            gsnParam.Gas = Gas;

            var nftHandle = web3.Eth.GetContractTransactionHandler<NftApprove>();
            var nftParam = new NftApprove() { tokenOwner = account.Address };
            nftParam.GasPrice = GasPrice;
            nftParam.Gas = Gas;


            var GsnReceipt = await gsnHandle.SendRequestAndWaitForReceiptAsync(ContractAddr, gsnParam);
            var NftReceipt = await nftHandle.SendRequestAndWaitForReceiptAsync(ContractAddr, nftParam);
        }



    }

    [Function("approveForGSN")]
    public class GsnApprove : FunctionMessage
    {
        [Parameter("uint256", "_amount", 1)]
        public int amount { get; set; }
    }

    [Function("approveForNFT")]
    public class NftApprove : FunctionMessage
    {
        [Parameter("address", "_tokenOwner", 1)]
        public string tokenOwner { get; set; }
    }
}
