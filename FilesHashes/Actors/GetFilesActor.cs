using Akka.Actor;
using FilesHashes.Messages;
using System;
using System.IO;

namespace FilesHashes.Actors
{
    class GetFilesActor : ReceiveActor
    {
        public GetFilesActor()
        {
            Receive<ScanRequestMessage>(message => ScanRequestHandler(message));
        }

        private void ScanRequestHandler(ScanRequestMessage message)
        {

            try
            {
                var files = Directory.GetFiles(message.Path);

                foreach (var item in files)
                {
                    var calcHashesActor = Context.ActorOf<CalcHashesActor>();
                    calcHashesActor.Tell(new ScanRequestMessage() { Path = item });
                }
            }
            catch
            {
                // In case if system directory is accessed
            }

            try
            {
                var folders = Directory.GetDirectories(message.Path, "*", SearchOption.TopDirectoryOnly);

                foreach (var folder in folders)
                {
                    var getFilesActor = Context.ActorOf<GetFilesActor>();
                    getFilesActor.Tell(folder);
                }
            }
            catch
            {
                // In case if system directory is accessed
            }
        }

        // Fault Tolerance
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeRange: TimeSpan.FromMinutes(1),
                localOnlyDecider: ex =>
                {
                    switch (ex)
                    {
                        case System.IO.IOException ioe:
                            return Directive.Resume;
                        case System.UnauthorizedAccessException uae:
                            return Directive.Resume;
                        default:
                            return Directive.Escalate;
                    }
                });
        }

        protected override void PreRestart(Exception reason, object message)
        {

        }
    }
}
