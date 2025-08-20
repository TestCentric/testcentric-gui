// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Text;

namespace TestCentric.Engine.Internal
{
    public static class ProcessUtils
    {
        private static readonly char[] CharsThatRequireQuoting = { ' ', '"' };
        private static readonly char[] CharsThatRequireEscaping = { '\\', '"' };

        /// <summary>
        /// Escapes arbitrary values so that the process receives the exact string you intend and injection is impossible.
        /// Spec: https://docs.microsoft.com/en-gb/windows/desktop/api/shellapi/nf-shellapi-commandlinetoargvw
        /// </summary>
        public static void EscapeProcessArgument(this StringBuilder builder, string literalValue, bool alwaysQuote = false)
        {
            if (string.IsNullOrEmpty(literalValue))
            {
                builder.Append("\"\"");
                return;
            }

            if (literalValue.IndexOfAny(CharsThatRequireQuoting) == -1) // Happy path
            {
                if (!alwaysQuote)
                {
                    builder.Append(literalValue);
                    return;
                }
                if (literalValue[literalValue.Length - 1] != '\\')
                {
                    builder.Append('"').Append(literalValue).Append('"');
                    return;
                }
            }

            builder.Append('"');

            var nextPosition = 0;
            while (true)
            {
                var nextEscapeChar = literalValue.IndexOfAny(CharsThatRequireEscaping, nextPosition);
                if (nextEscapeChar == -1) break;

                builder.Append(literalValue, nextPosition, nextEscapeChar - nextPosition);
                nextPosition = nextEscapeChar + 1;

                switch (literalValue[nextEscapeChar])
                {
                    case '"':
                        builder.Append("\\\"");
                        break;
                    case '\\':
                        var numBackslashes = 1;
                        while (nextPosition < literalValue.Length && literalValue[nextPosition] == '\\')
                        {
                            numBackslashes++;
                            nextPosition++;
                        }
                        if (nextPosition == literalValue.Length || literalValue[nextPosition] == '"')
                            numBackslashes <<= 1;

                        for (; numBackslashes != 0; numBackslashes--)
                            builder.Append('\\');
                        break;
                }
            }

            builder.Append(literalValue, nextPosition, literalValue.Length - nextPosition);
            builder.Append('"');
        }
    }
}
