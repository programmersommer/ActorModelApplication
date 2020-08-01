using Akka.Actor;
using FilesHashes.Messages;
using System;
using System.IO;
using System.Security.Cryptography;

namespace FilesHashes.Actors
{
    class CalcHashesActor : ReceiveActor
    {
        public CalcHashesActor()
        {
            Receive<ScanRequestMessage>(message => ScanRequestHandler(message));
        }

        private void ScanRequestHandler(ScanRequestMessage message)
        {
            using var sha256 = SHA256.Create();
            var hash = "";
            try
            {
                using (var stream = File.OpenRead(message.Path))
                {
                    hash = BitConverter.ToString(sha256.ComputeHash(stream));
                }
                Context.ActorSelection("/user/MyCoordinator").Tell(new ScanResponseMessage() { File = $"{message.Path} {hash}" });
            }
            catch
            {
                Context.ActorSelection("/user/MyCoordinator").Tell(new ScanResponseMessage() { File = $"{message.Path} Hash was not calculated" });
            }
        }
    }
}
