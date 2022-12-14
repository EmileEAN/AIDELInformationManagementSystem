using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EEANWorks.Microsoft
{
    public static class Excel
    {
        public static T GetCellValue<T>(Cell _cell, WorkbookPart _wbPart, T _defaultValue = default)
        {
            try
            {
                if (_cell == null)
                    return _defaultValue;

                object value = null;

                string cellString = _cell.InnerText;
                if (_cell.DataType != null)
                {
                    switch (_cell.DataType.Value)
                    {
                        case CellValues.SharedString:
                            var stringTable = _wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                            if (stringTable != null)
                            {
                                value = stringTable.SharedStringTable.ElementAt(int.Parse(cellString)).InnerText;
                            }
                            break;

                        case CellValues.Date:
                            value = DateTime.FromOADate(Convert.ToDouble(cellString));
                            break;

                        case CellValues.Boolean:
                            switch (cellString)
                            {
                                case "0":
                                    value = "FALSE";
                                    break;
                                default:
                                    value = "TRUE";
                                    break;
                            }
                            break;

                        case CellValues.Number:
                            {
                                if (default(T) is int) value = Convert.ToInt32(cellString);
                                if (default(T) is decimal) value = Convert.ToDecimal(cellString);
                                if (default(T) is DateTime) value = DateTime.FromOADate(Convert.ToDouble(cellString));
                            }
                            break;
                    }
                }
                else if (cellString == "")
                    value = _defaultValue;
                else
                {
                    if (default(T) is int) value = Convert.ToInt32(cellString);
                    if (default(T) is decimal) value = Convert.ToDecimal(cellString);
                    if (default(T) is DateTime) value = DateTime.FromOADate(Convert.ToDouble(cellString));
                }

                if (value == null)
                    return default;

                return (T)value;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }

    public static class ExcelStringExtension
    {
        public static string NumToColumnName(this int _columnNumber)
        {
            string columnName = "";

            while (_columnNumber > 0)
            {
                int modulo = (_columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                _columnNumber = (_columnNumber - modulo) / 26;
            }

            return columnName;
        }
        public static string IndexToColumnName(this int _columnIndex)
        {
            return (_columnIndex + 1).NumToColumnName();
        }
    }

    public static class ExcelIntExtension
    {
        public static int ToColumnNum(this string _columnName)
        {
            //return columnIndex;
            string name = _columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }
        public static int ToColumnIndex(this string _columnName)
        {
            return _columnName.ToColumnNum() - 1;
        }
    }

    public static class CellEnumerableExtension
    {
        public static Cell GetCell(this IEnumerable<Cell> _cells, string _cellReference)
        {
            return _cells.FirstOrDefault(x => x.CellReference == _cellReference);
        }

        public static Cell GetCellForColumn(this IEnumerable<Cell> _cells, string _columnName)
        {
            return _cells.FirstOrDefault(x => GetColumnName(x.CellReference) == _columnName);
        }

        private static string GetColumnName(string _cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(_cellReference);
            return match.Value;
        }
    }
}