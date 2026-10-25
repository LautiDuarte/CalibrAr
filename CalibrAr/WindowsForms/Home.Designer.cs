namespace WindowsForms
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            instrumentsToolStripMenuItem = new ToolStripMenuItem();
            instrumentTypesToolStripMenuItem = new ToolStripMenuItem();
            locationsToolStripMenuItem = new ToolStripMenuItem();
            areasToolStripMenuItem = new ToolStripMenuItem();
            proceduresToolStripMenuItem = new ToolStripMenuItem();
            calibrationsToolStripMenuItem = new ToolStripMenuItem();
            nonConformitiesToolStripMenuItem = new ToolStripMenuItem();
            referenceStandardsToolStripMenuItem = new ToolStripMenuItem();
            instrumentStatusHistoryToolStripMenuItem = new ToolStripMenuItem();
            logoutToolStripMenuItem = new ToolStripMenuItem();
            usersToolStripMenuItem = new ToolStripMenuItem();
            reportsToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { usersToolStripMenuItem, instrumentsToolStripMenuItem, instrumentTypesToolStripMenuItem, locationsToolStripMenuItem, areasToolStripMenuItem, proceduresToolStripMenuItem, calibrationsToolStripMenuItem, nonConformitiesToolStripMenuItem, referenceStandardsToolStripMenuItem, instrumentStatusHistoryToolStripMenuItem, reportsToolStripMenuItem, logoutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1013, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked;
            // 
            // instrumentsToolStripMenuItem
            // 
            instrumentsToolStripMenuItem.Name = "instrumentsToolStripMenuItem";
            instrumentsToolStripMenuItem.Size = new Size(82, 20);
            instrumentsToolStripMenuItem.Text = "Instruments";
            instrumentsToolStripMenuItem.Click += instrumentsToolStripMenuItem_Click;
            // 
            // instrumentTypesToolStripMenuItem
            // 
            instrumentTypesToolStripMenuItem.Name = "instrumentTypesToolStripMenuItem";
            instrumentTypesToolStripMenuItem.Size = new Size(108, 20);
            instrumentTypesToolStripMenuItem.Text = "Instrument types";
            instrumentTypesToolStripMenuItem.Click += instrumentTypesToolStripMenuItem_Click;
            // 
            // locationsToolStripMenuItem
            // 
            locationsToolStripMenuItem.Name = "locationsToolStripMenuItem";
            locationsToolStripMenuItem.Size = new Size(70, 20);
            locationsToolStripMenuItem.Text = "Locations";
            locationsToolStripMenuItem.Click += locationsToolStripMenuItem_Click;
            // 
            // areasToolStripMenuItem
            // 
            areasToolStripMenuItem.Name = "areasToolStripMenuItem";
            areasToolStripMenuItem.Size = new Size(48, 20);
            areasToolStripMenuItem.Text = "Areas";
            areasToolStripMenuItem.Click += areasToolStripMenuItem_Click;
            // 
            // proceduresToolStripMenuItem
            // 
            proceduresToolStripMenuItem.Name = "proceduresToolStripMenuItem";
            proceduresToolStripMenuItem.Size = new Size(78, 20);
            proceduresToolStripMenuItem.Text = "Procedures";
            proceduresToolStripMenuItem.Click += proceduresToolStripMenuItem_Click;
            // 
            // calibrationsToolStripMenuItem
            // 
            calibrationsToolStripMenuItem.Name = "calibrationsToolStripMenuItem";
            calibrationsToolStripMenuItem.Size = new Size(82, 20);
            calibrationsToolStripMenuItem.Text = "Calibrations";
            calibrationsToolStripMenuItem.Click += calibrationsToolStripMenuItem_Click;
            // 
            // nonConformitiesToolStripMenuItem
            // 
            nonConformitiesToolStripMenuItem.Name = "nonConformitiesToolStripMenuItem";
            nonConformitiesToolStripMenuItem.Size = new Size(112, 20);
            nonConformitiesToolStripMenuItem.Text = "Non conformities";
            nonConformitiesToolStripMenuItem.Click += nonConformitiesToolStripMenuItem_Click;
            // 
            // referenceStandardsToolStripMenuItem
            // 
            referenceStandardsToolStripMenuItem.Name = "referenceStandardsToolStripMenuItem";
            referenceStandardsToolStripMenuItem.Size = new Size(125, 20);
            referenceStandardsToolStripMenuItem.Text = "Reference standards";
            referenceStandardsToolStripMenuItem.Click += referenceStandardsToolStripMenuItem_Click;
            // 
            // instrumentStatusHistoryToolStripMenuItem
            // 
            instrumentStatusHistoryToolStripMenuItem.Name = "instrumentStatusHistoryToolStripMenuItem";
            instrumentStatusHistoryToolStripMenuItem.Size = new Size(90, 20);
            instrumentStatusHistoryToolStripMenuItem.Text = "Status history";
            instrumentStatusHistoryToolStripMenuItem.Click += instrumentStatusHistoryToolStripMenuItem_Click;
            // 
            // logoutToolStripMenuItem
            // 
            logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            logoutToolStripMenuItem.Size = new Size(87, 20);
            logoutToolStripMenuItem.Text = "Cerrar sesión";
            logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click;
            // 
            // usersToolStripMenuItem
            // 
            usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            usersToolStripMenuItem.Size = new Size(47, 20);
            usersToolStripMenuItem.Text = "Users";
            usersToolStripMenuItem.Click += usersToolStripMenuItem_Click;
            //
            // reportsToolStripMenuItem
            //
            reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            reportsToolStripMenuItem.Size = new Size(65, 20);
            reportsToolStripMenuItem.Text = "Reportes";
            reportsToolStripMenuItem.Click += reportsToolStripMenuItem_Click;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1013, 450);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Home";
            Text = "Home";
            Load += Home_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem instrumentsToolStripMenuItem;
        private ToolStripMenuItem instrumentTypesToolStripMenuItem;
        private ToolStripMenuItem locationsToolStripMenuItem;
        private ToolStripMenuItem areasToolStripMenuItem;
        private ToolStripMenuItem proceduresToolStripMenuItem;
        private ToolStripMenuItem calibrationsToolStripMenuItem;
        private ToolStripMenuItem logoutToolStripMenuItem;
        private ToolStripMenuItem nonConformitiesToolStripMenuItem;
        private ToolStripMenuItem referenceStandardsToolStripMenuItem;
        private ToolStripMenuItem instrumentStatusHistoryToolStripMenuItem;
        private ToolStripMenuItem usersToolStripMenuItem;
        private ToolStripMenuItem reportsToolStripMenuItem;
    }
}