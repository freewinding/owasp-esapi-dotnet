﻿// Copyright© 2015 OWASP.org. 
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;

using Owasp.Esapi.Errors;
using Owasp.Esapi.Interfaces;

using EM = Owasp.Esapi.Resources.Errors;

namespace Owasp.Esapi
{
    /// <inheritdoc cref="Owasp.Esapi.Interfaces.IHttpUtilities" />
    /// <summary>
    ///     Reference implementation for the <see cref="Owasp.Esapi.Interfaces.IHttpUtilities" /> class.
    /// </summary>
    public class HttpUtilities : IHttpUtilities
    {
        /// <summary>
        ///     The name to use for the CSRF token.
        /// </summary>
        public const string CSRF_TOKEN_NAME = "CsrfToken";

        #region IHttpUtilities Members

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IHttpUtilities.AddCsrfToken()" />
        public void AddCsrfToken()
        {
            HttpContext context = HttpContext.Current;
            ((Page)context.CurrentHandler).ViewStateUserKey = context.Session.SessionID;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IHttpUtilities.AddCsrfToken(string)" />
        public string AddCsrfToken(string href)
        {
            string csrfToken = (string)HttpContext.Current.Session[CSRF_TOKEN_NAME];
            if (csrfToken == null)
            {
                csrfToken = Esapi.Randomizer.GetRandomString(8, CharSetValues.Alphanumerics);
                HttpContext.Current.Session[CSRF_TOKEN_NAME] = csrfToken;
            }
            string token = CSRF_TOKEN_NAME + "=" + csrfToken;
            return href.IndexOf('?') != -1 ? href + "&" + token : href + "?" + token;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IHttpUtilities.VerifyCsrfToken()" />
        public void VerifyCsrfToken()
        {
            string csrfToken = (string)HttpContext.Current.Session[CSRF_TOKEN_NAME];
            string receivedCsrfToken = HttpContext.Current.Request.QueryString[CSRF_TOKEN_NAME];

            // We have a previously set token and the value does not match
            if (csrfToken != null && !csrfToken.Equals(receivedCsrfToken))
            {
                throw new IntrusionException(EM.HttpUtilities_AuthFailed, EM.HttpUtilities_CsrfCheckFailed);
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IHttpUtilities.ChangeSessionIdentifier()" />
        public void ChangeSessionIdentifier()
        {
            SessionIDManager manager = new SessionIDManager();
            string newSessionId = manager.CreateSessionID(HttpContext.Current);
            bool redirected = false;
            bool IsAdded = false;
            manager.SaveSessionID(HttpContext.Current, newSessionId, out redirected, out IsAdded);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IHttpUtilities.AddNoCacheHeaders()" />
        public void AddNoCacheHeaders()
        {
            HttpResponse response = HttpContext.Current.Response;
            // HTTP 1.1
            response.AddHeader("Cache-Control", "no-store, no-cache, must-revalidate");
            // HTTP 1.0
            response.AddHeader("Pragma", "no-cache");
            response.Expires = -1;
        }

        /// <inheriteddoc
        ///     cref="Owasp.Esapi.Interfaces.IHttpUtilities.LogHttpRequest(HttpRequest, ILogger, ICollection&lt;string&gt;)" />
        public void LogHttpRequest(HttpRequest request, ILogger logger, ICollection<string> obfuscatedParams)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            StringBuilder requestData = new StringBuilder();

            // Write request data
            using (TextWriter dataWriter = new StringWriter(requestData))
            {
                HttpRequestWriter writer = new HttpRequestWriter(dataWriter);
                writer.Write(request, Esapi.SecurityConfiguration.CurrentUser, obfuscatedParams, true);
            }

            // Log 
            logger.Info(LogEventTypes.FUNCTIONALITY, requestData.ToString());
        }

        /// <summary>
        ///     Assert request is secure (over SSL and using POST)
        /// </summary>
        /// <param name="request">Request to test</param>
        public void AssertSecureRequest(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!request.IsSecureConnection)
            {
                throw new AccessControlException(EM.HttpUtilities_InsecureRequest, EM.HttpUtilities_InsecureProtocol);
            }
            if (string.Compare(request.HttpMethod, "POST", true) != 0)
            {
                throw new AccessControlException(EM.HttpUtilities_InsecureRequest, EM.HttpUtilities_InsecureMethod);
            }
        }

        #endregion
    }
}
