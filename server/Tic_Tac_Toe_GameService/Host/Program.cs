using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGameService;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(TicTacToeServiceFunction));
            host.Open();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            host.Close();
        }
    }
}
