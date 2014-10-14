namespace BeeWiCar.Universal.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using BeeWiCar.Universal.Common;
    using BeeWiCar.Universal.Data;
    using BeeWiCar.Universal.Services;

    using Windows.Devices.Enumeration;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;

    public class MainViewModel : BaseViewModel
    {
        private ICommand connectCommand;

        private ObservableCollection<DeviceInformation> deviceInformations;

        private string hello;

        private DeviceInformation selectedDevice;

        private BluetoothRf bluetoothRf;

        private StreamSocket socket;

        private DataWriter dataWriter;

        private DataReader dataReader;

        private ICommand forewardCommand;

        public MainViewModel()
        {
            this.Hello = "Hallo Leipzig";
            this.ConnectCommand = new DelegateCommand<object>(this.HandleConnectCommand);
            this.ForewardCommand = new DelegateCommand<object>(this.HandleForwardCommand);
        }

        private async void HandleForwardCommand(object obj)
        {
            await this.SendByteSequenceAsync(new[] { BeeWiCarCommands.Forward_Go });
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

        public ICommand ForewardCommand
        {
            get
            {
                return this.forewardCommand;
            }

            set
            {
                this.SetProperty(ref this.forewardCommand, value);
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

            this.socket = await this.bluetoothRf.ConnectToDeviceAsync(this.SelectedDevice);

            // DataWriter and -Reader not StreamWriter because StreamWriter only supports char and string. 
            this.dataWriter = new DataWriter(this.socket.OutputStream);
            this.dataReader = new DataReader(this.socket.InputStream);
            var read = this.ReadDataAsync();
        }

        private async Task ReadDataAsync()
        {
            while (this.dataReader != null)
            {
                try
                {
                    // Read first byte (length of the subsequent message, 255 or less).  
                    uint sizeFieldCount = await this.dataReader.LoadAsync(1);
                    if (sizeFieldCount != 1)
                    {
                        // The underlying socket was closed before we were able to read the whole data.  
                        return;
                    }

                    // Read the message.  
                    uint messageLength = this.dataReader.ReadByte();
                    uint actualMessageLength = await this.dataReader.LoadAsync(messageLength);
                    if (messageLength != actualMessageLength)
                    {
                        // The underlying socket was closed before we were able to read the whole data.  
                        return;
                    }

                    // Read the message and process it. 
                    string message = this.dataReader.ReadString(actualMessageLength);

                    this.HandleMessageReceived(message);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        public async Task SendByteSequenceAsync(byte[] sequence)
        {
            if (this.dataWriter != null)
            {
                foreach (byte data in sequence)
                {
                    this.dataWriter.WriteByte(data);
                }

                await this.dataWriter.StoreAsync();
            }
        }

        private void HandleMessageReceived(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
