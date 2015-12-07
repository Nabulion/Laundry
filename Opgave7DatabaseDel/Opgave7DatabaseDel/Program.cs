using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opgave7DatabaseDel
{
    class Program
    {
        static void Main(string[] args)
        {
            BackEnd.BackEnd b = new BackEnd.BackEnd();

            b.TestData();
     
           Console.WriteLine("Total machines in reservations: " + b.TotalResMachine(b.LaundryRoom));
            b.AllLundries();
            
           Console.WriteLine("Before removed amount of reservations: "+b.LaundryUser.Reservations.Count);
           b.RemoveFutureRes(b.LaundryUser);
           Console.WriteLine("After removed amount of reservations:" + b.LaundryUser.Reservations.Count);
            
          
           b.DeleteTestData();
           Console.ReadLine();
          

        }
    }
}
