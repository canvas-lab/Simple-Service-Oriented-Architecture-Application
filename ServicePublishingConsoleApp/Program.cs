using System;
using System.ServiceModel;
using InterfaceLib;
using RestSharp;

namespace ServicePublishingConsoleApp 
{
    /**
     *  This is a C# console application to publish services. It is a user interface
     *  It have 4 operations; Registration, Log in, Publish service and Unpublish service
     */
    class Program
    {
        static void Main(string[] args)
        {
            //write description in the console
            Console.WriteLine("This is my console application to publish services. It is currently running.");
            //This is the actual host service system
            ServiceHost host;
            //This represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            //Bind server to the implementation of ServicePublishing
            host = new ServiceHost(typeof(ServicePublishing));
            //present the publicly accessible interface to the client. It tells .net to accept on any interface, use port 8200 and service name of ServicePublishing.
            host.AddServiceEndpoint(typeof(ServicePublishingInterface), tcp, "net.tcp://0.0.0.0:8200/ServicePublishing");
            //open the host
            host.Open();
            //write description in the console
            Console.WriteLine("System Online");

            //creates a new ServicePublishing object
            ServicePublishing servicePublishing = new ServicePublishing();
            //initialize token to 0
            int token = 0;
            //initialize isDone to false
            Boolean isDone = false;
            //while isDone is false, do loop until isDone is true
            while(isDone == false)
            {
                //call getNumChoice method and set it to numChosen
                string numChosen = getNumChoice();

                //if numChosen is equal to 1, do regristration operation
                if (numChosen == "1")//Registration
                {
                    //write description in the console
                    Console.WriteLine("Registration requires 2 user input(Username and Password)");
                    Console.WriteLine("Username:");
                    string username = Console.ReadLine();
                    //while the user input is empty, ask for the user input until it is correct
                    while (string.IsNullOrWhiteSpace(username) == true)
                    {
                        Console.WriteLine("No username has been inputted. Please input the username:");
                        //get the user input for serviceName
                        username = Console.ReadLine();
                    }

                    //write description in the console
                    Console.WriteLine("Password:");
                    string password = Console.ReadLine();
                    //while the user input is empty, ask for the user input until it is correct
                    while (string.IsNullOrWhiteSpace(password) == true)
                    {
                        Console.WriteLine("No password has been inputted. Please input the password:");
                        //get the user input for serviceName
                        password = Console.ReadLine();
                    }

                    try
                    {
                        //servicePublishing object calls Registration method 
                        string str = servicePublishing.Registration(username, password);
                        //write description in the console
                        Console.WriteLine(str);
                    }
                    //catch the EndpointNotFoundException, if no end point, the server, is found
                    catch (EndpointNotFoundException)
                    {
                        Console.WriteLine("No end point is found.");
                    }
                    catch (CommunicationObjectFaultedException)
                    {
                        Console.WriteLine("the communication object is faulted.");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }
                //if numChosen is equal to 2, do Log in operation
                else if (numChosen == "2")//Log in
                {
                    //write description in the console and get the user input of username and password
                    Console.WriteLine("Log in requires 2 user input(Username and Password)");
                    Console.WriteLine("Username:");
                    string username = Console.ReadLine();
                    //while the user input is empty, ask for the user input until it is correct
                    while (string.IsNullOrWhiteSpace(username) == true)
                    {
                        Console.WriteLine("No username has been inputted. Please input the username:");
                        //get the user input for serviceName
                        username = Console.ReadLine();
                    }

                    //write description in the console
                    Console.WriteLine("Password:");
                    string password = Console.ReadLine();
                    //while the user input is empty, ask for the user input until it is correct
                    while (string.IsNullOrWhiteSpace(password) == true)
                    {
                        Console.WriteLine("No password has been inputted. Please input the password:");
                        //get the user input for serviceName
                        password = Console.ReadLine();
                    }
                    try
                    { 
                        //servicePublishing object calls LogIn method 
                        token = servicePublishing.LogIn(username, password);

                        //if token is equal to 0, write description in the console that login is unsuccesful
                        if (token == 0)
                        {
                            //write description in the console that login is unsuccesful
                            Console.WriteLine("Unsuccessful login");
                        }
                        //if token is not equal to 0, write description in the console that login is succesful
                        else
                        {
                            //write description in the console that login is successful
                            Console.WriteLine("Successfuly login");
                            //write the token generated in the console
                            Console.WriteLine("random token " + token.ToString() + " is created");
                        }
                    }
                    //catch the EndpointNotFoundException, if no end point, the server, is found
                    catch (EndpointNotFoundException)
                    {
                        Console.WriteLine("No end point is found.");
                    }
                    catch(CommunicationObjectFaultedException)
                    {
                        Console.WriteLine("the communication object is faulted.");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }
                //if numChosen is equal to 3, do Publish service operation
                else if (numChosen == "3")//Publish service
                {
                    //write description in the console and get the 6 user input which are service name, description, API endpoint, number of operands, operand types
                    Console.WriteLine("Publish service requires 6 user input(Service name, description, API endpoint, number of operands, operand types)");
                    //write description in the console
                    Console.WriteLine("Service name:");
                    //get the user input for serviceName
                    string serviceName = Console.ReadLine();
                    //while the user input is empty, ask for the user input until it is correct
                    while (string.IsNullOrWhiteSpace(serviceName) == true)
                    { 
                        Console.WriteLine("No service name has been inputted. Please input the service name:");
                        //get the user input for serviceName
                        serviceName = Console.ReadLine();
                    }

                    //write description in the console
                    Console.WriteLine("Description:");
                    //get the user input for description
                    string description = Console.ReadLine();
                    //while the user input is empty, ask for the user input until it is correct
                    while (string.IsNullOrWhiteSpace(description) == true)
                    {
                        Console.WriteLine("No description has been inputted. Please input the description:");
                        //get the user input for description
                        description = Console.ReadLine();
                    }

                    //write description in the console
                    Console.WriteLine("API endpoint:");
                    //get the user input for apiEndpoint
                    string apiEndpoint = Console.ReadLine();
                    //initialize bool done to false
                    bool done = false;
                    //set done is true and exit the while loop if user input value is correct
                    if ((string.IsNullOrWhiteSpace(apiEndpoint) == false) && (apiEndpoint == "https://localhost:44349/api/addthreenum" || apiEndpoint == "https://localhost:44349/api/addtwonum"
                    || apiEndpoint == "https://localhost:44349/api/genprimenuminrange" || apiEndpoint == "https://localhost:44349/api/genprimenumtovalue"
                    || apiEndpoint == "https://localhost:44349/api/isprimenum" || apiEndpoint == "https://localhost:44349/api/multhreenum" || apiEndpoint == "https://localhost:44349/api/multwonum"))
                    {
                        //set done to true
                        done = true;
                    }
                    //while the user input is empty, ask for the user input until it is correct
                    while (done == false)
                    {
                        Console.WriteLine("Please input the correct API endpoint. E.g.\nhttps://localhost:44349/api/addthreenum\nhttps://localhost:44349/api/addtwonum\nhttps://localhost:44349/api/genprimenuminrange" +
                            "\nhttps://localhost:44349/api/genprimenumtovalue\nhttps://localhost:44349/api/isprimenum\nhttps://localhost:44349/api/multhreenum\nhttps://localhost:44349/api/multwonum");
                        Console.WriteLine("Please input the API endpoint:");
                        //get the user input for apiEndpoint
                        apiEndpoint = Console.ReadLine();

                        //set done is true and exit the while loop if user input value is correct
                        if((string.IsNullOrWhiteSpace(apiEndpoint) == false) && (apiEndpoint == "https://localhost:44349/api/addthreenum" || apiEndpoint == "https://localhost:44349/api/addtwonum"
                        || apiEndpoint == "https://localhost:44349/api/genprimenuminrange" || apiEndpoint == "https://localhost:44349/api/genprimenumtovalue"
                        || apiEndpoint == "https://localhost:44349/api/isprimenum" || apiEndpoint == "https://localhost:44349/api/multhreenum" || apiEndpoint == "https://localhost:44349/api/multwonum"))
                        {
                            //set done to true
                            done = true;
                        }
                    }

                    //write description in the console
                    Console.WriteLine("Number of Operand:");
                    //get the user input for numOfOperandsStr
                    string numOfOperandsStr = Console.ReadLine();
                    //initialize bool iDone to false
                    bool iDone = false;
                    if ((numOfOperandsStr == "1" || numOfOperandsStr == "2" || numOfOperandsStr == "3") && string.IsNullOrWhiteSpace(numOfOperandsStr) == false)
                    {
                        //set iDone to true
                        iDone = true;
                    }
                    //while the user input is not an integer or empty, ask for the user input until it is correct
                    while (iDone == false)
                    {
                        Console.WriteLine("Number of Operand should be an integer from 1 to 3 and not null. Please input the correct value for number of operand:");
                        //get the user input for numOfOperandsStr
                        numOfOperandsStr = Console.ReadLine();

                        //set done is true and exit the while loop if user input value is correct
                        if ((numOfOperandsStr == "1" || numOfOperandsStr == "2" || numOfOperandsStr == "3")  && string.IsNullOrWhiteSpace(numOfOperandsStr) == false)
                        {
                            //set iDone to true
                            iDone = true;
                        }
                    }
                    //convert string to integer of numOfOperands
                    int numOfOperands = Convert.ToInt32(numOfOperandsStr);

                    //write description in the console
                    Console.WriteLine("Operand Type:");
                    //get the user input for operandType
                    string operandType = Console.ReadLine();
                    //while the user input is empty, ask for the user input until it is correct
                    while (string.IsNullOrWhiteSpace(operandType) == true)
                    {
                        Console.WriteLine("No operand type has been inputted. Please input the operand type:");
                        //get the user input for operandType
                        operandType = Console.ReadLine();
                    }

                    //servicePublishing object calls PublishService method 
                    IRestResponse resp = servicePublishing.PublishService(serviceName, description, apiEndpoint, numOfOperands, operandType);
                    //if response is unsuccesful, tell it to user
                    if (!resp.IsSuccessful)
                    {
                        //show message to user
                        Console.WriteLine("Error: the operation is unsuccesful. Please try again.");
                    }
                    else
                    {
                        //show message to user
                        Console.WriteLine("succesfully published a service.");
                    }
                }
                //if numChosen is equal to 4, do Unpublish service operation
                else if (numChosen == "4")//Unpublish service
                {
                    //write description in the console
                    Console.WriteLine("UnPublish service requires 1 user input(API endpoint)");
                    Console.WriteLine("API endpoint:");
                    //get user input for the apiEndpoint
                    string apiEndpoint = Console.ReadLine();
                    //initialize bool done to false
                    bool done = false;
                    //while the user input is empty, ask for the user input until it is correct
                    while (done == false)
                    {
                        Console.WriteLine("Please input the correct API endpoint. E.g.\nhttps://localhost:44349/api/addthreenum\nhttps://localhost:44349/api/addtwonum\nhttps://localhost:44349/api/genprimenuminrange" +
                            "\nhttps://localhost:44349/api/genprimenumtovalue\nhttps://localhost:44349/api/isprimenum\nhttps://localhost:44349/api/multhreenum\nhttps://localhost:44349/api/multwonum");
                        Console.WriteLine("Please input the API endpoint:");
                        //get the user input for apiEndpoint
                        apiEndpoint = Console.ReadLine();

                        //set done is true and exit the while loop if user input value is correct
                        if ((string.IsNullOrWhiteSpace(apiEndpoint) == false) && (apiEndpoint == "https://localhost:44349/api/addthreenum" || apiEndpoint == "https://localhost:44349/api/addtwonum"
                        || apiEndpoint == "https://localhost:44349/api/genprimenuminrange" || apiEndpoint == "https://localhost:44349/api/genprimenumtovalue"
                        || apiEndpoint == "https://localhost:44349/api/isprimenum" || apiEndpoint == "https://localhost:44349/api/multhreenum" || apiEndpoint == "https://localhost:44349/api/multwonum"))
                        {
                            //set done to true
                            done = true;
                        }
                    }

                    //servicePublishing object calls UnPublishService method 
                    IRestResponse resp = servicePublishing.UnPublishService(apiEndpoint);
                    //if response is unsuccesful, tell it to user
                    if (!resp.IsSuccessful)
                    {
                        //show message to user
                        Console.WriteLine("Error: the operation is unsuccesful. Please try again.");
                    }
                    else
                    {
                        //show message to user
                        Console.WriteLine("succesfully unpublished a service.");
                    }
                }
                //if numChosen is equal to 1, do exit or logout
                else if (numChosen == "5")//Exit
                {
                    //set isDone to true. while loop is done
                    isDone = true;
                    //write description in the console
                    Console.WriteLine("Exit! Bye!");
                }  
            }
            //close host
            host.Close();
        }

        /**
         * getNumChoice is a static method that return a string of user input
         * It ask for user input between 1 to 5 and return it as a string
         */
        public static string getNumChoice()
        {
            //write description in the console for user input
            Console.WriteLine("Please choose a number between 1 to 5: ");
            Console.WriteLine("1. Registration\n2. Log in\n3. Publish Service\n4. Unpublish Service\n5. Exit");
            //get user input of numChosen
            string numChosen = Console.ReadLine();

            //while numChoosen is not 1, 2, 3, 4 or 5, do loop until it is one of those numbers
            while (!numChosen.Equals("1") && !numChosen.Equals("2") && !numChosen.Equals("3") && !numChosen.Equals("4") && !numChosen.Equals("5"))
            {
                //write description in the console for user input
                Console.WriteLine("Please only choose a number between 1 to 5!");
                //get user input of numChosen
                numChosen = Console.ReadLine();
            }

            //return the numChosen
            return numChosen;
        }
    }
}
