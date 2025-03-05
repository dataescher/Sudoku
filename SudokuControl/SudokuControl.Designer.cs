
namespace Sudoku {
	partial class SudokuControl {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		internal System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		internal void InitializeComponent() {
			this.SuspendLayout();
			// 
			// Grid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "Grid";
			this.Size = new System.Drawing.Size(574, 444);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Grid_KeyPress);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDown);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Grid_PreviewKeyDown);
			this.Resize += new System.EventHandler(this.Grid_Resize);
			this.ResumeLayout(false);

		}

		#endregion
	}
}