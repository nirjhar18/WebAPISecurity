using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


//A message handler is a class that receives an HTTP request and returns an HTTP response. Message handlers derive from the 
//abstract HttpMessageHandler class.Typically, a series of message handlers are chained together. The first handler receives 
//an HTTP request, does some processing, and gives the request to the next handler. At some point, the response is created and
//goes back up the chain. This pattern is called a delegating handler.




namespace WebApiSecurity.MessageHandlers
{
    public class ApiKeyMessageHandler : DelegatingHandler
    {
        private const string APIKeyToCheck = "abc1234";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool validKey = false;
            IEnumerable<string> requestHeaders;
            var checkApiExists = httpRequestMessage.Headers.TryGetValues("APIKey", out requestHeaders);
            if (checkApiExists)
            {
                if (requestHeaders.FirstOrDefault().Equals(APIKeyToCheck))
                {
                    validKey = true;
                }
            }
            if (!validKey)
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden, "Invalid Key");

            }

            //Response will go upto the chain to go to controller
            var response = await base.SendAsync(httpRequestMessage, cancellationToken);
            return response;    
        }
    }
}