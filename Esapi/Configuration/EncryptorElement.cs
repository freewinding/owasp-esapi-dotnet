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
    ///     The EncryptorElement Configuration Element.
    /// </summary>
    public class EncryptorElement : ConfigurationElement
    {
        #region Type Property

        /// <summary>
        ///     The XML name of the <see cref="Type" /> property.
        /// </summary>
        internal const string TypePropertyName = "type";

        /// <summary>
        ///     Gets or sets the Type.
        /// </summary>
        [ConfigurationProperty(TypePropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false)]
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
    }

    /// <summary>
    ///     A collection of CodecElement instances.
    /// </summary>
    [ConfigurationCollection(typeof(CodecElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMapAlternate)]
    public class CodecCollection : ConfigurationElementCollection
    {
        #region Constants

        /// <summary>
        ///     The XML name of the individual <see cref="CodecElement" /> instances in this collection.
        /// </summary>
        internal const string CodecElementPropertyName = "codec";

        #endregion

        #region Indexer

        /// <summary>
        ///     Gets the <see cref="CodecElement" /> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="CodecElement" /> to retrieve</param>
        public CodecElement this[int index]
        {
            get
            {
                return (CodecElement)this.BaseGet(index);
            }
        }

        #endregion

        #region Add

        /// <summary>
        ///     Adds the specified <see cref="CodecElement" />.
        /// </summary>
        /// <param name="codec">The <see cref="CodecElement" /> to add.</param>
        public void Add(CodecElement codec)
        {
            this.BaseAdd(codec);
        }

        #endregion

        #region Remove

        /// <summary>
        ///     Removes the specified <see cref="CodecElement" />.
        /// </summary>
        /// <param name="codec">The <see cref="CodecElement" /> to remove.</param>
        public void Remove(CodecElement codec)
        {
            this.BaseRemove(codec);
        }

        #endregion

        #region Overrides

        /// <summary>
        ///     Gets the type of the <see cref="ConfigurationElementCollection" />.
        /// </summary>
        /// <returns>The <see cref="ConfigurationElementCollectionType" /> of this collection.</returns>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMapAlternate;
            }
        }

        /// <summary>
        ///     Indicates whether the specified <see cref="ConfigurationElement" /> exists in the
        ///     <see cref="ConfigurationElementCollection" />.
        /// </summary>
        /// <param name="elementName">The name of the element to verify.</param>
        /// <returns>
        ///     <see langword="true" /> if the element exists in the collection; otherwise, <see langword="false" />. The default
        ///     is <see langword="false" />.
        /// </returns>
        protected override bool IsElementName(string elementName)
        {
            return (elementName == CodecElementPropertyName);
        }

        /// <summary>
        ///     Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="ConfigurationElement" /> to return the key for.</param>
        /// <returns>
        ///     An <see cref="object" /> that acts as the key for the specified <see cref="ConfigurationElement" />.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CodecElement)element).Name;
        }

        /// <summary>
        ///     When overridden in a derived class, creates a new <see cref="ConfigurationElement" />.
        /// </summary>
        /// <returns>
        ///     A new <see cref="ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CodecElement();
        }

        #endregion

        #region Assemblies Property

        /// <summary>
        ///     The XML name of the <see cref="Assemblies" /> property.
        /// </summary>
        internal const string AssembliesPropertyName = "assemblies";

        /// <summary>
        ///     Gets or sets the Assemblies.
        /// </summary>
        [ConfigurationProperty(AssembliesPropertyName, IsRequired = false, IsKey = false, IsDefaultCollection = false)]
        public AddinAssemblyCollection Assemblies
        {
            get
            {
                return (AddinAssemblyCollection)base[AssembliesPropertyName];
            }
            set
            {
                base[AssembliesPropertyName] = value;
            }
        }

        #endregion
    }

    /// <summary>
    ///     The CodecElement Configuration Element.
    /// </summary>
    public class CodecElement : AddinElement
    {
    }
}
