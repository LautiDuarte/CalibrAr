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
    public partial class NonConformityList : Form
    {
        private readonly NonConformityApiClient nonConformityApiClient = new(AuthServiceProvider.Instance);
        private bool canClose;

        public NonConformityList()
        {
            InitializeComponent();
            ConfigureColumns();
            EnumText.ApplyTo(nonConformitiesDataGridView, "Origin", "Status");
        }

        private void AddColumn(string name, string header, int width, string? format = null)
        {
            var column = new DataGridViewTextBoxColumn { Name = name, HeaderText = header, DataPropertyName = name, Width = width };
            if (format != null) column.DefaultCellStyle.Format = format;
            nonConformitiesDataGridView.Columns.Add(column);
        }

        private void ConfigureColumns()
        {
            nonConformitiesDataGridView.AutoGenerateColumns = false;
            AddColumn("Code", "Código", 100);
            AddColumn("InstrumentCode", "Instrumento", 100);
            AddColumn("CalibrationCertificateNumber", "Certificado de calibración", 150);
            AddColumn("Origin", "Origen", 140);
            AddColumn("Status", "Estado", 140);
            AddColumn("Description", "Descripción", 250);
            AddColumn("CorrectiveAction", "Acción correctiva", 200);
            AddColumn("OpenedAt", "Fecha de apertura", 130, "dd/MM/yyyy HH:mm");
            AddColumn("ClosedAt", "Fecha de cierre", 130, "dd/MM/yyyy HH:mm");
            AddColumn("DetectedByUserName", "Detectada por", 150);
            AddColumn("ClosedByUserName", "Cerrada por", 150);
            AddColumn("CreatedAt", "Fecha de alta", 130, "dd/MM/yyyy HH:mm");
            nonConformitiesDataGridView.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Close",
                HeaderText = string.Empty,
                Text = "Cerrar",
                UseColumnTextForButtonValue = true,
                Width = 70
            });
        }

        private async void NonConformityList_Load(object sender, EventArgs e)
        {
            ConfigureButtonPermissions();
            await LoadNonConformities();
        }

        private void ConfigureButtonPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            bool canCreate = authService.HasPermission("NonConformities.create");
            bool canUpdate = authService.HasPermission("NonConformities.update");
            bool canDelete = authService.HasPermission("NonConformities.delete");

            createButton.Visible = canCreate;
            updateButton.Visible = canUpdate;
            deleteButton.Visible = canDelete;

            // Cerrar es una actualización: mismo permiso que update
            canClose = canUpdate;
            nonConformitiesDataGridView.Columns["Close"].Visible = canUpdate;

            updateButton.Tag = canUpdate;
            deleteButton.Tag = canDelete;
        }

        private bool CanClose(NonConformityDTO nonConformity)
        {
            return canClose && !nonConformity.ClosedByUserId.HasValue;
        }

        private void nonConformitiesDataGridView_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (nonConformitiesDataGridView.Columns[e.ColumnIndex].Name != "Close") return;

            // Si no se puede cerrar, se pinta la celda vacía (el DataGridView no oculta botones por fila)
            if (nonConformitiesDataGridView.Rows[e.RowIndex].DataBoundItem is NonConformityDTO nonConformity && !CanClose(nonConformity))
            {
                e.PaintBackground(e.ClipBounds, (e.State & DataGridViewElementStates.Selected) != 0);
                e.Handled = true;
            }
        }

        private async void nonConformitiesDataGridView_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (nonConformitiesDataGridView.Columns[e.ColumnIndex].Name != "Close") return;
            if (nonConformitiesDataGridView.Rows[e.RowIndex].DataBoundItem is not NonConformityDTO nonConformity) return;
            if (!CanClose(nonConformity)) return;

            int? closerId = AuthServiceProvider.Instance.GetUserId();
            if (closerId == null)
            {
                MessageBox.Show("No se encontró el usuario actual. Revisá tu sesión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show(
                $"¿Cerrar la no conformidad {nonConformity.Code}?",
                "Confirm close", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK) return;

            try
            {
                DisableControls();
                await nonConformityApiClient.CloseAsync(nonConformity.Id, closerId.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo cerrar la no conformidad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await LoadNonConformities();
            }
        }

        private async void createButton_Click(object sender, EventArgs e)
        {
            NonConformityDetail detail = new NonConformityDetail(FormMode.Create, new NonConformityDTO());
            detail.ShowDialog(this);
            await LoadNonConformities();
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            NonConformityDTO? selected = SelectedItem();
            if (selected == null) return;
            try
            {
                DisableControls();
                NonConformityDTO nonConformity = await nonConformityApiClient.GetAsync(selected.Id);
                NonConformityDetail detail = new NonConformityDetail(FormMode.Update, nonConformity);
                detail.ShowDialog(this);
                await LoadNonConformities();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo modificar la no conformidad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            NonConformityDTO? nonConformity = SelectedItem();
            if (nonConformity == null) return;

            var result = MessageBox.Show($"¿Eliminar {nonConformity.Code}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                DisableControls();
                await nonConformityApiClient.DeleteAsync(nonConformity.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo eliminar la no conformidad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await LoadNonConformities();
            }
        }

        private async Task LoadNonConformities()
        {
            try
            {
                DisableControls();
                nonConformitiesDataGridView.DataSource = null;
                nonConformitiesDataGridView.DataSource = await nonConformityApiClient.GetAllAsync();

                bool canUpdate = updateButton.Tag is bool u && u;
                bool canDelete = deleteButton.Tag is bool d && d;
                bool hasRows = nonConformitiesDataGridView.Rows.Count > 0;

                if (hasRows) nonConformitiesDataGridView.Rows[0].Selected = true;
                if (canUpdate) updateButton.Enabled = hasRows;
                if (canDelete) deleteButton.Enabled = hasRows;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar las no conformidades: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private NonConformityDTO? SelectedItem()
        {
            if (nonConformitiesDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Primero seleccione una no conformidad.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            return (NonConformityDTO)nonConformitiesDataGridView.SelectedRows[0].DataBoundItem;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            nonConformitiesDataGridView.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            nonConformitiesDataGridView.Enabled = true;
        }
    }
}
