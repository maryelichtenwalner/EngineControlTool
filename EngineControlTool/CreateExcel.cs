using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace EngineControlTool
{
    public class CreateExcel
    {
        public ViewModel viewModel;
        Excel.Application xlApp;
        Excel.Workbook xlBook;
        Excel.Worksheet xlSheet1;
        Excel.Worksheet xlSheet2;
        public CreateExcel(ViewModel vm)
        {
            viewModel = vm;
        }

        public void startUpExcel()
        {
            xlApp = new Excel.Application();
            xlApp.Visible = true;
            xlApp.DisplayAlerts = false;
            xlBook = xlApp.Workbooks.Add();
            xlSheet1 = xlBook.ActiveSheet;
            xlSheet2 = xlBook.Worksheets.Add();

            xlSheet1.Name = "Metric";
            xlSheet2.Name = "Measured Values";

        }

        // Generate Excel spreadsheet with metric values
        public void generateMetricExcel(List<string> timeList, List<double> tCompressed, List<double> tChamber, List<double> tExhaust,
            List<double> pAmbient, List<double> pCompressed, List<double> tAmbient, List<double> humidity, List<double> shaftSpeed,
            List<string> logList)
        {
            // Insert desired headers
            xlSheet1.Range["C1", "F1"].Merge();
            xlSheet1.Range["G1", "H1"].Merge();
            xlSheet1.Range["A1", "B1"].Merge();
            xlSheet1.Cells[1, 1] = "Time Stamps";
            xlSheet1.Cells[1, 3] = "Temperature (ºC)";
            xlSheet1.Cells[1, 7] = "Pressure (psi)";
            xlSheet1.Cells[1, 9] = "Humidity (%)";
            xlSheet1.Cells[1, 10] = "Shaft Speed (RPM)";
            xlSheet1.Cells[1, 11] = "Flow Rate (idk)";
            xlSheet1.Cells[2, 1] = "Time";
            xlSheet1.Cells[2, 2] = "Log";
            xlSheet1.Cells[2, 3] = "Ambient";
            xlSheet1.Cells[2, 4] = "Compressed";
            xlSheet1.Cells[2, 5] = "Chamber";
            xlSheet1.Cells[2, 6] = "Exhaust";
            xlSheet1.Cells[2, 7] = "Ambient";
            xlSheet1.Cells[2, 8] = "Compressed";
            xlSheet1.Cells[2, 9] = "Ambient";
            xlSheet1.Cells[2, 10] = "Turbine";
            xlSheet1.Cells[2, 11] = "Exhaust";

            // Insert the data into the correct column
            for (int i = 0; i < timeList.Count(); i++ )
            {
                xlSheet1.Cells[i + 3, 1] = timeList[i];
                xlSheet1.Cells[i + 3, 2] = logList[i];
                xlSheet1.Cells[i + 3, 4] = (tCompressed[i] - 32) * 0.5556;
                xlSheet1.Cells[i + 3, 5] = (tChamber[i] - 32) * 0.5556;
                xlSheet1.Cells[i + 3, 6] = (tExhaust[i] - 32) * 0.5556;
                xlSheet1.Cells[i + 3, 7] = pAmbient[i];
                xlSheet1.Cells[i + 3, 8] = pCompressed[i];
                xlSheet1.Cells[i + 3, 3] = (tAmbient[i] - 32) * 0.5556;
                xlSheet1.Cells[i + 3, 9] = humidity[i];
                xlSheet1.Cells[i + 3, 10] = shaftSpeed[i];
            }

            xlSheet1.Cells[3, 11] = viewModel.flowRate;

            // Style the sheet
            // Borders
            int count = timeList.Count() + 2;
            Excel.Range headers = xlSheet1.Range[xlSheet1.Cells[1, 1], xlSheet1.Cells[1, 11]];
            Excel.Range headers2 = xlSheet1.Range[xlSheet1.Cells[2, 1], xlSheet1.Cells[2, 11]];
            Excel.Range timeCol = xlSheet1.Range[xlSheet1.Cells[3, 1], xlSheet1.Cells[count, 1]];
            Excel.Range tAmbientCol = xlSheet1.Range[xlSheet1.Cells[3, 3], xlSheet1.Cells[count, 3]];
            Excel.Range tCompressedCol = xlSheet1.Range[xlSheet1.Cells[3, 4], xlSheet1.Cells[count, 4]];
            Excel.Range tChamberCol = xlSheet1.Range[xlSheet1.Cells[3, 5], xlSheet1.Cells[count, 5]];
            Excel.Range tExhaustCol = xlSheet1.Range[xlSheet1.Cells[3, 6], xlSheet1.Cells[count, 6]];
            Excel.Range pAmbientCol = xlSheet1.Range[xlSheet1.Cells[3, 7], xlSheet1.Cells[count, 7]];
            Excel.Range pCompressedCol = xlSheet1.Range[xlSheet1.Cells[3, 8], xlSheet1.Cells[count, 8]];
            Excel.Range humidityCol = xlSheet1.Range[xlSheet1.Cells[3, 9], xlSheet1.Cells[count, 9]];
            Excel.Range shaftCol = xlSheet1.Range[xlSheet1.Cells[3, 10], xlSheet1.Cells[count, 10]];
            Excel.Range flowCol = xlSheet1.Range[xlSheet1.Cells[3, 11], xlSheet1.Cells[count, 11]];
            Excel.Range logCol = xlSheet1.Range[xlSheet1.Cells[3, 2], xlSheet1.Cells[count, 2]];

            Excel.Range tempRange = xlSheet1.Range[xlSheet1.Cells[3, 3], xlSheet1.Cells[count, 6]];
            Excel.Range pressureRange = xlSheet1.Range[xlSheet1.Cells[3, 7], xlSheet1.Cells[count, 8]];

            timeCol.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            tempRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            pressureRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            humidityCol.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            shaftCol.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            flowCol.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            logCol.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            headers.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            headers.Borders.Weight = Excel.XlBorderWeight.xlThick;
            headers2.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            headers2.Borders.Weight = Excel.XlBorderWeight.xlThick;

            DrawThickBorderAround(timeCol);
            DrawThickBorderAround(logCol);
            DrawThickBorderAround(tAmbientCol);
            DrawThickBorderAround(tCompressedCol);
            DrawThickBorderAround(tChamberCol);
            DrawThickBorderAround(tExhaustCol);
            DrawThickBorderAround(pAmbientCol);
            DrawThickBorderAround(pCompressedCol);
            DrawThickBorderAround(humidityCol);
            DrawThickBorderAround(shaftCol);
            DrawThickBorderAround(flowCol);

            // Colors and thin borders
            xlSheet1.Range["C1", "F2"].Interior.Color = 0xe7c6b4;
            tempRange.Interior.Color = 0xf2e1d9;
            xlSheet1.Range["G1", "H2"].Interior.Color = 0xadcbf8;
            xlSheet1.Range["I1", "I2"].Interior.Color = 0x99e6ff;
            xlSheet1.Range["J1", "J2"].Interior.Color = 0xb4e0c6;
            xlSheet1.Range["K1", "K2"].Interior.Color = 0xe597b8;
            xlSheet1.Range["A1", "B2"].Interior.Color = 0xa9a9a9;
            timeCol.Interior.Color = 0xd9d9d9;
            logCol.Interior.Color = 0xd9d9d9;
            pressureRange.Interior.Color = 0xd6e4fc;
            humidityCol.Interior.Color = 0xccf2ff;
            shaftCol.Interior.Color = 0xdaefe2;
            flowCol.Interior.Color = 0xf3cde2;

            // Bold Fonts
            headers.Font.Bold = true;
            headers2.Font.Bold = true;

            // Miscellaneous, autofit columns, date/time format, center text
            timeCol.NumberFormat = "mm/dd/yy  hh:mm:ss";
            xlSheet1.Columns.AutoFit();
            xlSheet1.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }

        // Generate Excel sheet with measured values
        public void generateExcel(List<string> timeList, List<double> tCompressed, List<double> tChamber, List<double> tExhaust,
            List<double> pAmbient, List<double> pCompressed, List<double> tAmbient, List<double> humidity, List<double> shaftSpeed,
            List<string> logList)
        {
            // Insert desired headers
            xlSheet2.Range["C1", "F1"].Merge();
            xlSheet2.Range["G1", "H1"].Merge();
            xlSheet2.Range["A1", "B1"].Merge();
            xlSheet2.Cells[1, 1] = "Time Stamps";
            xlSheet2.Cells[1, 3] = "Temperature (ºF)";
            xlSheet2.Cells[1, 7] = "Pressure (psi)";
            xlSheet2.Cells[1, 9] = "Humidity (%)";
            xlSheet2.Cells[1, 10] = "Shaft Speed (RPM)";
            xlSheet2.Cells[1, 11] = "Flow Rate (idk)";
            xlSheet2.Cells[2, 1] = "Time";
            xlSheet2.Cells[2, 2] = "Log";
            xlSheet2.Cells[2, 3] = "Ambient";
            xlSheet2.Cells[2, 4] = "Compressed";
            xlSheet2.Cells[2, 5] = "Chamber";
            xlSheet2.Cells[2, 6] = "Exhaust";
            xlSheet2.Cells[2, 7] = "Ambient";
            xlSheet2.Cells[2, 8] = "Compressed";
            xlSheet2.Cells[2, 9] = "Ambient";
            xlSheet2.Cells[2, 10] = "Turbine";
            xlSheet2.Cells[2, 11] = "Exhaust";

            // Insert the data into the correct column
            for (int i = 0; i < timeList.Count(); i++)
            {
                xlSheet2.Cells[i + 3, 1] = timeList[i];
                xlSheet2.Cells[i + 3, 2] = logList[i];
                xlSheet2.Cells[i + 3, 4] = tCompressed[i];
                xlSheet2.Cells[i + 3, 5] = tChamber[i];
                xlSheet2.Cells[i + 3, 6] = tExhaust[i];
                xlSheet2.Cells[i + 3, 7] = pAmbient[i];
                xlSheet2.Cells[i + 3, 8] = pCompressed[i];
                xlSheet2.Cells[i + 3, 3] = tAmbient[i];
                xlSheet2.Cells[i + 3, 9] = humidity[i];
                xlSheet2.Cells[i + 3, 10] = shaftSpeed[i];
            }

            xlSheet2.Cells[3, 11] = viewModel.flowRate;

            // Style the sheet
            // Borders
            int count = timeList.Count() + 2;
            Excel.Range headers = xlSheet2.Range[xlSheet2.Cells[1, 1], xlSheet2.Cells[1, 11]];
            Excel.Range headers2 = xlSheet2.Range[xlSheet2.Cells[2, 1], xlSheet2.Cells[2, 11]];
            Excel.Range timeCol = xlSheet2.Range[xlSheet2.Cells[3, 1], xlSheet2.Cells[count, 1]];
            Excel.Range tAmbientCol = xlSheet2.Range[xlSheet2.Cells[3, 3], xlSheet2.Cells[count, 3]];
            Excel.Range tCompressedCol = xlSheet2.Range[xlSheet2.Cells[3, 4], xlSheet2.Cells[count, 4]];
            Excel.Range tChamberCol = xlSheet2.Range[xlSheet2.Cells[3, 5], xlSheet2.Cells[count, 5]];
            Excel.Range tExhaustCol = xlSheet2.Range[xlSheet2.Cells[3, 6], xlSheet2.Cells[count, 6]];
            Excel.Range pAmbientCol = xlSheet2.Range[xlSheet2.Cells[3, 7], xlSheet2.Cells[count, 7]];
            Excel.Range pCompressedCol = xlSheet2.Range[xlSheet2.Cells[3, 8], xlSheet2.Cells[count, 8]];
            Excel.Range humidityCol = xlSheet2.Range[xlSheet2.Cells[3, 9], xlSheet2.Cells[count, 9]];
            Excel.Range shaftCol = xlSheet2.Range[xlSheet2.Cells[3, 10], xlSheet2.Cells[count, 10]];
            Excel.Range flowCol = xlSheet2.Range[xlSheet2.Cells[3, 11], xlSheet2.Cells[count, 11]];
            Excel.Range logCol = xlSheet2.Range[xlSheet2.Cells[3, 2], xlSheet2.Cells[count, 2]];

            Excel.Range tempRange = xlSheet2.Range[xlSheet2.Cells[3, 3], xlSheet2.Cells[count, 6]];
            Excel.Range pressureRange = xlSheet2.Range[xlSheet2.Cells[3, 7], xlSheet2.Cells[count, 8]];

            timeCol.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            tempRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            pressureRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            humidityCol.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            shaftCol.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            flowCol.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            logCol.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            headers.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            headers.Borders.Weight = Excel.XlBorderWeight.xlThick;
            headers2.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            headers2.Borders.Weight = Excel.XlBorderWeight.xlThick;

            DrawThickBorderAround(timeCol);
            DrawThickBorderAround(logCol);
            DrawThickBorderAround(tAmbientCol);
            DrawThickBorderAround(tCompressedCol);
            DrawThickBorderAround(tChamberCol);
            DrawThickBorderAround(tExhaustCol);
            DrawThickBorderAround(pAmbientCol);
            DrawThickBorderAround(pCompressedCol);
            DrawThickBorderAround(humidityCol);
            DrawThickBorderAround(shaftCol);
            DrawThickBorderAround(flowCol);

            // Colors and thin borders
            xlSheet2.Range["C1", "F2"].Interior.Color = 0xe7c6b4;
            tempRange.Interior.Color = 0xf2e1d9;
            xlSheet2.Range["G1", "H2"].Interior.Color = 0xadcbf8;
            xlSheet2.Range["I1", "I2"].Interior.Color = 0x99e6ff;
            xlSheet2.Range["J1", "J2"].Interior.Color = 0xb4e0c6;
            xlSheet2.Range["K1", "K2"].Interior.Color = 0xe597b8;
            xlSheet2.Range["A1", "B2"].Interior.Color = 0xa9a9a9;
            timeCol.Interior.Color = 0xd9d9d9;
            logCol.Interior.Color = 0xd9d9d9;
            pressureRange.Interior.Color = 0xd6e4fc;
            humidityCol.Interior.Color = 0xccf2ff;
            shaftCol.Interior.Color = 0xdaefe2;
            flowCol.Interior.Color = 0xf3cde2;

            // Bold Fonts
            headers.Font.Bold = true;
            headers2.Font.Bold = true;

            // Miscellaneous, autofit columns, date/time format, center text
            timeCol.NumberFormat = "mm/dd/yy  hh:mm:ss";
            xlSheet2.Columns.AutoFit();
            xlSheet2.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }

        private void DrawThickBorderAround(Excel.Range cells)
        {
            cells.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
        }

        private void AllBorders(Excel.Borders _borders)
        {
            _borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            _borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            _borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            _borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
        }
    }
}
