using System;
using System.Diagnostics;
using System.Linq;

using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Java.Util;
using Random = System.Random;
using Xamarin.Essentials;
namespace BLE.Server.Droid
{
    public class BleServer
    {
        private readonly BluetoothManager _bluetoothManager;
        private BluetoothAdapter _bluetoothAdapter;
        private BleGattServerCallback _bluettothServerCallback;
        private BluetoothGattServer _bluetoothServer;
        private BluetoothGattCharacteristic _characteristic;

        public BleServer(Context ctx)
        {
            _bluetoothManager = (BluetoothManager)ctx.GetSystemService(Context.BluetoothService);
            _bluetoothAdapter = _bluetoothManager.Adapter;

            _bluettothServerCallback = new BleGattServerCallback();
            _bluetoothServer = _bluetoothManager.OpenGattServer(ctx, _bluettothServerCallback);

            var service = new BluetoothGattService(UUID.FromString("ffe0ecd2-3d16-4f8d-90de-e89e7fc396a5"),
                GattServiceType.Primary);
            _characteristic = new BluetoothGattCharacteristic(UUID.FromString("d8de624e-140f-4a22-8594-e2216b84a5f2"), GattProperty.Read | GattProperty.Notify | GattProperty.Write, GattPermission.Read | GattPermission.Write);

            service.AddCharacteristic(_characteristic);
            _bluetoothServer.AddService(service);
            
             SensorSpeed speed = SensorSpeed.UI;     
             OrientationSensor.Start(speed);
             OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;

            _bluettothServerCallback.CharacteristicReadRequest += _bluettothServerCallback_CharacteristicReadRequest;
            Console.WriteLine("Server created!");

            BluetoothLeAdvertiser myBluetoothLeAdvertiser = _bluetoothAdapter.BluetoothLeAdvertiser;

            var builder = new AdvertiseSettings.Builder();
            builder.SetAdvertiseMode(AdvertiseMode.LowLatency);
            builder.SetConnectable(true);
            builder.SetTimeout(0);
            builder.SetTxPowerLevel(AdvertiseTx.PowerHigh);
            AdvertiseData.Builder dataBuilder = new AdvertiseData.Builder();
            dataBuilder.SetIncludeDeviceName(true);
            dataBuilder.SetIncludeTxPowerLevel(true);

            myBluetoothLeAdvertiser.StartAdvertising(builder.Build(), dataBuilder.Build(), new BleAdvertiseCallback());
        }
  
        private string _data = "";
        void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            var data = e.Reading;
            _data = $"Reading: X: {data.Orientation.X}, Y: {data.Orientation.Y}, Z: {data.Orientation.Z}";
        }

        void _bluettothServerCallback_CharacteristicReadRequest(object sender, BleEventArgs e)
        {
            e.Characteristic.SetValue(_data);
            _bluetoothServer.SendResponse(e.Device, e.RequestId, GattStatus.Success, e.Offset, e.Characteristic.GetValue());
        }
    }

    public class BleAdvertiseCallback : AdvertiseCallback
    {
        public override void OnStartFailure(AdvertiseFailure errorCode)
        {
            Console.WriteLine("Adevertise start failure {0}", errorCode);
            base.OnStartFailure(errorCode);
        }

        public override void OnStartSuccess(AdvertiseSettings settingsInEffect)
        {
            Console.WriteLine("Adevertise start success {0}", settingsInEffect.Mode);
            base.OnStartSuccess(settingsInEffect);
        }
    }
}