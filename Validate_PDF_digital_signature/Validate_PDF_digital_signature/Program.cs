using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validate_PDF_digital_signature
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
                    //Validate all the digital signatures present in the PDF document.
                    bool isValid = form.Fields.ValidateSignatures(out List<PdfSignatureValidationResult> results);

                    //Show the result based on the result.
                    if (isValid)
                        Console.WriteLine("All signatures are valid");
                    else
                        Console.WriteLine("At least one signature is invalid");
                }
            }
        }
    }
}
