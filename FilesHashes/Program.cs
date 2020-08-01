using Akka.Actor;
using FilesHashes.Actors;
using FilesHashes.Messages;
using System;

namespace FilesHashes
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = args.Length > 0 ? args[0] : @"D:\";
            if (!System.IO.Directory.Exists(path))
            {
                Console.WriteLine("Provided path does not exist");
                Console.ReadLine();
            }

            // create a new actor system (a container for actors)
            using (var system = ActorSystem.Create("MySystem"))
            {
                var coordinatorActor = system.ActorOf<CoordinatorActor>("MyCoordinator");
                coordinatorActor.Tell(new ScanRequestMessage() { Path = path });

                Console.ReadLine();
            }
        }
    }
}
