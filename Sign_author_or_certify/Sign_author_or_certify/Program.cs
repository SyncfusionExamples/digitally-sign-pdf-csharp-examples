using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;

namespace Sign_author_or_certify
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load existing PDF document.
            using (PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/PDF_Succinctly.pdf"))
            {
                //Load digital ID with password.
                PdfCertificate certificate = new PdfCertificate(@"../../Data/DigitalSignatureTest.pfx", "DigitalPass123");

                //Create a signature with loaded digital ID.
                PdfSignature signature = new PdfSignature(document, document.Pages[0], certificate, "DigitalSignature");

                signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
                signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA256;

                //This property enables the author or certifying signature.
                signature.Certificated = true;

                //Allow the form fill and and comments.
                signature.DocumentPermissions = PdfCertificationFlags.AllowFormFill | PdfCertificationFlags.AllowComments;

                //Save the PDF document.
                document.Save("Certifying.pdf");
            }
        }
    }
}
