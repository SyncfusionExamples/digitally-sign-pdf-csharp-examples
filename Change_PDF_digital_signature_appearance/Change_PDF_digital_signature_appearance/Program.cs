using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;

namespace Change_PDF_digital_signature_appearance
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

                //Set bounds to the signature.
                signature.Bounds = new System.Drawing.RectangleF(40, 30, 350, 100);

                //Enable the signature validation appearance.
                signature.EnableValidationAppearance = true;

                //Load image from file.
                PdfImage image = PdfImage.FromFile(@"../../Data/signature.png");
                //Create a font to draw text.
                PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 15);

                signature.Appearance.Normal.Graphics.DrawImage(image, new System.Drawing.RectangleF(0, 0, 75, 75));
                signature.Appearance.Normal.Graphics.DrawString("Digitally Signed by Syncfusion", font, PdfBrushes.Black, 110, 5);
                signature.Appearance.Normal.Graphics.DrawString("Reason: Testing signature", font, PdfBrushes.Black, 110, 25);
                signature.Appearance.Normal.Graphics.DrawString("Location: USA", font, PdfBrushes.Black, 110, 45);

                //Save the PDF document.
                document.Save("SignedAppearance.pdf");
            }
        }
    }
}
