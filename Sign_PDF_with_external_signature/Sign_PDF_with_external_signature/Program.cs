using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace Sign_PDF_with_external_signature
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load existing PDF document.
            PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/PDF_Succinctly.pdf");

            //Create a Revision 2 signature with loaded digital ID.
            PdfSignature signature = new PdfSignature(document, document.Pages[0], null, "DigitalSignature");
            signature.ComputeHash += Signature_ComputeHash;

            //Save the PDF document.
            document.Save("ExternalSignature.pdf");

            //Close the document.
            document.Close(true);

            void Signature_ComputeHash(object sender, PdfSignatureEventArgs arguments)
            {
                //Get the document bytes.
                byte[] documentBytes = arguments.Data;

                SignedCms signedCms = new SignedCms(new ContentInfo(documentBytes), detached: true);
                //Compute the signature using the specified digital ID file and the password.
                X509Certificate2 certificate = new X509Certificate2(@"../../Data/DigitalSignatureTest.pfx", "DigitalPass123");
                var cmsSigner = new CmsSigner(certificate);
                //Set the digest algorithm SHA256.
                cmsSigner.DigestAlgorithm = new Oid("2.16.840.1.101.3.4.2.1");
                signedCms.ComputeSignature(cmsSigner);
                //Embed the encoded digital signature to the PDF document.
                arguments.SignedData = signedCms.Encode();
            }
        }
    }
}
