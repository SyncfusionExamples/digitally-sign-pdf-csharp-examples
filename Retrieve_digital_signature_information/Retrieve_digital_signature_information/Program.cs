using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrieve_digital_signature_information
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load an existing PDF document.
            PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/SignedAppearance.pdf");

            //Get the signature field from PdfLoadedDocument form field collection.
            PdfLoadedSignatureField signatureField = document.Form.Fields[0] as PdfLoadedSignatureField;
            PdfSignature signature = signatureField.Signature;

            //Extract the signature information.
            Console.WriteLine("Digitally Signed by: " + signature.Certificate.IssuerName);
            Console.WriteLine("Valid From: " + signature.Certificate.ValidFrom);
            Console.WriteLine("Valid To: " + signature.Certificate.ValidTo);
            Console.WriteLine("Hash Algorithm : " + signature.Settings.DigestAlgorithm);
            Console.WriteLine("Cryptographics Standard : " + signature.Settings.CryptographicStandard);

            //Close the document.
            document.Close(true);
        }
    }
}
