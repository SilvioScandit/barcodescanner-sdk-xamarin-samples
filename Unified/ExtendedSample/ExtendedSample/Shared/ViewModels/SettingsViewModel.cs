// Copyright 2016 Scandit AG
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using ExtendedSample.Helpers;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ExtendedSample.ViewModels.Abstract;

namespace ExtendedSample.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public class BoolSetting : BaseViewModel
        {
            string _id;

            public BoolSetting(string id) { _id = id; }
            public bool Value
            {
                get
                {
                    return Settings.getBoolSetting(_id);
                }
                set
                {
                    Settings.setBoolSetting(_id, value);
                    OnPropertyChanged("Value");
                }
            }
        }
        public class FloatSetting : BaseViewModel
        {
            string _id;

            public FloatSetting(string id) { _id = id; }
            public float Value
            {
                get
                {
                    return (float)Settings.getDoubleSetting(_id);
                }
                set
                {
                    Settings.setDoubleSetting(_id, value);
                    OnPropertyChanged("Value");
                }
            }
        }

        public class SymbologySetting : BaseViewModel
        {
            public bool Enabled {
                get
                {
                    return Settings.getBoolSetting(_id);
                }
                set
                {
                    Settings.setBoolSetting(_id, value);
                    OnPropertyChanged("Value");
                    InverseVisible = value && Settings.hasInvertedSymbology(_id);
                }
            }
            public BoolSetting InverseEnabled { get; set; }
            public string Display { get; set; }
            public string InverseDisplay { get; set; }

            bool _inverseVisible = false;
            public bool InverseVisible
            {
                get { return _inverseVisible; }
                set { _inverseVisible = value; OnPropertyChanged("Value"); }
            }

            string _id;
            public SymbologySetting(string id)
            {
                _id = id;
                Display = Convert.settingToDisplay[id];
                string invID = Settings.getInvertedSymboloby(id);
                if (invID != null)
                {
                    InverseDisplay = Convert.settingToDisplay[invID];
                    InverseEnabled = new BoolSetting(invID);
                }
            }
        }

        public BoolSetting RestrictedAreaCell { get; set; } = new BoolSetting(Settings.RestrictedAreaString);
        public BoolSetting BeepCell { get; set; } = new BoolSetting(Settings.BeepString);
        public BoolSetting VibrateCell { get; set; } = new BoolSetting(Settings.VibrateString);
        public BoolSetting TorchButtonCell { get; set; } = new BoolSetting(Settings.TorchButtonString);

        public FloatSetting HotSpotHeightSlider { get; set; } = new FloatSetting(Settings.HotSpotHeightString);
        public FloatSetting HotSpotYSlider { get; set; } = new FloatSetting(Settings.HotSpotYString);
        public FloatSetting ViewFinderPortraitWidth { get; set; } = new FloatSetting(Settings.ViewFinderPortraitWidthString);
        public FloatSetting ViewFinderPortraitHeight { get; set; } = new FloatSetting(Settings.ViewFinderPortraitWidthString);
        public FloatSetting ViewFinderLandscapeWidth { get; set; } = new FloatSetting(Settings.ViewFinderLandscapeWidthString);
        public FloatSetting ViewFinderLandscapeHeight { get; set; } = new FloatSetting(Settings.ViewFinderLandscapeHeightString);

        public ObservableCollection<SymbologySetting> Symbologies { get; set; } = new ObservableCollection<SymbologySetting>();

        private IBarcodePicker _picker;
        private ScanSettings _scanSettings;

        public SettingsViewModel()
        {
            _picker = ScanditService.BarcodePicker;
            _scanSettings = _picker.GetDefaultScanSettings();

            foreach (string setting in Convert.settingToSymbologies.Keys)
            {
                Symbologies.Add(new SymbologySetting(setting));
            }
        }

        // Adds a new switch (two incase the symbology has an inverse) to SymbologySection
        void addSymbologySwitches(String settingString)
        {
            //// add switch for regular symbology
            //SwitchCell cell = new SwitchCell { Text = Convert.settingToDisplay[settingString] };
            //initializeSwitch(cell, settingString);
            //SymbologySection.Add(cell);

            //if (Helpers.Settings.hasInvertedSymbology(settingString))
            //{
            //    string invString = Helpers.Settings.getInvertedSymboloby(settingString);

            //    // add switch for inverse symbology
            //    SwitchCell invCell = new SwitchCell { Text = Convert.settingToDisplay[invString] };
            //    initializeSwitch(invCell, invString);
            //    SymbologySection.Add(invCell);

            //    // Gray out the inverse symbology switch if the regular symbology is disabled
            //    invCell.IsEnabled = Settings.getBoolSetting(settingString);
            //    cell.OnChanged += (object sender, ToggledEventArgs e) =>
            //    {
            //        invCell.IsEnabled = e.Value;
            //    };
            //}
        }
        
        
        // Bind an the camera picker to the permanent storage
        // and the currently active settings		
        private void initializeCameraPicker()
        {
        //    CameraButtonPicker.SelectedIndex =
        //          Convert.cameraButtonToIndex[Settings.getStringSetting(Settings.CameraButtonString)];

        //    CameraButtonPicker.SelectedIndexChanged += (object sender, EventArgs e) =>
        //    {
        //        Settings.setStringSetting(
        //            Settings.CameraButtonString,
        //            Convert.indexToCameraButton[CameraButtonPicker.SelectedIndex]);
        //        updateScanOverlay();
        //        updateScanSettings();
        //    };
        }

        // Bind an the GUIStyle picker to the permanent storage
        // and the currently active settings
        private void initializeGuiStylePicker()
        {
            //GuiStylePicker.SelectedIndex =
            //    Convert.guiStyleToIndex[Settings.getStringSetting(Settings.GuiStyleString)];

            //GuiStylePicker.SelectedIndexChanged += (object sender, EventArgs e) =>
            //{
            //    Settings.setStringSetting(
            //        Settings.GuiStyleString,
            //        Convert.indexToGuiStyle[GuiStylePicker.SelectedIndex]);
            //    updateScanOverlay();
            //    updateScanSettings();
            //};
        }

        // reads the values needed for ScanSettings from the Settings class
        // and applies them to the Picker
        public void updateScanSettings()
        {
            foreach (string setting in Convert.settingToSymbologies.Keys)
            {
                bool enabled = Settings.getBoolSetting(setting);
                foreach (Symbology sym in Convert.settingToSymbologies[setting])
                {
                    _scanSettings.EnableSymbology(sym, enabled);
                    if (Settings.hasInvertedSymbology(setting))
                    {
                        _scanSettings.Symbologies[sym].ColorInvertedEnabled = Settings.getBoolSetting(
                            Settings.getInvertedSymboloby(setting));
                    }
                }
            }
            _scanSettings.RestrictedAreaScanningEnabled = Settings.getBoolSetting(Settings.RestrictedAreaString);

            Double HotSpotHeight = Settings.getDoubleSetting(Settings.HotSpotHeightString);
            Double HotSpotY = Settings.getDoubleSetting(Settings.HotSpotYString);

            _scanSettings.ActiveScanningAreaPortrait = new Rect(
                0,
                HotSpotY - 0.5 * HotSpotHeight,
                1,
                HotSpotHeight);

            _scanSettings.ActiveScanningAreaLandscape = new Rect(
                0,
                HotSpotY - 0.5 * HotSpotHeight,
                1,
                HotSpotHeight);

            _scanSettings.ScanningHotSpot = new Scandit.BarcodePicker.Unified.Point(
                0.5,
                Settings.getDoubleSetting(Settings.HotSpotYString));
            Debug.WriteLine("RestrictedArea {0}", _scanSettings.RestrictedAreaScanningEnabled);
            _picker.ApplySettingsAsync(_scanSettings);
        }

        // reads the values needed for ScanOverlay from the Settings class
        // and applies them to the Picker
        public void updateScanOverlay()
        {
            _picker.ScanOverlay.BeepEnabled = Settings.getBoolSetting(Settings.BeepString);
            _picker.ScanOverlay.VibrateEnabled = Settings.getBoolSetting(Settings.VibrateString);
            _picker.ScanOverlay.TorchButtonVisible = Settings.getBoolSetting(Settings.TorchButtonString);

            _picker.ScanOverlay.ViewFinderSizePortrait = new Scandit.BarcodePicker.Unified.Size(
                (float)Settings.getDoubleSetting(Settings.ViewFinderPortraitWidthString),
                (float)Settings.getDoubleSetting(Settings.ViewFinderPortraitHeightString)
            );
            _picker.ScanOverlay.ViewFinderSizeLandscape = new Scandit.BarcodePicker.Unified.Size(
                   (float)Settings.getDoubleSetting(Settings.ViewFinderLandscapeWidthString),
                   (float)Settings.getDoubleSetting(Settings.ViewFinderLandscapeHeightString)
            );

            _picker.ScanOverlay.CameraSwitchVisibility =
                Convert.cameraToScanSetting[Settings.getStringSetting(Settings.CameraButtonString)];

            _picker.ScanOverlay.GuiStyle =
                Convert.guiStyleToScanSetting[Settings.getStringSetting(Settings.GuiStyleString)];
        }

    }
}
