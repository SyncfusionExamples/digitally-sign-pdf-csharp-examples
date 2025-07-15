using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;

namespace Validate_PDF_individual_signatures
{
    internal class Program
    {
        static void Main(string[] args)
        {
//Load an existing PDF document.
using (PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/MultipleSignature.pdf"))
{

    //Load PDF form.
    PdfLoadedForm form = document.Form;

    if (form != null)
    {
        foreach (PdfLoadedField field in form.Fields)
        {
            Console.WriteLine();
            if (field is PdfLoadedSignatureField)
            {
                PdfLoadedSignatureField signatureField = field as PdfLoadedSignatureField;

                //Check whether the signature is signed.
                if (signatureField.IsSigned)
                {
                    //Validate the digital signature.
                    PdfSignatureValidationResult result = signatureField.ValidateSignature();

                    if (result.IsSignatureValid)
                        Console.WriteLine("Signature is valid");
                    else
                        Console.WriteLine("Signature is invalid");

                    //Retrieve the signature information.
                    Console.WriteLine("<<<<Validation summary>>>>>>");
                    Console.WriteLine("Digitally Signed by: " + signatureField.Signature.Certificate.IssuerName);
                    Console.WriteLine("Valid From: " + signatureField.Signature.Certificate.ValidFrom);
                    Console.WriteLine("Valid To: " + signatureField.Signature.Certificate.ValidTo);
                    Console.WriteLine("Signature Algorithm : " + result.SignatureAlgorithm);
                    Console.WriteLine("Hash Algorithm : " + result.DigestAlgorithm);
                    Console.WriteLine("Cryptographic Standard : " + result.CryptographicStandard);
                }
            }
        }
        Console.Read();
    }
}
        }
    }
}
