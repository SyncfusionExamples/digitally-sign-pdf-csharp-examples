using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System.IO;

namespace PDF_multiple_signature
{
    internal class Program
    {
        static void Main(string[] args)
        {           
            //Load existing PDF document.
            PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/PDF_Succinctly.pdf");

            //Load digital ID with password.
            PdfCertificate certificate = new PdfCertificate(@"../../Data/TestAgreement.pfx", "Test123");

            //Create a Revision 1 signature with loaded digital ID.
            PdfSignature signature = new PdfSignature(document, document.Pages[0], certificate, "DigitalSignature1");

            //Changing the digital signature standard and hashing algorithm.
            signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
            signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA512;

            MemoryStream stream = new MemoryStream();

            //Save the PDF document.
            document.Save(stream);

            //Close the document.
            document.Close(true);

            //Load the saved PDF document from the stream.
            PdfLoadedDocument document2 = new PdfLoadedDocument(stream);

            //Load digital ID with password.
            PdfCertificate certificate2 = new PdfCertificate(@"../../Data/DigitalSignatureTest.pfx", "DigitalPass123");

            //Create a signature Revision 2 with loaded digital ID.
            PdfSignature signature2 = new PdfSignature(document2, document2.Pages[0], certificate2, "DigitalSignature2");

            //Changing the digital signature standard and hashing algorithm.
            signature2.Settings.CryptographicStandard = CryptographicStandard.CADES;
            signature2.Settings.DigestAlgorithm = DigestAlgorithm.SHA512;

            //Save the PDF document.
            document2.Save("MultipleSignature.pdf");

            //Close the document.
            document2.Close(true);
        }
    }
}
