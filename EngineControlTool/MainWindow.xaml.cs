/// 
/// Engine Control Tool
///     Receives data from Arduino and displays it in GUI
///     Records measurements and logs in an Excel spreadsheet when user clicks "Excel" button
/// 
/// MainWindow.xaml.cs
///     Class that controls program
///     Sets colors on GUI
///     Receives data from serial port and passes data between View Model
///     Checks for dangerous temperature or pressure values
///     Initializes Excel creation 
/// 
/// Written by Mary Lichtenwalner
/// 
/// Last Update: April 4, 2022
///

using System;
using System.IO.Ports;
using System.Threading;
using System.Windows;


namespace EngineControlTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        // Initialize possible ports
        private SerialPort port;
        private SerialPort port1 = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        private SerialPort port2 = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
        private SerialPort port3 = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);

        // Initialize view model and Excel classes
        public ViewModel viewModel;
        public CreateExcel createExcel;

        // Initialize thread to check for dangerous temperature/pressure values
        private Thread warningCheck;

        // Variables to save current sensor readings
        double tempCompressed;
        double tempChamber;
        double tempExhaust;
        double pressureAmbient;
        double pressureCompressed;
        double tempAmbient;
        double humidity;
        double shaftSpeed;

        //*****************************************************************************************
        // Method 1. Construct MainWindow
        //*****************************************************************************************
        public MainWindow()
        {
            InitializeComponent();

            // Set viewModel and createExcel to an instance of respective objects
            viewModel = this.FindResource("viewModel") as ViewModel;
            createExcel = new CreateExcel(viewModel);

            // Start the thread to check for dangerous pressures/temperatures
            warningCheck = new Thread(new ThreadStart(WarningChecker));
            warningCheck.IsBackground = true;
            warningCheck.Start();
        }

        //*****************************************************************************************
        // Methods 2-11. Methods to change colors in GUI. Dark or light mode, accent color
        //*****************************************************************************************
        // Set correct variable in viewModel to corresponding color based on user selection
        //*****************************************************************************************
        private void DarkItem_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.Color_Theme = 0;
        }

        private void LightItem_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.Color_Theme = 1;
        }

        private void RedItem_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.Accent_Color = 0;
            viewModel.Accent_Color_String = "Red";
        }

        private void OrangeItem_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.Accent_Color = 1;
            viewModel.Accent_Color_String = "Orange";
        }

        private void YellowItem_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.Accent_Color = 2;
            viewModel.Accent_Color_String = "Yellow";
        }

        private void GreenItem_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.Accent_Color = 3;
            viewModel.Accent_Color_String = "Green";
        }

        private void BlueItem_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.Accent_Color = 4;
            viewModel.Accent_Color_String = "#36f5ff";
        }

        private void PurpleItem_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.Accent_Color = 5;
            viewModel.Accent_Color_String = "Purple";
        }

        private void PinkItem_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.Accent_Color = 6;
            viewModel.Accent_Color_String = "#ff00e6";
        }

        private void GoldItem_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.Accent_Color = 7;
            viewModel.Accent_Color_String = "#bfa900";
        }

        //*****************************************************************************************
        // Method 12. Method runs when window loads. Find the correct port and begin reading data
        //*****************************************************************************************
        [STAThread]
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Try reading from serial port "COM3"
            try
            {
                port1.Open();
                port = port1;
            }
            catch
            {
                // Try reading from serial port "COM4"
                try
                {
                    port2.Open();
                    port = port2;
                }
                catch
                {
                    // Try reading from serial port "COM5"
                    try 
                    { 
                        port3.Open();
                        port = port3;
                    }
                    // If no port successfully connects, just set port to port1 to avoid crashing
                    catch
                    {
                        port = port1;
                    }
                }
            }

            // Calls method to receive serial port data
            SerialPortProgram();
        }

        //*****************************************************************************************
        // Method 13. Serial Port Program reads incoming data from serial port
        //*****************************************************************************************
        private void SerialPortProgram()
        {
            Console.WriteLine("Incoming Data:");

            // Receiving data triggers method "port_DataReceived"
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            try
            {
                port.Open();
            }
            catch
            {
                // If port does not receive data, do nothing
            }
            Console.ReadLine();
        }

        //*****************************************************************************************
        // Method 14. Uses the data received from the serial port
        //*****************************************************************************************
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Reads the string from the Arduino and splits the values at the spaces
            string measurements = port.ReadLine();
            string[] meas = measurements.Split(' ');

            // Grab a current time stamp
            DateTime now = DateTime.Now;

            // Convert all the measurements from strings to doubles
            // If not possible (ex: thermocouple had error so reading was NAN), just set value to 0
            try
            {
                tempCompressed = Convert.ToDouble(meas[0]);
            }
            catch
            {
                tempCompressed = 0;
            }
            try
            {
                tempChamber = Convert.ToDouble(meas[1]);
            }
            catch
            {
                tempChamber = 0;
            }
            try
            {
                tempExhaust = Convert.ToDouble(meas[2]);
            }
            catch
            {
                tempExhaust = 0;
            }
            try
            {
                pressureAmbient = Convert.ToDouble(meas[3]);
            }
            catch
            {
                pressureAmbient = 0;
            }
            try
            {
                pressureCompressed = Convert.ToDouble(meas[4]);
            }
            catch
            {
                pressureCompressed = 0;
            }
            try
            {
                tempAmbient = 1.8 * Convert.ToDouble(meas[5]) + 32;
            }
            catch
            {
                tempAmbient = 0;
            }
            try
            {
                humidity = Convert.ToDouble(meas[6]);
            }
            catch
            {
                humidity = 0;
            }
            try
            {
                shaftSpeed = Convert.ToDouble(meas[7]);
            }
            catch
            {
                shaftSpeed = 0;
            }

            // Take all the measurements and place in correct spot in viewModel
            // These values are bound to correct label/gauge in GUI
            viewModel.tempCompressed = tempCompressed;
            viewModel.tempChamber = tempChamber;
            viewModel.tempExhaust = tempExhaust;
            viewModel.pressureAmbient = pressureAmbient;
            viewModel.pressureCompressed = pressureCompressed;
            viewModel.tempAmbient = tempAmbient;
            viewModel.humidity = humidity;
            viewModel.shaftSpeed = shaftSpeed;

            // Add the current measurement to the correct list in viewModel
            // These lists will be used in the Excel spreadsheet later
            viewModel.timeList.Add(Convert.ToString(now));
            viewModel.tCompressedList.Add(tempCompressed);
            viewModel.tChamberList.Add(tempChamber);
            viewModel.tExhaustList.Add(tempExhaust);
            viewModel.pAmbientList.Add(pressureAmbient);
            viewModel.pCompressedList.Add(pressureCompressed);
            viewModel.tAmbientList.Add(tempAmbient);
            viewModel.humidityList.Add(humidity);
            viewModel.shaftSpeedList.Add(shaftSpeed);

            // Check to see if the log button was clicked
            // If so, log the inputted message or just mark the time if no message inputted
            if (viewModel.Log)
            {
                // Add the log message to the list in the viewModel
                viewModel.logList.Add("Log: " + viewModel.logNote);

                // Reset the log checker to false
                viewModel.Log = false;

                // Clear the log message
                viewModel.logNote = "";
            }

            // If log button was not clicked, just add a blank to the log list
            else
            {
                viewModel.logList.Add("");
            }
        }

        //*****************************************************************************************
        // Method 15. Run when user clicks "Excel" button
        //*****************************************************************************************
        private void Excel_Button_Click(object sender, RoutedEventArgs e)
        {
            // Call method to initialize the Excel application
            createExcel.startUpExcel();

            // Create the first spreadsheet with recorded values taken from the viewModel
            createExcel.generateExcel(viewModel.timeList, viewModel.tCompressedList, viewModel.tChamberList, viewModel.tExhaustList,
                viewModel.pAmbientList, viewModel.pCompressedList, viewModel.tAmbientList, viewModel.humidityList, viewModel.shaftSpeedList,
                viewModel.logList);

            // Create a second spreadsheet that has data in metric units (Celsius and pascals)
            createExcel.generateMetricExcel(viewModel.timeList, viewModel.tCompressedList, viewModel.tChamberList, viewModel.tExhaustList,
                viewModel.pAmbientList, viewModel.pCompressedList, viewModel.tAmbientList, viewModel.humidityList, viewModel.shaftSpeedList,
                viewModel.logList);
        }

        //*****************************************************************************************
        // Method 16. Run if user changes flow rate value
        //*****************************************************************************************
        private void FlowRate_Box_TextChanged(object sender, RoutedEventArgs e)
        {
            // Try to convert the user input to a double
            try
            {
                viewModel.flowRate = Convert.ToDouble(FlowRate_Box.Text);
            }

            // If unable to convert to double, set text box input to 0
            catch
            {

                viewModel.flowRate = 0;
                FlowRate_Box.Text = "0";
            }
        }

        //*****************************************************************************************
        // Method 17. Checks for dangerous temperature or pressure values
        //*****************************************************************************************
        private void WarningChecker()
        {
            // Limits on temperature and pressure (degrees Fahrenheit and psi)
            double tempLimit = 3000;
            double pressureLimit = 5000;

            // Constantly checking
            while (true)
            {
                // If any measured value is above limit, set viewModel.isDangerous to true
                if (viewModel.tempAmbient > tempLimit || viewModel.tempCompressed > tempLimit || 
                    viewModel.tempChamber > tempLimit || viewModel.tempExhaust > tempLimit ||
                    viewModel.pressureAmbient > pressureLimit || viewModel.pressureCompressed > pressureLimit)
                {
                    viewModel.isDangerous = true;
                }

                // If everything is within safe limits, set the boolean to false
                else
                {
                    viewModel.isDangerous = false;
                }

                // If the boolen is set to true, flash the warning label so that the user can stop the propane flow
                if (viewModel.isDangerous)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        viewModel.showWarning = 1;
                        Thread.Sleep(1000);
                        viewModel.showWarning = 0;
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        //*****************************************************************************************
        // Method 18. Runs when user clicks the log button
        //*****************************************************************************************
        private void Log_Button_Click(object sender, RoutedEventArgs e)
        {
            // Sets the log value in viewModel to true so that a marker is placed in the Excel
            // Message is directly bound from text box to viewModel
            viewModel.Log = true;
        }
    }
}
