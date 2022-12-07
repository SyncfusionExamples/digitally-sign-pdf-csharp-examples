using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sign_PDF_with_LTA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load existing PDF document.
            PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/PDF_Succinctly.pdf");

            //Load digital ID with password.
            PdfCertificate certificate = new PdfCertificate(@"../../Data/DigitalSignatureTest.pfx", "DigitalPass123");

            //Create a signature with loaded digital ID.
            PdfSignature signature = new PdfSignature(document, document.Pages[0], certificate, "DigitalSignature");

            signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
            signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA256;

            signature.TimeStampServer = new TimeStampServer(new Uri("http://timestamping.ensuredca.com"));

            //Enable LTV document.
            signature.EnableLtv = true;

            //Save the PDF document.
            document.Save("LTV_document.pdf");

            //Close the document.
            document.Close(true);

            PdfLoadedDocument ltDocument = new PdfLoadedDocument("LTV_document.pdf");
            //Load the existing PDF page.

            PdfLoadedPage lpage = ltDocument.Pages[0] as PdfLoadedPage;


            //Create PDF signature with empty certificate.

            PdfSignature timeStamp = new PdfSignature(lpage, "timestamp");


            timeStamp.TimeStampServer = new TimeStampServer(new Uri("http://timestamping.ensuredca.com"));

            ltDocument.Save("PAdES B-LTA.pdf");

            ltDocument.Close(true);
        }
    }
}
