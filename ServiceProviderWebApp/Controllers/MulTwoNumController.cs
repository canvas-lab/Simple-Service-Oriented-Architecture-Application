using APIClasses;
using System.Net.Http;
using System.Web.Http;

namespace ServiceProviderWebApp.Controllers
{
    /**
     * MulTwoNumController is a ASP.NET Web API controller class
     * It has a Get method
     */
    public class MulTwoNumController : ApiController
    {
        /**
         * This rest service multiplies two input integers and returns the output in JSON
         * 
         * Also, the registry expects a valid token with every service call. 
         * The registry will calls the validate function of the Authentication service and if validated the service is provided. 
         * Otherwise, the StatusAndReason JSON output is sent.
         */
        public int Get(int numOne, int numTwo, int token)
        {
            //initialize result to 0
            int result = 0;
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
            //if the datamodel is valid, it set result to the multiplication of numOne and numTwo
            else
            {
                //multiply numOne and numTwo
                result = numOne * numTwo;
            }
            //return the result
            return result;
        }
    }
}