using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;

namespace Retrieve_digital_signature_information
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load an existing PDF document.
            using (PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/SignedAppearance.pdf"))
            {
                //Get the signature field from PdfLoadedDocument form field collection.
                PdfLoadedSignatureField signatureField = document.Form.Fields[0] as PdfLoadedSignatureField;
                //Check if the signature field is signed or not
                if (signatureField != null && signatureField.IsSigned)
                {
                    PdfSignature signature = signatureField.Signature;

                    //Extract the signature information.
                    Console.WriteLine("Digitally Signed by: " + signature.Certificate.IssuerName);
                    Console.WriteLine("Valid From: " + signature.Certificate.ValidFrom);
                    Console.WriteLine("Valid To: " + signature.Certificate.ValidTo);
                    Console.WriteLine("Hash Algorithm : " + signature.Settings.DigestAlgorithm);
                    Console.WriteLine("Cryptographic Standard : " + signature.Settings.CryptographicStandard);
                }
                else
                {
                    Console.WriteLine("The signature field is not signed.");
                }
            }
        }
    }
}
