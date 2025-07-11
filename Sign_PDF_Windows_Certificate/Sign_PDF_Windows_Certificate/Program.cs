using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Sign_PDF_Windows_Certificate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Initialize the Windows store.
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            if (store.Certificates != null && store.Certificates.Count > 0)
            {
                //Find the certificate using thumb print.
                X509Certificate2Collection thumbprintCollection = store.Certificates.Find(X509FindType.FindByThumbprint, "B8EA768D7672A3E56A400F063C968F7E025737F8", true);
                //Get first certificate
                X509Certificate2 digitalID = thumbprintCollection[0];

                if (digitalID != null)
                {
                    //Load existing PDF document.
                    using (PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/PDF_Succinctly.pdf"))
                    {
                        //Load X509Certificate2.
                        PdfCertificate certificate = new PdfCertificate(digitalID);

                        //Create a Revision 2 signature with loaded digital ID.
                        PdfSignature signature = new PdfSignature(document, document.Pages[0], certificate, "DigitalSignature");

                        //Changing the digital signature standard and hashing algorithm.
                        signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
                        signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA512;

                        //Save the PDF document.
                        document.Save("WindowsStore.pdf");
                    }
                }
                else
                {
                    Console.WriteLine("Certificate not found in the store with the specified thumbprint.");
                }
            }
            else
            {
                Console.WriteLine("No certificates found in the store.");
            }
            store.Close();
        }
    }
}
