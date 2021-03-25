using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpecToolGUI
{
    class Scanner
    {
        /// <summary>
        /// Quick and easy way to convery numeric strings of bits into a formated GB string
        /// </summary>
        /// <param name="str">
        /// Input Sting in (bits)
        /// </param>
        /// <returns>
        /// 
        /// returns formatted string
        /// </returns>
        string FormatToGB(string str)
        {

            string s = "";
            ulong cap = ulong.Parse(str);
            cap = cap / 1024 / 1024 / 1024;
            s = cap.ToString();
            s += ("GB");
            return s;
        }

      public  void ScanForStorageDevices(ref ListBox StorageBox, bool QuerySize, bool QueryModel)
        {
            ManagementObjectSearcher mgmtObjSearcherp = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            ManagementObjectCollection colProc = mgmtObjSearcherp.Get();
            StorageBox.Items.Clear();
            foreach (ManagementObject objDisk in colProc)
            {
                string storage = "";
                if (QuerySize)
                    storage += FormatToGB(objDisk["Size"].ToString());
                if (QueryModel)
                {
                    storage += " ";
                    storage += objDisk["Model"].ToString();
                }
               StorageBox.Items.Add(storage);
                //   Console.WriteLine("Device ID : {0}", objDisk["DeviceID"]);
            }

        }

      public  void ScanForRam(ref ListBox RAMBox, bool QueryCapacity, bool QuerySpeed)
        {
            ManagementObjectSearcher mgmtObjSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
            ManagementObjectCollection colDisks = mgmtObjSearcher.Get();
            RAMBox.Items.Clear();
            foreach (ManagementObject objDisk in colDisks)
            {
                string ram = "";
                if (objDisk["Capacity"] != null)
                {
                    if (QueryCapacity)
                    {
                        ram = FormatToGB(objDisk["Capacity"].ToString());

                    }


                }
                if (objDisk["Speed"] != null)
                {
                    if (QuerySpeed)
                    {
                        ram += " @ ";
                        ram += objDisk["Speed"].ToString();
                        ram += "Mhz";
                    }
                }

                RAMBox.Items.Add(ram);



                //   Console.WriteLine("Device ID : {0}", objDisk["DeviceID"]);
            }


        }
      public  void ScanForNetworkDevices(ref ListBox FeatureBox)
        {
            ManagementObjectSearcher mgmtObjSearcherp = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");
            ManagementObjectCollection colProc = mgmtObjSearcherp.Get();
            //     listBox2.Items.Clear();

            bool hasEthernet = false;
            bool hasWifi = false;

            foreach (ManagementObject objDisk in colProc)
            {
                hasWifi |= (objDisk["Name"].ToString().Contains("Wi-Fi")) ||

                     (objDisk["Name"].ToString().Contains("Wireless"));

                hasEthernet |=
                    (objDisk["Name"].ToString().Contains("Ethernet")) ||
                 (objDisk["Name"].ToString().Contains("PCIe")) ||
                  (objDisk["Name"].ToString().Contains("Realtek"));


                //   Console.WriteLine("Device ID : {0}", objDisk["DeviceID"]);
            }
            if (hasWifi)
                FeatureBox.Items.Add("Wifi");
            if (hasEthernet)
                FeatureBox.Items.Add("Ethernet");
        }

      public  void ScanForOS(ref TextBox OSBox)
        {

            ManagementObjectSearcher mgmtObjSearcherp = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectCollection colProc = mgmtObjSearcherp.Get();
            foreach (ManagementObject objDisk in colProc)
            {
                string os = objDisk["Caption"].ToString();
                if (os.StartsWith("Microsoft"))
                {
                    os = os.Remove(0, 10);
                }
                os += " - ";
                os += objDisk["OSArchitecture"].ToString();
                //   Console.WriteLine("Device ID : {0}", objDisk["DeviceID"]);
                OSBox.Text = os;
            }


        }
     public   void ScanForModel(ref TextBox ModelBox, ref TextBox FlyerNameBox)
        {
            ManagementObjectSearcher mgmtObjSearcherp = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            ManagementObjectCollection colProc = mgmtObjSearcherp.Get();
            foreach (ManagementObject objDisk in colProc)
            {

                ModelBox.Text = objDisk["Model"].ToString();

                string file_name = objDisk["Model"].ToString();

                file_name = file_name.Replace(" ", "_");
                FlyerNameBox.Text = file_name;
                //   Console.WriteLine("Device ID : {0}", objDisk["DeviceID"]);
            }

        }
     public   void ScanForCPU(ref TextBox CPUBox)
        {
            ManagementObjectSearcher mgmtObjSearcherp = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            ManagementObjectCollection colProc = mgmtObjSearcherp.Get();
            foreach (ManagementObject objDisk in colProc)
            {
                //Intel(R) Core(TM) 
                string cpu = objDisk["Name"].ToString();
                if (cpu.StartsWith("Intel(R) Core(TM)"))
                {
                    cpu = cpu.Remove(0, 18);

                }
                cpu = cpu.Replace("CPU @", "@");


                CPUBox.Text = cpu;
                //   Console.WriteLine("Device ID : {0}", objDisk["DeviceID"]);
            }
        }

     public   void ScanForGPU(ref TextBox GPUBox)
        {

            ManagementObjectSearcher mgmtObjSearcherp = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            ManagementObjectCollection colProc = mgmtObjSearcherp.Get();
            foreach (ManagementObject objDisk in colProc)
            {

                GPUBox.Text = objDisk["Name"].ToString();
                //   Console.WriteLine("Device ID : {0}", objDisk["DeviceID"]);
            }
        }
     public   void ScanForTouchScreen(ref ListBox FeatureBox)
        {

            ManagementObjectSearcher mgmtObjSearcherp = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
            ManagementObjectCollection colProc = mgmtObjSearcherp.Get();
            bool hasTouch = false;
            foreach (ManagementObject objDisk in colProc)
            {
                if (objDisk["Name"] != null)
                    hasTouch |= ((objDisk["Name"].ToString().Contains("Touchscreen")) || (objDisk["Name"].ToString().Contains("touch screen")));
                //   Console.WriteLine("Device ID : {0}", objDisk["DeviceID"]);
            }
            if (hasTouch)
            {
                FeatureBox.Items.Add("TouchScreen");

            }
        }


    public    void ScanForBluetooth(ref ListBox FeatureBox)
        {
            //

            //
            ManagementObjectSearcher mgmtObjSearcherp = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
            ManagementObjectCollection colProc = mgmtObjSearcherp.Get();
            bool hasBluetooth = false;
            foreach (ManagementObject objDisk in colProc)
            {
                if (objDisk["Name"] != null)
                {
                    hasBluetooth |= objDisk["Name"].ToString().StartsWith("Bluetooth");



                }
                //   Console.WriteLine("Device ID : {0}", objDisk["DeviceID"]);
            }
            if (hasBluetooth)
            {
                FeatureBox.Items.Add("Bluetooth");

            }
        }





    }
}
