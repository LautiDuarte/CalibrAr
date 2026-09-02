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

        private async void ConfigureMenuPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            // Verificar permiso para Instruments
            bool canViewInstruments = await authService.HasPermissionAsync("Instruments.read");
            instrumentsToolStripMenuItem.Visible = canViewInstruments;

            // Verificar permiso para IntrumentTypes
            bool canViewInstrumentTypes = await authService.HasPermissionAsync("InstrumentTypes.read");
            instrumentTypesToolStripMenuItem.Visible = canViewInstrumentTypes;

            // Verificar permiso para Locations
            bool canViewLocations = await authService.HasPermissionAsync("Locations.read");
            locationsToolStripMenuItem.Visible = canViewLocations;

            // Verificar permiso para Areas
            bool canViewAreas = await authService.HasPermissionAsync("Areas.read");
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
    }
}
