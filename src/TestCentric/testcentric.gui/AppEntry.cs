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
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui
{
    using Model;
    using Views;
    using Presenters;

    /// <summary>
    /// Class to manage application startup.
    /// </summary>
    public class AppEntry
    {
        static Logger log = InternalTrace.GetLogger(typeof(AppEntry));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            var options = new CommandLineOptions(args);

            if (options.ShowHelp)
            {
                // TODO: We would need to have a custom message box
                // in order to use a fixed font and display the options
                // so that the values all line up.
                MessageDisplay.Info(GetHelpText(options));
                return 0;
            }

            if (!options.Validate())
            {
                var NL = Environment.NewLine;
                var sb = new StringBuilder($"Error(s) in command line:{NL}");
                foreach (string msg in options.ErrorMessages)
                    sb.Append($"  {msg}{NL}");
                sb.Append($"{NL}{GetHelpText(options)}");
                MessageDisplay.Error(sb.ToString());
                return 2;
            }

            // Currently the InternalTraceLevel can only be set from the command-line.
            // We can't use user settings to provide a default because the settings
            // are an engine service and the engine have the internal trace level
            // set as part of its initialization.
            var traceLevel = options.InternalTraceLevel != null
                ? (InternalTraceLevel)Enum.Parse(typeof(InternalTraceLevel), options.InternalTraceLevel)
                : InternalTraceLevel.Off;

            // This initializes the trace setting for the GUI itself.
            InternalTrace.Initialize($"InternalTrace.{Process.GetCurrentProcess().Id}.gui.log", traceLevel);
            log.Info($"Starting TestCentric Runner - InternalTraceLevel = {traceLevel}");

            log.Info("Creating TestEngine");
            ITestEngine testEngine = TestEngineActivator.CreateInstance();
            testEngine.InternalTraceLevel = traceLevel;

            log.Info("Instantiating TestModel");
            ITestModel model = new TestModel(testEngine);
            model.PackageSettings.Add(EnginePackageSettings.InternalTraceLevel, traceLevel.ToString());

            if (options.ProcessModel != null)
                model.PackageSettings.Add(EnginePackageSettings.ProcessModel, options.ProcessModel);
            if (options.DomainUsage != null)
                model.PackageSettings.Add(EnginePackageSettings.DomainUsage, options.DomainUsage);
            if (options.MaxAgents >= 0)
                model.PackageSettings.Add(EnginePackageSettings.MaxAgents, options.MaxAgents);
            if (options.RunAsX86)
                model.PackageSettings.Add(EnginePackageSettings.RunAsX86, true);

            log.Info("Constructing Form");
            TestCentricMainView view = new TestCentricMainView();

            log.Info("Constructing presenters");
            new ProgressBarPresenter(view.ProgressBarView, model);
            new StatusBarPresenter(view.StatusBarView, model);
            new ErrorsAndFailuresPresenter(view.ErrorsAndFailuresView, model);
            new TestsNotRunPresenter(view.TestsNotRunView, model);
            new TextOutputPresenter(view.TextOutputView, model);
            new TreeViewPresenter(view.TreeView, model);
            new CategoryPresenter(view.CategoryView, model);
            new TestCentricPresenter(view, model, options);

            try
            {
                log.Info("Starting Gui Application");
                Application.Run(view);
                log.Info("Application Exit");
            }
            catch (Exception ex)
            {
                log.Error("Gui Application threw an exception", ex);
                throw;
            }
            finally
            {
                log.Info("Exiting TestCentric Runner");
                InternalTrace.Close();
                testEngine.Dispose();
            }

            return 0;
        }

        private static string GetHelpText(CommandLineOptions options)
        {
            StringWriter writer = new StringWriter();

            writer.WriteLine("TESTCENTRIC [inputfiles] [options]");
            writer.WriteLine();
            writer.WriteLine("Starts the TestCentric Runner, optionally loading and running a set of NUnit tests. You may specify any combination of assemblies and supported project files as arguments.");
            writer.WriteLine();
            writer.WriteLine("InputFiles:");
            writer.WriteLine("   One or more assemblies or test projects of a recognized type.");
            writer.WriteLine("   If no input files are given, the tests contained in the most");
            writer.WriteLine("   recently used project or assembly are loaded, unless the");
            writer.WriteLine("   --noload option is specified");
            writer.WriteLine();
            writer.WriteLine("Options:");
            options.WriteOptionDescriptions(writer);

            return writer.GetStringBuilder().ToString();
        }

        private static IMessageDisplay MessageDisplay
        {
            get { return new MessageDisplay("TestCentric Runner for NUnit"); }
        }
    }
}
