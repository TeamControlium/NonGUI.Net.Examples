using System;

namespace HTTPPost
{
    class Program
    {
        static void Main(string[] args)
        {
            Errored.NonExistantDomain();
            Errored.InvalidResourcePath();
            Errored.ConnectionTimeout();
            Errored.BadHTTPHeaderItem();
            Errored.BadlyFormedHTTPHeaderString();
            Errored.BadlyFormedHTTPHeaderBySetting();
            Errored.RejectSSLCertificate();
            Errored.RejectSSLCertificateUsingCustomDelegate();
        }
    }
}


