namespace BeeWiCar.Universal.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Windows.Devices.Bluetooth.Rfcomm;
    using Windows.Devices.Enumeration;
    using Windows.Foundation;
    using Windows.Networking.Sockets;

    public class BluetoothRf
    {
        private IAsyncOperation<RfcommDeviceService> connectServiceOperation;

        private RfcommDeviceService rfConnection;

        private StreamSocket socket;

        public async Task<IEnumerable<DeviceInformation>> EnumerateDevicesAsync()
        {
            var deviceInformationCollection = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
            return deviceInformationCollection;
        }

        public async Task ConnectToDeviceAsync(DeviceInformation deviceInfromation)
        {
            try
            {
                this.connectServiceOperation = RfcommDeviceService.FromIdAsync(deviceInfromation.Id);
                this.rfConnection = await this.connectServiceOperation;
                if (this.rfConnection != null)
                {
                    this.socket = new StreamSocket();
                    await this.socket.ConnectAsync(this.rfConnection.ConnectionHostName, this.rfConnection.ConnectionServiceName, SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
                    //// Do something useful with the socket
                }
                else
                {
                    Debug.WriteLine("Error connection to device " + deviceInfromation.Name);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}
