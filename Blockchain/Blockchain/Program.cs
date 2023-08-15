
using Newtonsoft.Json;

namespace Blockchain 
{
    class Program
    {
        public static Blockchain ourBlockChain = new Blockchain();
        public static int Port = 0;
        public static P2PClient client = new P2PClient();
        public static P2PServer server = null;
        public static string name = "Unkown";
        static void Main(string[] args)
        {
            DateTime starTime = DateTime.Now;
            ourBlockChain.InitializeChain();

            if(args.Length >= 1) Port = int.Parse(args[0]);

            if (args.Length >= 2) name = args[1];

            if (Port > 0) 
            {
                server = new P2PServer();
                server.Start();
            }

            if (name != "Unkown") Console.WriteLine($"user: {name}");

            Console.WriteLine("====================================");
            Console.WriteLine("1. Connect to server");
            Console.WriteLine("2. Add transaction");
            Console.WriteLine("3. Show blockchain");
            Console.WriteLine("4. exit");
            Console.WriteLine("====================================");

            int selection = 0;

            while (selection != 4) 
            {
                switch (selection) 
                {
                    case 1:
                        Console.WriteLine("Please write a server url: ");
                        string serverUrl = Console.ReadLine();
                        client.Connect($"{serverUrl}/Blockchain");
                        break;
                    case 2:
                        Console.WriteLine("Please write a client name: ");
                        string receiverName = Console.ReadLine();
                        Console.WriteLine("Write amount: ");
                        string amount = Console.ReadLine();
                        ourBlockChain.CreateTransaction(new Transaction(name, receiverName, int.Parse(amount)));
                        ourBlockChain.ProcessPendingTransaction(name);
                        client.Broadcast(JsonConvert.SerializeObject(ourBlockChain));
                        break;
                    case 3:
                        Console.WriteLine("Blockchain");
                        Console.WriteLine(JsonConvert.SerializeObject(ourBlockChain, Formatting.Indented));
                        break;
                }

                Console.WriteLine("Please choose a section: ");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }

            client.Close();
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