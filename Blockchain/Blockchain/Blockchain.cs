﻿
namespace Blockchain
{
    public class Blockchain
    {
        public IList<Transaction> PendingTransacions = new List<Transaction>();
        public IList<Block> Chain { get; set; }
        public int difficulty { get; set; } = 3;
        public int Reward { get; set; } = 1;
        
        public Blockchain() 
        {
            InitializeChain();
        }

        public void InitializeChain() 
        {
            Chain = new List<Block>();
            AddGenesisBlock();
        }

        public Block CreateGenesisBlock() 
        {
            Block block = new Block(DateTime.Now, null, PendingTransacions);
            block.Mine(difficulty);
            PendingTransacions = new List<Transaction>();
            return block;
        }

        public void AddGenesisBlock() 
        {
            Chain.Add(CreateGenesisBlock());
        }

        public Block GetLatestBlock()  
        {
            return Chain[Chain.Count - 1];
        }

        public void AddBlock(Block block) 
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            block.Mine(this.difficulty);
            Chain.Add(block);
        }

        public void CreateTransaction(Transaction transaction) 
        {
            PendingTransacions.Add(transaction);
        }

        public void ProcessPendingTransaction(string minerAddress)
        {
            CreateTransaction(new Transaction(null, minerAddress, Reward));
            Block block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransacions);
            AddBlock(block);
            PendingTransacions = new List<Transaction>();
        }

        public bool IsValid() 
        {
            for (var i=1; i<Chain.Count;i++) 
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if(currentBlock.Hash != currentBlock.CalculateHash()) return false;
                if (currentBlock.PreviousHash != previousBlock.Hash) return false;
            }
            return true;
        }

        public int GetBalance(string address)
        {
            int balance = 0;
            for (var i = 0; i < Chain.Count; i++)
            {
                for(var j = 0; j < Chain[i].Transactions.Count; j++)
                {
                    var transaction = Chain[i].Transactions[j];
                    if (transaction.FromAddress == address) balance -= transaction.Amount;
                    if (transaction.ToAddress == address) balance += transaction.Amount;
                }
            }
            return balance;
        }
    }
}
