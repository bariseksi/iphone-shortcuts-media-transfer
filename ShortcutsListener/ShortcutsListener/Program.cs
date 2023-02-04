using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ShortcutsListener
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int port = 2560;
            byte[] responseBytes = Encoding.ASCII.GetBytes(HTTPRequest.BasicResponse);
            TcpListener server = new(IPAddress.Any, port);
            byte[] msg = new byte[10000];
            server.Start();

            while (true)
            {
                Console.WriteLine($"Listening on port {port}");
                TcpClient client = server.AcceptTcpClient();  //if a connection exists, the server will accept it
                NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

                HTTPRequest.Parse(ns);

                //get the file size 
                if (HTTPRequest.Headers.ContainsKey(HTTPReqHeaderKey.ContentLength) && int.TryParse(HTTPRequest.Headers[HTTPReqHeaderKey.ContentLength], out int numberOfbytesToRead))
                {

                    string? fileName;
                    if (HTTPRequest.Headers.ContainsKey(HTTPReqHeaderKey.FileName))
                    {
                        fileName = HTTPRequest.Headers[HTTPReqHeaderKey.FileName];
                    }
                    else //if file name is not specified generate something unique
                    {
                        fileName = $"file_{Guid.NewGuid()}";
                    }

                    //get the extention of the file its been always image/*, video/*, */*
                    string? fileExtention = HTTPRequest.Headers[HTTPReqHeaderKey.ContentType].Split('/')[1].ToLower();


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
                    FileStream fs = new FileStream(fileName, FileMode.Create);
                    int readCounter = 0;
                    while (readCounter < numberOfbytesToRead)
                    {
                        int numberOfBytesRead = ns.Read(msg, 0, msg.Length);
                        readCounter += numberOfBytesRead;
                        fs.Write(msg, 0, numberOfBytesRead);
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write($"{((UInt64)readCounter * 100) / ((UInt64)numberOfbytesToRead - 1)}% Completed");
                    }
                    fs.Close();
                    Console.WriteLine('\n');
                }
                ns.Write(responseBytes, 0, responseBytes.Length);
                ns.Close();
                client.Close();
            }
        }
    }
}
