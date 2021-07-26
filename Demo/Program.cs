using System;
using RealDigital.AppMessageBus;
using RealDigital.AppMessageBus.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static MessageBus _bus;
        static bool _done;

        static void Main(string[] args)
        {
            _bus = new MessageBus();
            _bus.Subscribe<DataReceivedEvent>(DisplayData1);
            _bus.Subscribe<Variant1>(DisplayData1);
            _bus.Subscribe<Variant2>(DisplayData1);
            Task.Factory.StartNew(GenerateEvents);
            _done = false;
            Task.Factory.StartNew(GenerateEvents2);
            Task.Factory.StartNew(GenerateEvents3);
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
            _done = true;
            _bus.ClearAllSubscriptions();
        }

        static void DisplayData1(DataReceivedEvent e)
        {
            Console.WriteLine(e.Description);
        }

        static void DisplayData2(DataReceivedEvent e)
        {
            Console.WriteLine(e.Description);
        }

        static void DisplayData3(DataReceivedEvent e)
        {
            Console.WriteLine(e.Description);
        }

        static void GenerateEvents()
        {
            _bus.Publish(new DataReceivedEvent($"{DateTime.Now.ToLongTimeString()} - 1st message.", false));
            Thread.Sleep(TimeSpan.FromSeconds(2));
            _bus.Publish(new DataReceivedEvent($"{DateTime.Now.ToLongTimeString()} - 2nd message.", false));
            Thread.Sleep(TimeSpan.FromSeconds(4));
            _bus.Publish(new DataReceivedEvent($"{DateTime.Now.ToLongTimeString()} - 3rd message.", false));
            Thread.Sleep(TimeSpan.FromSeconds(8));
            _bus.Publish(new DataReceivedEvent($"{DateTime.Now.ToLongTimeString()} - 4th message.", false));
            Thread.Sleep(TimeSpan.FromSeconds(16));
            _bus.Publish(new DataReceivedEvent($"{DateTime.Now.ToLongTimeString()} - 5th message.", false));
            Thread.Sleep(TimeSpan.FromSeconds(32));
            _bus.Publish(new DataReceivedEvent($"{DateTime.Now.ToLongTimeString()} - 6th message.", false));
        }

        static void GenerateEvents2()
        {
            var rnd = new Random();
            int i = 0;
            while (!_done)
            {
                int seconds = rnd.Next(1, 5);
                _bus.Publish(new Variant1($"{DateTime.Now.ToLongTimeString()} - Random message #{++i}.", false));
                Thread.Sleep(TimeSpan.FromSeconds(seconds));
            }
        }

        static void GenerateEvents3()
        {
            while (!_done)
            {
                _bus.Publish(new Variant1($"{DateTime.Now.ToLongTimeString()}", false));
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }

    public class Variant1 : DataReceivedEvent
    {
        public Variant1(string desc, bool main) : base(desc, main)
        {
        }
    }

    public class Variant2 : DataReceivedEvent
    {
        public Variant2(string desc, bool main) : base(desc, main)
        {
        }
    }
}
