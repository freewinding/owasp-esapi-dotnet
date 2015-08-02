﻿using System;
using Owasp.Esapi.Interfaces;

namespace Owasp.Esapi.Codecs
{   
    /// <summary>
    /// This class performs XML encoding.
    /// </summary>
    [Codec(BuiltinCodecs.Xml)]
    public class XmlCodec : ICodec
    {
        #region ICodec Members

        /// <summary>
        /// XML encode the input.
        /// </summary>
        /// <param name="input">The input to encode.</param>
        /// <returns>The encoded input.</returns>
        public string Encode(string input)
        {
            if (input == null)
            {
                input = string.Empty;
            }

            return Microsoft.Security.Application.Encoder.XmlEncode(input);
        }

        /// <summary>
        /// XML decode the input.
        /// </summary>
        /// <param name="input">The input to decode.</param>
        /// <returns>The decoded input.</returns>
        /// <remarks>This method is not implemented.</remarks>
        public string Decode(string input)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
