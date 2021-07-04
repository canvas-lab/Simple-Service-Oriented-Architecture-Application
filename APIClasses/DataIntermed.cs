namespace APIClasses
{
    /**
     * DataIntermed represent the templates for the .NET JSON serializers.
     * It is going to be the go-betweens the web data.
     * DataIntermed will represent the format of the data to be passed through from the APIClasses Biz tier to the GUI.
     * DataIntermed defines the service description which contains a name, description, api endpoint, number of operands and operand type,
     */
    public class DataIntermed
    {
        public string name { get; set; }
        public string description { get; set; }
        public string apiEndpoint { get; set; }
        public int numOfOperands { get; set; }
        public string operandType { get; set; }
    }
}