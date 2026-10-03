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
            logoutToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { instrumentsToolStripMenuItem, instrumentTypesToolStripMenuItem, locationsToolStripMenuItem, areasToolStripMenuItem, proceduresToolStripMenuItem, calibrationsToolStripMenuItem, logoutToolStripMenuItem});
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 3, 0, 3);
            menuStrip1.Size = new Size(914, 30);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // instrumentsToolStripMenuItem
            // 
            instrumentsToolStripMenuItem.Name = "instrumentsToolStripMenuItem";
            instrumentsToolStripMenuItem.Size = new Size(99, 24);
            instrumentsToolStripMenuItem.Text = "Instruments";
            instrumentsToolStripMenuItem.Click += instrumentsToolStripMenuItem_Click;
            // 
            // instrumentTypesToolStripMenuItem
            // 
            instrumentTypesToolStripMenuItem.Name = "instrumentTypesToolStripMenuItem";
            instrumentTypesToolStripMenuItem.Size = new Size(132, 24);
            instrumentTypesToolStripMenuItem.Text = "Instrument types";
            instrumentTypesToolStripMenuItem.Click += instrumentTypesToolStripMenuItem_Click;
            // 
            // locationsToolStripMenuItem
            // 
            locationsToolStripMenuItem.Name = "locationsToolStripMenuItem";
            locationsToolStripMenuItem.Size = new Size(86, 24);
            locationsToolStripMenuItem.Text = "Locations";
            locationsToolStripMenuItem.Click += locationsToolStripMenuItem_Click;
            // 
            // areasToolStripMenuItem
            // 
            areasToolStripMenuItem.Name = "areasToolStripMenuItem";
            areasToolStripMenuItem.Size = new Size(60, 24);
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
            // logoutToolStripMenuItem
            // 
            logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            logoutToolStripMenuItem.Size = new Size(108, 24);
            logoutToolStripMenuItem.Text = "Cerrar sesión";
            logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
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
    }
}