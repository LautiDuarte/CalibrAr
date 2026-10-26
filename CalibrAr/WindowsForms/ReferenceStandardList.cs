using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using API.Auth.WindowsForms;
using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class ReferenceStandardList : Form
    {
        private readonly ReferenceStandardApiClient referenceStandardApiClient = new(AuthServiceProvider.Instance);

        public ReferenceStandardList()
        {
            InitializeComponent();
            ConfigureColumns();
        }

        private void AddColumn(string name, string header, int width, string? format = null)
        {
            var column = new DataGridViewTextBoxColumn { Name = name, HeaderText = header, DataPropertyName = name, Width = width };
            if (format != null) column.DefaultCellStyle.Format = format;
            referenceStandardsDataGridView.Columns.Add(column);
        }

        private void ConfigureColumns()
        {
            referenceStandardsDataGridView.AutoGenerateColumns = false;
            AddColumn("Description", "Descripción", 250);
            AddColumn("CertifyingBody", "Organismo certificante", 150);
            AddColumn("CertificateNumber", "N° de certificado", 150);
            AddColumn("CertificateIssuedAt", "Emisión", 100, "dd/MM/yyyy");
            AddColumn("CertificateExpiresAt", "Vencimiento", 100, "dd/MM/yyyy");
            AddColumn("CreatedAt", "Fecha de alta", 130, "dd/MM/yyyy HH:mm");
        }

        private async void ReferenceStandardList_Load(object sender, EventArgs e)
        {
            ConfigureButtonPermissions();
            await LoadReferenceStandards();
        }

        private void ConfigureButtonPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            bool canCreate = authService.HasPermission("ReferenceStandards.create");
            bool canUpdate = authService.HasPermission("ReferenceStandards.update");
            bool canDelete = authService.HasPermission("ReferenceStandards.delete");

            createButton.Visible = canCreate;
            updateButton.Visible = canUpdate;
            deleteButton.Visible = canDelete;

            updateButton.Tag = canUpdate;
            deleteButton.Tag = canDelete;
        }

        private async void createButton_Click(object sender, EventArgs e)
        {
            ReferenceStandardDetail detail = new ReferenceStandardDetail(FormMode.Create, new ReferenceStandardDTO());
            detail.ShowDialog(this);
            await LoadReferenceStandards();
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            ReferenceStandardDTO? selected = SelectedItem();
            if (selected == null) return;
            try
            {
                DisableControls();
                ReferenceStandardDTO referenceStandard = await referenceStandardApiClient.GetAsync(selected.Id);
                ReferenceStandardDetail detail = new ReferenceStandardDetail(FormMode.Update, referenceStandard);
                detail.ShowDialog(this);
                await LoadReferenceStandards();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo modificar el patrón de referencia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            ReferenceStandardDTO? referenceStandard = SelectedItem();
            if (referenceStandard == null) return;

            var result = MessageBox.Show($"¿Eliminar {referenceStandard.Description}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                DisableControls();
                await referenceStandardApiClient.DeleteAsync(referenceStandard.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo eliminar el patrón de referencia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await LoadReferenceStandards();
            }
        }

        private async Task LoadReferenceStandards()
        {
            try
            {
                DisableControls();
                referenceStandardsDataGridView.DataSource = null;
                referenceStandardsDataGridView.DataSource = await referenceStandardApiClient.GetAllAsync();

                bool canUpdate = updateButton.Tag is bool u && u;
                bool canDelete = deleteButton.Tag is bool d && d;
                bool hasRows = referenceStandardsDataGridView.Rows.Count > 0;

                if (hasRows) referenceStandardsDataGridView.Rows[0].Selected = true;
                if (canUpdate) updateButton.Enabled = hasRows;
                if (canDelete) deleteButton.Enabled = hasRows;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar los patrones de referencia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private ReferenceStandardDTO? SelectedItem()
        {
            if (referenceStandardsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Primero seleccione un patrón de referencia.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            return (ReferenceStandardDTO)referenceStandardsDataGridView.SelectedRows[0].DataBoundItem;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            referenceStandardsDataGridView.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            referenceStandardsDataGridView.Enabled = true;
        }
    }
}
