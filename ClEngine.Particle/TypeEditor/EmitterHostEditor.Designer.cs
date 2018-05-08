namespace ClEngine.Particle.TypeEditor
{
	partial class EmitterHostEditor
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
			this.elementHost = new System.Windows.Forms.Integration.ElementHost();
			this.SuspendLayout();
			// 
			// elementHost
			// 
			this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.elementHost.Location = new System.Drawing.Point(0, 0);
			this.elementHost.Name = "elementHost";
			this.elementHost.Size = new System.Drawing.Size(504, 561);
			this.elementHost.TabIndex = 0;
			this.elementHost.Text = "elementHost1";
			this.elementHost.Child = null;
			// 
			// EmitterHostEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(504, 561);
			this.Controls.Add(this.elementHost);
			this.Name = "EmitterHostEditor";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "发射器编辑器";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Integration.ElementHost elementHost;
	}
}