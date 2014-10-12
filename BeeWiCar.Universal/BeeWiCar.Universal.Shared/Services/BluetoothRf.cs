namespace BeeWiCar.Universal.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Windows.Devices.Bluetooth.Rfcomm;
    using Windows.Devices.Enumeration;

    public class BluetoothRf
    {
        public async Task<IEnumerable<DeviceInformation>> EnumerateDevicesAsync()
        {
            var deviceInformationCollection = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
            return deviceInformationCollection;
        }
    }
}
