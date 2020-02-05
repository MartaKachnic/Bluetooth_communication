package md59b855cec48b15f886b1d86f27024b829;


public class BleAdvertiseCallback
	extends android.bluetooth.le.AdvertiseCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onStartFailure:(I)V:GetOnStartFailure_IHandler\n" +
			"n_onStartSuccess:(Landroid/bluetooth/le/AdvertiseSettings;)V:GetOnStartSuccess_Landroid_bluetooth_le_AdvertiseSettings_Handler\n" +
			"";
		mono.android.Runtime.register ("BLE.Server.Droid.BleAdvertiseCallback, BLE.Server.Droid", BleAdvertiseCallback.class, __md_methods);
	}


	public BleAdvertiseCallback ()
	{
		super ();
		if (getClass () == BleAdvertiseCallback.class)
			mono.android.TypeManager.Activate ("BLE.Server.Droid.BleAdvertiseCallback, BLE.Server.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onStartFailure (int p0)
	{
		n_onStartFailure (p0);
	}

	private native void n_onStartFailure (int p0);


	public void onStartSuccess (android.bluetooth.le.AdvertiseSettings p0)
	{
		n_onStartSuccess (p0);
	}

	private native void n_onStartSuccess (android.bluetooth.le.AdvertiseSettings p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
