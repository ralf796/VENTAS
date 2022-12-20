using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Ventas.Class
{
    public class Utils
    {

        public int GetUsuarioRol()
        {
            int respuesta = 0;


            return respuesta;
        }


        private static string ObtenerConexion()
        {
            return ConfigurationManager.ConnectionStrings["genesysConnectionString"].ToString();
        }

        public static string ObtenerServidor()
        {
            var server = ConfigurationManager.ConnectionStrings["genesysEntities"].ConnectionString;
            if (server.Contains("192.168.136.23"))
                server = "DESARROLLO";
            else if (server.Contains("dbgenesys"))
                server = "ORACLE AUTONOMOUS";
            else
                server = "SERVER UNKNOWN";
            return server;
        }

        public static string GenerarToken(string cadena)
        {
            int aleatorio = new Random().Next(10000, 99999);
            cadena += aleatorio.ToString() + DateTime.Now.ToString("ddMMyyyy").Trim().ToUpper();
            return Encryption.Instance.Encrypt(cadena);
        }


        public enum Roles : int
        {
            SUPER_USUARIO = 1,
            ADMINISTRADOR = 2,
            CAJERO = 3,
            BODEGUERO = 4,
            SECRETARIA = 5,
            VENDEDOR = 6
        }

        public static class RandomGenerator
        {
            // Generate a random number between two numbers    
            public static int RandomNumber(int min, int max)
            {
                Random random = new Random();
                return random.Next(min, max);
            }

            // Generate a random string with a given size    
            public static string RandomString(int size, bool lowerCase)
            {
                StringBuilder builder = new StringBuilder();
                Random random = new Random();
                char ch;
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                if (lowerCase)
                    return builder.ToString().ToLower();
                return builder.ToString();
            }

            // Generate a random password    
            public static string RandomPassword()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomString(4, true));
                builder.Append(RandomNumber(1000, 9999));
                builder.Append(RandomString(2, false));
                return builder.ToString();
            }
        }

        public static bool ValidarSesion(string usuario = "")
        {
            bool resultado = true;
            try
            {
                if (usuario == "" || usuario == null)
                {
                    resultado = false;
                }
            }
            catch
            {
                resultado = false;
            }
            return resultado;
        }

        public static void FillBackgroundColorCells(ExcelWorksheet workSheet, ExcelRange rango, int row, int col, string color)
        {
            rango = workSheet.Cells[row, col];
            rango.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

            if (color == "gray1")   //TOTALES
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D3D3D3"));
            else if (color == "gray2")
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C0C0C0"));
            else if (color == "gray3")
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAECEE"));
            else if (color == "gray4")
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#A9A9A9"));
            else if (color == "gray5")
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#808080"));
            else if (color == "green1")
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFF66"));
            else if (color == "green2")
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ADFF2F"));
            else if (color == "blue1")
            {
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0070C0"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
            }
        }

        public static void FillBackgroundRange(ExcelWorksheet workSheet, ExcelRange rango, int row, int col1, int col2, string color)
        {
            for (int i = col1; i <= col2; i++)
            {
                rango = workSheet.Cells[row, i];
                rango.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                if (color.ToUpper() == "GRAY1")   //TOTALES
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D3D3D3"));
                else if (color.ToUpper() == "GRAY2")
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C0C0C0"));
                else if (color.ToUpper() == "GRAY3")
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAECEE"));
                else if (color.ToUpper() == "GRAY4")
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#A9A9A9"));
                else if (color.ToUpper() == "GRAY5")
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#808080"));
                else if (color.ToUpper() == "GREEN1")
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFF66"));
                else if (color.ToUpper() == "GREEN2")
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ADFF2F"));
                else if (color.ToUpper() == "BLUE1")
                {
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0070C0"));
                    rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                }
                else if (color.ToUpper() == "BLACK")
                {
                    rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#000000"));
                    rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                }
            }
        }

        public static void FillBorderCellsAll(ExcelWorksheet workSheet, string rango)
        {
            using (ExcelRange Rng = workSheet.Cells[rango])
            {
                Rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                Rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                Rng.Style.Border.Top.Color.SetColor(Color.Black);
                Rng.Style.Border.Bottom.Color.SetColor(Color.Black);
                Rng.Style.Border.Left.Color.SetColor(Color.Black);
                Rng.Style.Border.Right.Color.SetColor(Color.Black);
                workSheet.Cells.AutoFitColumns();
            }
        }

        public static void AddTextCells(ExcelWorksheet workSheet, string position, string text, int row, int col)
        {
            workSheet.Cells[row, col].Value = text;

            if (position.ToUpper() == "L")
                workSheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            else if (position.ToUpper() == "R")
                workSheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            else if (position.ToUpper() == "C")
                workSheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        public static void MergeCellsExcel(ExcelWorksheet workSheet, ExcelRange rango, int row, int col1, int col2, string texto, int fontSize, bool bold, string position)
        {
            workSheet.Cells[row, col1].Value = texto;
            workSheet.Select(ExcelAddress.GetAddress(row, col1, row, col2));
            workSheet.Cells[row, col1].Style.Font.Bold = bold;
            workSheet.Cells[row, col1].Style.Font.Size = fontSize;
            //workSheet.Cells[row, col1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            if (position.ToUpper() == "L")
                workSheet.Cells[row, col1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            else if (position.ToUpper() == "R")
                workSheet.Cells[row, col1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            else if (position.ToUpper() == "C")
                workSheet.Cells[row, col1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            rango = workSheet.SelectedRange;
            rango.Merge = true;
        }

        public static void AddText(ExcelWorksheet workSheet, int row, int col, string texto, decimal valor, int size, bool bold, string position, int formatCell)
        {
            if (texto != "")
                workSheet.Cells[row, col].Value = texto;
            else
                workSheet.Cells[row, col].Value = valor;

            workSheet.Cells[row, col].Style.Font.Size = size;
            workSheet.Cells[row, col].Style.Font.Bold = bold;
            workSheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

            if (position.ToUpper() == "L")
                workSheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            else if (position.ToUpper() == "R")
                workSheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            else if (position.ToUpper() == "C")
                workSheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            if (formatCell == 1)
            {
                workSheet.Cells[row, col].Style.Numberformat.Format = "#,##0.00";
            }
        }

        public static void MergeCell(ExcelWorksheet workSheet, ExcelRange rango, string ini, string fin)
        {
            rango = workSheet.Cells[$"{ini}:{fin}"];
            rango.Merge = true;
        }

        public static void SizeColumn(ExcelWorksheet workSheet, int column, int size)
        {
            workSheet.Column(column).Width = size;
        }

        public static void GridLinesExcel(ExcelWorksheet workSheet, bool lines)
        {
            workSheet.View.ShowGridLines = lines;
        }

        public static void ChartProperties(ExcelChart chart, string titleChart, int sizeChart1, int sizeChart2, int positionRow, int positionCol, bool showTable)
        {
            chart.Title.Text = titleChart;
            chart.SetSize(sizeChart1, sizeChart2);
            chart.SetPosition(positionRow, 0, positionCol, 0);

            if (showTable)
                chart.PlotArea.CreateDataTable();
        }

        public static void SerieChartProperties(ExcelChart chart, ExcelBarChartSerie serie, bool bold, bool showValue, bool showPercent, bool showLeaderLines, bool showTable, eLabelPosition position)
        {
            serie.DataLabel.Font.Bold = bold;
            serie.DataLabel.ShowValue = showValue;
            serie.DataLabel.ShowPercent = showPercent;
            serie.DataLabel.ShowLeaderLines = showLeaderLines;
            serie.DataLabel.Position = position;

            if (showTable)
                chart.PlotArea.CreateDataTable();
        }
        public static void CellBold(ExcelWorksheet workSheet, int row, int col, bool bold, int fontSize)
        {
            workSheet.Cells[row, col].Style.Font.Bold = bold;
            workSheet.Cells[row, col].Style.Font.Size = fontSize;
        }

        public static int GetPaisSocio(string codCliente = "")
        {
            int codEmp = 0;

            char[] charArray = codCliente.ToCharArray();
            char prefijo = charArray[0];

            switch (prefijo.ToString())
            {
                case "G":
                    codEmp = 1;
                    break;
                case "E":
                    codEmp = 2;
                    break;
                case "H":
                    codEmp = 3;
                    break;
                case "N":
                    codEmp = 4;
                    break;
                case "C":
                    codEmp = 5;
                    break;
                case "D":
                    codEmp = 5;
                    break;
                case "R":
                    codEmp = 8;
                    break;
                case "T":
                    codEmp = 8;
                    break;
            }
            return codEmp;
        }
        public static string mes_letras(int mes)
        {
            string resultado = "";

            switch (mes.ToString())
            {

                case "1":
                    resultado = "Enero";
                    break;
                case "2":
                    resultado = "Febrero";
                    break;
                case "3":
                    resultado = "Marzo";
                    break;
                case "4":
                    resultado = "Abril";
                    break;
                case "5":
                    resultado = "Mayo";
                    break;
                case "6":
                    resultado = "Junio";
                    break;
                case "7":
                    resultado = "Julio";
                    break;
                case "8":
                    resultado = "Agosto";
                    break;
                case "9":
                    resultado = "Septiembre";
                    break;
                case "10":
                    resultado = "Octubre";
                    break;
                case "11":
                    resultado = "Noviembre";
                    break;
                case "12":
                    resultado = "Diciembre";
                    break;



                default:
                    break;
            }



            return resultado;
        }
        public static string Dia_Semana(int dia)
        {
            string resultado = "";

            switch (dia.ToString())
            {
                case "0":
                    resultado = "Domingo";
                    break;
                case "1":
                    resultado = "Lunes";
                    break;
                case "2":
                    resultado = "Martes";
                    break;
                case "3":
                    resultado = "Miercoles";
                    break;
                case "4":
                    resultado = "Jueves";
                    break;
                case "5":
                    resultado = "Viernes";
                    break;
                case "6":
                    resultado = "Sabado";
                    break;
                default:
                    break;
            }



            return resultado;
        }
        public static string Fecha_Larga_Letras(DateTime fecha)
        {
            string resultado = "";

            resultado += Dia_Semana((int)fecha.DayOfWeek);
            resultado += ", " + fecha.Day.ToString();
            resultado += " de " + mes_letras(fecha.Month);
            resultado += " de " + fecha.Year.ToString();


            return resultado;
        }
        public static string Mes_Corto_Letras(int mes)
        {
            string resultado = "";

            switch (mes.ToString())
            {

                case "1":
                    resultado = "Ene";
                    break;
                case "2":
                    resultado = "Feb";
                    break;
                case "3":
                    resultado = "Mar";
                    break;
                case "4":
                    resultado = "Abr";
                    break;
                case "5":
                    resultado = "May";
                    break;
                case "6":
                    resultado = "Jun";
                    break;
                case "7":
                    resultado = "Jul";
                    break;
                case "8":
                    resultado = "Ago";
                    break;
                case "9":
                    resultado = "Sep";
                    break;
                case "10":
                    resultado = "Oct";
                    break;
                case "11":
                    resultado = "Nov";
                    break;
                case "12":
                    resultado = "Dic";
                    break;



                default:
                    break;
            }



            return resultado;
        }
        public static string Mes_Completo_Letras(int mes)
        {
            string resultado = "";

            switch (mes.ToString())
            {

                case "1":
                    resultado = "Enero";
                    break;
                case "2":
                    resultado = "Febrero";
                    break;
                case "3":
                    resultado = "Marzo";
                    break;
                case "4":
                    resultado = "Abril";
                    break;
                case "5":
                    resultado = "Mayo";
                    break;
                case "6":
                    resultado = "Junio";
                    break;
                case "7":
                    resultado = "Julio";
                    break;
                case "8":
                    resultado = "Agosto";
                    break;
                case "9":
                    resultado = "Septiembre";
                    break;
                case "10":
                    resultado = "Octubre";
                    break;
                case "11":
                    resultado = "Noviembre";
                    break;
                case "12":
                    resultado = "Diciembre";
                    break;



                default:
                    break;
            }



            return resultado;
        }
    }
}