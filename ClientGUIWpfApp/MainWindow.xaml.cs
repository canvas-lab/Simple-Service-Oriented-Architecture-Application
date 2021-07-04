using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using APIClasses;
using InterfaceLib;
using RestSharp;

namespace ClientGUIWpfApp
{
    /**
     *  MainWindow is a public partial class and is a user interface
     *  It is an Interaction logic for MainWindow.xaml
     */
    public partial class MainWindow : Window
    {
        //private fields
        public AuthenticatorServiceInterface authServer;
        public int token;

        //the MainWindow
        public MainWindow()
        {
            //initialize the component
            InitializeComponent();

            //hide all the inputBoxes of the test service part
            Input1Box.Visibility = Visibility.Hidden;
            Input2Box.Visibility = Visibility.Hidden;
            Input3Box.Visibility = Visibility.Hidden;

            //represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection
            string URL = "net.tcp://localhost:8100/AuthenticationService";
            //This is a factory that generates remote connections to our remote class which hides the RPC stuff
            ChannelFactory<AuthenticatorServiceInterface> authServerFactory = new ChannelFactory<AuthenticatorServiceInterface>(tcp, URL);//ConsoleApp1 namespace == the server prog
            //create the channel
            authServer = authServerFactory.CreateChannel();
        }

        /**
         * the app asks for the username and password in the GUI 
         * and sends them to an appropriate Authentication service
         */
        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = UsernameBox.Text;
            string password = PasswordBox.Text;

