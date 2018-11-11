// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;

namespace NUnit.UiException.CodeFormatters
{
    /// <summary>
    /// This enum defines the list of all tags
    /// that can be assigned to a particular string.
    /// </summary>
    public enum LexerTag
    {
        /// <summary>
        /// All sequences but the ones below
        /// </summary>
        Text,

        /// <summary>
        /// White characters: ' ' \t \n
        /// and other separators like:
        ///     - '[' ']' '(' ')' ';'
        /// </summary>
        Separator,

        /// <summary>
        /// Char: \n
        /// </summary>
        EndOfLine,

        /// <summary>
        /// string: /*
        /// </summary>
        CommentC_Open,

        /// <summary>
        /// string: */
        /// </summary>
        CommentC_Close,

        /// <summary>
        /// string: //
        /// </summary>
        CommentCpp,

        /// <summary>
        /// Char: '
        /// </summary>
        SingleQuote,

        /// <summary>
        /// Char: "
        /// </summary>
        DoubleQuote
    }

    /// <summary>
    /// This class is used to make the link between a string and a LexerTag value.
    /// </summary>
    public class LexToken
    {
        /// <summary>
        /// The string in this token.
        /// </summary>
        protected string _text;

        /// <summary>
        /// The current tag.
        /// </summary>
        protected LexerTag _tag;

        /// <summary>
        /// The starting startingPosition.
        /// </summary>
        protected int _start;

        public LexToken()
        {
            _text = null;
            _tag = LexerTag.Text;
            _start = -1;

            return;
        }

        public LexToken(string text, LexerTag tag, int start)
        {
            _text = text;
            _tag = tag;
            _start = start;

            return;
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        public string Text
        {
            get { return (_text); }
        }

        /// <summary>
        /// Gets the tag value
        /// </summary>
        public LexerTag Tag
        {
            get { return (_tag); }
        }

        /// <summary>
        /// Gets the starting startingPosition of the string.
        /// </summary>
        public int IndexStart
        {
            get { return (_start); }
        }

        public override bool Equals(object obj)
        {
            LexToken token;

            if (obj == null || !(obj is LexToken))
                return (false);

            token = (LexToken)obj;

            return (token.Text == Text &&
                    token.IndexStart == IndexStart &&
                    token.Tag == Tag);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return (String.Format("Token=([{0}], Index={1}, Tag={2})",
                Text, IndexStart, Tag));
        }
    }
}
