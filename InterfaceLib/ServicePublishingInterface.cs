using System.ServiceModel;
using APIClasses;
using RestSharp;

namespace InterfaceLib
{
    //makes this a service contract as it is a service interface
    [ServiceContract]
    /**
     * AuthenticatorServiceInterface is the public interface for the .NET server 
     * It is the .NET Remoting network interface of ServicePublishingConsole
     */
    public interface ServicePublishingInterface
    {
        //OperationContracts is tagged as it is a service function contracts
        [OperationContract]
        string Registration(string name, string password);

        //OperationContracts is tagged as it is a service function contracts
        [OperationContract]
        int LogIn(string name, string password);

        //OperationContracts is tagged as it is a service function contracts
        [OperationContract]
        IRestResponse PublishService(string name, string description, string apiEndpoint, int numOfOperands, string operandType);

        //OperationContracts is tagged as it is a service function contracts
        [OperationContract]
        IRestResponse UnPublishService(string apiEndpoint);
    }
}

