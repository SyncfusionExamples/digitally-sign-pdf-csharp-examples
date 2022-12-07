using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remove_digital_signatures_from_PDF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load an existing PDF document.
            PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/SignedAppearance.pdf");

            //Get the signature field from PdfloadedDocument form field collection.
            PdfLoadedSignatureField signatureField = document.Form.Fields[0] as PdfLoadedSignatureField;
            //Remove signature field from PdfLoadedDocument form field collection.
            document.Form.Fields.Remove(signatureField);

            //Save the PDF document.
            document.Save("RemoveDigital.pdf");
            document.Close(true);
        }
    }
}
