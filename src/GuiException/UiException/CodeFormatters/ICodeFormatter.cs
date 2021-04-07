// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace NUnit.UiException.CodeFormatters
{
    /// <summary>
    /// ICodeFormatter is the interface to make the syntax
    /// coloring of a string for a specific developpment language.
    /// </summary>
    public interface ICodeFormatter
    {
        /// <summary>
        /// The language name handled by this formatter.
        /// Ex: "C#", "Java", "C++" and so on...
        /// </summary>
        string Language { get; }

        /// <summary>
        /// Makes the coloring syntax of the given text.
        /// </summary>
        /// <param name="code">The text to be formatted. This
        /// parameter cannot be null.</param>
        /// <returns>A FormattedCode instance.</returns>
        FormattedCode Format(string code);
    }
}
