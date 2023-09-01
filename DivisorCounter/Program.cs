 
using System.Diagnostics;
using CacheService.Controllers;  

public class Program
{ 

    public static void Main()
    {
        CacheController cc = new CacheController();

        var first = 1_000_000_000;
        var last = 1_000_000_020;

        var numberWithMostDivisors = first;
        var result = 0;

        cc.Open();
        var tables = cc.Tables();
        if (!tables.Any())
        {
            cc.CreateTable();
        }

        var watch = Stopwatch.StartNew();
        for (var i = first; i <= last; i++)
        {
            var innerWatch = Stopwatch.StartNew();
            var divisorCounter = cc.Get(i);

            if (divisorCounter == 0)
            {
                for (var divisor = 1; divisor <= i; divisor++)
                {
                    if (i % divisor == 0)
                    {
                        divisorCounter++;
                    }
                }

                cc.Post(i, divisorCounter);
            }

            innerWatch.Stop();
            Console.WriteLine("Counted " + divisorCounter + " divisors for " + i + " in " + innerWatch.ElapsedMilliseconds + "ms");

            if (divisorCounter > result)
            {
                numberWithMostDivisors = i;
                result = divisorCounter;
            }
        }
        watch.Stop();

        Console.WriteLine("The number with most divisors inside range is: " + numberWithMostDivisors + " with " + result + " divisors.");
        Console.WriteLine("Elapsed time: " + watch.ElapsedMilliseconds + "ms");
        Console.ReadLine();
        cc.Close();
    }
}