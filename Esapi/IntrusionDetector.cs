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
using System.Diagnostics;
using System.Security.Principal;
using System.Web.Security;

using Owasp.Esapi.Errors;
using Owasp.Esapi.Interfaces;

using EM = Owasp.Esapi.Resources.Errors;

namespace Owasp.Esapi
{
    /// <inheritdoc cref="Owasp.Esapi.Interfaces.IIntrusionDetector" />
    /// <summary>
    ///     Reference implementation of the <see cref="Owasp.Esapi.Interfaces.IIntrusionDetector" /> interface.
    /// </summary>
    /// <remarks>
    ///     This implementation monitors EnterpriseSecurityExceptions to see if any user
    ///     exceeds a configurable threshold in a configurable time period. For example,
    ///     it can monitor to see if a user exceeds 10 input validation issues in a 1
    ///     minute period. Or if there are more than 3 authentication problems in a 10
    ///     second period. More complex implementations are certainly possible, such as
    ///     one that establishes a baseline of expected behavior, and then detects
    ///     deviations from that baseline.
    /// </remarks>
    public class IntrusionDetector : IIntrusionDetector
    {
        private static readonly Dictionary<string, Dictionary<string, Event>> users =
            new Dictionary<string, Dictionary<string, Event>>();

        private readonly ActionManager _actionManager;

        /// <summary>The logger. </summary>
        private readonly ILogger _logger;

        private readonly Dictionary<string, Threshold> _thresholds;

        /// <summary>
        ///     Public constructor.
        /// </summary>
        public IntrusionDetector()
        {
            this._thresholds = new Dictionary<string, Threshold>();
            this._logger = Esapi.Logger;
            this._actionManager = new ActionManager();
        }

        /// <summary>
        ///     Add event threshold
        /// </summary>
        /// <param name="threshold"></param>
        public void AddThreshold(Threshold threshold)
        {
            if (threshold == null)
            {
                throw new ArgumentNullException("threshold");
            }
            if (this._thresholds.ContainsKey(threshold.Event))
            {
                throw new ArgumentException();
            }

            // Validate all required actions have been registered already
            if (threshold.Actions != null)
            {
                foreach (string name in threshold.Actions)
                {
                    if (!this._actionManager.Contains(name))
                    {
                        string message = string.Format(EM.IntrusionDetector_ActionNotFound1, name);
                        throw new ArgumentException(message, "threshold");
                    }
                }
            }

            // Add threshold
            this._thresholds.Add(threshold.Event, threshold);
        }

        /// <summary>
        ///     Remove event threshold
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public bool RemoveThreshold(string eventName)
        {
            return this._thresholds.Remove(eventName);
        }

        // FIXME: ENHANCE consider allowing both per-user and per-application quotas
        // e.g. number of failed logins per hour is a per-application quota

        /// <summary>
        ///     This implementation uses an exception store in each User object to track
        ///     exceptions.
        /// </summary>
        /// <param name="e">
        ///     The exception to add.
        /// </param>
        /// <seealso cref="Owasp.Esapi.Interfaces.IIntrusionDetector.AddException(Exception)">
        /// </seealso>
        public void AddException(Exception e)
        {
            if (e is EnterpriseSecurityException)
            {
                this._logger.Warning(LogEventTypes.SECURITY, ((EnterpriseSecurityException)e).LogMessage, e);
            }
            else
            {
                this._logger.Warning(LogEventTypes.SECURITY, e.Message, e);
            }

            string eventName = e.GetType().FullName;

            if (e is IntrusionException)
            {
                return;
            }

            // add the exception to the user's store, handle IntrusionException if thrown
            try
            {
                this.AddSecurityEvent(eventName);
            }
            catch (IntrusionException ie)
            {
                this.OnIntrusionDetected(eventName, ie);
            }
        }

