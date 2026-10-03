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
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            ConfigureMenuPermissions();
        }

        private void OpenMdiChild<T>() where T : Form, new()
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is T)
                {
                    child.Activate();
                    return;
                }
            }

            T form = new T();
            form.MdiParent = this;
            form.Show();
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

            bool canViewProcedures = await authService.HasPermissionAsync("Procedures.read");
            proceduresToolStripMenuItem.Visible = canViewProcedures;

            bool canViewCalibrations = await authService.HasPermissionAsync("Calibrations.read");
            calibrationsToolStripMenuItem.Visible = canViewCalibrations;


        }

        private void instrumentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChild<InstrumentList>();
        }

        private void proceduresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChild<ProcedureList>();
        }

        private void instrumentTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChild<InstrumentTypeList>();
        }

        private void locationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChild<LocationList>();
        }

        private void areasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChild<AreaList>();
        }

        private void calibrationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChild<CalibrationList>();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }
    }
}
