using BlockChain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Services
{
    public class BlockChainService
    {
        public List<Block> Chain { get; set; }

        public int Difficulty { get; set; } = 4;
        private readonly double _targetBlockTime = 30000; 
        private readonly int _adjustmentInterval = 4; 

        private readonly MiningService miningService;

        public readonly HashingService _hashingService;

        public BlockChainService(int difficulty,double _targetBlockTime, int _adjustmentInterval)
        {
            this.Difficulty = difficulty;
            this._targetBlockTime = _targetBlockTime;
            this._adjustmentInterval = _adjustmentInterval;

            _hashingService = new HashingService();
            Chain = new List<Block>();
            miningService= new MiningService();
            AddGenesisBlock();
        }

        

        private void AddGenesisBlock()
        {
            var genesis = new Block() { Data = "0", Index = 0, Timestamp = DateTime.UtcNow, Author = "System", PrevHash = "0" };
            miningService.MineBlock(genesis, Difficulty);
            Chain.Add(genesis);
        }

        public void AddBlock(string data,string author)
        {
            Block lastBlock = Chain[^1];
            var newBlock = new Block()
            {
                Index = lastBlock.Index + 1,
                Data = data,
                Author = author,
                PrevHash = lastBlock.Hash,
                Difficulty = Difficulty,
            };
            miningService.MineBlock(newBlock, Difficulty);
            Chain.Add(newBlock);
            if (newBlock.Index % _adjustmentInterval == 0) // Adjust difficulty every 10 blocks
            {
                AdjustDifficulty();
            }
        }

        //private void AdjustDifficulty()
        //{
        //    var recentBlock = Chain.Where(b=>b.Index>0).TakeLast(_adjustmentInterval).ToList();

        //    if(recentBlock.Count < 0) return;

        //    var avgTime = recentBlock.Average(b => b.MiningDuration);

        //    int minDifficulty = 1;
        //    int maxDifficulty = 4;

        //    int oldDifficulty = Difficulty;
        //    int newDifficulty = Difficulty;

        //    if (avgTime < _targetBlockTime * 0.25) 
        //    {
        //        newDifficulty += 2;
        //        Console.WriteLine($"Середній час ({avgTime:f2} мс) менший за 25% від target ({_targetBlockTime} мс). Збільшено на 2.");
        //    }

        //    else if (avgTime < _targetBlockTime * 0.50)
        //    {
        //        newDifficulty += 1;
        //        Console.WriteLine($"Середній час ({avgTime:f2} мс) менший за 50% від target ({_targetBlockTime} мс). Збільшено на 1.");
        //    }

        //    else if (avgTime > _targetBlockTime * 4.0) 
        //    {
        //        newDifficulty -= 2;
        //        Console.WriteLine($"Середній час ({avgTime:F2} мс) більший за 400% від target ({_targetBlockTime} мс). Зменшено на 2.");
        //    }
        //    else if (avgTime > _targetBlockTime * 2.0) 
        //    {
        //        newDifficulty -= 1;
        //        Console.WriteLine($"Середній час ({avgTime:F2} мс) більший за 200% від target ({_targetBlockTime} мс). Зменшено на 1.");
        //    }

        //    if(newDifficulty==oldDifficulty) Console.WriteLine("Difficulty не изменилась.");

        //    newDifficulty = Math.Clamp(newDifficulty, minDifficulty, maxDifficulty);
        //    Difficulty = newDifficulty;

        //    Console.WriteLine($"Средний час майнинга: {avgTime:F2} мс");
        //    Console.WriteLine($"TargetBlockTime: {_targetBlockTime} мс");
        //    Console.WriteLine($"Старая difficulty: {oldDifficulty}");
        //    Console.WriteLine($"Новая difficulty: {Difficulty}");

        //    Console.WriteLine(new string('-', 45) + "\n");
        //}

        public void ModifyBlockData(int index, string newData)
        {
            Chain[index].Data = newData;
            Console.WriteLine($"\n[УВАГА] Дані блоку #{index} були змінені на: '{newData}'");
        }

        public void RemineSingleBlock(int index)
        {
            
            var block = Chain[index];
            block.Nonce = 0;

            var sw = Stopwatch.StartNew();
            miningService.MineBlock(block, Difficulty);
            sw.Stop();
            block.MiningDuration = sw.Elapsed.TotalSeconds;

            Console.WriteLine($"\nБлок #{index} перемайнено окремо. Новий хеш: {block.Hash}");
            
        }

        public RepairResult RepairChain()
        {
            double totalTime = 0;
            long totalAttempts = 0;

            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];

                currentBlock.PrevHash = previousBlock.Hash;
                currentBlock.Nonce = 0;

                var sw = Stopwatch.StartNew();
                miningService.MineBlock(currentBlock, currentBlock.Difficulty);
                sw.Stop();

                currentBlock.MiningDuration = sw.Elapsed.TotalSeconds;
                totalTime += currentBlock.MiningDuration;
                totalAttempts += currentBlock.Nonce;
            }

            return new RepairResult
            {
                TotalTime = totalTime,
                TotalAttempts = totalAttempts
            };
        }
        private void AdjustDifficulty()
        {
            var recentBlock = Chain.Where(b => b.Index > 0).TakeLast(_adjustmentInterval).ToList();

            if (recentBlock.Count < 0) return;

            var avtTime = recentBlock.Average(b => b.MiningDuration);

            if (avtTime < _targetBlockTime)
            {
                Difficulty++;
                Console.WriteLine($"Difficulty increased to {Difficulty}");
            }
            else if (avtTime > _targetBlockTime)
            {
                Difficulty = Math.Max(1, Difficulty - 1); // Ensure difficulty doesn't go below 1
                Console.WriteLine($"Difficulty decreased to {Difficulty}");
            }
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];

                string currentHash = _hashingService.ComputeHash(currentBlock);
                if (currentBlock.Hash != currentHash)
                {
                    return false;
                }
                if (currentBlock.PrevHash != previousBlock.Hash)
                {
                    return false;
                }
                
                if (!currentBlock.Hash.StartsWith(new string('0', Difficulty)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
