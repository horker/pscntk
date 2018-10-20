using System.Management.Automation;
using System.Runtime.InteropServices;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKDevice")]
    [Alias("cntk.device")]
    public class GetCNTKDevice : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "gpu")]
        public int GPUDeviceId;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "cpu")]
        public SwitchParameter CPUDevice;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "default")]
        public SwitchParameter DefaultDevice;

        protected override void EndProcessing()
        {
            DeviceDescriptor device;

            if (CPUDevice)
                device = DeviceDescriptor.CPUDevice;
            else if (DefaultDevice)
                device = DeviceDescriptor.UseDefaultDevice();
            else
                device = DeviceDescriptor.GPUDevice(GPUDeviceId);

            WriteObject(device);
        }
    }

    [Cmdlet("Get", "CNTKAllDevices")]
    [Alias("cntk.allDevices")]
    public class GetCNTKAllDevices : PSCmdlet
    {
        protected override void EndProcessing()
        {
            foreach (var device in DeviceDescriptor.AllDevices())
                WriteObject(device);
        }
    }

    [Cmdlet("Set", "CNTKDefaultDevice")]
    [Alias("cntk.setDefaultDevice")]
    public class SetCNTKDefaultDevice : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "gpu")]
        public int GPUDeviceId;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "cpu")]
        public SwitchParameter CPUDevice;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "default")]
        public SwitchParameter DefaultDevice;

        protected override void EndProcessing()
        {
            DeviceDescriptor device;

            if (CPUDevice)
                device = DeviceDescriptor.CPUDevice;
            else if (DefaultDevice)
                device = DeviceDescriptor.UseDefaultDevice();
            else
                device = DeviceDescriptor.GPUDevice(GPUDeviceId);

            WriteObject(DeviceDescriptor.TrySetDefaultDevice(device));
        }
    }

    [Cmdlet("Set", "CNTKExcludedDevice")]
    [Alias("cntk.setExcludedDevice")]
    public class SetCNTKExcludedDevice : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public DeviceDescriptor[] Devices;

        protected override void EndProcessing()
        {
            DeviceDescriptor.SetExcludedDevices(Devices);
        }
    }
}
