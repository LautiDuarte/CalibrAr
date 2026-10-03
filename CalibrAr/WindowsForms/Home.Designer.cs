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
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { instrumentsToolStripMenuItem, instrumentTypesToolStripMenuItem, locationsToolStripMenuItem, areasToolStripMenuItem, proceduresToolStripMenuItem, calibrationsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
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
            // Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}