using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Add_timestamp_to_digital_signature
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

            //Change the digital signature standard and hashing algorithm.
            signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
            signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA512;

            //Add timestamp server link to the signature.
            signature.TimeStampServer = new TimeStampServer(new Uri("http://timestamp.digicert.com/"));

            //Save the PDF document.
            document.Save("SignedTimestamp.pdf");

            //Close the document.
            document.Close(true);
        }
    }
}
