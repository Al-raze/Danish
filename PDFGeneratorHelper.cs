using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using iTextSharp.text.html.simpleparser;
using Infrastrucutre.Core.Models.ViewModels;
using System.Web.Mvc;


namespace AkraTechFramework.Helpers
{
    public class PDFGeneratorHelper
    {
        public static bool FreightForwarderPDFGenerator(OrderRequest request)
        {

//            StringBuilder freightPDFBuilder = new StringBuilder();
//            freightPDFBuilder.AppendFormat(@"<html><body><p  align=center style=font-family:Times;font-weight:bold;>Freight Forwarder Notifications</p>");

//            freightPDFBuilder.AppendFormat(@"<br/><br/><br/><table border=1 width=800><tr style=font-family:Times;font-weight:bold;text-align:center;>
//                                                            <th >Supplier Name</th>
//                                                            <th>PO Number</th>
//                                                            <th >MM Item</th>
//                                                            <th >PM Item</th>
//                                                            <th >Item Description</th>
//                                                            <th >Supplier Ship Date</th>
//                                                            <th >Port Description</th>
//                                                             </tr><tr style=font-family:Times;text-align:center;>");

//            foreach (var item in details)
//            {
//                freightPDFBuilder.AppendFormat(@"<td>{0}</td>", item.SupplierName);
//                freightPDFBuilder.AppendFormat(@"<td>{0}</td>", item.HeaderID);
//                freightPDFBuilder.AppendFormat(@"<td>{0}</td>", item.MM);
//                freightPDFBuilder.AppendFormat(@"<td>{0}</td>", item.PM);
//                freightPDFBuilder.AppendFormat(@"<td>{0}</td>", item.Description);
//                freightPDFBuilder.AppendFormat(@"<td>{0}</td>", item.SupplierShipDate);
//                freightPDFBuilder.AppendFormat(@"<td>{0}</td>", item.PortDescription);
//            }

//            freightPDFBuilder.AppendFormat(@"</tr></table></body></html>");

//            String htmlText = freightPDFBuilder.ToString();
//            Document document = new Document();
//            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
//            if (!Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\" + "\\FFPDF\\"))
//            {
//                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\" + "\\FFPDF\\");
//            }
//            pdfName = "FF_" + DateTime.Now.ToFileTime().ToString() + "_" + headerID + ".pdf";
//            PdfWriter.GetInstance(document, new System.IO.FileStream(HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\" + "\\FFPDF\\" + pdfName, System.IO.FileMode.Create));
//            document.Open();

//            iTextSharp.text.html.simpleparser.HTMLWorker hw =
//                         new iTextSharp.text.html.simpleparser.HTMLWorker(document);
//            hw.Parse(new System.IO.StringReader(htmlText));
//            document.Close();

            return true;

        }


        



    }
}