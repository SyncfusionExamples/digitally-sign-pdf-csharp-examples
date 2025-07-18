﻿using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;

namespace PDF_digital_signature_with_CAdES
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

                //Changing the digital signature standard and hashing algorithm.
                signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
                signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA512;

                //Save the PDF document.
                document.Save("SigneCAdES.pdf");
            }
        }
    }
}
