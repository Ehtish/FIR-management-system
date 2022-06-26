using System;
using System.ComponentModel;
using System.Globalization;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using RuFramework.RuConfig;

namespace Dashboard
{
    public class AppSettings : EventArgs
    {

        public AppSettings()
        {
            // Path in the config is located
            // RuConfigManager.AppDataPath.Local   = C:\users\UserName\AppData\Local\ProductName\ProductName\ProductVersion\ProductName.Config
            // RuConfigManager.AppDataPath.Roaming = C:\users\UserName\AppData\Roaming\ProductName\ProductName\ProductVersion\ProductName.Config
            // RuConfigManager.AppDataPath.Common  = C:\ProgramData\ProductName\ProductName\ProductVersion\ProductName.Config
            // RuConfigManager.AppDataPath.ExePath = C:\Programs\ProductSaveDirectory\ProductName.Config
            RuConfigManager.Open(RuConfigManager.AppDataPath.Roaming);

            // User Filename in the config is locaded
            // RuConfigManager.Open("D:\\UserDirectory\\UsenName.config");
            Load();
        }
        ~AppSettings()
        {
            Save();//Data store, if class is destroyed.
        }

        // Insert RuConfigManager.Set and RuConfigManager..Get for all Properties in Save() and Load()
        private void Save()
        {
            // Example
            RuConfigManager.Set("MyAddress", MyAddress);
            RuConfigManager.Set("MyInteger", MyInteger);
        }
        private void Load()
        {
            // Example
            MyAddress = RuConfigManager.Get("MyAddress", myAddress);
            MyInteger = RuConfigManager.Get("MyInteger", myInteger);
        }

        // Example Property 
        private Address myAddress = new Address();
        [CategoryAttribute("Nested Property")]
        [Description("Example for a property of a user class with field description")]
        public Address MyAddress
        {
            set { myAddress = value; }
            get { return myAddress; }
        }

        // Example Property 
        int myInteger = 5;
        [CategoryAttribute("General property")]
        [Description("Integer in the range of 3 to 5, default = 5 ")]
        public int MyInteger
        {
            set { myInteger = value; }
            get { return myInteger; }
        }

    }
}
