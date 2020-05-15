using System;

namespace HelloWorldConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Hello World! 2");
            Console.WriteLine("Hello World! 3");
            //System.Threading.Thread.Sleep(8000);


            CreateSchedularJob(args);
        } 

        public static void CreateSchedularJob(string[] args){
            if(args != null && args.Length > 0){
                Console.WriteLine(args[0].ToString());
            }
        }
    }
}
