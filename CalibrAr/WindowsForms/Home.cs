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
using API.Auth.WindowsForms;

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

            bool canViewProcedures = authService.HasPermission("Procedures.read");
            proceduresToolStripMenuItem.Visible = canViewProcedures;

            bool canViewCalibrations = authService.HasPermission("Calibrations.read");
            calibrationsToolStripMenuItem.Visible = canViewCalibrations;

            bool canViewNonConformities = authService.HasPermission("NonConformities.read");
            nonConformitiesToolStripMenuItem.Visible = canViewNonConformities;

            bool canViewReferenceStandards = authService.HasPermission("ReferenceStandards.read");
            referenceStandardsToolStripMenuItem.Visible = canViewReferenceStandards;

            bool canViewInstrumentStatusHistory = authService.HasPermission("InstrumentStatusHistory.read");
            instrumentStatusHistoryToolStripMenuItem.Visible = canViewInstrumentStatusHistory;

            bool canViewUsers = authService.HasPermission("Users.read");
            usersToolStripMenuItem.Visible = canViewUsers;
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

        private void referenceStandardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChild<ReferenceStandardList>();
        }

        private void nonConformitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChild<NonConformityList>();
        }

        private void instrumentStatusHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChild<InstrumentStatusHistoryList>();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            OpenMdiChild<UserList>(); 
        }
        private void Home_Load(object sender, EventArgs e)
        {

        }

        private async void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await AuthServiceProvider.Instance.LogoutAsync();
            this.Close();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


    }
}
