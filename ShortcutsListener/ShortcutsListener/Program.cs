using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ShortcutsListener
{
    class Program
    {
        private static byte[] responseBytes = Encoding.ASCII.GetBytes(HTTPRequest.BasicResponse);
        private static byte[] msg = new byte[10000];

        [STAThread]
        static void Main(string[] args)
        {
            var port = 2560;
            var server = new TcpListener(IPAddress.Any, port);
            server.Start();

            while (true)
            {
                Console.WriteLine($"Listening on port {port}");
                var client = server.AcceptTcpClient();  //if a connection exists, the server will accept it
                var networkStream = client.GetStream(); //networkstream is used to send/receive messages

                HTTPRequest.Parse(networkStream);

                //get the file size 
                if (HTTPRequest.Headers.ContainsKey(HTTPReqHeaderKey.ContentLength) && int.TryParse(HTTPRequest.Headers[HTTPReqHeaderKey.ContentLength], out int numberOfbytesToRead))
                {                 
                    var fileName = HTTPRequest.Headers.ContainsKey(HTTPReqHeaderKey.FileName) 
                        ? HTTPRequest.Headers[HTTPReqHeaderKey.FileName] 
                        : $"file_{Guid.NewGuid()}";

                    //get the extention of the file its been always image/*, video/*, */*
                    var fileExtention = HTTPRequest.Headers[HTTPReqHeaderKey.ContentType].Split('/')[1].ToLower();


                    switch (fileExtention)
                    {
                        case "plain":
                            fileExtention = "txt";
                            break;
                        case "quicktime": //ios puts video/quicktime content-type header for their video files.
                            fileExtention = "mov";
                            break;
                        default:
                            break;
                    }

                    fileName = fileName + "." + fileExtention;

                    Console.WriteLine(fileName);
                    var fileStream = new FileStream(fileName, FileMode.Create);
                    int readCounter = 0;

                    while (readCounter < numberOfbytesToRead)
                    {
                        int numberOfBytesRead = networkStream.Read(msg, 0, msg.Length);
                        readCounter += numberOfBytesRead;
                        fileStream.Write(msg, 0, numberOfBytesRead);
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write($"{((UInt64)readCounter * 100) / ((UInt64)numberOfbytesToRead - 1)}% Completed");
                    }
                    fileStream.Close();
                    Console.WriteLine('\n');
                }
                networkStream.Write(responseBytes, 0, responseBytes.Length);
                networkStream.Close();
                client.Close();
            }
        }
    }
}
