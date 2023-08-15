using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Blockchain 
{
    public class P2PServer : WebSocketBehavior
    {
        WebSocketServer wss = null;
        bool chainSynched = false;
        public void Start()
        {
            wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            wss.AddWebSocketService<P2PServer>("/Blockchain");
            wss.Start();
            Console.WriteLine("Server is started...");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi Server")
            {
                Console.WriteLine(e.Data);
                Send("Hi Client");
            }
            else
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                if (newChain.IsValid() && newChain.Chain.Count > Program.ourBlockChain.Chain.Count)
                {
                    List<Transaction> newTransaction = new List<Transaction>();
                    newTransaction.AddRange(newChain.PendingTransacions);
                    newTransaction.AddRange(Program.ourBlockChain.PendingTransacions);
                    newChain.PendingTransacions = newTransaction;
                    Program.ourBlockChain = newChain;
                }
            }

            if (!chainSynched)
            {
                Send(JsonConvert.SerializeObject(Program.ourBlockChain));
                chainSynched = true;
            }
        }
    }
}
