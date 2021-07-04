using APIClasses;
using System;
using System.Net.Http;
using System.Web.Http;

namespace ServiceProviderWebApp.Controllers
{
    /**
     * IsPrimeNumController is a ASP.NET Web API controller class
     * It has a Get method
     */
    public class IsPrimeNumController : ApiController
    {
        /**
         * This rest service checks whether a number is prime and returns the output accordingly in JSON
         * 
         * Also, the registry expects a valid token with every service call. 
         * The registry will calls the validate function of the Authentication service and if validated the service is provided. 
         * Otherwise, the StatusAndReason JSON output is sent.
         */
        public bool Get(int numOne, int token)
        {
            //initiliaze result to false
            bool result = false;
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
            //if the datamodel is valid, it set result to true if it is the prime number, otherwise set result to false
            else
            {
                //if numOne is less than or equal to 1, set result to false
                if (numOne <= 1)
                {
                    //set result to false
                    result = false;
                }
                //if numOne is equal to 2, set result to true
                else if (numOne == 2)
                {
                    //set result to true
                    result = true;
                }
                //if numOne % 2 is equal to 0, set result to false
                else if (numOne % 2 == 0)
                {
                    //set result to false
                    result = false;
                }
                //set the boundary using Math Floor squareroot of numOne
                var boundary = (int)Math.Floor(Math.Sqrt(numOne));
                //loop from 3 to less than or equal to the boundary, and increment i by 2 each time to get the prime number
                for (int i = 3; i <= boundary; i += 2)
                {
                    //if numOne % index is equal to 0, set result to false
                    if (numOne % i == 0)
                    {
                        //set result o false
                        result = false;
                    }
                    //if numOne % index is not equal to 0, set result to true
                    else
                    {
                        //set result to false
                        result = true;
                    }
                }
            }
            //return the result
            return result;
        }
    }
}