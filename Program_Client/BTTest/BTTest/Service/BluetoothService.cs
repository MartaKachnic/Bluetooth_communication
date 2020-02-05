using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace BTTest.Service
{
    public class BluetoothService
    {
        private BluetoothLEDevice _device;
        private GattCharacteristic _characteristic;

        public BluetoothService()
        {
        }

        public void GetDevicesList()
        {
            BluetoothLEAdvertisementWatcher BleWatcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };
            BleWatcher.Received += Watcher_Received;
            BleWatcher.Start();
        }

        private async void Watcher_Received(BluetoothLEAdvertisementWatcher sender,
                                            BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var device = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);
            if (device != null)
            {
                if (device.Name == "Galaxy A70")
                {
                    _device = device;
                    var gaUID = Guid.Parse("ffe0ecd2-3d16-4f8d-90de-e89e7fc396a5");

                    GattDeviceServicesResult result = await device.GetGattServicesForUuidAsync(gaUID);
                    if (result.Status == GattCommunicationStatus.Success)
                    {
                        var genericAccess = result.Services.FirstOrDefault(s => s.Uuid == gaUID);
                        var charUID = Guid.Parse("d8de624e-140f-4a22-8594-e2216b84a5f2");
                        var chara = await genericAccess.GetCharacteristicsForUuidAsync(charUID);

                        if (chara.Status == GattCommunicationStatus.Success)
                        {
                            sender.Stop();
                            var c = chara.Characteristics.FirstOrDefault(x => x.Uuid == charUID);
                            _characteristic = c;
                            System.Timers.Timer Timer1 = new System.Timers.Timer();
                            Timer1.Interval = 1000;
                            Timer1.Enabled = true;
                            Timer1.Elapsed += Timer1_Elapsed;
                            Timer1.Start();
                        }
                    }
                }
            }
        }

        private async void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var res = await _characteristic.ReadValueAsync(BluetoothCacheMode.Uncached);
            var reader = DataReader.FromBuffer(res.Value);
            byte[] input = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(input);

            string value = Encoding.UTF8.GetString(input);
            Console.WriteLine(value);
        }
    }
}
