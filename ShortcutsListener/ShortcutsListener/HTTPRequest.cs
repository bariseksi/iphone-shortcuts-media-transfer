using System.Net.Sockets;
using System.Text;

namespace ShortcutsListener
{
    public static class HTTPRequest
    {
        private static byte[] byteArray = new byte[10000];
        public static Dictionary<string, string> Headers = new Dictionary<string, string>();
        public static string? Method;
        public static string? Request;
        public static string BasicResponse = "HTTP/1.1 200 OK\n";

        public static void Parse(NetworkStream stream)
        {
            int foundCounter = 0;
            byte[] endOfLine = { 13, 10 };
            bool parsingDone = false;
            int numberOfLineFound = 0;
            Headers.Clear();
            while (!parsingDone)
            {
                for (int i = 0; i < byteArray.Length; i++)
                {
                    byte _byte = (byte)stream.ReadByte(); //reads -1 when reaches the end
                    byteArray[i] = _byte;
                    if (_byte == endOfLine[foundCounter])
                    {
                        foundCounter++;
                        if (foundCounter == 2) //we found the end of line
                        {
                            if (i == 1)
                            {
                                parsingDone = true;
                                break;
                            }
                            foundCounter = 0;
                            numberOfLineFound++;
                            if (numberOfLineFound == 1) //first line
                            {
                                string[] values = Encoding.ASCII.GetString(byteArray, 0, i - 1).Split(' ');
                                Method = values[0].Trim('\r', ' ');
                                Request = values[1].Trim('\r', ' ');
                            }
                            else
                            {
                                string[] values = Encoding.ASCII.GetString(byteArray, 0, i - 1).Split(':');
                                Headers.Add(values[0].Trim('\r', ' ').ToLower(), values[1].Trim('\r', ' ').ToLower());
                            }
                            break;
                        }
                    }
                }


            }
        }
    }

    public static class HTTPReqHeaderKey
    {
        public const string ContentType = "content-type";
        public const string FileName = "filename";
        public const string ContentLength = "content-length";
    }

    public static class HTTPReqHeaderValue
    {
        public const string ContentType_Image_JPEG = "image/jpeg";
        public const string ContentType_Image_HEIC = "image/heic";
        public const string ContentType_Image_PNG = "image/png";
        public const string ContentType_Text_Plain = "text/plain";
        public const string ContentType_Video_MP4 = "video/mp4";
        public const string ContentType_Video_MOV = "video/quicktime";
        public const string ContentType_text_HTML = "text/html";
    }
}
