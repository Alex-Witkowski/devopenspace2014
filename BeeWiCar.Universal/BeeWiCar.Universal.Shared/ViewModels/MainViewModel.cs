namespace BeeWiCar.Universal.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;

    using BeeWiCar.Universal.Common;
    using BeeWiCar.Universal.Services;

    using Windows.Devices.Enumeration;

    public class MainViewModel : BaseViewModel
    {
        private ICommand connectCommand;

        private ObservableCollection<DeviceInformation> deviceInformations;

        private string hello;

        private DeviceInformation selectedDevice;

        private BluetoothRf bluetoothRf;

        public MainViewModel()
        {
            this.Hello = "Hallo Leipzig";
            this.ConnectCommand = new DelegateCommand<object>(this.HandleConnectCommand);
        }

        public ICommand ConnectCommand
        {
            get
            {
                return this.connectCommand;
            }

            set
            {
                this.SetProperty(ref this.connectCommand, value);
            }
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

        public DeviceInformation SelectedDevice
        {
            get
            {
                return this.selectedDevice;
            }

            set
            {
                this.SetProperty(ref this.selectedDevice, value);
            }
        }

        public async void LoadData()
        {
            this.bluetoothRf = new BluetoothRf();
            var bluetoothDeviceInformations = await this.bluetoothRf.EnumerateDevicesAsync();
            this.DeviceInformations = new ObservableCollection<DeviceInformation>(bluetoothDeviceInformations);
        }

        private async void HandleConnectCommand(object obj)
        {
            if (this.SelectedDevice == null)
            {
                return;
            }

            await this.bluetoothRf.ConnectToDeviceAsync(this.SelectedDevice);
        }
    }
}
