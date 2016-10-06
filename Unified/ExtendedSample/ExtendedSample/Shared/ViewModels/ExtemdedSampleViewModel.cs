using ExtendedSample.ViewModels.Abstract;
using Scandit.BarcodePicker.Unified;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;

namespace ExtendedSample.ViewModels
{
    public class ExtendedSampleViewModel : BaseViewModel
    {
        private string _recognizedCode;

        public string RecognizedCode
        {
            get
            {
                return (_recognizedCode == null) ? "" : "Code scanned: " + _recognizedCode;
            }

            set
            {
                _recognizedCode = value;
            }
        }

        public ExtendedSampleViewModel()
        {
            // will get invoked whenever a code has been scanned.
            ScanditService.BarcodePicker.DidScan += BarcodePickerOnDidScan;
        }

        private async void BarcodePickerOnDidScan(ScanSession session)
        {
            // guaranteed to always have at least one element, so we don't have to 
            // check the size of the NewlyRecognizedCodes array.
            RecognizedCode = session.NewlyRecognizedCodes.LastOrDefault()?.Data;
		    await ScanditService.BarcodePicker.StopScanningAsync();
        }

        public ICommand OnScanButtonClicked => new Command(async () => await StartScanning());

        private async Task StartScanning()
        {
            SettingsViewModel settingsModel = ViewModelProvider.GetViewModel<SettingsViewModel>();
            settingsModel.updateScanSettings();
            settingsModel.updateScanOverlay();

            // The scanning behavior of the barcode picker is configured through scan
            // settings. We start with empty scan settings and enable a very generous
            // set of symbologies. In your own apps, only enable the symbologies you
            // actually need.
            await ScanditService.BarcodePicker.StartScanningAsync(false);
        }
    }
}
