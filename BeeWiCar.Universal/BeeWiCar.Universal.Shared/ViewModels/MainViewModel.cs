namespace BeeWiCar.Universal.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Windows.Devices.Enumeration;

    using BeeWiCar.Universal.Services;

    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<DeviceInformation> deviceInformations;

        private string hello;

        public MainViewModel()
        {
            this.Hello = "Hallo Leipzig";
        }

        public ObservableCollection<DeviceInformation> DeviceInformations
        {
            get
            {
                return this.deviceInformations;
            }

            set
            {
                this.SetProperty(ref this.deviceInformations, value);
            }
        }

        public string Hello
        {
            get
            {
                return this.hello;
            }

            set
            {
                this.SetProperty(ref this.hello, value);
            }
        }

        private async void LoadData()
        {
            var bluetoothRf = new BluetoothRf();
            var bluetoothDeviceInformations = await bluetoothRf.EnumerateDevicesAsync();
            this.DeviceInformations = new ObservableCollection<DeviceInformation>(bluetoothDeviceInformations);
        }
    }
}
