using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.IO;

namespace Sign_PDF_with_LTV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load existing PDF document.
            PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/PDF_Succinctly.pdf");

            //Load digital ID with password.
            PdfCertificate certificate = new PdfCertificate(@"../../Data/DigitalSignatureTest.pfx", "DigitalPass123");

            //Create a signature with loaded digital ID.
            PdfSignature signature = new PdfSignature(document, document.Pages[0], certificate, "DigitalSignature");

            signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
            signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA256;

            signature.TimeStampServer = new TimeStampServer(new Uri("http://timestamping.ensuredca.com"));

            MemoryStream memoryStream = new MemoryStream();

            //Save the PDF document.
            document.Save(memoryStream);

            //Close the document.
            document.Close(true);

            //Load the signed document to update LTV information.
            PdfLoadedDocument signedDocument = new PdfLoadedDocument(memoryStream);

            //Get the signed signature field.
            PdfLoadedSignatureField signatureField = signedDocument.Form.Fields[0] as PdfLoadedSignatureField;

            //Update LTV information.
            signatureField.Signature.EnableLtv = true;

            //Save the signed document with LTV information.
            signedDocument.Save("SignPDFWithLTV.pdf");

            //Close the signed document.
            signedDocument.Close(true);
        }
    }
}
