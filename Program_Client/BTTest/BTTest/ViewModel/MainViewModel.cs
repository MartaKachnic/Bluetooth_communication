using BTTest.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BTTest.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        BluetoothService _bluetoothService;

        public RelayCommand GetDevicesCommand { get; private set; }


        public MainViewModel()
        {
            _bluetoothService = new BluetoothService();

            GetDevicesCommand = new RelayCommand(GetDevices, true);

        }

        public void GetDevices()
        {
            _bluetoothService.GetDevicesList();
        }
       

    }
}