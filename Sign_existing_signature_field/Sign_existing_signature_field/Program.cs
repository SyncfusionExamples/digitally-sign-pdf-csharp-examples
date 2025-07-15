using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sign_existing_signature_field
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load existing PDF document.
            using (PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/PDF_SignField.pdf"))
            {
                //Get the first page of the document.
                PdfLoadedPage page = document.Pages[0] as PdfLoadedPage;

                //Gets the first signature field from the PDF document.
                PdfLoadedSignatureField field = document.Form.Fields[0] as PdfLoadedSignatureField;

                //Load digital ID with password.
                PdfCertificate certificate = new PdfCertificate(@"../../Data/DigitalSignatureTest.pfx", "DigitalPass123");

                //Create PdfSignature.
                PdfSignature signature = new PdfSignature(document, page, certificate, field.Name, field);

                //Get graphics from form the signature.
                PdfGraphics graphics = signature.Appearance.Normal.Graphics;

                //Load image from file.
                PdfImage image = PdfImage.FromFile(@"../../Data/signature.png");

                //Create a font to draw text.
                PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 15);

                //Draw text, shape, and image into the signature appearance.
                graphics.DrawRectangle(PdfPens.Black, PdfBrushes.White, new System.Drawing.RectangleF(0, 0, field.Bounds.Width, field.Bounds.Height));
                graphics.DrawImage(image, 0, 0, 100, field.Bounds.Height);
                graphics.DrawString("Digitally Signed by Syncfusion", font, PdfBrushes.Black, 120, 17);
                graphics.DrawString("Reason: Testing signature", font, PdfBrushes.Black, 120, 39);
                graphics.DrawString("Location: USA", font, PdfBrushes.Black, 120, 60);

                //Save the document.
                document.Save("SignedField.pdf");
            }
        }
    }
}
