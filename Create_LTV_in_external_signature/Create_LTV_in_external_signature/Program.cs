using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Create_LTV_in_external_signature
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load existing PDF document.
            PdfLoadedDocument document = new PdfLoadedDocument(@"../../Data/PDF_Succinctly.pdf");
            //Create certificate chain list.
            System.Collections.Generic.List<X509Certificate2> certificates = new System.Collections.Generic.List<X509Certificate2>();
            X509Certificate2 digitalId = new X509Certificate2(@"../../Data/certchain.pfx", "password", X509KeyStorageFlags.Exportable);
            X509Chain chain = new X509Chain();
            chain.Build(digitalId);

            for (int i = 0; i < chain.ChainElements.Count; i++)
            {
                certificates.Add(chain.ChainElements[i].Certificate);
            }
            //Create a revision 2 signature with loaded digital ID.
            PdfSignature signature = new PdfSignature(document, document.Pages[0], null, "DigitalSignature");

            //Set the cryptographic standard.
            signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
            signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA1;

            //Create an external signature.
            IPdfExternalSigner externalSignature = new ExternalSigner("SHA1");

            //Add external signer to the signature.
            signature.AddExternalSigner(externalSignature, certificates, null);
            //Create long term validity
            signature.CreateLongTermValidity(certificates);

            //Set timestamp server.
            signature.TimeStampServer = new TimeStampServer(new Uri("http://timestamping.ensuredca.com"));
            //Save the PDF document.
            document.Save("ExternalSignature.pdf");
            //Close the document.
            document.Close(true);
        }

        //Create an external signature to sign the document hash.
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
            //Sing the PDF hash.
            public byte[] Sign(byte[] message, out byte[] timeStampResponse)
            {
                timeStampResponse = null;
                return SignDocumentHash(message);
            }
            private byte[] SignDocumentHash(byte[] documentHash)
            {
                X509Certificate2 digitalID = new X509Certificate2(@"../../Data/certchain.pfx", "password", X509KeyStorageFlags.Exportable);
                if (digitalID.PrivateKey is RSACryptoServiceProvider)
                {
                    System.Security.Cryptography.RSACryptoServiceProvider rsa = (System.Security.Cryptography.RSACryptoServiceProvider)digitalID.PrivateKey;
                    return rsa.SignData(documentHash, HashAlgorithm);
                }
                else if (digitalID.PrivateKey is RSACng)
                {
                    RSACng rsa = (RSACng)digitalID.PrivateKey;
                    return rsa.SignData(documentHash, System.Security.Cryptography.HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                }
                else
                {
                    return null;
                }

            }

        }

    }
}

