using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            this.LoadRedirectionRules(redirectionMatrixPath);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(address, portNumber);
            serverSocket.Bind(ipEndPoint);
        }

        public void StartServer()
        {
            serverSocket.Listen(1000);
            while (true)
            {
                Socket acceptClientSocket = serverSocket.Accept();
                Thread newThread = new Thread(new ParameterizedThreadStart(HandleConnection));
                newThread.Start(acceptClientSocket);
            }
        }

        public void HandleConnection(object obj)
        {
            Socket clientSocket = (Socket)obj;
            clientSocket.ReceiveTimeout = 0;

            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] buffer = new byte[1024 * 1024];
                    int receivedLen = clientSocket.Receive(buffer);
                    if (receivedLen == 0) { break; }

                    // TODO: Create a Request object using received request string
                    Request req = new Request(Encoding.ASCII.GetString(buffer));

                    // TODO: Call HandleRequest Method that returns the response
                    //Response resp = HandleRequest(req);
                    // TODO: Send Response back to client
                    //clientSocket.Send(Encoding.ASCII.GetBytes(resp.ResponseString));

                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }

            clientSocket.Shutdown(SocketShutdown.Both); 
            clientSocket.Close();   
        }

        Response HandleRequest(Request request)
        {
            throw new NotImplementedException();
            string content;
            try
            {
                //TODO: check for bad request 

                //TODO: map the relativeURI in request to get the physical path of the resource.

                //TODO: check for redirect

                //TODO: check file exists

                //TODO: read the physical file

                // Create OK response
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            string RedirectionPath;
            if (relativePath[0] == '/')
            {
                relativePath = relativePath.Substring(1);
            }
            bool exist = Configuration.RedirectionRules.TryGetValue(relativePath, out RedirectionPath);
            if (exist)
            {
                return RedirectionPath;
            }
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            bool existPage = File.Exists(filePath);
            if (!existPage) {
                Logger.LogException(new Exception(defaultPageName + " Page not Exist"));
                return string.Empty;
            }

            string content = File.ReadAllText(filePath);
            return content;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                string[] pageName = File.ReadAllLines(filePath);
                Configuration.RedirectionRules = new Dictionary<string, string>();
                for (int i = 0; i < pageName.Length; i++)
                {
                    string[] pageNameAfterSplit = pageName[i].Split('-');
                    Configuration.RedirectionRules.Add(pageNameAfterSplit[0], pageNameAfterSplit[1]);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}
