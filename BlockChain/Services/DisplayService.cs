using BlockChain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Services
{
    public class DisplayService
    {
        public void ShowChain(List<Block> chain)
        {
            foreach (var block in chain)
            {
                Console.WriteLine($"Index: {block.Index}");
                Console.WriteLine($"Timestamp: {block.Timestamp.ToString("o")}");
                Console.WriteLine($"Author: {block.Author}");
                Console.WriteLine($"Data: {block.Data}");
                Console.WriteLine($"Hash: {block.Hash}");
                Console.WriteLine($"Nonce: {block.Nonce}");
                Console.WriteLine($"Mining Duration: {block.MiningDuration} seconds");
                Console.WriteLine($"Difficulty: {block.Difficulty}");
                Console.WriteLine($"PrevHash: {block.PrevHash}");
                Console.WriteLine(new string('-', 50));
            }
        }
    }
}
