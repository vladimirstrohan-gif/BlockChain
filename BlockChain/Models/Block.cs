using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Models
{
    public class Block
    {
        // 
        public int Index { get; set; }
        //
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        //
        public string Author { get; set; }
        //
        public string Data { get; set; }
        //
        public string Hash { get; set; }
        // 
        public string PrevHash { get; set; }
        //Nonce - це число, которое используется в процессе майнинга для нахождения хэша блока, удовлетворяющего определенным условиям (например, начинающегося с определенного количества нулей).
        public long Nonce { get; set; } = 0;
        // Difficulty - это параметр, который определяет сложность задачи майнинга. Чем выше значение difficulty, тем сложнее найти подходящий хэш для блока.
        public int Difficulty { get; set; } = 1;

        public double MiningDuration { get; set; } = 0; // Time taken to mine the block in milliseconds
    }
}
