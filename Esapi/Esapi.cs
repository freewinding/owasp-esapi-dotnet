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

using System.Threading;

using Owasp.Esapi.Configuration;
using Owasp.Esapi.Interfaces;

namespace Owasp.Esapi
{
    /// <summary>
    ///     This class provides accessor methods for the various ESAPI implementations.
    /// </summary>
    public static class Esapi
    {
        private static IAccessController _accessController;

        private static readonly object _accessControllerLock = new object();

        private static IEncoder _encoder;

        private static readonly object _encoderLock = new object();

        private static IEncryptor _encryptor;

        private static readonly object _encryptorLock = new object();

        private static IHttpUtilities _httpUtilities;

        private static readonly object _httpUtilitiesLock = new object();

        private static IIntrusionDetector _intrusionDetector;

        private static readonly object _instrusionDetectorLock = new object();

        private static IRandomizer _randomizer;

        private static readonly object _randomizerLock = new object();

        private static ISecurityConfiguration _securityConfiguration;

        private static readonly object _securityConfigurationLock = new object();

        private static IValidator _validator;

        private static readonly object _validatorLock = new object();

        /// <summary>
        ///     The IAccessController implementation.
        /// </summary>
        public static IAccessController AccessController
        {
            set
            {
                lock (_accessControllerLock)
                {
                    _accessController = value;
                }
            }

            get
            {
                if (_accessController == null)
                {
                    lock (_accessControllerLock)
                    {
                        if (_accessController == null)
                        {
                            Thread.MemoryBarrier();
                            _accessController = EsapiLoader.LoadAccessController(EsapiConfig.Instance.AccessController);
                        }
                    }
                }
                return _accessController;
            }
        }

        /// <summary>
        ///     The IEncoder implementation.
        /// </summary>
        public static IEncoder Encoder
        {
            set
            {
                lock (_encoderLock)
                {
                    _encoder = value;
                }
            }

            get
            {
                if (_encoder == null)
                {
                    lock (_encoderLock)
                    {
                        if (_encoder == null)
                        {
                            Thread.MemoryBarrier();
                            _encoder = EsapiLoader.LoadEncoder(EsapiConfig.Instance.Encoder);
                        }
                    }
                }
                return _encoder;
            }
        }

        /// <summary>
        ///     The IEncryptor implementation.
        /// </summary>
        public static IEncryptor Encryptor
        {
            set
            {
                lock (_encryptorLock)
                {
                    _encryptor = value;
                }
            }
            get
            {
                if (_encryptor == null)
                {
                    lock (_encryptorLock)
                    {
                        if (_encryptor == null)
                        {
                            Thread.MemoryBarrier();
                            _encryptor = EsapiLoader.LoadEncryptor(EsapiConfig.Instance.Encryptor);
                        }
                    }
                }
                return _encryptor;
            }
        }

        /// <summary>
        ///     The IHttpUtilties implementation.
        /// </summary>
        public static IHttpUtilities HttpUtilities
        {
            set
            {
                lock (_httpUtilitiesLock)
                {
                    _httpUtilities = value;
                }
            }
            get
            {
                if (_httpUtilities == null)
                {
                    lock (_httpUtilitiesLock)
                    {
                        if (_httpUtilities == null)
                        {
                            Thread.MemoryBarrier();
                            _httpUtilities = EsapiLoader.LoadHttpUtilities(EsapiConfig.Instance.HttpUtilities);
                        }
                    }
                }
                return _httpUtilities;
            }
        }

        /// <summary>
        ///     The IIntrusionDetector implementation.
        /// </summary>
        public static IIntrusionDetector IntrusionDetector
        {
            set
            {
                lock (_instrusionDetectorLock)
                {
                    _intrusionDetector = value;
                }
            }
            get
            {
                if (_intrusionDetector == null)
                {
                    lock (_instrusionDetectorLock)
                    {
                        if (_intrusionDetector == null)
                        {
                            Thread.MemoryBarrier();
                            _intrusionDetector =
                                EsapiLoader.LoadIntrusionDetector(EsapiConfig.Instance.IntrusionDetector);
                        }
                    }
                }
                return _intrusionDetector;
            }
        }

        /// <summary>
        ///     The IRandomizer implementation.
        /// </summary>
        public static IRandomizer Randomizer
        {
            set
            {
                lock (_randomizerLock)
                {
                    _randomizer = value;
                }
            }
            get
            {
                if (_randomizer == null)
                {
                    lock (_randomizerLock)
                    {
                        if (_randomizer == null)
                        {
                            Thread.MemoryBarrier();
                            _randomizer = EsapiLoader.LoadRandomizer(EsapiConfig.Instance.Randomizer);
                        }
                    }
                }
                return _randomizer;
            }
        }

        /// <summary>
        ///     The IValidator implementation.
        /// </summary>
        public static IValidator Validator
        {
            set
            {
                lock (_validatorLock)
                {
                    _validator = value;
                }
            }

            get
            {
                if (_validator == null)
                {
                    lock (_validatorLock)
                    {
                        if (_validator == null)
                        {
                            Thread.MemoryBarrier();
                            _validator = EsapiLoader.LoadValidator(EsapiConfig.Instance.Validator);
                        }
                    }
                }
                return _validator;
            }
        }

        /// <summary>
        ///     The ISecurityConfiguration implementation.
        /// </summary>
        public static ISecurityConfiguration SecurityConfiguration
        {
            set
            {
                lock (_securityConfigurationLock)
                {
                    _securityConfiguration = value;
                }
            }
            get
            {
                if (_securityConfiguration == null)
                {
                    lock (_securityConfigurationLock)
                    {
                        if (_securityConfiguration == null)
                        {
                            Thread.MemoryBarrier();
                            _securityConfiguration =
                                EsapiLoader.LoadSecurityConfiguration(EsapiConfig.Instance.SecurityConfiguration);
                        }
                    }
                }
                return _securityConfiguration;
            }
        }

        /// <summary>
        ///     The ILogger implementation.
        /// </summary>
        public static ILogger Logger
        {
            get
            {
                return GetLogger("Owasp.Esapi");
            }
        }

        /// <summary>
        ///     Gets a specific logger for a different class name.
        /// </summary>
        /// <param name="className">The class name to get the logger for.</param>
        /// <returns>The logger associated with the class name.</returns>
        public static ILogger GetLogger(string className)
        {
            return new Logger(className);
        }

        /// <summary>
        ///     Reset all cached instances
        /// </summary>
        internal static void Reset()
        {
            AccessController = null;
            Encoder = null;
            Encryptor = null;
            HttpUtilities = null;
            IntrusionDetector = null;
            Randomizer = null;
            SecurityConfiguration = null;
            Validator = null;
        }
    }
}
