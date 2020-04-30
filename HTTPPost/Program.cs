using System;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Comment/Uncomment as needed...
            // By default all examples uncommented.
            //
            //
            HTTP_Good.Example_SimplePostUsingProperties();
            HTTP_Good.Example_SimplePostUsingParameters();
            HTTP_Good.Example_SimpleGetUsingProperties();
            HTTP_Good.Example_SimpleGetUsingParameters();
            HTTP_Good.Example_HTTPSPostAutoAcceptSSLCertificate();
            HTTP_Good.Example_AcceptSSLCertificateUsingCustomDelegate();
            HTTP_Errored.Example_NonExistantDomain();
            HTTP_Errored.Example_InvalidResourcePath();
            HTTP_Errored.Example_ConnectionTimeout();
            HTTP_Errored.Example_BadHTTPHeaderItem();
            HTTP_Errored.Example_BadlyFormedHTTPHeaderString();
            HTTP_Errored.Example_BadlyFormedHTTPHeaderBySetting();
            HTTP_Errored.Example_RejectSSLCertificate();
            HTTP_Errored.Example_RejectSSLCertificateUsingCustomDelegate();
        }
    }
}


