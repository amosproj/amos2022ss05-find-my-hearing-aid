using System;
using System.Threading.Tasks;

namespace FindMyBLEDevice.XamarinAccess
{
    public interface IDeviceAccess
    {
        Task InvokeOnMainThreadAsync(Func<Task> func);
    }
}