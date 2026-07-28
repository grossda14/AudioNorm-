using System;
using System.ComponentModel;

namespace AudioNormPlus.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer? components = null;

        /// <summary>
        /// InitializeComponent skeleton for non-designer builds.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            this.SuspendLayout();
            // Designer would normally add control initialization here.
            // The runtime SetupUI() method in MainForm creates and configures all controls.
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Dispose resources.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
