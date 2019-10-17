using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace GetMoniorInfo
{
    class GetMonitorInfo
    {
        //private void findBtn1_Click(object sender, EventArgs e)
        public List<string> getMoinitorInfo()
        {
            List<string> resultInfo = new List<string>();

            string[] deviceNames = new string[256];
            bool error = false;
            //Here I am listing all DisplayDevices (Monitors)
            int count1 = 0;
            for (int devId = 0; !error; devId++)
            {
                try
                {
                    WinApi.DISPLAY_DEVICE device = new WinApi.DISPLAY_DEVICE();
                    device.cb = Marshal.SizeOf(typeof(WinApi.DISPLAY_DEVICE));
                    error = !WinApi.User_32.EnumDisplayDevices(null, devId, ref device, 0);
                    //Console.WriteLine("DeviceStateFlag:" + device.StateFlags.ToString());
                    if ((device.StateFlags & WinApi.DisplayDeviceStateFlags.AttachedToDesktop) == WinApi.DisplayDeviceStateFlags.AttachedToDesktop)
                    {
                        Console.WriteLine("DeviceID:" + device.DeviceID.ToString());
                        Console.WriteLine("DeviceName:" + device.DeviceName.ToString());
                        Console.WriteLine("DeviceKey:" + device.DeviceKey.ToString());
                        //devices.Add(device);
                        deviceNames[count1] = device.DeviceName.ToString();
                        count1 += 1;
                    }

                }
                catch (Exception)
                {
                    error = true;
                }
            }

            WinApi.DEVMODE device2 = new WinApi.DEVMODE();
            int count = 0;
            foreach(string devName in deviceNames)
            {
                Console.WriteLine("devName:" + devName);
                if (devName == null) break;
                int num = WinApi.User_32.EnumDisplaySettings(devName, -1, ref device2);
                Console.WriteLine("DeviceNum:" + num.ToString());
                Console.WriteLine("DevName:" + device2.dmDeviceName.ToString());
                string width = device2.dmPelsWidth.ToString();
                string height = device2.dmPelsHeight.ToString();
                string frequency = device2.dmDisplayFrequency.ToString();
                Console.WriteLine("DevWidth:" + width);
                Console.WriteLine("DevHeight:" + height);
                Console.WriteLine("DevDisFrequency:" + frequency);

                resultInfo.Add(string.Format("{0}:{1}x{2}@{3}", count.ToString(),width, height, frequency));
                count += 1;
            }

            return resultInfo;
        }
        
    }
    public class WinApi
    {
        public const Int32 CCHDEVICENAME = 32;
        public const Int32 CCHFORMNAME = 32;

        public enum DEVMODE_SETTINGS
        {
            ENUM_CURRENT_SETTINGS = (-1),
            ENUM_REGISTRY_SETTINGS = (-2)
        }
        [Flags()]
        public enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        public enum Display_Device_Stateflags
        {
            DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x1,
            DISPLAY_DEVICE_MIRRORING_DRIVER = 0x8,
            DISPLAY_DEVICE_MODESPRUNED = 0x8000000,
            DISPLAY_DEVICE_MULTI_DRIVER = 0x2,
            DISPLAY_DEVICE_PRIMARY_DEVICE = 0x4,
            DISPLAY_DEVICE_VGA_COMPATIBLE = 0x10
        }

        public enum DeviceFlags
        {
            CDS_FULLSCREEN = 0x4,
            CDS_GLOBAL = 0x8,
            CDS_NORESET = 0x10000000,
            CDS_RESET = 0x40000000,
            CDS_SET_PRIMARY = 0x10,
            CDS_TEST = 0x2,
            CDS_UPDATEREGISTRY = 0x1,
            CDS_VIDEOPARAMETERS = 0x20,
        }

        public enum DEVMODE_Flags
        {
            DM_BITSPERPEL = 0x40000,
            DM_DISPLAYFLAGS = 0x200000,
            DM_DISPLAYFREQUENCY = 0x400000,
            DM_PELSHEIGHT = 0x100000,
            DM_PELSWIDTH = 0x80000,
            DM_POSITION = 0x20
        }

        public enum DisplaySetting_Results
        {
            DISP_CHANGE_BADFLAGS = -4,
            DISP_CHANGE_BADMODE = -2,
            DISP_CHANGE_BADPARAM = -5,
            DISP_CHANGE_FAILED = -1,
            DISP_CHANGE_NOTUPDATED = -3,
            DISP_CHANGE_RESTART = 1,
            DISP_CHANGE_SUCCESSFUL = 0
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINTL
        {
            [MarshalAs(UnmanagedType.I4)]
            public int x;
            [MarshalAs(UnmanagedType.I4)]
            public int y;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmSpecVersion;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmDriverVersion;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmSize;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmDriverExtra;

            [MarshalAs(UnmanagedType.U4)]
            public DEVMODE_Flags dmFields;

            public POINTL dmPosition;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayOrientation;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFixedOutput;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmColor;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmDuplex;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmYResolution;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmTTOption;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmLogPixels;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmBitsPerPel;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPelsWidth;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPelsHeight;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFlags;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFrequency;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmICMMethod;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmICMIntent;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmMediaType;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDitherType;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmReserved1;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmReserved2;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPanningWidth;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPanningHeight;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        public class User_32
        {
            [DllImport("user32.dll")]
            public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

            //[DllImport("user32.dll")]
            //public static extern int ChangeDisplaySettingsEx(ref DEVMODE devMode, int flags);

            [DllImport("user32.dll")]
            public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, [In] ref DEVMODE lpDevMode, IntPtr hwnd, int dwFlags, IntPtr lParam);

            [DllImport("user32.dll")]
            public static extern bool EnumDisplayDevices(string lpDevice, int iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, int dwFlags);

            [DllImport("user32.dll")]
            public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        }

    }

   
}
