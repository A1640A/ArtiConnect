using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Hugin
{
    public class OnDeviceChange
    {
        public const int WM_DEVICECHANGE = 0x219;

        public const int DBT_DEVICEARRIVAL = 0x00008000;

        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

        public const int DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;

        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;
        public const int DBT_DEVNODES_CHANGED = 0x0007;

        public static Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("{A5DCBF10-6530-11D2-901F-00C04FB951ED}");



        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]

        static extern IntPtr RegisterDeviceNotification(

            IntPtr hRecipient,

            ref DEV_BROADCAST_DEVICEINTERFACEW NotificationFilter,

            uint Flags);



        [StructLayout(LayoutKind.Sequential)]

        struct DEV_BROADCAST_HDR

        {

            public int dbch_Size;

            public int dbch_DeviceType;

            public int dbch_Reserved;

        }



        [StructLayout(LayoutKind.Sequential)]

        struct DEV_BROADCAST_DEVICEINTERFACEW

        {

            public DEV_BROADCAST_HDR hdr;

            public Guid dbcc_classguid;

            public char dbcc_name;

            public static readonly int Size = Marshal.SizeOf(typeof(DEV_BROADCAST_DEVICEINTERFACEW));

        }
        public static void RegisterUsbDeviceNotification(IntPtr Handle)

        {

            var filter = new DEV_BROADCAST_DEVICEINTERFACEW()

            {

                hdr = new DEV_BROADCAST_HDR()

                {

                    dbch_Size = DEV_BROADCAST_DEVICEINTERFACEW.Size,

                    dbch_DeviceType = DBT_DEVTYP_DEVICEINTERFACE,

                },

                dbcc_classguid = GUID_DEVINTERFACE_USB_DEVICE

            };



            IntPtr cookie = RegisterDeviceNotification(  // 1

                Handle,

                ref filter,

                DEVICE_NOTIFY_WINDOW_HANDLE);

        }

    }
}
