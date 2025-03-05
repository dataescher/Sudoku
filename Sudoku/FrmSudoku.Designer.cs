
namespace Sudoku {
	partial class FrmSudoku {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.BtnCheck = new System.Windows.Forms.Button();
			this.BtnSolve = new System.Windows.Forms.Button();
			this.CbCandidatesMode = new System.Windows.Forms.CheckBox();
			this.BtnCandidates = new System.Windows.Forms.Button();
			this.TlpSudoku = new System.Windows.Forms.TableLayoutPanel();
			this.TlpControls = new System.Windows.Forms.TableLayoutPanel();
			this.GbControls = new System.Windows.Forms.GroupBox();
			this.BtnCopyGraphic = new System.Windows.Forms.Button();
			this.BtnClearUserEdits = new System.Windows.Forms.Button();
			this.BtnPaste = new System.Windows.Forms.Button();
			this.BtnGeneratePuzzle = new System.Windows.Forms.Button();
			this.BtnRandomPuzzle = new System.Windows.Forms.Button();
			this.BtnBruteForce = new System.Windows.Forms.Button();
			this.CbUserMode = new System.Windows.Forms.CheckBox();
			this.BtnBlank = new System.Windows.Forms.Button();
			this.CboHighlightCandidates = new System.Windows.Forms.ComboBox();
			this.BtnSolveCell = new System.Windows.Forms.Button();
			this.BtnCopy = new System.Windows.Forms.Button();
			this.TvPuzzles = new System.Windows.Forms.TreeView();
			this.SgBoard = new Sudoku.SudokuControl();
			this.GbLog = new System.Windows.Forms.GroupBox();
			this.TvLog = new System.Windows.Forms.TreeView();
			this.CbCopyRawBoard = new System.Windows.Forms.CheckBox();
			this.TlpSudoku.SuspendLayout();
			this.TlpControls.SuspendLayout();
			this.GbControls.SuspendLayout();
			this.GbLog.SuspendLayout();
			this.SuspendLayout();
			// 
			// BtnCheck
			// 
			this.BtnCheck.Location = new System.Drawing.Point(16, 117);
			this.BtnCheck.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnCheck.Name = "BtnCheck";
			this.BtnCheck.Size = new System.Drawing.Size(126, 63);
			this.BtnCheck.TabIndex = 3;
			this.BtnCheck.Text = "Check";
			this.BtnCheck.UseVisualStyleBackColor = true;
			this.BtnCheck.Click += new System.EventHandler(this.BtnCheck_Click);
			// 
			// BtnSolve
			// 
			this.BtnSolve.Location = new System.Drawing.Point(178, 439);
			this.BtnSolve.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnSolve.Name = "BtnSolve";
			this.BtnSolve.Size = new System.Drawing.Size(126, 63);
			this.BtnSolve.TabIndex = 4;
			this.BtnSolve.Text = "Solve Grid";
			this.BtnSolve.UseVisualStyleBackColor = true;
			this.BtnSolve.Click += new System.EventHandler(this.BtnSolve_Click);
			// 
			// CbCandidatesMode
			// 
			this.CbCandidatesMode.AutoSize = true;
			this.CbCandidatesMode.Location = new System.Drawing.Point(17, 287);
			this.CbCandidatesMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.CbCandidatesMode.Name = "CbCandidatesMode";
			this.CbCandidatesMode.Size = new System.Drawing.Size(160, 24);
			this.CbCandidatesMode.TabIndex = 5;
			this.CbCandidatesMode.Text = "Candidates Mode";
			this.CbCandidatesMode.UseVisualStyleBackColor = true;
			this.CbCandidatesMode.CheckedChanged += new System.EventHandler(this.CbCandidatesMode_CheckedChanged);
			// 
			// BtnCandidates
			// 
			this.BtnCandidates.Location = new System.Drawing.Point(178, 216);
			this.BtnCandidates.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnCandidates.Name = "BtnCandidates";
			this.BtnCandidates.Size = new System.Drawing.Size(126, 63);
			this.BtnCandidates.TabIndex = 7;
			this.BtnCandidates.Text = "Reset Candidates";
			this.BtnCandidates.UseVisualStyleBackColor = true;
			this.BtnCandidates.Click += new System.EventHandler(this.BtnCandidates_Click);
			// 
			// TlpSudoku
			// 
			this.TlpSudoku.ColumnCount = 3;
			this.TlpSudoku.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 338F));
			this.TlpSudoku.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TlpSudoku.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 338F));
			this.TlpSudoku.Controls.Add(this.TlpControls, 2, 0);
			this.TlpSudoku.Controls.Add(this.SgBoard, 1, 0);
			this.TlpSudoku.Controls.Add(this.GbLog, 0, 0);
			this.TlpSudoku.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TlpSudoku.Location = new System.Drawing.Point(0, 0);
			this.TlpSudoku.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.TlpSudoku.Name = "TlpSudoku";
			this.TlpSudoku.RowCount = 1;
			this.TlpSudoku.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TlpSudoku.Size = new System.Drawing.Size(1884, 1508);
			this.TlpSudoku.TabIndex = 8;
			// 
			// TlpControls
			// 
			this.TlpControls.ColumnCount = 1;
			this.TlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TlpControls.Controls.Add(this.GbControls, 0, 0);
			this.TlpControls.Controls.Add(this.TvPuzzles, 0, 1);
			this.TlpControls.Location = new System.Drawing.Point(1549, 3);
			this.TlpControls.Name = "TlpControls";
			this.TlpControls.RowCount = 2;
			this.TlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TlpControls.Size = new System.Drawing.Size(332, 1502);
			this.TlpControls.TabIndex = 4;
			// 
			// GbControls
			// 
			this.GbControls.Controls.Add(this.CbCopyRawBoard);
			this.GbControls.Controls.Add(this.BtnCopyGraphic);
			this.GbControls.Controls.Add(this.BtnClearUserEdits);
			this.GbControls.Controls.Add(this.BtnPaste);
			this.GbControls.Controls.Add(this.BtnGeneratePuzzle);
			this.GbControls.Controls.Add(this.BtnRandomPuzzle);
			this.GbControls.Controls.Add(this.BtnBruteForce);
			this.GbControls.Controls.Add(this.CbUserMode);
			this.GbControls.Controls.Add(this.BtnBlank);
			this.GbControls.Controls.Add(this.CboHighlightCandidates);
			this.GbControls.Controls.Add(this.BtnSolveCell);
			this.GbControls.Controls.Add(this.BtnCopy);
			this.GbControls.Controls.Add(this.BtnCheck);
			this.GbControls.Controls.Add(this.BtnCandidates);
			this.GbControls.Controls.Add(this.BtnSolve);
			this.GbControls.Controls.Add(this.CbCandidatesMode);
			this.GbControls.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbControls.Location = new System.Drawing.Point(4, 5);
			this.GbControls.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.GbControls.Name = "GbControls";
			this.GbControls.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.GbControls.Size = new System.Drawing.Size(324, 741);
			this.GbControls.TabIndex = 3;
			this.GbControls.TabStop = false;
			this.GbControls.Text = "Game Controls";
			// 
			// BtnCopyGraphic
			// 
			this.BtnCopyGraphic.Location = new System.Drawing.Point(178, 289);
			this.BtnCopyGraphic.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnCopyGraphic.Name = "BtnCopyGraphic";
			this.BtnCopyGraphic.Size = new System.Drawing.Size(126, 63);
			this.BtnCopyGraphic.TabIndex = 18;
			this.BtnCopyGraphic.Text = "Copy Graphic";
			this.BtnCopyGraphic.UseVisualStyleBackColor = true;
			this.BtnCopyGraphic.Click += new System.EventHandler(this.BtnCopyGraphic_Click);
			// 
			// BtnClearUserEdits
			// 
			this.BtnClearUserEdits.Location = new System.Drawing.Point(178, 117);
			this.BtnClearUserEdits.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnClearUserEdits.Name = "BtnClearUserEdits";
			this.BtnClearUserEdits.Size = new System.Drawing.Size(126, 63);
			this.BtnClearUserEdits.TabIndex = 17;
			this.BtnClearUserEdits.Text = "Clear User Edits";
			this.BtnClearUserEdits.UseVisualStyleBackColor = true;
			this.BtnClearUserEdits.Click += new System.EventHandler(this.BtnClearUserEdits_Click);
			// 
			// BtnPaste
			// 
			this.BtnPaste.Location = new System.Drawing.Point(178, 366);
			this.BtnPaste.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnPaste.Name = "BtnPaste";
			this.BtnPaste.Size = new System.Drawing.Size(126, 63);
			this.BtnPaste.TabIndex = 16;
			this.BtnPaste.Text = "Paste";
			this.BtnPaste.UseVisualStyleBackColor = true;
			this.BtnPaste.Click += new System.EventHandler(this.BtnPaste_Click);
			// 
			// BtnGeneratePuzzle
			// 
			this.BtnGeneratePuzzle.Location = new System.Drawing.Point(16, 512);
			this.BtnGeneratePuzzle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnGeneratePuzzle.Name = "BtnGeneratePuzzle";
			this.BtnGeneratePuzzle.Size = new System.Drawing.Size(126, 63);
			this.BtnGeneratePuzzle.TabIndex = 15;
			this.BtnGeneratePuzzle.Text = "Generate Puzzle";
			this.BtnGeneratePuzzle.UseVisualStyleBackColor = true;
			this.BtnGeneratePuzzle.Click += new System.EventHandler(this.BtnGeneratePuzzle_Click);
			// 
			// BtnRandomPuzzle
			// 
			this.BtnRandomPuzzle.Location = new System.Drawing.Point(178, 29);
			this.BtnRandomPuzzle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnRandomPuzzle.Name = "BtnRandomPuzzle";
			this.BtnRandomPuzzle.Size = new System.Drawing.Size(126, 63);
			this.BtnRandomPuzzle.TabIndex = 14;
			this.BtnRandomPuzzle.Text = "Random Puzzle";
			this.BtnRandomPuzzle.UseVisualStyleBackColor = true;
			this.BtnRandomPuzzle.Click += new System.EventHandler(this.BtnRandomPuzzle_Click);
			// 
			// BtnBruteForce
			// 
			this.BtnBruteForce.Location = new System.Drawing.Point(178, 512);
			this.BtnBruteForce.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnBruteForce.Name = "BtnBruteForce";
			this.BtnBruteForce.Size = new System.Drawing.Size(126, 63);
			this.BtnBruteForce.TabIndex = 13;
			this.BtnBruteForce.Text = "Brute Force Solve";
			this.BtnBruteForce.UseVisualStyleBackColor = true;
			this.BtnBruteForce.Click += new System.EventHandler(this.BtnBruteForce_Click);
			// 
			// CbUserMode
			// 
			this.CbUserMode.AutoSize = true;
			this.CbUserMode.Location = new System.Drawing.Point(16, 313);
			this.CbUserMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.CbUserMode.Name = "CbUserMode";
			this.CbUserMode.Size = new System.Drawing.Size(113, 24);
			this.CbUserMode.TabIndex = 12;
			this.CbUserMode.Text = "User Mode";
			this.CbUserMode.UseVisualStyleBackColor = true;
			this.CbUserMode.CheckedChanged += new System.EventHandler(this.CbUserMode_CheckedChanged);
			// 
			// BtnBlank
			// 
			this.BtnBlank.Location = new System.Drawing.Point(17, 29);
			this.BtnBlank.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnBlank.Name = "BtnBlank";
			this.BtnBlank.Size = new System.Drawing.Size(126, 63);
			this.BtnBlank.TabIndex = 11;
			this.BtnBlank.Text = "Blank";
			this.BtnBlank.UseVisualStyleBackColor = true;
			this.BtnBlank.Click += new System.EventHandler(this.BtnBlank_Click);
			// 
			// CboHighlightCandidates
			// 
			this.CboHighlightCandidates.FormattingEnabled = true;
			this.CboHighlightCandidates.Location = new System.Drawing.Point(7, 474);
			this.CboHighlightCandidates.Name = "CboHighlightCandidates";
			this.CboHighlightCandidates.Size = new System.Drawing.Size(125, 28);
			this.CboHighlightCandidates.TabIndex = 10;
			this.CboHighlightCandidates.SelectedIndexChanged += new System.EventHandler(this.CboHighlightCandidates_SelectedIndexChanged);
			// 
			// BtnSolveCell
			// 
			this.BtnSolveCell.Location = new System.Drawing.Point(16, 216);
			this.BtnSolveCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnSolveCell.Name = "BtnSolveCell";
			this.BtnSolveCell.Size = new System.Drawing.Size(126, 63);
			this.BtnSolveCell.TabIndex = 9;
			this.BtnSolveCell.Text = "Solve Cell";
			this.BtnSolveCell.UseVisualStyleBackColor = true;
			this.BtnSolveCell.Click += new System.EventHandler(this.BtnSolveCell_Click);
			// 
			// BtnCopy
			// 
			this.BtnCopy.Location = new System.Drawing.Point(17, 366);
			this.BtnCopy.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BtnCopy.Name = "BtnCopy";
			this.BtnCopy.Size = new System.Drawing.Size(126, 63);
			this.BtnCopy.TabIndex = 8;
			this.BtnCopy.Text = "Copy";
			this.BtnCopy.UseVisualStyleBackColor = true;
			this.BtnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
			// 
			// TvPuzzles
			// 
			this.TvPuzzles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TvPuzzles.HideSelection = false;
			this.TvPuzzles.Location = new System.Drawing.Point(3, 754);
			this.TvPuzzles.Name = "TvPuzzles";
			this.TvPuzzles.Size = new System.Drawing.Size(326, 745);
			this.TvPuzzles.TabIndex = 4;
			this.TvPuzzles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvPuzzles_AfterSelect);
			// 
			// SgBoard
			// 
			this.SgBoard.CandidatesMode = false;
			this.SgBoard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SgBoard.Location = new System.Drawing.Point(344, 6);
			this.SgBoard.Margin = new System.Windows.Forms.Padding(6);
			this.SgBoard.Name = "SgBoard";
			this.SgBoard.Size = new System.Drawing.Size(1196, 1496);
			this.SgBoard.TabIndex = 2;
			this.SgBoard.UserMode = true;
			this.SgBoard.LogItemAdded += new Sudoku.SudokuControl.LogAddedEventHandler(this.SgBoard_LogItemAdded);
			// 
			// GbLog
			// 
			this.GbLog.Controls.Add(this.TvLog);
			this.GbLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbLog.Location = new System.Drawing.Point(3, 3);
			this.GbLog.Name = "GbLog";
			this.GbLog.Size = new System.Drawing.Size(332, 1502);
			this.GbLog.TabIndex = 5;
			this.GbLog.TabStop = false;
			this.GbLog.Text = "Log";
			// 
			// TvLog
			// 
			this.TvLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TvLog.HideSelection = false;
			this.TvLog.Location = new System.Drawing.Point(3, 22);
			this.TvLog.Name = "TvLog";
			this.TvLog.Size = new System.Drawing.Size(326, 1477);
			this.TvLog.TabIndex = 0;
			this.TvLog.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvLog_AfterSelect);
			// 
			// CbCopyRawBoard
			// 
			this.CbCopyRawBoard.AutoSize = true;
			this.CbCopyRawBoard.Location = new System.Drawing.Point(16, 338);
			this.CbCopyRawBoard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.CbCopyRawBoard.Name = "CbCopyRawBoard";
			this.CbCopyRawBoard.Size = new System.Drawing.Size(154, 24);
			this.CbCopyRawBoard.TabIndex = 19;
			this.CbCopyRawBoard.Text = "Copy Raw Board";
			this.CbCopyRawBoard.UseVisualStyleBackColor = true;
			this.CbCopyRawBoard.CheckedChanged += new System.EventHandler(this.CbCopyRawBoard_CheckedChanged);
			// 
			// FrmSudoku
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1884, 1508);
			this.Controls.Add(this.TlpSudoku);
			this.KeyPreview = true;
			this.Name = "FrmSudoku";
			this.Text = "Sudoku";
			this.Load += new System.EventHandler(this.FrmSudoku_Load);
			this.TlpSudoku.ResumeLayout(false);
			this.TlpControls.ResumeLayout(false);
			this.GbControls.ResumeLayout(false);
			this.GbControls.PerformLayout();
			this.GbLog.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private SudokuControl SgBoard;
		private System.Windows.Forms.Button BtnCheck;
		private System.Windows.Forms.Button BtnSolve;
		private System.Windows.Forms.CheckBox CbCandidatesMode;
		private System.Windows.Forms.Button BtnCandidates;
		private System.Windows.Forms.TableLayoutPanel TlpSudoku;
		private System.Windows.Forms.GroupBox GbControls;
		private System.Windows.Forms.Button BtnCopy;
		private System.Windows.Forms.Button BtnSolveCell;
		private System.Windows.Forms.ComboBox CboHighlightCandidates;
		private System.Windows.Forms.TableLayoutPanel TlpControls;
		private System.Windows.Forms.TreeView TvPuzzles;
		private System.Windows.Forms.Button BtnBlank;
		private System.Windows.Forms.CheckBox CbUserMode;
		private System.Windows.Forms.GroupBox GbLog;
		private System.Windows.Forms.TreeView TvLog;
		private System.Windows.Forms.Button BtnBruteForce;
		private System.Windows.Forms.Button BtnRandomPuzzle;
		private System.Windows.Forms.Button BtnGeneratePuzzle;
		private System.Windows.Forms.Button BtnPaste;
		private System.Windows.Forms.Button BtnClearUserEdits;
		private System.Windows.Forms.Button BtnCopyGraphic;
		private System.Windows.Forms.CheckBox CbCopyRawBoard;
	}
}