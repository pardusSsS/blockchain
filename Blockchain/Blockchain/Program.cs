
using Newtonsoft.Json;

namespace Blockchain 
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain ourBlockChain = new Blockchain();
            DateTime starTime = DateTime.Now;

            //ourBlockChain.AddBlock(new Block(DateTime.Now, null, "{sender:Ömer,receiver:Uğur,amount:5}"));
            //ourBlockChain.AddBlock(new Block(DateTime.Now, null, "{sender:Uğur,receiver:Arda,amount:3}"));
            //ourBlockChain.AddBlock(new Block(DateTime.Now, null, "{sender:Arda,receiver:Derya,amount:1}"));

            //DateTime finishTime = DateTime.Now;

            //Console.WriteLine("****************************************************");

            //Console.WriteLine("IsValid: " + ourBlockChain.IsValid());

            ////bir tanesi patlarsa hepsi patlar
            //ourBlockChain.Chain[1].Data = "{sender:Ömer,receiver:Ali,amount:99}";
            //ourBlockChain.Chain[1].Hash = ourBlockChain.Chain[1].CalculateHash();

            //Console.WriteLine("****************************************************");

            //Console.WriteLine("Changed IsValid: " + ourBlockChain.IsValid());

            //Console.WriteLine("****************************************************");

            //ourBlockChain.Chain[2].PreviousHash = ourBlockChain.Chain[1].Hash;
            //ourBlockChain.Chain[2].Hash = ourBlockChain.Chain[2].CalculateHash();

            //Console.WriteLine("Changed IsValid: " + ourBlockChain.IsValid());

            //Console.WriteLine("****************************************************");

            //ourBlockChain.Chain[3].PreviousHash = ourBlockChain.Chain[2].Hash;
            //ourBlockChain.Chain[3].Hash = ourBlockChain.Chain[3].CalculateHash();

            //Console.WriteLine("Is chain hacked: " + ourBlockChain.IsValid());


            ourBlockChain.CreateTransaction(new Transaction("Ömer", "Uğur", 15));
            ourBlockChain.ProcessPendingTransaction("Ali");//ödül ali ye gidecek
            ourBlockChain.CreateTransaction(new Transaction("Uğur", "Ömer", 10));
            ourBlockChain.CreateTransaction(new Transaction("Uğur", "Ömer", 2));
            ourBlockChain.ProcessPendingTransaction("Ali");

            DateTime finishTime = DateTime.Now;

            Console.WriteLine("Preccess time: " + (finishTime - starTime).ToString());
            Console.WriteLine("Ömer Balance: " + ourBlockChain.GetBalance("Ömer"));
            Console.WriteLine("Uğur Balance: " + ourBlockChain.GetBalance("Uğur"));
            Console.WriteLine("Ali  Balance: " + ourBlockChain.GetBalance("Ali"));
            Console.WriteLine(JsonConvert.SerializeObject(ourBlockChain, Formatting.Indented));

            Console.WriteLine("****************************************************");

            Console.WriteLine("IsValid: " + ourBlockChain.IsValid());

            Console.ReadKey();
        }
    }
}