

namespace Examples
{
    using System;
    using System.Net.Security;
    using System.Collections.Generic;
    using System.Text;
    using TeamControlium.NonGUI;
    using TeamControlium.Utilities;
    using System.Security.Cryptography.X509Certificates;

    static class HTTP_Errored
    {
        private static List<string> logOutput = new List<string>();

        public static void Example_NonExistantDomain()
        {
            // Set a non-existant domain.
            string url = "non.existant.domain/post";

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            // Setup the HTTP Header items.  Header should be well-formed so we use the HTTPBased.ItemList to set the items as that will be well-formed.
            // NOTE.  We have NOT set the Connection: close" and so 
            HTTPBased.ItemList headerItems = new HTTPBased.ItemList();
            headerItems.Add("accept", "*/*");
            headerItems.Add("host", httpRequest.Domain);
            headerItems.Add("accept-encoding", "identity");
            headerItems.Add("connection", "close");
            httpRequest.SetHeaderStringFromItemList(headerItems);

            // Perform HTTP Post.  Use TryHttpPOST as we are expecting an error.  In a Test script, using the Try... would be preferential anyway incase a defect is touched and
            // the POST fails badly.  If an HTTP POST endpoint ouside the test domain is being used (IE. A Mock) then standard HttpPOST could be used.
            HTTPBased.ItemList response;
            bool result = httpRequest.TryHttpPOST(out response);

            OutputDetails("Example of an HTTP POST to a non-existant domain", result, httpRequest, response,
                $"Domain {httpRequest.Domain} does not exist so we expect this to fail.",
                "An exception is thrown as the TCP layer cannot resolve the URI.");


        }

        public static void Example_InvalidResourcePath()
        {
            // Set a non-existant domain.  postman-echo.com exists but not the path /not/here
            string url = "postman-echo.com/not/here"; ;

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            // Setup the HTTP Header items.  Header should be well-formed so we use the HTTPBased.ItemList to set the items as that will be well-formed.
            // NOTE.  We have NOT set the Connection: close" and so 
            HTTPBased.ItemList headerItems = new HTTPBased.ItemList();
            headerItems.Add("accept", "*/*");
            headerItems.Add("host", httpRequest.Domain);
            headerItems.Add("accept-encoding", "identity");
            headerItems.Add("connection", "close");
            httpRequest.SetHeaderStringFromItemList(headerItems);

            // Perform HTTP Post.  Use TryHttpPOST as we are expecting an error.  In a Test script, using the Try... would be preferential anyway incase a defect is touched and
            // the POST fails badly.  If an HTTP POST endpoint ouside the test domain is being used (IE. A Mock) then standard HttpPOST could be used.
            HTTPBased.ItemList response;
            bool result = httpRequest.TryHttpPOST(out response);

            OutputDetails("Example of an invalid resource path", result, httpRequest, response,
                "We try to access a non-existant resource and so the HTTP server responds with a 404 error");
        }

        public static void Example_ConnectionTimeout()
        {
            // We use the Postman example website for this as the server is well setup for examples.
            string url = "postman-echo.com/post";

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            // Setup the HTTP Header items.  Header should be well-formed so we use the HTTPBased.ItemList to set the items as that will be well-formed.
            // NOTE.  We have NOT set the Connection: close" and so 
            HTTPBased.ItemList headerItems = new HTTPBased.ItemList();
            headerItems.Add("accept", "*/*");
            headerItems.Add("host", httpRequest.Domain);
            headerItems.Add("accept-encoding", "identity");
            //headerItems.Add("connection", "close");              // Unconnect this line to see the POST succeed
            httpRequest.SetHeaderStringFromItemList(headerItems);

            // Perform HTTP Post.  Use TryHttpPOST as we are expecting an error.  In a Test script, using the Try... would be preferential anyway incase a defect is touched and
            // the POST fails badly.  If an HTTP POST endpoint ouside the test domain is being used (IE. A Mock) then standard HttpPOST could be used.
            HTTPBased.ItemList response;
            bool result = httpRequest.TryHttpPOST(out response);

            OutputDetails("Example of a Connection Timeout occuring on an HTTP Post", result, httpRequest, response,
                "We have not added a 'Connection: close' header item and so HTTP default is 'keep-alive.",
                "As HTTPBased does not handle Async connections a timeout occures");
        }

