using System.ServiceModel;
using InterfaceLib;
using RestSharp;
using APIClasses;

namespace ServicePublishingConsoleApp
{
    /**
     *  ServicePublishing is a C# console application to publish services
     *  It have 4 operations; Registration, Log in, Publish service and Unpublish service
     */
    //defining the behaviours of a service yb ServiceBehavior, makes the service multi-threaded by ConcurrencyMode and allow management of the thread synchronisation
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class ServicePublishing : ServicePublishingInterface
    {
        //public fields
        public AuthenticatorServiceInterface authServer;
        public int token;

        //public constructor
        public ServicePublishing()
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
         * Registration method returns a string.
         * the app asks for the username and password in the console and sends them to an appropriate Authentication service
         */
        public string Registration(string name, string password)
        {
            //returns the Register method from Authenticator console service  
            string str = authServer.Register(name, password);
            return str;
        }

        /**
         * Login method returns an int.
         *  the app asks for the username and password in the console and sends them to an appropriate Authentication service to verify. 
         *  If successful, the returned token is saved in its program memory. 
         *  This token will be sent as an additional parameter for every subsequent service cal
         */
        public int LogIn(string name, string password)
        {
            //returns the Login method from Authenticator console service and save the token
            token = authServer.Login(name, password);
            return token;
        }

        /**
         * the app asks for the service name, description, API endpoint, number of operands and operand types in the console 
         * and sends them to an appropriate Registry service to publish.
         */
        public IRestResponse PublishService(string name, string description, string apiEndpoint, int numOfOperands, string operandType)
        {
            //set the base URl
            string URL = "https://localhost:44347/";
            //use RestClient and set the URL
            RestClient client = new RestClient(URL);

            //creates a new DataIntermed object and set its fields
            DataIntermed dataIntermed = new DataIntermed();
            dataIntermed.name = name;
            dataIntermed.description = description;
            dataIntermed.apiEndpoint = apiEndpoint;
            dataIntermed.numOfOperands = numOfOperands;
            dataIntermed.operandType = operandType;

            //set up and call the API method...
            RestRequest request = new RestRequest("api/publish/?token=" + token + "&");
            //Build a request with the json in the body
            request.AddJsonBody(dataIntermed);
            //use IRestResponse and set the request in the client post method
            IRestResponse resp = client.Post(request);
            //return the resp
            return resp;
        }

        /**
         *  the app asks for API endpoint in the console 
         *  and sends them to an appropriate Registry service to unpublish
         */
        public IRestResponse UnPublishService(string serviceEndpoint)
        {
            //set the base URl
            string URL = "https://localhost:44347/";
            //use RestClient and set the URL
            RestClient client = new RestClient(URL);
            //set up and call the API method
            RestRequest request = new RestRequest("api/unpublish/?serviceEndpoint=" + serviceEndpoint + "&token=" + token);
            //use IRestResponse and set the request in the client get method
            IRestResponse resp = client.Get(request);
            //return the resp
            return resp;
        }

        
    }
}
