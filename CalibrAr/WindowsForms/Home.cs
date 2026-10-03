using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;

namespace WindowsForms
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            ConfigureMenuPermissions();
        }

        private void ConfigureMenuPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            // Verificar permiso para Instruments
            bool canViewInstruments = authService.HasPermission("Instruments.read");
            instrumentsToolStripMenuItem.Visible = canViewInstruments;

            // Verificar permiso para IntrumentTypes
            bool canViewInstrumentTypes = authService.HasPermission("InstrumentTypes.read");
            instrumentTypesToolStripMenuItem.Visible = canViewInstrumentTypes;

            // Verificar permiso para Locations
            bool canViewLocations = authService.HasPermission("Locations.read");
            locationsToolStripMenuItem.Visible = canViewLocations;

            // Verificar permiso para Areas
            bool canViewAreas = authService.HasPermission("Areas.read");
            areasToolStripMenuItem.Visible = canViewAreas;
        }

        private void instrumentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InstrumentList instrumentsForm = new InstrumentList();
            instrumentsForm.ShowDialog();
        }

        private void instrumentTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InstrumentTypeList instrumentTypesForm = new InstrumentTypeList();
            instrumentTypesForm.ShowDialog();
        }

        private void locationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocationList locationsForm = new LocationList();
            locationsForm.ShowDialog();
        }

        private void areasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AreaList areasForm = new AreaList();
            areasForm.ShowDialog();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private async void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await AuthServiceProvider.Instance.LogoutAsync();
            this.Close();
        }
    }
}
