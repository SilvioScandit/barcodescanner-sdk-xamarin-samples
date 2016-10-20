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

// Helpers/Settings.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;

namespace ExtendedSample.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters.
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		private static IBarcodePicker picker = ScanditService.BarcodePicker;
		private static ScanSettings scanSettings = picker.GetDefaultScanSettings();

		public const string SymbologyPrefix = "Sym_";
		public const string InvSymbologyPrefix = "Inv_Sym_";

		// Feedback
		public const string BeepString = "Overlay_BeepEnabled";
		public const string VibrateString = "Overlay_VibrateEnabled";

		// Torch button
		public const string TorchButtonString = "Overlay_TorchButtonVisible";

		// Camera button
		public const string CameraButtonString = "Overlay_CameraButton";
		public const string CameraButtonString_Always = "Overlay_CameraButton_Always";
		public const string CameraButtonString_Never = "Overlay_CameraButton_Never";
		public const string CameraButtonString_OnlyTablet = "Overlay_CameraButton_OnlyTablets";

		// Hotspot
		public const string RestrictedAreaString = "ScanSettings_RestrictedAreaScanningEnabled";
		public const string HotSpotHeightString = "ScanOverlay_HotSpotHeight";
		public const string HotSpotYString = "ScanOverlay_HotSpotY";

		// ViewFinder
		public const string ViewFinderPortraitWidthString = "Overlay_ViewFinderSizePortrait_Width";
		public const string ViewFinderPortraitHeightString = "Overlay_ViewFinderSizePortrait_Height";
		public const string ViewFinderLandscapeWidthString = "Overlay_ViewFinderSizeLandscape_Width";
		public const string ViewFinderLandscapeHeightString = "Overlay_ViewFinderSizeLandscape_Height";

		public static readonly string[] SliderStrings = {
			ViewFinderPortraitWidthString,
			ViewFinderPortraitHeightString,
			ViewFinderLandscapeWidthString,
			ViewFinderLandscapeHeightString
		};

		public const string GuiStyleString = "Overlay_GuiStyle";
		public const string GuiStyleString_Rectangle = "Overlay_GuiStyle_Rectangle";
		public const string GuiStyleString_Laser = "Overlay_GuiStyle_Laser";
		public const string GuiStyleString_None = "Overlay_GuiStyle_None";

		public static bool hasInvertedSymbology(string symbology)
		{
			return (symbology == "Sym_Qr" || symbology == "Sym_DataMatrix");
		}

		public static string getInvertedSymboloby(string symbology)
		{
			if (hasInvertedSymbology(symbology))
			{
				return ("Inv_" + symbology);
			}
			else {
				return null;
			}
		}

		public static bool getBoolSetting(string setting)
        {
            if (setting == RestrictedAreaString) { Debug.WriteLine("get {0} {1}", setting, AppSettings.GetValueOrDefault<bool>(setting, defaultBool(setting))); }

            return AppSettings.GetValueOrDefault<bool>(setting, defaultBool(setting));
		}

		public static void setBoolSetting(string setting, bool value)
		{
            if (setting == RestrictedAreaString) { Debug.WriteLine("set {0} {1}", setting, value); }

            AppSettings.AddOrUpdateValue<bool>(setting, value);
		}

		private static bool defaultBool(string setting)
		{
			if (Array.IndexOf(Convert.EnabledSettings, setting) >= 0)
			{
				return true;
			} else
			{
				return false;
			}
		}

		public static Double getDoubleSetting(string setting)
		{
			return AppSettings.GetValueOrDefault<Double>(setting, defaultDouble(setting));
		}

		public static void setDoubleSetting(string setting, Double value)
		{
			AppSettings.AddOrUpdateValue<Double>(setting, value);
		}

		private static Double defaultDouble(string setting)
		{
			switch (setting)
			{
				case HotSpotHeightString:
					return 0.25;
				case HotSpotYString:
					return scanSettings.ScanningHotSpot.Y;
					
				case ViewFinderPortraitWidthString:
					return picker.ScanOverlay.ViewFinderSizePortrait.Width;
				case ViewFinderPortraitHeightString:
					return picker.ScanOverlay.ViewFinderSizePortrait.Height;
				case ViewFinderLandscapeWidthString:
					return picker.ScanOverlay.ViewFinderSizeLandscape.Width;
				case ViewFinderLandscapeHeightString:
					return picker.ScanOverlay.ViewFinderSizePortrait.Height;
					
				default:
					throw (new Exception("No such Double setting: " + setting));
			}
		}
		public static string getStringSetting(string setting)
		{
			return AppSettings.GetValueOrDefault<string>(setting, defaultString(setting));
		}

		public static void setStringSetting(string setting, string value)
		{
			AppSettings.AddOrUpdateValue<string>(setting, value);
		}

		private static string defaultString(string setting)
		{
			switch (setting)
			{
				case CameraButtonString:
					return CameraButtonString_Always;
				case GuiStyleString:
					return GuiStyleString_Rectangle;
				default:
					throw new Exception("No default setting for " + setting);
			}
		}
	}
}