            //if both username and password is not empty, do register
            if (string.IsNullOrWhiteSpace(name) == false && string.IsNullOrWhiteSpace(password) == false)
            {
                try
                {
                    //calls the Register method from Authenticator console service  
                    string res = authServer.Register(UsernameBox.Text, PasswordBox.Text);

                    //set the result to the ResultLabel's content
                    ResultLabel.Content = res;
                }
                //catch the EndpointNotFoundException, if no end point, the server, is found
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show("No end point is found.");
                }
                //catch the other exception
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                //show message to user
                MessageBox.Show("Error: username or password is empty. Please enter a value for both the username and password.");
            }
        }

        /**
         * the app asks for the username and password in the GUI and sends them to an appropriate Authentication service to verify.
         * If successful, the returned token is saved in its program memory. 
         * This token will be sent as an additional parameter for every subsequent service call.
         */
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = UsernameBox.Text;
            string password = PasswordBox.Text;

            //if both username and password is not empty, do register
            if (string.IsNullOrWhiteSpace(name) == false && string.IsNullOrWhiteSpace(password) == false)
            {
                try
                {
                    //calls the Login method from Authenticator console service and save the token
                    token = authServer.Login(UsernameBox.Text, PasswordBox.Text);

                    //if token is equal to 0, write description in the console that login is unsuccesful
                    if (token == 0)
                    {
                        //set the result to the ResultLabel's content
                        ResultLabel.Content = "Unsuccessful login";
                    }
                    //if token is not equal to 0, write description in the console that login is succesful
                    else
                    {
                        //set the result to the ResultLabel's content
                        ResultLabel.Content = "Successfully login";
                    }
                }
                //catch the EndpointNotFoundException, if no end point, the server, is found
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show("No end point is found.");
                }
                //catch the other exception
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                //show message to user
                MessageBox.Show("Error: username or password is empty. Please enter a value for both the username and password.");
            }
        }

        /**
         * The GUI will call the appropriate Registry service to retrieve all the available services. 
         * The list of available services will be displayed in a manner so that they will be selectable.
         */
        private void ShowAllAvailableServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            //set the base URl
            string URL = "https://localhost:44347/";
            //use RestClient and set the URL
            RestClient client = new RestClient(URL);
            //set up and call the API method
            RestRequest request = new RestRequest("api/allservices/?token=" + token);
            //use IRestResponse and set the request in the client get method
            IRestResponse<List<DataIntermed>> resp = client.Get<List<DataIntermed>>(request);
            //get the list of the dataIntermed
            List<DataIntermed> dataIntermedList = resp.Data;
            //set dataIntermedList to the ServiceListBox's items 
            ServiceListBox.ItemsSource = dataIntermedList;

            //if response is unsuccesful, tell it to user
            if (!resp.IsSuccessful)
            {
                //input is not a number, show message to user
                MessageBox.Show("Error: the operation is unsuccesful. Please try again.");
            }
        }

        /**
         * the app asks for the service description in the GUI and sends them to an appropriate Registry service. 
         * The list of search results will be displayed in a manner so that they will be selectable
         */
        private void SearchAServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            //get the user input from the SearchBox's text 
            string descp = SearchBox.Text;
            //if both username and password is not empty, do register
            if(string.IsNullOrWhiteSpace(descp) == true)
            {
                //show message to user
                MessageBox.Show("Error: search box is empty. Please enter a value for search box.");
            }
            else
            {
                //set the base URl
                string URL = "https://localhost:44347/";
                //use RestClient and set the URL
                RestClient client = new RestClient(URL);
                //set up and call the API method
                RestRequest request = new RestRequest("api/search/?id=" + descp + "&token=" + token);
                //use IRestResponse and set the request in the client get method
                IRestResponse<List<DataIntermed>> resp = client.Get<List<DataIntermed>>(request);
                //get the list of the dataIntermed
                List<DataIntermed> dataIntermedList = resp.Data;
                //set dataIntermedList to the ServiceListBox's items 
                ServiceListBox.ItemsSource = dataIntermedList;

                //if response is unsuccesful, tell it to user
                if (!resp.IsSuccessful)
                {
                    //input is not a number, show message to user
                    MessageBox.Show("Error: the operation is unsuccesful. Please try again.");
                }
            }
        }

        /**
         * The user will select a service graphically. 
         * Assume the user selects the ADDTwoNumbers service:
         * The GUI app knows the API endpoint and number of operands from its search results. 
         * Next, it will create the input boxes for the service testing automatically in the GUI. 
         * As ADDTwoNumbers needs two operands two input boxes will be shown for the input and a ‘test’ button. 
         * When the button is pressed the GUI will call the service using the API endpoint and display the result
         */
        private void TestingAServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            //if at least a ServiceListBox is selected, do test service
            if (ServiceListBox.SelectedItem != null)
            {
                //get the numOfOperands from the selected DataIntermed object
                int numOfOperands = (ServiceListBox.SelectedItem as DataIntermed).numOfOperands;
                //get the apiEndpoint from the selected DataIntermed object
                string apiEndpoint = (ServiceListBox.SelectedItem as DataIntermed).apiEndpoint;

                //initialize numOne, numTwo snd numThree
                string numOne = "";
                string numTwo = "";
                string numThree = "";

                //initialize client
                RestClient client = null;
                try
                {
                    //use RestClient and set the URL
                    client = new RestClient(apiEndpoint);
                }
                //catch argument null exception
                catch(ArgumentNullException)
                {
                    //show message to user
                    MessageBox.Show("Status: Denied. Reason: Unauthorized Access.");
                }

                //initialize RestRequest
                RestRequest request = null;

                //if numOfOperands is equal to 1, unhide only Input1Box and set up and call the API method
                if (numOfOperands == 1)
                {
                    //unhide only Input1Box
                    Input1Box.Visibility = Visibility.Visible;
                    Input2Box.Visibility = Visibility.Hidden;
                    Input3Box.Visibility = Visibility.Hidden;

                    //check if input is an integer
                    if (int.TryParse(Input1Box.Text, out var x))
                    {
                        //textBox value is a number, get the user input by the Input1Box text 
                        numOne = Input1Box.Text;
                    }
                    else
                    {
                        //input is not a number, show message to user
                        MessageBox.Show("Error: Please input the correct value for Input1Box. It is an integer");
                    }

                    //set up and call the API method
                    request = new RestRequest(apiEndpoint + "?numOne=" + numOne + "&token=" + token);
                     
                }
                //if numOfOperands is equal to 2, unhide only Input1Box and Input2Box and set up and call the API method
                else if (numOfOperands == 2)
                {
                    //unhide only Input1Box and Input2Box
                    Input1Box.Visibility = Visibility.Visible;
                    Input2Box.Visibility = Visibility.Visible;
                    Input3Box.Visibility = Visibility.Hidden;

                    //check if input is an integer
                    if (int.TryParse(Input1Box.Text, out var x))
                    {
                        //textBox value is a number, get the user input by the Input1Box text 
                        numOne = Input1Box.Text;
                    }
                    else
                    {
                        //input is not a number, show message to user
                        MessageBox.Show("Error: Please input the correct value for Input1Box. It is an integer");
                    }

                    //check if input is an integer
                    if (int.TryParse(Input2Box.Text, out var y))
                    {
                        //textBox value is a number, get the user input by the Input1Box text 
                        numTwo = Input2Box.Text;
                    }
                    else
                    {
                        //input is not a number, show message to user
                        MessageBox.Show("Error: Please input the correct value for Input2Box. It is an integer");
                    }

                    //set up and call the API method
                    request = new RestRequest(apiEndpoint + "?numOne=" + numOne + "&numTwo=" + numTwo + "&token=" + token);
                }
                //if numOfOperands is equal to 3, unhide all Input Boxes and set up and call the API method
                else if (numOfOperands == 3)
                {
                    //unhide all Input Boxes
                    Input1Box.Visibility = Visibility.Visible;
                    Input2Box.Visibility = Visibility.Visible;
                    Input3Box.Visibility = Visibility.Visible;

                    //check if input is an integer
                    if (int.TryParse(Input1Box.Text, out var x))
                    {
                        //textBox value is a number, get the user input by the Input1Box text 
                        numOne = Input1Box.Text;
                    }
                    else
                    {
                        //input is not a number, show message to user
                        MessageBox.Show("Error: Please input the correct value for Input1Box. It is an integer");
                    }

                    //check if input is an integer
                    if (int.TryParse(Input2Box.Text, out var y))
                    {
                        //textBox value is a number, get the user input by the Input1Box text 
                        numTwo = Input2Box.Text;
                    }
                    else
                    {
                        //input is not a number, show message to user
                        MessageBox.Show("Error: Please input the correct value for Input2Box. It is an integer");
                    }

                    //check if input is an integer
                    if (int.TryParse(Input3Box.Text, out var z))
                    {
                        //textBox value is a number, get the user input by the Input1Box text 
                        numThree = Input3Box.Text;
                    }
                    else
                    {
                        //input is not a number, show message to user
                        MessageBox.Show("Error: Please input the correct value for Input3Box. It is an integer");
                    }

                    //set up and call the API method
                    request = new RestRequest(apiEndpoint + "?numOne=" + numOne + "&numTwo=" + numTwo + "&numThree=" + numThree + "&token=" + token);
                }

                try
                {
                    //use IRestResponse and set the request in the client get method
                    IRestResponse resp = client.Get(request);
                    //if response is unsuccesful, tell it to user
                    if (!resp.IsSuccessful)
                    {
                        //set resp's content as the ResLabel's content
                        ResLabel.Content = "Error: the operation is unsuccesful. Please try again.";
                    }
                    else
                    {
                        //set resp's content as the ResLabel's content
                        ResLabel.Content = resp.Content;
                    }
                }
                //catch argument null exception
                catch (ArgumentNullException)
                {
                    //show message to user
                    MessageBox.Show("Status: Denied. Reason: Unauthorized Access.");
                }
            }
        }

        private void theListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            //if at least a ServiceListBox is selected, do test service
            if (ServiceListBox.SelectedItem != null)
            {
                //get the numOfOperands from the selected DataIntermed object
                int numOfOperands = (ServiceListBox.SelectedItem as DataIntermed).numOfOperands;
                //set all input boxes text to none
                Input1Box.Text = "";
                Input2Box.Text = "";
                Input3Box.Text = "";

                //if numOfOperands is equal to 1, unhide only Input1Box and set up and call the API method
                if (numOfOperands == 1)
                {
                    //unhide only Input1Box
                    Input1Box.Visibility = Visibility.Visible;
                    Input2Box.Visibility = Visibility.Hidden;
                    Input3Box.Visibility = Visibility.Hidden;
                }
                //if numOfOperands is equal to 2, unhide only Input1Box and Input2Box and set up and call the API method
                else if (numOfOperands == 2)
                {
                    //unhide only Input1Box and Input2Box
                    Input1Box.Visibility = Visibility.Visible;
                    Input2Box.Visibility = Visibility.Visible;
                    Input3Box.Visibility = Visibility.Hidden;
                }
                //if numOfOperands is equal to 3, unhide all Input Boxes and set up and call the API method
                else if (numOfOperands == 3)
                {
                    //unhide all Input Boxes
                    Input1Box.Visibility = Visibility.Visible;
                    Input2Box.Visibility = Visibility.Visible;
                    Input3Box.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
