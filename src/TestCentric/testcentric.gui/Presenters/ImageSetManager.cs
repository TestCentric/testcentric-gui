// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCentric.Engine;
using TestCentric.Gui.Model;
using TestCentric.Gui.Views;

namespace TestCentric.Gui.Presenters
{
    public class ImageSetManager
    {
        private static readonly string BASE_DIR = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine("Images", "Tree"));

        private ITestModel _model;
        private IMainView _view;

        public ImageSetManager(ITestModel model, IMainView view)
        {
            Guard.OperationValid(Directory.Exists(BASE_DIR), $"Directory not found {BASE_DIR}");

            _model = model;
            _view = view;

            foreach (string dir in Directory.GetDirectories(BASE_DIR))
            {
                if (OutcomeImageSet.IsValidImageSetDirectory(dir))
                {
                    string name = Path.GetFileNameWithoutExtension(dir);
                    ImageSets.Add(name, new OutcomeImageSet(dir));
                }
            }

            Guard.OperationValid(ImageSets.Count > 0, "No ImageSets were found");

            UpdateViews(CurrentImageSet = LoadImageSet(_model.Settings.Gui.TestTree.AlternateImageSet));

            _model.Settings.Changed += (s, e) =>
            {
                if (e.SettingName == "TestCentric.Gui.TestTree.AlternateImageSet")
                    UpdateViews(CurrentImageSet = LoadImageSet(_model.Settings.Gui.TestTree.AlternateImageSet));
            };
        }

        // CurrentImageSet updates all come here to update the views
        // This method has built-in knowledge of which views use the
        // CurrentImageSet and what method to call on each.
        // TODO: Use an event-oriented approach and let views pull
        // changes as they need it, provided it doesn't duplicate
        // too much code.
        private void UpdateViews(OutcomeImageSet imgSet)
        {
            _view.TreeView.OutcomeImages = imgSet;
            _view.TestResultSubView.LoadImages(imgSet);
        }

        public IDictionary<string, OutcomeImageSet> ImageSets { get; } = new Dictionary<string, OutcomeImageSet>();

        public OutcomeImageSet CurrentImageSet { get; private set; }

        public OutcomeImageSet LoadImageSet(string imageSetName)
        {
            Guard.ArgumentValid(ImageSets.ContainsKey(imageSetName), $"Invalid ImageSet name: {imageSetName}", nameof(imageSetName));

            return CurrentImageSet = ImageSets[imageSetName];
        }
    }
}
