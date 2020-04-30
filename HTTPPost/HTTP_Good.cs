namespace Examples
{
    using System;
    using System.Net.Security;
    using System.Collections.Generic;
    using System.Text;
    using TeamControlium.NonGUI;
    using TeamControlium.Utilities;
    using System.Security.Cryptography.X509Certificates;

    class HTTP_Good
    {
        private static List<string> logOutput = new List<string>();
        private static string serverCertificate = string.Empty;

        public static void Example_SimplePostUsingProperties()
        {
            // Set an existant domain and resource path.
            string url = "postman-echo.com/post";

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            // So this loads httpRequest.Domain with postman-echo.com
            // and httpRequest.ResourcePath with /post
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.HTTPMethod = HTTPBased.HTTPMethods.Post;
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            // Setup the HTTP Header items.  We could use a list and call SetHeaderStringFromItemList.  But for sake of example we will build the string
            // ourselves.
            httpRequest.HeaderString = $"content-type: text/plain\r\naccept: */*\r\nhost: {httpRequest.Domain}\r\naccept-encoding: identity\r\nconnection: close\r\n";

            httpRequest.Body = "Hello";

            // Perform HTTP Post.  Use TryHttpPOST as we are expecting an error.  In a Test script, using the Try... would be preferential anyway incase a defect is touched and
            // the POST fails badly.  If an HTTP POST endpoint ouside the test domain is being used (IE. A Mock) then standard HttpPOST could be used.

            try
            {
                HTTPBased.ItemList response = httpRequest.Http();

                OutputDetails("Example of a simple HTTP POST", httpRequest, response,
                    $"Domain {httpRequest.Domain} exists and just echo's the post request so we",
                    "expect the response to contain our requests body in the JSON 'data' field","",
                    $"Note how the header line 'Content-Length: {httpRequest.Body.Length}' has been",
                    "automatically added to the Request header by HTTPBased.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public static void Example_SimplePostUsingParameters()
        {
            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();
            HTTPBased httpRequest = new HTTPBased();

            try
            {
                HTTPBased.ItemList response = httpRequest.HttpPOST("postman-echo.com", "/post", null, "content-type: text/plain\r\naccept: */*\r\nhost: postman-echo.com\r\naccept-encoding: identity\r\nconnection: close\r\n", "Hello");

                OutputDetails("Example of a simple HTTP POST", httpRequest, response,
                    $"Domain postman-echo.com exists and just echo's the post request in the JSON. So we",
                    "expect the response to contain our requests body in the JSON 'data' field", "",
                    $"Note how the header line 'Content-Length: 5' has been automatically added in the.",
                    "request header.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public static void Example_SimpleGetUsingProperties()
        {
            // Set an existant domain and resource path.
            string url = "postman-echo.com/get";

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            // So this loads httpRequest.Domain with postman-echo.com
            // and httpRequest.ResourcePath with /post
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.HTTPMethod = HTTPBased.HTTPMethods.Get;
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            HTTPBased.ItemList queryArguments = new HTTPBased.ItemList();
            queryArguments.Add("foo", "bar");
            queryArguments.Add("alpha", "7");
            httpRequest.SetQueryStringFromItemList(queryArguments);

            // Setup the HTTP Header items.  We could use a list and call SetHeaderStringFromItemList.  But for sake of example we will build the string
            // ourselves.
            httpRequest.HeaderString = $"content-type: text/plain\r\naccept: */*\r\nhost: {httpRequest.Domain}\r\naccept-encoding: identity\r\nconnection: close\r\n";


            // Perform HTTP Post.  Use TryHttpPOST as we are expecting an error.  In a Test script, using the Try... would be preferential anyway incase a defect is touched and
            // the POST fails badly.  If an HTTP POST endpoint ouside the test domain is being used (IE. A Mock) then standard HttpPOST could be used.

            try
            {
                HTTPBased.ItemList response = httpRequest.Http();

                OutputDetails("Example of a simple HTTP GET", httpRequest, response,
                    $"Domain {httpRequest.Domain} exists and just echo's the get request. So we",
                    "expect the response to contain our requests arguments (foo=bar and alpha=7) in the",
                    "JSON 'args' field", "",
                    $"Note how the header line 'Content-Length: {httpRequest.Body.Length}' has been",
                    "automatically added to the Request header by HTTPBased.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public static void Example_SimpleGetUsingParameters()
        {
            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();
            HTTPBased httpRequest = new HTTPBased();

            try
            {
                HTTPBased.ItemList response = httpRequest.HttpGET("postman-echo.com", "/get", "foo=bar&alpha=7", "content-type: text/plain\r\naccept: */*\r\nhost: postman-echo.com\r\naccept-encoding: identity\r\nconnection: close\r\n");

                OutputDetails("Example of a simple HTTP GET", httpRequest, response,
                    $"Domain postman-echo.com exists and just echo's the get request in the JSON.  So we",
                    "expect the response to contain our requests arguments (foo=bar and alpha=7) in the",
                    "JSON 'args' field", "",
                    $"Note how the header line 'Content-Length: 0' has been automatically added in the.",
                    "request header.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public static void Example_HTTPSPostAutoAcceptSSLCertificate()
        {
            // We use the Postman example website for this as the server is well setup for examples.
            string url = "postman-echo.com/post";

            // To prevent Log from output messages to Console we catch them and tell Log to not output to console.
            Log.LogOutputDelegate = LogDelegate;
            Log.LogToConsole = false;

            // Ensure Repository has been cleared - incase any examples have changed anything...
            Repository.ClearRepositoryAll();

            // Tell NonGUI test library to accept the Server Certificate in an SSL (HTTPS) call
            Repository.ItemLocal["TeamControlium.NonGUI", "SSL_AcceptServerCertificate"] = true;

            // Instantiate HTTPBased and set the domain and resoure path of the URL.
            HTTPBased httpRequest = new HTTPBased();
            httpRequest.HTTPMethod = HTTPBased.HTTPMethods.Post;
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];

            // Setup the HTTP Header items.  Header should be well-formed so we use the HTTPBased.ItemList to set the items as that will be well-formed.
            // NOTE.  We have NOT set the Connection: close" and so 
            HTTPBased.ItemList headerItems = new HTTPBased.ItemList();
            headerItems.Add("accept", "*/*");
            headerItems.Add("content-type", "text/plain");
            headerItems.Add("host", httpRequest.Domain);
            headerItems.Add("accept-encoding", "identity");
            headerItems.Add("connection", "close");
            httpRequest.SetHeaderStringFromItemList(headerItems);

            httpRequest.UseSSL = true;

            httpRequest.Body = "Hello";

            try
            {
                HTTPBased.ItemList response = httpRequest.Http();

                OutputDetails("Example of a simple HTTP POST", httpRequest, response,
                    $"Domain {httpRequest.Domain} exists and just echo's the post request so we",
                    "expect the response to contain our requests body in the JSON 'data' field", "",
                    $"Note how the header line 'Content-Length: {httpRequest.Body.Length}' has been",
                    "automatically added to the Request header by HTTPBased.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public static void Example_AcceptSSLCertificateUsingCustomDelegate()
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
            httpRequest.HTTPMethod = HTTPBased.HTTPMethods.Post;
            httpRequest.Domain = url.Split('/')[0];
            httpRequest.ResourcePath = "/" + url.Split('/')[1];
            httpRequest.CertificateValidationCallback = AcceptHTTPSCertificate;  // This tells HTTPBased to use the AcceptHTTPSCertificate method to make certificate acceptance decision.

            // Setup the HTTP Header items.  Header should be well-formed so we use the HTTPBased.ItemList to set the items as that will be well-formed.
            // NOTE.  We have NOT set the Connection: close" and so 
            HTTPBased.ItemList headerItems = new HTTPBased.ItemList();
            headerItems.Add("accept", "*/*");
            headerItems.Add("content-type", "text/plain");
            headerItems.Add("host", httpRequest.Domain);
            headerItems.Add("accept-encoding", "identity");
            headerItems.Add("connection", "close");
            httpRequest.SetHeaderStringFromItemList(headerItems);

            httpRequest.UseSSL = true;

            httpRequest.Body = "Hello";

            try
            {
                HTTPBased.ItemList response = httpRequest.Http();

                OutputDetails("Example of a simple HTTP POST", httpRequest, response,
                    "Property .UseSSL is set true and .CertificateValidationCallback is set",
                    "to 'AcceptHTTPSCertificate' method.  Method receives callback with",
                    "certificate details:-",
                    $"{serverCertificate})",
                    "which we just accept in this case.  A test may want to validate the certificate",
                    "or just log the details etc...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private static void LogDelegate(string logLine)
        {
            logOutput.Add(logLine);
        }

        private static bool AcceptHTTPSCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            serverCertificate = certificate.Subject + $" (Issued by: {certificate.Issuer}, Expires: {certificate.GetExpirationDateString()}) etc..." ;
            return true;
        }

        private static void OutputDetails(string heading, HTTPBased httpRequest, HTTPBased.ItemList response, params string[] details)
        {
            // Get the response body
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
            Console.WriteLine($" - HTTP POST to " + httpRequest.ActualURL);
            Console.Write(" - HTTP Payload - ");
            foreach(string payLoadLine in httpRequest.ActualPayload.Split("\r\n"))
            {
                Console.Write(payLoadLine + "\r\n -                ");
            }
            Console.WriteLine("");
            Console.WriteLine(" - RESPONSE:- ");
            Console.Write(" -   ");
            foreach (string payLoadLine in httpRequest.ActualRawResponse.Split("\r\n"))
            {
                Console.Write(payLoadLine + "\r\n -   ");
            }
        }

    }
}
