using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Sign_PDF_External_Signer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the existing PDF document.
            using (PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/PDF_Succinctly.pdf"))
            {

                //Create PDF signature field with PdfCertificate as null.
                PdfSignature signature = new PdfSignature(document, document.Pages[0], null, "DigitalSignature");

                //Create an external signer.
                IPdfExternalSigner externalSignature = new ExternalSigner("SHA1");

                //Add public certificates.
                List<X509Certificate2> certificates = new List<X509Certificate2>();

                certificates.Add(new X509Certificate2("../../Data/PublicCertificate.cer"));

                signature.AddExternalSigner(externalSignature, certificates, null);

                //Save the PDF document.
                document.Save("ExternalSignature.pdf");
            }
        }
    }
    //Create the external signer class and sign the document hash.
    class ExternalSigner : IPdfExternalSigner
    {
        private string _hashAlgorithm;
        public string HashAlgorithm
        {
            get { return _hashAlgorithm; }
        }

        public ExternalSigner(string hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm;
        }
        public byte[] Sign(byte[] message, out byte[] timeStampResponse)
        {
            timeStampResponse = null;

            // Note: This message should be used for computing and signing with your HSM, USB token or an external signing service.
            // For demonstration purposes only, a local certificate is being used to sign the message.

            X509Certificate2 digitalID = new X509Certificate2("../../Data/DigitalSignatureTest.pfx", "DigitalPass123");

            if (digitalID.PrivateKey is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                System.Security.Cryptography.RSACryptoServiceProvider rsa = (System.Security.Cryptography.RSACryptoServiceProvider)digitalID.PrivateKey;
                return rsa.SignData(message, HashAlgorithm);
            }
            else if (digitalID.PrivateKey is RSACng)
            {
                RSACng rsa = (RSACng)digitalID.PrivateKey;
                return rsa.SignData(message, System.Security.Cryptography.HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
            return null;
        }
    }
}
