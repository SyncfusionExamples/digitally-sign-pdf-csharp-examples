using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;

namespace Sign_PDF_with_LTA
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

            //Save the PDF document.
            document.Save("LTV_document.pdf");

            //Close the document.
            document.Close(true);

            PdfLoadedDocument ltDocument = new PdfLoadedDocument("LTV_document.pdf");
            //Load the existing PDF page.

            PdfLoadedPage lpage = ltDocument.Pages[0] as PdfLoadedPage;

            //Enable the LTV (Long Term Validation) for the existing signed field            
            PdfLoadedSignatureField signatureField = ltDocument.Form.Fields[0] as PdfLoadedSignatureField;

            signatureField.Signature.EnableLtv = true;

            //Create PDF signature with empty certificate.
            PdfSignature timeStamp = new PdfSignature(lpage, "timestamp");


            // Note: If you encounter an "Arithmetic operation resulted in an overflow" error while creating an LTA,
            // it is likely due to the default estimated signature size being insufficient. 
            //timeStamp.EstimatedSignatureSize = 24000;

            //Configure the time stamp server for the signature.
            timeStamp.TimeStampServer = new TimeStampServer(new Uri("http://timestamping.ensuredca.com"));

            //Save and close the document
            ltDocument.Save("PAdES B-LTA.pdf");

            ltDocument.Close(true);
        }
    }
}
