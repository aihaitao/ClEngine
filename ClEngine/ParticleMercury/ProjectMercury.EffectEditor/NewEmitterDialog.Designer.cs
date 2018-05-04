namespace ProjectMercury.EffectEditor
{
    partial class NewEmitterDialog
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			this.uxOK = new System.Windows.Forms.Button();
			this.uxCancel = new System.Windows.Forms.Button();
			this.uxBudget = new System.Windows.Forms.NumericUpDown();
			this.uxTerm = new System.Windows.Forms.NumericUpDown();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.uxBudget)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uxTerm)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(12, 8);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(89, 12);
			label1.TabIndex = 4;
			label1.Text = "发射器的预算：";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(9, 37);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(101, 12);
			label2.TabIndex = 5;
			label2.Text = "粒子期限（秒）：";
			// 
			// uxOK
			// 
			this.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.uxOK.Location = new System.Drawing.Point(198, 77);
			this.uxOK.Name = "uxOK";
			this.uxOK.Size = new System.Drawing.Size(75, 21);
			this.uxOK.TabIndex = 0;
			this.uxOK.Text = "确认";
			this.uxOK.UseVisualStyleBackColor = true;
			// 
			// uxCancel
			// 
			this.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.uxCancel.Location = new System.Drawing.Point(12, 77);
			this.uxCancel.Name = "uxCancel";
			this.uxCancel.Size = new System.Drawing.Size(75, 21);
			this.uxCancel.TabIndex = 1;
			this.uxCancel.Text = "取消";
			this.uxCancel.UseVisualStyleBackColor = true;
			// 
			// uxBudget
			// 
			this.uxBudget.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.uxBudget.Location = new System.Drawing.Point(153, 5);
			this.uxBudget.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
			this.uxBudget.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.uxBudget.Name = "uxBudget";
			this.uxBudget.Size = new System.Drawing.Size(120, 21);
			this.uxBudget.TabIndex = 2;
			this.uxBudget.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// uxTerm
			// 
			this.uxTerm.DecimalPlaces = 2;
			this.uxTerm.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
			this.uxTerm.Location = new System.Drawing.Point(153, 33);
			this.uxTerm.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.uxTerm.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.uxTerm.Name = "uxTerm";
			this.uxTerm.Size = new System.Drawing.Size(120, 21);
			this.uxTerm.TabIndex = 3;
			this.uxTerm.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// NewEmitterDialog
			// 
			this.AcceptButton = this.uxOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.uxCancel;
			this.ClientSize = new System.Drawing.Size(285, 109);
			this.Controls.Add(label2);
			this.Controls.Add(label1);
			this.Controls.Add(this.uxTerm);
			this.Controls.Add(this.uxBudget);
			this.Controls.Add(this.uxCancel);
			this.Controls.Add(this.uxOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewEmitterDialog";
			this.Text = "发射器属性";
			((System.ComponentModel.ISupportInitialize)(this.uxBudget)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uxTerm)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uxOK;
        private System.Windows.Forms.Button uxCancel;
        private System.Windows.Forms.NumericUpDown uxBudget;
        private System.Windows.Forms.NumericUpDown uxTerm;

    }
}