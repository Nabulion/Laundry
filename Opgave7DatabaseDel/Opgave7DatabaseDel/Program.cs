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

            
            b.AllLundries();
            
            Console.WriteLine("Before removed amount of reservations: " + b.LaundryUser.Reservations.Count);

            b.RemoveFutureRes("eunji1");

            Console.WriteLine("After removed amount of reservations:" + b.LaundryUser.Reservations.Count);


            b.DeleteTestData();
           
            Console.ReadLine();


        }


    }
}
