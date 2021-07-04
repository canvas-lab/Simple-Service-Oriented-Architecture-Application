using System.ServiceModel;
using InterfaceLib;

namespace ServiceProviderWebApp.Models
{
    /**
      * DataModel is a public class that connects to the Data Tier via .NET remoting
      * It allows for the usual services, such as validate
      */
    public class DataModel
    {
        //public field
        public AuthenticatorServiceInterface authServer;

        //public constructor
        public DataModel()
        {
            //represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection
            string URL = "net.tcp://localhost:8100/AuthenticationService";
            //This is a factory that generates remote connections to our remote class which hides the RPC stuff
            ChannelFactory<AuthenticatorServiceInterface> authServerFactory = new ChannelFactory<AuthenticatorServiceInterface>(tcp, URL);//ConsoleApp1 namespace == the server prog
            //create the channel
            authServer = authServerFactory.CreateChannel();
        }

        /**
        * validate method return a string
        * It has an int token parameter and returns the Authenticator Service validate method result
        */
        public string validate(int token)
        {
            //return the validate functions of the Authenticator Service 
            return authServer.validate(token);
        }

    }
}