using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Excel;

namespace EngineControlTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private SerialPort port;
        private SerialPort port1 = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        private SerialPort port2 = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
        private SerialPort port3 = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);
        public ViewModel viewModel;
        public CreateExcel createExcel;

        private Thread warningCheck;

        double tempCompressed;
        double tempChamber;
        double tempExhaust;
        double pressureAmbient;
        double pressureCompressed;
        double tempAmbient;
        double humidity;
        double shaftSpeed;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = this.FindResource("viewModel") as ViewModel;
            createExcel = new CreateExcel(viewModel);

            // Start the thread to check for dangerous pressures/temperatures
            warningCheck = new Thread(new ThreadStart(WarningChecker));
            warningCheck.IsBackground = true;
            warningCheck.Start();
        }

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

        [STAThread]
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                port1.Open();
                port = port1;
            }
            catch
            {
                try
                {
                    port2.Open();
                    port = port2;
                }
                catch
                {
                    try 
                    { 
                        port3.Open();
                        port = port3;
                    }
                    catch
                    {
                        // Do Nothing
                    }
                }
            }

            SerialPortProgram();
        }

        private void SerialPortProgram()
        {
            Console.WriteLine("Incoming Data:");

            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            try
            {
                port.Open();
            }
            catch
            {
                // Do nothing
            }
            Console.ReadLine();
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string measurements = port.ReadLine();
            string[] meas = measurements.Split(' ');
            DateTime now = DateTime.Now;

            // Convert all the measurements to doubles
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
            

            //double conversion = 1023 / 210;
            //Console.WriteLine(temp);

            viewModel.tempCompressed = tempCompressed;
            viewModel.tempChamber = tempChamber;
            viewModel.tempExhaust = tempExhaust;
            viewModel.pressureAmbient = pressureAmbient;
            viewModel.pressureCompressed = pressureCompressed;
            viewModel.tempAmbient = tempAmbient;
            viewModel.humidity = humidity;
            viewModel.shaftSpeed = shaftSpeed;


            viewModel.timeList.Add(Convert.ToString(now));
            viewModel.tCompressedList.Add(tempCompressed);
            viewModel.tChamberList.Add(tempChamber);
            viewModel.tExhaustList.Add(tempExhaust);
            viewModel.pAmbientList.Add(pressureAmbient);
            viewModel.pCompressedList.Add(pressureCompressed);
            viewModel.tAmbientList.Add(tempAmbient);
            viewModel.humidityList.Add(humidity);
            viewModel.shaftSpeedList.Add(shaftSpeed);

            // Decide if a log message should be added
            if (viewModel.Log)
            {
                viewModel.logList.Add("Log: " + viewModel.logNote);
                viewModel.Log = false;
                viewModel.logNote = "";
            }
            else
            {
                viewModel.logList.Add("");
            }
            

            //viewModel.Angle = (int)Math.Round(viewModel.Measurement / conversion);
            //Console.WriteLine(viewModel.pressureAmbient);

        }

        private void Excel_Button_Click(object sender, RoutedEventArgs e)
        {
            createExcel.startUpExcel();
            createExcel.generateExcel(viewModel.timeList, viewModel.tCompressedList, viewModel.tChamberList, viewModel.tExhaustList,
                viewModel.pAmbientList, viewModel.pCompressedList, viewModel.tAmbientList, viewModel.humidityList, viewModel.shaftSpeedList,
                viewModel.logList);
            createExcel.generateMetricExcel(viewModel.timeList, viewModel.tCompressedList, viewModel.tChamberList, viewModel.tExhaustList,
                viewModel.pAmbientList, viewModel.pCompressedList, viewModel.tAmbientList, viewModel.humidityList, viewModel.shaftSpeedList,
                viewModel.logList);
        }

        private void FlowRate_Box_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.flowRate = Convert.ToDouble(FlowRate_Box.Text);
            }
            catch
            {

                viewModel.flowRate = 0;
                FlowRate_Box.Text = "0";
            }
        }

        // Checks to see if a high temperature/pressure is reached. Flashes warning if true
        private void WarningChecker()
        {
            double tempLimit = 3000;
            double pressureLimit = 5000;

            while (true)
            {
                if (viewModel.tempAmbient > tempLimit || viewModel.tempCompressed > tempLimit || 
                    viewModel.tempChamber > tempLimit || viewModel.tempExhaust > tempLimit ||
                    viewModel.pressureAmbient > pressureLimit || viewModel.pressureCompressed > pressureLimit)
                {
                    viewModel.isDangerous = true;
                }
                else
                {
                    viewModel.isDangerous = false;
                }

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

        // Save a message to the log if clicked
        private void Log_Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Log = true;
        }
    }
}
