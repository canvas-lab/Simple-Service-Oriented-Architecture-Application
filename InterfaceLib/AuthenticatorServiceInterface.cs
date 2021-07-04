using System.ServiceModel;

namespace InterfaceLib
{
    //makes this a service contract as it is a service interface
    [ServiceContract]
    /**
     * AuthenticatorServiceInterface is the public interface for the .NET server 
     * It is the .NET Remoting network interface of AuthenticatorConsole
     */
    public interface AuthenticatorServiceInterface
    {
        //OperationContracts is tagged as it is a service function contracts
        [OperationContract]
        string Register(string name, string password);

        //OperationContracts is tagged as it is a service function contracts
        [OperationContract]
        int Login(string name, string password);

        //OperationContracts is tagged as it is a service function contracts
        [OperationContract]
        string validate(int token);
    }
}

