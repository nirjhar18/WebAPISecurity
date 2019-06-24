using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

//1. Create a new class for your filter that inherits from System.Web.Http.Filters.ActionFilterAttribute
//2. Check for an authorization header
//3. Get the user id and password from the header
//4. Perform your authentication check
//5. Return a 401 if not authorized

//In your controller class decorate the class with the Authentication class name you created to perform the authentication. In
//this case decorate with [AuthenticationFilter].

namespace WebApiSecurity
{
    public class AuthenticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //Check if header exists
            if(actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                //Base 64 Decoding
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                string userName = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                string userPassword = decodedToken.Substring(decodedToken.IndexOf(":")+1);

                //Add your Logic to check for username and password from database
                if(userName == "Admin" && userPassword == "Password")
                {
                    //Authorize - you do not have to do anything
                }
                else
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }

            }

        }

    }
}
