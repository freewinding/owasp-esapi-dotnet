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

using System.Collections.Generic;

namespace Owasp.Esapi.Interfaces
{
    /// <summary>
    ///     The IEncoder interface contains a number of methods related to encoding input
    ///     so that it will be safe for a variety of interpreters.
    /// </summary>
    public interface IEncoder
    {
        /// <summary>
        ///     This method performs canonicalization on data received to ensure that it
        ///     has been reduced to its most basic form before validation. The application can supply a list of
        ///     codecs and the data will be decoded by each codec cosecutively, until it has reached its
        ///     canonical form.
        /// </summary>
        /// <param name="codecNames">
        ///     The names of the codecs to use for canonicalization. These codecs will be used in order.
        /// </param>
        /// <param name="input">
        ///     Unvalidated input.
        /// </param>
        /// <param name="strict">
        ///     If this is true, then double encodings will cause an exception.
        /// </param>
        /// <returns>
        ///     The canonicalized string.
        /// </returns>
        string Canonicalize(IEnumerable<string> codecNames, string input, bool strict);

        /// <summary>
        ///     Canonicalize input using loaded codecs
        /// </summary>
        /// <param name="input">String input</param>
        /// <param name="strict">If true double encoding causes exception</param>
        /// <returns>Canonicalized string</returns>
        string Canonicalize(string input, bool strict);

        /// <summary>
        ///     Reduce all non-ascii characters to their ASCII form so that simpler
        ///     validation rules can be applied. For example, an accented-e character
        ///     will be changed into a regular ASCII e character.
        /// </summary>
        /// <param name="input">
        ///     The value to normalize.
        /// </param>
        /// <returns>
        ///     The normalized value.
        /// </returns>
        string Normalize(string input);

        /// <summary>
        ///     This method encodes the input according to the given codec name.
        /// </summary>
        /// <param name="codecName">The codec to use to encode the input.</param>
        /// <param name="input">The input to encode.</param>
        /// <returns>The encoded input.</returns>
        string Encode(string codecName, string input);

        /// <summary>
        ///     This method decodes the input according to the given codec name.
        /// </summary>
        /// <param name="codecName">The codec to use to decode the input.</param>
        /// <param name="input">The input to decode.</param>
        /// <returns>The decoded input.</returns>
        string Decode(string codecName, string input);

        /// <summary>
        ///     This method returns the codec associated with the given codec name.
        /// </summary>
        /// <param name="codecName">The codec name to return.</param>
        /// <returns>The codec associated with the codec name.</returns>
        ICodec GetCodec(string codecName);

        /// <summary>
        ///     This method adds the given codec with the given codec name to the Encoder.
        /// </summary>
        /// <param name="codecName">The name of the codec to add.</param>
        /// <param name="codec">The codec to add.</param>
        void AddCodec(string codecName, ICodec codec);

        /// <summary>
        ///     This method removes the codec with the given codec name.
        /// </summary>
        /// <param name="codecName">The name of the codec to remove.</param>
        void RemoveCodec(string codecName);
    }
}
