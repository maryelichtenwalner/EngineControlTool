/// 
/// ViewModel.cs
///     Class that passes data between other classes as necessary 
/// 
/// Written by Mary Lichtenwalner
/// 
/// Last Update: April 4, 2022
///

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EngineControlTool
{
    public class ViewModel : INotifyPropertyChanged
    {
        //*****************************************************************************************
        // Section 1. Methods to control changes in data
        //*****************************************************************************************
        public ViewModel MainModel { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        protected void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void onPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //************************************************************************************
        // Section 2. Methods to control which color is selected in GUI
        //************************************************************************************

        // Integer to set dark or light theme
        private int color_theme = 0;
        public int Color_Theme
        {
            get { return color_theme; }
            set { color_theme = value; onPropertyChange(nameof(Color_Theme)); }
        }

        // Integer that determines which accent color is selected
        private int accent_color = 0;
        public int Accent_Color
        {
            get { return accent_color; }
            set { accent_color = value; onPropertyChange(nameof(Accent_Color)); }
        }

        // String that determines which accent color is selected
        private string accent_color_string = "Red";
        public string Accent_Color_String
        {
            get { return accent_color_string; }
            set { accent_color_string = value; onPropertyChange(nameof(Accent_Color_String)); }
        }

        //************************************************************************************
        // Section 3. All real-time measurements read from serial port
        //************************************************************************************

        // Ambient pressure reading
        private double _pressureAmbient;
        public double pressureAmbient
        {
            get { return _pressureAmbient; }
            set{ _pressureAmbient = value; onPropertyChange(nameof(pressureAmbient)); }
            
        }

        // Compressed pressure reading
        private double _pressureCompressed;
        public double pressureCompressed
        {
            get { return _pressureCompressed; }
            set { _pressureCompressed = value; onPropertyChange(nameof(pressureCompressed)); }

        }

        //Ambient temperature reading
        private double _tempAmbient;
        public double tempAmbient
        {
            get { return _tempAmbient; }
            set { _tempAmbient = value; onPropertyChange(nameof(tempAmbient)); }

        }

        //Compressed temperature reading
        private double _tempCompressed;
        public double tempCompressed
        {
            get { return _tempCompressed; }
            set { _tempCompressed = value; onPropertyChange(nameof(tempCompressed)); }

        }

        //Chamber temperature reading
        private double _tempChamber;
        public double tempChamber
        {
            get { return _tempChamber; }
            set { _tempChamber = value; onPropertyChange(nameof(tempChamber)); }

        }

        //Exhaust temperature reading
        private double _tempExhaust;
        public double tempExhaust
        {
            get { return _tempExhaust; }
            set { _tempExhaust = value; onPropertyChange(nameof(tempExhaust)); }

        }

        //Ambient humidity reading
        private double _humidity;
        public double humidity
        {
            get { return _humidity; }
            set { _humidity = value; onPropertyChange(nameof(humidity)); }
        }

        //Shaft speed reading
        private double _shaftSpeed;
        public double shaftSpeed
        {
            get { return _shaftSpeed; }
            set { _shaftSpeed = value; onPropertyChange(nameof(shaftSpeed)); }
        }

        //Flow rate from user input
        private double _flowRate;
        public double flowRate
        {
            get { return _flowRate; }
            set { _flowRate = value; onPropertyChange(nameof(flowRate)); }
        }

        // Bool determining if log button was clicked by user
        private bool _log;
        public bool Log
        {
            get { return _log; }
            set { _log = value; onPropertyChange(nameof(Log)); }
        }

        // Optional message to log inputted by user
        private string _logNote;
        public string logNote
        {
            get { return _logNote; }
            set { _logNote = value; onPropertyChange(nameof(logNote)); }
        }

        //************************************************************************************
        // Section 4. Lists that hold all the data that has been received
        //************************************************************************************

        // Ambient pressure list
        private List<double> _pAmbientList = new List<double>();
        public List<double> pAmbientList
        {
            get { return _pAmbientList; }
            set { _pAmbientList = value; onPropertyChange(nameof(pAmbientList)); }

        }

        // Compressed pressure list
        private List<double> _pCompressedList = new List<double>();
        public List<double> pCompressedList
        {
            get { return _pCompressedList; }
            set { _pCompressedList = value; onPropertyChange(nameof(pCompressedList)); }

        }

        // Ambient temperature list
        private List<double> _tAmbientList = new List<double>();
        public List<double> tAmbientList
        {
            get { return _tAmbientList; }
            set { _tAmbientList = value; onPropertyChange(nameof(tAmbientList)); }

        }

        // Compressed temperature list
        private List<double> _tCompressedList = new List<double>();
        public List<double> tCompressedList
        {
            get { return _tCompressedList; }
            set { _tCompressedList = value; onPropertyChange(nameof(tCompressedList)); }

        }

        // Chamber temperature list
        private List<double> _tChamberList = new List<double>();
        public List<double> tChamberList
        {
            get { return _tChamberList; }
            set { _tChamberList = value; onPropertyChange(nameof(tChamberList)); }

        }

        // Exhaust temperature list
        private List<double> _tExhaustList = new List<double>();
        public List<double> tExhaustList
        {
            get { return _tExhaustList; }
            set { _tExhaustList = value; onPropertyChange(nameof(tExhaustList)); }

        }

        // Ambient humidity list
        private List<double> _humidityList = new List<double>();
        public List<double> humidityList
        {
            get { return _humidityList; }
            set { _humidityList = value; onPropertyChange(nameof(humidityList)); }

        }

        // Shaft speed list
        private List<double> _shaftSpeedList = new List<double>();
        public List<double> shaftSpeedList
        {
            get { return _shaftSpeedList; }
            set { _shaftSpeedList = value; onPropertyChange(nameof(shaftSpeedList)); }

        }

        // List with all time stamps
        private List<string> _timeList = new List<string>();
        public List<string> timeList
        {
            get { return _timeList; }
            set { _timeList = value; onPropertyChange(nameof(timeList)); }
        }

        // List containing the log messages/instances of log button clicked
        private List<string> _logList = new List<string>();
        public List<string> logList
        {
            get { return _logList; }
            set { _logList = value; onPropertyChange(nameof(logList)); }
        }

        //************************************************************************************
        // Section 5. Values used to control warning label
        //************************************************************************************

        // Boolean showing if dangerous values are being recorded
        private bool _isDangerous = false;
        public bool isDangerous
        {
            get { return _isDangerous; }
            set { _isDangerous = value; onPropertyChange(nameof(isDangerous)); }
        }

        // Integer determining if warning label is displayed
        private int _showWarning = 0;
        public int showWarning
        {
            get { return _showWarning; }
            set { _showWarning = value; onPropertyChange(nameof(showWarning)); }
        }
    }
}
