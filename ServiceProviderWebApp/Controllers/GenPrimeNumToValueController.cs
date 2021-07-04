using APIClasses;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace ServiceProviderWebApp.Controllers
{
    /**
     * GenPrimeNumToValueController is a ASP.NET Web API controller class
     * It has a Get method
     */
    public class GenPrimeNumToValueController : ApiController
    {
        /**
         * This rest service generates prime numbers from 1 to an input value and returns the output in JSON
         * 
         * Also, the registry expects a valid token with every service call. 
         * The registry will calls the validate function of the Authentication service and if validated the service is provided. 
         * Otherwise, the StatusAndReason JSON output is sent.
         */
        public List<int> Get(int numOne, int token)
        {
            //initialize list of int result to null
            List<int> result = null;
            //initialize dataModel of DataModel class
            Models.DataModel dataModel = new Models.DataModel();
            //creates a StatusAndReason object
            StatusAndReason statusAndReason = new StatusAndReason();

            //calls validate function of the Authentication service and if the datamodel is not valid, StatusAndReason JSON output is sent.
            if (dataModel.validate(token) == "not validated")
            {
                //set the status and reason as Denied and Authentication Error
                statusAndReason.Status = "Denied";
                statusAndReason.Reason = "Authentication Error";
                //sent status and reason json output
                throw new HttpResponseException(Request.CreateResponse(statusAndReason));
            }
            //if the datamodel is valid, it set result to the prime number from 1 to numOne
            else
            {
                //initialize the start number to 1
                int startNum = 1;
                //loop from 1 to less than or equal to numOne, and increment primeNum by 1 each time 
                for (int primeNum = startNum; primeNum <= numOne; primeNum++)
                {
                    //set numIndex to 0
                    int numIndex = 0;
                    //loop from 2 to less than or equal to primeNum divide by 2, and increment i by 1 each time
                    for (int i = 2; i <= primeNum / 2; i++)
                    {
                        //if primeNum % i is equal to 0, increment numIndex by 1
                        if (primeNum % i == 0)
                        {
                            //increment numIndex by 1 and break
                            numIndex++;
                            break;
                        }
                    }
                    //if numIndex is equal to 0 and primeNum is not 1, add primeNum to the list of result
                    if (numIndex == 0 && primeNum != 1)
                    {
                        //add primeNum to the list of result
                        result.Add(primeNum);
                    }
                }
            }
            //return the result
            return result;
        }
    }
}