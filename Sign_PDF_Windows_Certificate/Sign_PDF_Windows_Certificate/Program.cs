using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sign_PDF_Windows_Certificate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Initialize the Windows store.
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            //Find the certificate using thumb print.
            X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByThumbprint, "F85E1C5D93115CA3F969DA3ABC8E0E9547FCCF5A", true);
            //Get first certificate
            X509Certificate2 digitalID = fcollection[0];

            //Load existing PDF document.
            PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/PDF_Succinctly.pdf");

            //Load X509Certificate2.
            PdfCertificate certificate = new PdfCertificate(digitalID);

            //Create a Revision 2 signature with loaded digital ID.
            PdfSignature signature = new PdfSignature(document, document.Pages[0], certificate, "DigitalSignature");

            //Changing the digital signature standard and hashing algorithm.
            signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
            signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA512;

            //Save the PDF document.
            document.Save("WindowsStore.pdf");

            //Close the document.
            document.Close(true);
        }
    }
}
