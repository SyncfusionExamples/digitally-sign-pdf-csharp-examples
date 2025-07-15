using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;

namespace Digital_signature_custom_appearance
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
                signature.Bounds = new System.Drawing.RectangleF(40, 40, 350, 100);

                //Load image from file.
                PdfBitmap image = new PdfBitmap(@"../../Data/signature.png");

                //Create a font to draw text.
                PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 15);

                //Drawing text, shape, and image into the signature appearance.
                signature.Appearance.Normal.Graphics.DrawRectangle(PdfPens.Black, PdfBrushes.White, new System.Drawing.RectangleF(0, 0, 350, 100));
                signature.Appearance.Normal.Graphics.DrawImage(image, 0, 0, 100, 100);
                signature.Appearance.Normal.Graphics.DrawString("Digitally Signed by Syncfusion", font, PdfBrushes.Black, 120, 17);
                signature.Appearance.Normal.Graphics.DrawString("Reason: Testing signature", font, PdfBrushes.Black, 120, 39);
                signature.Appearance.Normal.Graphics.DrawString("Location: USA", font, PdfBrushes.Black, 120, 60);

                //Save the PDF document.
                document.Save("SignedAppearance.pdf");
            }
        }
    }
}