        /// <summary>
        ///     Adds the event to the IntrusionDetector.
        /// </summary>
        /// <param name="eventName">
        ///     The event to add.
        /// </param>
        /// <seealso cref="Owasp.Esapi.Interfaces.IIntrusionDetector.AddEvent(string)">
        /// </seealso>
        public virtual void AddEvent(string eventName)
        {
            this._logger.Warning(LogEventTypes.SECURITY, string.Format(EM.InstrusionDetector_EventReceived1, eventName));

            // add the event to the current user, which may trigger a detector 
            try
            {
                this.AddSecurityEvent(eventName);
            }
            catch (IntrusionException ie)
            {
                this.OnIntrusionDetected(eventName, ie);
            }
        }

        /// <summary>
        ///     Get event threshold
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private Threshold GetEventThreshold(string eventName)
        {
            Threshold threshold;
            this._thresholds.TryGetValue(eventName, out threshold);

            // Event not found, create default
            if (threshold == null)
            {
                threshold = new Threshold(eventName, 0, 0, null);
            }

            return threshold;
        }

        /// <summary>
        ///     Instrusion was detected
        /// </summary>
        /// <param name="eventName"></param>
        private void OnIntrusionDetected(string eventName, IntrusionException e)
        {
            Debug.Assert(e != null);

            Threshold quota = this.GetEventThreshold(eventName);
            if (quota == null)
            {
                throw new ArgumentException(EM.IntrusionDetector_UnknownEventName, "eventName");
            }

            // Take actions
            foreach (string action in quota.Actions)
            {
                // Log action execution
                string message = string.Format(
                    EM.InstrusionDetector_ExceededQuota4,
                    quota.MaxOccurences,
                    quota.MaxTimeSpan,
                    eventName,
                    action);
                this._logger.Fatal(LogEventTypes.SECURITY, "INTRUSION - " + message);

                this._actionManager.Execute(action, e);
            }
        }

        /// <summary>
        ///     Adds a security event to the user.
        /// </summary>
        /// <param name="eventName">
        ///     The security event to add.
        /// </param>
        private void AddSecurityEvent(string eventName)
        {
            IPrincipal currentUser = Esapi.SecurityConfiguration.CurrentUser;
            string username = (currentUser != null && currentUser.Identity != null
                                   ? currentUser.Identity.Name
                                   : "Anonymous");

            // Get user events
            Dictionary<string, Event> events;
            if (!users.TryGetValue(username, out events))
            {
                events = new Dictionary<string, Event>();
                users[username] = events;
            }

            // Get user security event
            Event securityEvent;
            if (!events.TryGetValue(eventName, out securityEvent))
            {
                securityEvent = new Event(eventName);
                events[eventName] = securityEvent;
            }

            Threshold q = this.GetEventThreshold(eventName);
            Debug.Assert(q != null);

            if (q.MaxOccurences > 0)
            {
                securityEvent.Increment(q.MaxOccurences, q.MaxTimeSpan);
            }
        }

        /// <summary>
        ///     Helper class to manage intrusion detection actions
        /// </summary>
        private class ActionManager
        {
            private readonly List<string> _actions = new List<string> { "log", "disable", "logout" };

            /// <summary>
            ///     Verify if the action is known
            /// </summary>
            /// <param name="action">Action name</param>
            /// <returns></returns>
            internal bool Contains(string action)
            {
                return this._actions.Contains(action);
            }

            /// <summary>
            ///     Execute action
            /// </summary>
            /// <param name="action">Action name</param>
            /// <param name="ie">Intrusion exception</param>
            /// <remarks>IntrusionException will be thrown if action not known</remarks>
            internal void Execute(string action, IntrusionException ie)
            {
                Debug.Assert(ie != null);

                if (0 == string.Compare(action, "log", true))
                {
                    Esapi.Logger.Fatal(LogEventTypes.SECURITY, ie.LogMessage);
                }
                else if (0 == string.Compare(action, "disable", true))
                {
                    MembershipUser user = Membership.GetUser();
                    if (user != null)
                    {
                        user.IsApproved = false;
                        Membership.UpdateUser(user);
                    }
                }
                else if (0 == string.Compare(action, "logout", true))
                {
                    FormsAuthentication.SignOut();
                }
                else
                {
                    throw ie;
                }
            }
        }
    }
}
