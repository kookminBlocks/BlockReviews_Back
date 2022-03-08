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
    }

    public class EtherConn : IEtherConn
    {
        private const string web3URL = "https://rinkeby.infura.io/v3/ee11c234c6cc4090aaef4f4718f76a9e";
        public EtherConn()
        {            
        }

        public async Task GetBlockNumber()
        {
            var web3 = new Web3(web3URL);
            var latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            Console.WriteLine($"Latest Block Number is: {latestBlockNumber}");
        }
    }    
}
