// ***********************************************************************
// Copyright (c) 2016 Charlie Poole
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
using System.Windows.Forms;
using Mono.Options;
using NUnit.Engine;

namespace TestCentric.Gui
{
    using Model;
    using Views;
    using Presenters;

    public static class AppEntry
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Run(args);
        }

        /// <summary>
        /// The main entry point for the application, minus GUI initialization
        /// logic. This method is intended for wrapper applications that
        /// already do their own GUI initializations that would conflict with
        /// those done in <see cref="Main"/>.
        /// </summary>
        public static void Run(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();
            try
            {
                options.Parse(args);
            }
            catch (OptionException ex)
            {
                string msg = string.Format("{0} {1}", ex.Message, ex.OptionName);
                MessageBox.Show(msg, "NUnit - OptionException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!options.Validate())
            {
                var errMessage = new string[options.ErrorMessages.Count];
                options.ErrorMessages.CopyTo(errMessage, 0);
                var msg =
                    "There were the following errors parsing the options:" + Environment.NewLine +
                    string.Join(Environment.NewLine, errMessage) + Environment.NewLine +
                    Environment.NewLine +
                    "Use the option '--help' to show the possible options and their values.";
                MessageBox.Show(msg, "NUnit - Problem parsing options", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (options.ShowHelp)
            {
                MessageDisplay.Info(GetHelpText(options));
                return;
            }

            var testEngine = TestEngineActivator.CreateInstance(true);
            var traceLevel = (InternalTraceLevel)Enum.Parse(typeof(InternalTraceLevel), options.InternalTraceLevel ?? "Off");
            testEngine.InternalTraceLevel = traceLevel;

            var model = new TestModel(testEngine);
            model.PackageSettings.Add(EnginePackageSettings.InternalTraceLevel, traceLevel.ToString());

            if (options.ProcessModel != null)
                model.PackageSettings.Add(EnginePackageSettings.ProcessModel, options.ProcessModel);
            if (options.DomainUsage != null)
                model.PackageSettings.Add(EnginePackageSettings.DomainUsage, options.DomainUsage);
            if (options.MaxAgents >= 0)
                model.PackageSettings.Add(EnginePackageSettings.MaxAgents, options.MaxAgents);
            if (options.RunAsX86)
                model.PackageSettings.Add(EnginePackageSettings.RunAsX86, true);

            var form = new MainForm();

            new ProgressBarPresenter(form.ProgressBarView, model);
            new StatusBarPresenter(form.StatusBarView, model);
            new TestPropertiesPresenter(form.PropertiesView, model);
            new XmlPresenter(form.XmlView, model);
            new TextOutputPresenter(form.TextOutputView, model);
            new TreeViewPresenter(form.TestTreeView, model);
            new MainPresenter(form, model, options);

            //new RecentFiles(settingsServiceServiceService._settings);
            //new RecentFilesPresenter(form, settingsServiceServiceService);

            try
            {
                Application.Run(form);
            }
            finally
            {
                testEngine.Dispose();
            }
        }

        private static string GetHelpText(CommandLineOptions options)
        {
            StringWriter writer = new StringWriter();

            writer.WriteLine("TC-NEXT [inputfiles] [options]");
            writer.WriteLine();
            writer.WriteLine("Starts the TestCentric Experimental Runner, optionally loading and running a set of NUnit tests. You may specify any combination of assemblies and supported project files as arguments.");
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
            get { return new MessageDisplay("TestCentric Experimental Runner for NUnit"); }
        }
    }
}
