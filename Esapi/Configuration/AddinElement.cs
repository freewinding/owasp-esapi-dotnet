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

using System.Configuration;

namespace Owasp.Esapi.Configuration
{
    /// <summary>
    ///     The AddinElement Configuration Element.
    /// </summary>
    public class AddinElement : ConfigurationElement
    {
        #region Name Property

        /// <summary>
        ///     The XML name of the <see cref="Name" /> property.
        /// </summary>
        internal const string NamePropertyName = "name";

        /// <summary>
        ///     Gets or sets the Name.
        /// </summary>
        [ConfigurationProperty(NamePropertyName, IsRequired = true, IsKey = true, IsDefaultCollection = false)]
        public string Name
        {
            get
            {
                return (string)base[NamePropertyName];
            }
            set
            {
                base[NamePropertyName] = value;
            }
        }

        #endregion

        #region Type Property

        /// <summary>
        ///     The XML name of the <see cref="Type" /> property.
        /// </summary>
        internal const string TypePropertyName = "type";

        /// <summary>
        ///     Gets or sets the Type.
        /// </summary>
        [ConfigurationProperty(TypePropertyName, IsRequired = true, IsKey = false, IsDefaultCollection = false)]
        public string Type
        {
            get
            {
                return (string)base[TypePropertyName];
            }
            set
            {
                base[TypePropertyName] = value;
            }
        }

        #endregion

        #region properties Property

        /// <summary>
        ///     The XML name of the properties property.
        /// </summary>
        internal const string PropertiesPropertyName = "properties";

        /// <summary>
        ///     Gets or sets the properties.
        /// </summary>
        [ConfigurationProperty(PropertiesPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false)]
        public KeyValueConfigurationCollection PropertyValues
        {
            get
            {
                return (KeyValueConfigurationCollection)base[PropertiesPropertyName];
            }
            set
            {
                base[PropertiesPropertyName] = value;
            }
        }

        #endregion
    }
}
