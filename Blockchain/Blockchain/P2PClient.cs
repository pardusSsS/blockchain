﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Blockchain
{
    public class P2PClient
    {
        public IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

        public void Connect(string url)
        {
            if (!wsDict.ContainsKey(url)) 
            {
                WebSocket ws = new WebSocket(url);
                ws.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Hi Client")
                    {
                        Console.WriteLine(e.Data);
                    }
                    else 
                    {
                        Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                        if(newChain.IsValid() && newChain.Chain.Count > Program.ourBlockChain.Chain.Count) 
                        {
                            List<Transaction> newTransaction = new List<Transaction>();
                            newTransaction.AddRange(newChain.PendingTransacions);
                            newTransaction.AddRange(Program.ourBlockChain.PendingTransacions);
                            newChain.PendingTransacions = newTransaction;
                            Program.ourBlockChain = newChain;
                        }
                    }
                };
                ws.Connect();
                ws.Send("Hi Server");
                ws.Send(JsonConvert.SerializeObject(Program.ourBlockChain));
                wsDict.Add(url,ws);
            }
        }

        public void Send(string url, string data) 
        {
            foreach(var item in wsDict)
            {
                if(item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data) 
        {
            foreach(var item in wsDict)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers() 
        {
            IList<string> servers = new List<string>();
            foreach(var item in wsDict)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public void Close() 
        {
            foreach (var item in wsDict) 
            {
                item.Value.Close();
            }
        }
    }
}
