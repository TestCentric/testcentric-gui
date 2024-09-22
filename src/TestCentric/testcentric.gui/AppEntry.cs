// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui
{
    using Model;
    using Views;
    using Presenters;
    using TestCentric.Gui.Dialogs;

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

            if (options.ShowHelp || !options.Validate())
            {
                new HelpDisplay(options).ShowDialog();
                return options.ShowHelp ? 0 : 2;
            }

            ITestEngine engine = TestEngineActivator.CreateInstance();

            log.Info("Instantiating TestModel");
            ITestModel model = TestModel.CreateTestModel(engine, options);

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
                model.Dispose();
            }

            return 0;
        }
    }
}
