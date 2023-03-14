using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Drawing;
using System.IO;
using System.Drawing;
/// <summary>
/// Summary description for ExportToExcel
/// </summary>
public static class ExportToExcel
{
   
    public static string Readcell(this WorksheetPart wsPart, string AddressName)
    {
        Cell theCell = wsPart.Worksheet.Descendants<Cell>().Where(c => c.CellReference == AddressName).FirstOrDefault<Cell>();

        string cellValue = "";
        if (theCell != null)
        {
            cellValue = theCell.InnerText;
            WorkbookPart wb = (WorkbookPart)wsPart.GetParentParts().FirstOrDefault();

            if (theCell.DataType != null)
            {
                if (theCell.DataType == CellValues.SharedString)
                {
                    switch (theCell.DataType.Value)
                    {
                        case CellValues.SharedString:

                            SharedStringTablePart stringTable = wb.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                            if (stringTable != null)
                            {
                                cellValue = stringTable.SharedStringTable.ElementAt(int.Parse(cellValue)).InnerText;
                            }
                            break;
                    }
                }
            }
        }
        return cellValue;
    }

}