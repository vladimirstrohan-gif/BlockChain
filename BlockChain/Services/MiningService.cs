using BlockChain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Services
{
    public class MiningService
    {
        private readonly HashingService _hashingService;

        public MiningService()
        {
            _hashingService = new HashingService();
        }

       
        public void MineBlock(Block block, int difficulty)
        {
            string target = new string('0', difficulty);
            var sw = Stopwatch.StartNew();
            while (true)
            {
                block.Hash = _hashingService.ComputeHash(block);
                if (block.Hash.Substring(0, difficulty) == target)
                {
                    block.MiningDuration = sw.Elapsed.TotalSeconds;
                    break;
                }
                block.Nonce++;

                if (block.Nonce % 100000 == 0)
                {
                    Console.WriteLine($"Mining... Current Nonce: {block.Nonce}, Current Hash: {block.Hash}");
                }
            }
        }
    }
}