        public static void Example_BadHTTPHeaderItem()
        {
            // We use the Postman example website for this as the server is well setup for examples.
            string url = "postman-echo.com/post";

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            // Setup the HTTP Header items.  Header should be well-formed so we use the HTTPBased.ItemList to set the items as that will be well-formed.
            // NOTE.  We have left out the Host item.  This is mandatory in HTTP and so we will get a fail.
            HTTPBased.ItemList headerItems = new HTTPBased.ItemList();
            headerItems.Add("accept", "*/*");
            //headerItems.Add("host", httpRequest.Domain);
            headerItems.Add("accept-encoding", "identity");
            headerItems.Add("connection", "close");
            httpRequest.SetHeaderStringFromItemList(headerItems);

            // Perform HTTP Post.  Use TryHttpPOST as we are expecting an error.  In a Test script, using the Try... would be preferential anyway incase a defect is touched and
            // the POST fails badly.  If an HTTP POST endpoint ouside the test domain is being used (IE. A Mock) then standard HttpPOST could be used.
            HTTPBased.ItemList response;
            bool result = httpRequest.TryHttpPOST(out response);

            OutputDetails("Example of a bad HTTP Header in an HTTP POST", result, httpRequest,response,
                "There is no 'host' item, which is mandatory in an HTTP request",
                "And so we see the request succeeds but the HTTP Response is a 400 (Bad Request)");
        }

        public static void Example_BadlyFormedHTTPHeaderString()
        {
             // We use the Postman example website for this as the server is well setup for examples.
            string url = "postman-echo.com/post";

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            // Setup the HTTP Header items.  We use a string so that we can deliberatly make it badley formed.  Note the missing colon (:) between 'accept-encoding' and 'identity'.
            string httpHeader = $"accept: */*\r\nhost: {httpRequest.Domain}\r\naccept-encoding identity\r\nconnection: close\r\n";
            httpRequest.HeaderString = httpHeader;

            // Perform HTTP Post.  Use TryHttpPOST as we are expecting an error.  In a Test script, using the Try... would be preferential anyway incase a defect is touched and
            // the POST fails badly.  If an HTTP POST endpoint ouside the test domain is being used (IE. A Mock) then standard HttpPOST could be used.
            HTTPBased.ItemList response;
            bool result = httpRequest.TryHttpPOST(out response);

            OutputDetails("Example of a badly formed HTTP Header in an HTTP POST", result, httpRequest, response,
                "There is a colon missing between the accept-encoding key and value",
                "And so we see the request succeeds but the HTTP Response is a 400 (Bad Request)");
        }

        public static void Example_BadlyFormedHTTPHeaderBySetting()
        {
            // We use the Postman example website for this as the server is well setup for examples.
            string url = "postman-echo.com/post";

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Tell NonGUI test library to use a ';' between HTTP Header item keys and values
            Repository.ItemLocal["TeamControlium.NonGUI", "HTTPHeader_ItemDelimiter"] = ";";

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            // Setup the HTTP Header items.  Header should be well-formed so we use the HTTPBased.ItemList to set the items as that will be well-formed.
            // NOTE.  We have left out the Host item.  This is mandatory in HTTP and so we will get a fail.
            HTTPBased.ItemList headerItems = new HTTPBased.ItemList();
            headerItems.Add("accept", "*/*");
            headerItems.Add("host", httpRequest.Domain);
            headerItems.Add("accept-encoding", "identity");
            headerItems.Add("connection", "close");
            httpRequest.SetHeaderStringFromItemList(headerItems);

            // Perform HTTP Post.  Use TryHttpPOST as we are expecting an error.  In a Test script, using the Try... would be preferential anyway incase a defect is touched and
            // the POST fails badly.  If an HTTP POST endpoint ouside the test domain is being used (IE. A Mock) then standard HttpPOST could be used.
            HTTPBased.ItemList response;
            bool result = httpRequest.TryHttpPOST(out response);

            OutputDetails("Example of a badly formed HTTP Header in an HTTP POST", result, httpRequest, response,
                "We set local Repository item HTTPHeader_ItemDelimiter (In the TeamControlium.NonGUI",
                "settings category) to a semi-colon (;). And so as the header is now malformed we see",
                "the request succeeds but the HTTP Response is a 400 (Bad Request)");
        }

