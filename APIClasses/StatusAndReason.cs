namespace APIClasses
{
    /**
     * StatusAndReason represent the templates for the .NET JSON serializers.
     * It is going to be the go-betweens the web data.
     * StatusAndReason will represent the format of the data to be passed through from the APIClasses Biz tier to the GUI.
     * StatusAndReason defines and contains the status and reason which is used for the web app rest service
     */
    public class StatusAndReason
    {
        public string Status { get; set; }
        public string Reason { get; set; }
    }
}
