using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Validating_signatures_against_trusted
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

                //Load Windows certificate store.
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;

                if (form != null)
                {
                    foreach (PdfLoadedField field in form.Fields)
                    {
                        if (field is PdfLoadedSignatureField)
                        {
                            PdfLoadedSignatureField signatureField = field as PdfLoadedSignatureField;

                            //Validate the digital signature against Windows certificate store.
                            PdfSignatureValidationResult result = signatureField.ValidateSignature(collection);

                            if (result.IsSignatureValid)
                                Console.WriteLine("Signature is valid");
                            else
                                Console.WriteLine("Signature is invalid");

                            //Update the signatures status based on the certificate validation against certificate store.
                            Console.WriteLine("Signature status: " + result.SignatureStatus);
                        }
                    }
                }
            }
        }
    }
}