        public static void Example_RejectSSLCertificate()
        {
            // We use the Postman example website for this as the server is well setup for examples.
            string url = "postman-echo.com/post";

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Tell NonGUI test library to reject the Server Certificate in an SSL (HTTPS) call
            Repository.ItemLocal["TeamControlium.NonGUI", "SSL_AcceptServerCertificate"] = false;  // Set to true to prove that the example would work if true.

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            // Setup the HTTP Header items.  Header should be well-formed so we use the HTTPBased.ItemList to set the items as that will be well-formed.
            // NOTE.  We have NOT set the Connection: close" and so 
            HTTPBased.ItemList headerItems = new HTTPBased.ItemList();
            headerItems.Add("accept", "*/*");
            headerItems.Add("host", httpRequest.Domain);
            headerItems.Add("accept-encoding", "identity");
            headerItems.Add("connection", "close");
            httpRequest.SetHeaderStringFromItemList(headerItems);

            httpRequest.UseSSL = true;

            // Perform HTTP Post.  Use TryHttpPOST as we are expecting an error.  In a Test script, using the Try... would be preferential anyway incase a defect is touched and
            // the POST fails badly.  If an HTTP POST endpoint ouside the test domain is being used (IE. A Mock) then standard HttpPOST could be used.
            HTTPBased.ItemList response;
            bool result = httpRequest.TryHttpPOST(out response);

            OutputDetails("Example of forcing an HTTPS request to fail due to rejected certificate", result, httpRequest, response,
                "We have set UseSSL to true (which tells NonGUI to use HTTPS rather tha HTTP.  But we have also set the",
                "repository item SSL_AcceptServerCertificate (Is category TeamControlium.NonGUI settings) to false. This",
                "forces NonGUI to reject the server certificate and so causes the exception.");
        }

        public static void Example_RejectSSLCertificateUsingCustomDelegate()
        {
            // We use the Postman example website for this as the server is well setup for examples.
            string url = "postman-echo.com/post";

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            // Setup the HTTP Header items.  Header should be well-formed so we use the HTTPBased.ItemList to set the items as that will be well-formed.
            // NOTE.  We have NOT set the Connection: close" and so 
            HTTPBased.ItemList headerItems = new HTTPBased.ItemList();
            headerItems.Add("accept", "*/*");
            headerItems.Add("host", httpRequest.Domain);
            headerItems.Add("accept-encoding", "identity");
            headerItems.Add("connection", "close");
            httpRequest.SetHeaderStringFromItemList(headerItems);

            httpRequest.UseSSL = true;
            httpRequest.CertificateValidationCallback = RejectHTTPSCertificate; // Comment out this line to see that the Server certificate gets accepted.

            // Perform HTTP Post.  Use TryHttpPOST as we are expecting an error.  In a Test script, using the Try... would be preferential anyway incase a defect is touched and
            // the POST fails badly.  If an HTTP POST endpoint ouside the test domain is being used (IE. A Mock) then standard HttpPOST could be used.
            HTTPBased.ItemList response;
            bool result = httpRequest.TryHttpPOST(out response);

            OutputDetails("Example of using the SSL Certificate validation override to reject an SSL certificate", result, httpRequest, response,
                "We have set UseSSL to true (which tells NonGUI to use HTTPS rather tha HTTP) and we have set",
                "CertificateValidationCallback to our own certificate validation method (RejectHTTPSCertificate) which",
                "forces the certificate to be rejected; hence the exception.  In testing, we could use the callback",
                "to actually inspect and verify the certificate as part of testing if we so wished.");
        }

        private static void LogDelegate(string logLine)
        {
            logOutput.Add(logLine);
        }

        private static bool RejectHTTPSCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return false;
        }

        private static void OutputDetails(string heading, bool result, HTTPBased httpRequest, HTTPBased.ItemList response, params string[] details)
        {
            // We are expecting this to fail, so get the Exception message.
            string exceptionMessage = httpRequest.TryException?.Message ?? "No Exception was thrown!";

            // Get the response body if there is one...
            string body = response?["Body"] ?? "Body not Set";

            // Finally, give the details of the call.
            Console.WriteLine(heading);
            if (details.Length > 0)
            {
                Console.WriteLine(" - ");
            }
            foreach (string detailItem in details)
            {
                Console.WriteLine(" - " + detailItem);
            }
            Console.WriteLine(" - ");
            Console.WriteLine(" - REQUEST:-");
            Console.WriteLine($" - HTTP POST to http{(httpRequest.UseSSL?"s":string.Empty)}://" + httpRequest.Domain + "/" + httpRequest.ResourcePath);
            Console.WriteLine(" - HTTP Header - " + httpRequest.HeaderString.Replace("\r\n", "\\r\\n"));
            Console.WriteLine(" - ");
            Console.WriteLine(" - RESPONSE:-");
            Console.WriteLine(" - Request " + (result ? "Succeeded" : "Failed.  An exception will have been thrown"));
            Console.WriteLine(" - " + (response == null ? "There was no response!" : $"Response was {response["StatusCode"] ?? "StatusCode Not set"} ({response["StatusText"] ?? "StatusText Not set"}) and body was {(string.IsNullOrEmpty(body) ? "Empty" : body)}"));
            Console.WriteLine(" - Exception message: " + exceptionMessage);
            Console.WriteLine(" - \r\n============================================================================================");
        }

    }
}
