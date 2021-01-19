// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Windows.Forms;
using Mono.Options;
using NUnit.Engine;
using TestCentric.Common;

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

            var model = new TestModel(testEngine, "Experimental.");
            model.PackageOverrides.Add(EnginePackageSettings.InternalTraceLevel, traceLevel.ToString());

            if (options.MaxAgents >= 0)
                model.PackageOverrides.Add(EnginePackageSettings.MaxAgents, options.MaxAgents);
            if (options.RunAsX86)
                model.PackageOverrides.Add(EnginePackageSettings.RunAsX86, true);

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
            get { return new MessageBoxDisplay("TestCentric Experimental Runner for NUnit"); }
        }
    }
}
