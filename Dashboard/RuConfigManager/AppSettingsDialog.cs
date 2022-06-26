using System;
using System.Windows.Forms;
using Dashboard;

namespace RuFramework.RuConfig
{
    public partial class AppSettingsDialog : Form
    {
        // The calling form (Form1) replace
        private Form1 settings = null;
        public AppSettingsDialog(Form1 f)
        {
            InitializeComponent();
            this.settings = f;

            /*
            // Insert this in the calling form (Form1)
            using RuFramework.RuConfig;
 
            public AppSettings appSettings = new AppSettings();

            private void buttonAppSettingsDialog_Click(object sender, EventArgs e)
            {  			
                AppSettingsDialog appSettingsDialog = new AppSettingsDialog(this);
                appSettingsDialog.Show();
            }
            */
            this.propertyGrid1.SelectedObject = this.settings.appSettings;
        }
    }
}
