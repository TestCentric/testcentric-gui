// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Globalization;
using System.Text;

namespace TestCentric.Gui.Model
{
    public static class ResultSummaryReporter
    {
        public static string WriteSummaryReport(ResultSummary summary)
        {
            var writer = new StringBuilder();
            writer.AppendLine($"Overall result: {summary.OverallResult}");

            writer.AppendUICultureFormattedNumber("  Test Count: ", summary.TestCount);
            writer.AppendUICultureFormattedNumber(", Passed: ", summary.PassCount);
            writer.AppendUICultureFormattedNumber(", Failed: ", summary.FailedCount);
            writer.AppendUICultureFormattedNumber(", Warnings: ", summary.WarningCount);
            writer.AppendUICultureFormattedNumber(", Inconclusive: ", summary.InconclusiveCount);
            writer.AppendUICultureFormattedNumber(", Skipped: ", summary.TotalSkipCount);
            writer.AppendLine();

            if (summary.FailedCount > 0)
            {
                writer.AppendUICultureFormattedNumber("    Failed Tests - Failures: ", summary.FailureCount);
                writer.AppendUICultureFormattedNumber(", Errors: ", summary.ErrorCount);
                writer.AppendUICultureFormattedNumber(", Invalid: ", summary.InvalidCount);
                writer.AppendLine();
            }
            if (summary.TotalSkipCount > 0)
            {
                writer.AppendUICultureFormattedNumber("    Skipped Tests - Ignored: ", summary.IgnoreCount);
                writer.AppendUICultureFormattedNumber(", Explicit: ", summary.ExplicitCount);
                writer.AppendUICultureFormattedNumber(", Other: ", summary.SkipCount);
                writer.AppendLine();
            }

            writer.AppendLine($"  Start time: {summary.StartTime:u}");
            writer.AppendLine($"    End time: {summary.EndTime:u}");
            writer.AppendUICultureFormattedNumber("    Duration: ", summary.Duration);

            return writer.ToString();
        }

        private static void AppendUICultureFormattedNumber(this StringBuilder sb, string label, int number)
        {
            sb.Append(label);
            sb.Append(number.ToString("n0", CultureInfo.CurrentUICulture));
        }

        private static void AppendUICultureFormattedNumber(this StringBuilder sb, string label, double number)
        {
            sb.Append(label);
            sb.Append(number.ToString("n3", CultureInfo.CurrentUICulture));
        }
    }
}
