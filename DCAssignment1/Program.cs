using System;
using System.ServiceModel;
using InterfaceLib;

namespace AuthenticatorConsoleApp
{
    /**
     *  This is the .NET remoting server which has three operations open as service functions in the AuthenticatorService internal class
     *  This class is for building the actual server part.
     *  It provides authentication services. 
     *  The other actors will communicate with the authenticator when they need to validate any information
     */
    class Program
    {
        static void Main(string[] args)
        {
            //write description in the console
            Console.WriteLine("This is my Authenticator Console App data-tier server. It is currently running.");
            //the actual host service system
            ServiceHost host;
            //represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            //bind server to the implementation of AuthenticatorService
            host = new ServiceHost(typeof(AuthenticatorService));
            //present the publicly accessible interface to the client. It tells .net to accept on any interface, use port 8100 and service name of AuthenticationService.
            host.AddServiceEndpoint(typeof(AuthenticatorServiceInterface), tcp, "net.tcp://0.0.0.0:8100/AuthenticationService");
            //open the host
            host.Open();
            //write description in the console
            Console.WriteLine("System is currently online. To exit press space.");
            //read console line
            Console.ReadLine();
            //close the host
            host.Close();
        }
    }
}
