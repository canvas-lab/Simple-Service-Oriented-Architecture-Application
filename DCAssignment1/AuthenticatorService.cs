using System.ServiceModel;
using System;
using System.IO;
using InterfaceLib;
using System.Linq;

namespace AuthenticatorConsoleApp
{
    /**
     *  AuthenticatorService is an internal class that has three operations open as service functions and implements AuthenticatorServiceInterface
     *  It is the internal implementation of the .NET Remoting network interface
     *  AuthenticatorService has three operations open as service functions; Register, Login and validate.
     */
    //defining the behaviours of a service yb ServiceBehavior, makes the service multi-threaded by ConcurrencyMode and allow management of the thread synchronisation
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class AuthenticatorService : AuthenticatorServiceInterface 
    {
        //public constructor of AuthenticatorService
        public AuthenticatorService(){}

        /**
         * Register method returns a string.
         * It expects two operands, name and  password, from an actor.
         * It saves these values in a local text file. 
         * If successful it returns "successfully registered". 
         */
        public string Register(string name, string password)
        {
            //initialize result
            string result = "";
            //set docPath as the Documents path 
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //set txtFileName as the local text file path for registered user
            string txtFileName = "Register-DCAssignment1.txt";
            //set the pathFileName as the Document and local text file path for registered user
            string pathFileName = Path.Combine(docPath, txtFileName);
            //set the name and password to be the word to register
            string wordToRegister = name + "," + password;
            //write line in the console
            Console.WriteLine("Try to register...");

            //Append text to an existing file in Document path with the name "Register-DCAssignment1.txt"
            using (StreamWriter outputFile = new StreamWriter(pathFileName, true))
            {
                //write name and password to the local text file
                outputFile.WriteLine(wordToRegister);
                //set result to successfully registered
                result = "successfully registered";
                //write description in the console
                Console.WriteLine($"Write to file {pathFileName} with name '{name}' and password '{password}'");
            }
            //return the result
            return result;
        }

        /**
         * Login method returns an int.
         * It expects two operands, name and password from an actor. 
         * It checks these values in a local text file. If a match is found,  it creates a random integer token from 10-99.
         */
        public int Login(string name, string password)
        {
            //initialize token to 0
            int token = 0;
            //set docPath as the Documents path 
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //set txtFileName as the local text file path for registered user
            string txtFileName = "Register-DCAssignment1.txt";
            //set the pathFileName as the Document and local text file path for registered user
            string pathFileName = Path.Combine(docPath, txtFileName);
            //set the name and password to be the word to register
            string wordToRegister = name + "," + password;
            //set tokenTxtFileName as the local text file path for the token
            string tokenTxtFileName = "LoginToken-DCAssignment1.txt";
            //set the tokenPathFileName as the Document and local text file path for registered user
            string tokenPathFileName = Path.Combine(docPath, tokenTxtFileName);
            //write line in the console
            Console.WriteLine("Try to login...");

            //if the the register text file contains the name and password, it will create a token
            if (File.ReadAllLines(pathFileName).Contains(wordToRegister) == true)
            {
                //delete the token file. so new token will be created when user re-login.
                File.Delete(tokenPathFileName);

                //Append text to an existing file token path file 
                using (StreamWriter outputFile = new StreamWriter(tokenPathFileName, true))
                {
                    //use Random
                    Random rnd = new Random();
                    //creates a number between 10 and 99
                    token = rnd.Next(10, 99);
                    //write random token to the token file
                    outputFile.WriteLine(token.ToString());
                }
                //write description in the console
                Console.WriteLine($"Write to file {tokenPathFileName} with token {token}");
            }
            //if the the register text file does not contain the name and password, it send message to user
            else
            {
                //write line in the console
                Console.WriteLine($"Name and password has no match");
            }
            //return the token
            return token;
        }

        /**
         *  validate method returns a string.
         *  It expects a token and checks whether the token is already generated. 
         *  If the token could be validated, the return is “validated”, else “not validated”.
         * 
         */
        public string validate(int token)
        {
            //initialize result
            string result = "";
            //set docPath as the Documents path
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //set tokenTxtFileName as the local text file path for the token
            string tokenTxtFileName = "LoginToken-DCAssignment1.txt";
            //set the tokenTxtFileName as the Document and local text file path for the token
            string tokenPathFileName = Path.Combine(docPath, tokenTxtFileName);

            //if the token file contains the token generated or could be validated, it returns 'validated'
            if (File.ReadAllLines(tokenPathFileName).Contains(token.ToString()) == true)
            {
                //set result to 'validated'
                result = "validated";
            }
            //if the token file does not contains the token generated or could not be validated, it returns 'not validated'
            else
            {
                //set result to 'not validated'
                result = "not validated";
            }
            //return result
            return result;
        }
    }
}
