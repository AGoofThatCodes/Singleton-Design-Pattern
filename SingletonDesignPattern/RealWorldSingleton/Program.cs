using System;

namespace RealWorldSingleton
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoadBalancer b1 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b2 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b3 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b4 = LoadBalancer.GetLoadBalancer();

            //same isntance?

            if(b1 == b2 && b2 == b3 && b3 == b4)
            {
                Console.WriteLine("same instance\n");
            }

            //Load balance 15 server request

            LoadBalancer balancer = LoadBalancer.GetLoadBalancer();
            for(int i = 0; i < 15; i++)
            {
                string server = balancer.Server;
                Console.WriteLine("Dispatch Request to:" + server);
            }
        }
    }

    public class LoadBalancer
    {
        static LoadBalancer instance;   
        List<string> servers = new List<string>();
        Random random = new Random();

        //lock synchronization object

        private static object locker = new object();

        protected LoadBalancer()
        {
            //List of available servers
            servers.Add("ServerI");
            servers.Add("ServerII");
            servers.Add("ServerIII");
            servers.Add("ServerIV");
            servers.Add("ServerV");
        }

        public static LoadBalancer GetLoadBalancer()
        {
            /**
             * Support multithreaded applications through Double locking pattern which
             * (once the instance exixsts) avoids locking each time
             * the method is invoked.
             * **/

            if(instance == null)
            {
                lock(locker)
                {
                    if(instance == null)
                    {
                        instance = new LoadBalancer();
                    }
                }
            }
            return instance;
        }

        public string Server
        {
            get
            {
                int r =  random.Next(servers.Count);
                return servers[r].ToString();
            }
        }
    }
}