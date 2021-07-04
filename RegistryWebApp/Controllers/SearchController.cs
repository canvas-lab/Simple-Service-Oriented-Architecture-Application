using APIClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RegistryWebApp.Controllers
{
    /**
     * SearchController is a ASP.NET Web API controller class
     * It has a Get method
     */
    public class SearchController : ApiController
    {
        /**
         * This rest service searches an input service description in a local text file.
         * It returns the service information.
         * 
         * Also, the registry expects a valid token with every service call. 
         * The registry will calls the validate function of the Authentication service and if validated the service is provided. 
         * Otherwise, the StatusAndReason JSON output is sent.
         */
        public List<DataIntermed> Get(string id, int token)
        {
            List<DataIntermed> dataIntermedList = new List<DataIntermed>();
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
                List<DataIntermed> dataIntermedListTemp = File.ReadAllLines(pathFileName)
                .Select(line => JsonConvert.DeserializeObject<DataIntermed>(line))
                .ToList();

                //loop for each data in dataIntermedList
                foreach (DataIntermed data in dataIntermedListTemp)
                {
                    //if description in data contains the id, add data to the list
                    if (data.description.Contains(id))
                    {
                        //add data to the list
                        dataIntermedList.Add(data);
                    }
                }
            }
            //return the dataIntermedList
            return dataIntermedList;
            }

    }
}