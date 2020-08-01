using Akka.Actor;
using FilesHashes.Messages;
using System;
using System.IO;

namespace FilesHashes.Actors
{
    class CoordinatorActor : UntypedActor
    {

        private void ScanResponseHandler(ScanResponseMessage message)
        {
            Console.WriteLine(message.File);
        }

        private void ScanRequestHandler(ScanRequestMessage message)
        {
            var folders = Directory.GetDirectories(message.Path, "*", SearchOption.TopDirectoryOnly);

            foreach (var folder in folders)
            {
                var getFilesActor = Context.ActorOf<GetFilesActor>();
                getFilesActor.Tell(new ScanRequestMessage() { Path = folder });
            }
        }

        protected override void OnReceive(object message)
        {
            if (message.GetType() == typeof(ScanRequestMessage))
            {
                ScanRequestHandler((ScanRequestMessage)message);
            }
            else if (message.GetType() == typeof(ScanResponseMessage))
            {
                ScanResponseHandler((ScanResponseMessage)message);
            }
        }

    }
}
