using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlockChain.Models;

namespace BlockChain.Services
{
    public class HashingService
    {
        public string ComputeHash(Block block)
        {

            string rawData = $"{block.Index}{block.Data}{block.Timestamp}{block.Author}{block.PrevHash}{block.Nonce}{block.Difficulty}";
            return ComputeHash(rawData);

        }
        private string ComputeHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA256.HashData(inputBytes);

            return Convert.ToBase64String(hashBytes).ToLower();
        }
    }
}
