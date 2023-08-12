using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        //public string Data { get; set; } instead of Transaction
        public IList<Transaction> Transactions { get; set; }
        public int Nonce { get; set; } = 0;
        public Block(DateTime timeStamp, string previousHash, IList<Transaction> transactions) 
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Transactions = transactions;
            //Hash = CalculateHash();
        }

        public string CalculateHash() 
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inbytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{Nonce}");
            byte[] bytes = sha256.ComputeHash( inbytes );
            return Convert.ToBase64String( bytes );
        }

        public void Mine(int difficulty) 
        {
            var leadingZeros = new string('0', difficulty);
            while (this.Hash == null || this.Hash.Substring(0,difficulty) != leadingZeros) 
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
        }
    }
}
