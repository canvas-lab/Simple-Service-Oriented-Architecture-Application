using APIClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace RegistryWebApp.Controllers
{
    /**
     * UnpublishController is a ASP.NET Web API controller class
     * It has a Get method
     */
    public class UnpublishController : ApiController
    {
        /**
         * Given a service endpoint, this rest service will remove the service description from the local text file
         * 
         * Also, the registry expects a valid token with every service call. 
         * The registry will calls the validate function of the Authentication service and if validated the service is provided. 
         * Otherwise, the StatusAndReason JSON output is sent.
         */
        public string Get(string serviceEndpoint, int token)
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

                //read the text file and saves the list of DataIntermed object
                List<DataIntermed> dataIntermedList = File.ReadAllLines(pathFileName)
                .Select(line => JsonConvert.DeserializeObject<DataIntermed>(line))
                .ToList();

                //delete old file
                File.Delete(pathFileName);

                //write to file using Stream Writer
                using (StreamWriter outputFile = new StreamWriter(pathFileName, true))
                {
                    //loop for each data in dataIntermedList
                    foreach (DataIntermed data in dataIntermedList)
                    {
                        //if apiEndpoint in data contains the serviceEndpoint, write to file
                        if (!data.apiEndpoint.Contains(serviceEndpoint))
                        {
                            //initialize a JsonSerializer object
                            JsonSerializer serializer = new JsonSerializer();
                            //serialize DataIntermed object directly into file stream
                            serializer.Serialize(outputFile, data);
                            //write a line in the text file
                            outputFile.WriteLine();
                        }
                    }
                }
            }
            //returns res
            return serviceEndpoint;
        }

    }
}