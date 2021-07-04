using System;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using APIClasses;
using Newtonsoft.Json;

namespace RegistryWebApp.Controllers
{
    /**
     * PublishController is a ASP.NET Web API controller class
     * It has a Post method
     */
    public class PublishController : ApiController
    {
        /**
         * This rest service saves the service description in a local text file. 
         * If successful it returns the status accordingly in JSON. 
         * This service expects the input in a JSON format.
         * 
         * Also, the registry expects a valid token with every service call. 
         * The registry will calls the validate function of the Authentication service and if validated the service is provided. 
         * Otherwise, the StatusAndReason JSON output is sent.
         */
        public DataIntermed Post(int token, [FromBody] DataIntermed dataIntermed)
        {
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
            //if the datamodel is valid, it saves the service description in a local text file.  
            else
            {
                //set docPath as the Documents path 
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //set txtFileName as the local text file path for the saved published service description
                string txtFileName = "Publish-RegistryProject-DCAssignment1.txt";
                //set the pathFileName as the Document and local text file path for rhe saved published service description
                string pathFileName = Path.Combine(docPath, txtFileName);

                //write to file using StreamWriter
                using (StreamWriter outputFile = new StreamWriter(pathFileName, true))
                {
                    //initialize a JsonSerializer object
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize DataIntermed object directly into file stream
                    serializer.Serialize(outputFile, dataIntermed);
                    //write a line in the text file
                    outputFile.WriteLine();
                }
            }
            //returns the dataIntermed
            return dataIntermed;
        }
    }
}