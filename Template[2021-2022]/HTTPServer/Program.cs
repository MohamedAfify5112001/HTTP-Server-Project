using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // ده ال هيتسلم
            CreateRedirectionRulesFile();
            //Start server
            // 1) Make server object on port 1000
            // 2) Start Server
        }

        static void CreateRedirectionRulesFile()
        {
            StreamWriter fileRedirection = new StreamWriter(" redirectionRules.txt");
            fileRedirection.WriteLine("aboutus.html-aboutus2.html");
            fileRedirection.Close();
        }

    }
}
