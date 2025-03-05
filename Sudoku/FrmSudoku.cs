using Sudoku.Engine.Logging;
using System;
using System.Windows.Forms;

namespace Sudoku {
	public partial class FrmSudoku : Form {
		public FrmSudoku() {
			InitializeComponent();
			DisplayPuzzles();
			SgBoard.FromString("");
		}

		private void LoadCategory(TreeNode node, GameData.Category category) {
			node.Tag = category;
			foreach (GameData.Category.Puzzle thisPuzzle in category.Puzzles) {
				TreeNode thisPuzzleNode = node.Nodes.Add(thisPuzzle.Description);
				thisPuzzleNode.Tag = thisPuzzle;
			}
			foreach (GameData.Category subCategory in category.Categories) {
				LoadCategory(node.Nodes.Add(subCategory.Description), subCategory);
			}
		}

		private void DisplayPuzzles() {
			TvPuzzles.Nodes.Clear();
			foreach (GameData.Category thisCategory in Program.Puzzles.Categories) {
				LoadCategory(TvPuzzles.Nodes.Add(thisCategory.Description), thisCategory);
			}
		}

		private void BtnCheck_Click(Object sender, EventArgs e) {
			Boolean checkGood = SgBoard.CheckComplete();
			SgBoard.Select();
		}

		private void FillAvailiableCandidates() {
			CboHighlightCandidates.Items.Clear();
			// Will need to do this every time a grid is loaded, since it could change the available characters
			foreach (Char thisCandidate in SgBoard.Grid.Candidates) {
				CboHighlightCandidates.Items.Add(thisCandidate);
			}
		}

		private void FrmSudoku_Load(Object sender, EventArgs e) {
			// Will need to do this every time a grid is loaded, since it could change the available characters
			foreach (Char thisCandidate in SgBoard.Grid.Candidates) {
				CboHighlightCandidates.Items.Add(thisCandidate);
			}
			FillAvailiableCandidates();
		}

		private void BtnSolve_Click(Object sender, EventArgs e) {
			SgBoard.Solve();
			SgBoard.Select();
		}

		private void CbCandidatesMode_CheckedChanged(Object sender, EventArgs e) {
			SgBoard.CandidatesMode = CbCandidatesMode.Checked;
			SgBoard.Select();
		}

		private void BtnClear_Click(Object sender, EventArgs e) {
			SgBoard.ClearUserEdits();
			SgBoard.Select();
		}

		private void BtnCandidates_Click(Object sender, EventArgs e) {
			SgBoard.GenerateCandidates();
			SgBoard.Select();
		}

		private void BtnCopy_Click(Object sender, EventArgs e) {
			Clipboard.SetText(SgBoard.ToString());
		}

		private void BtnPaste_Click(Object sender, EventArgs e) {
			try {
				SgBoard.FromString(Clipboard.GetText());
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			SgBoard.FromString(SgBoard.ToString());
			FillAvailiableCandidates();
			TvLog.Nodes.Clear();
		}

		private void BtnSolveCell_Click(Object sender, EventArgs e) {
			SgBoard.Solve(false);
			SgBoard.Select();
		}

		private void CboHighlightCandidates_SelectedIndexChanged(Object sender, EventArgs e) {
			SgBoard.HighlightCandidate((Char)CboHighlightCandidates.SelectedItem);
		}

		private void TvPuzzles_AfterSelect(Object sender, TreeViewEventArgs e) {
			if (e.Node.Tag is GameData.Category.Puzzle puzzle) {
				SgBoard.FromString(puzzle.Board);
			}
			SgBoard.Select();
			CbUserMode.Checked = true;
			FillAvailiableCandidates();
			TvLog.Nodes.Clear();
		}

		private void BtnBlank_Click(Object sender, EventArgs e) {
			SgBoard.ClearAll();
			SgBoard.Select();
		}

		private void CbUserMode_CheckedChanged(Object sender, EventArgs e) {
			SgBoard.UserMode = CbUserMode.Checked;
			SgBoard.Select();
		}

		private void BtnBruteForce_Click(Object sender, EventArgs e) {
			SgBoard.SolveBruteForce();
			SgBoard.Select();
		}

		private void BtnRandomPuzzle_Click(Object sender, EventArgs e) {
			SgBoard.GeneratePuzzle();
			SgBoard.Select();
		}

		private void BtnGeneratePuzzle_Click(Object sender, EventArgs e) {
			SgBoard.GeneratePuzzle();
			Application.DoEvents();
			String thisPuzzle = SgBoard.ToString();
			SgBoard.GenerateCandidates();
			SgBoard.Solve();
			Application.DoEvents();
		}

		private void SgBoard_LogItemAdded(Object sender, LogItemGroup log) {
			TreeNode thisLogNode = TvLog.Nodes.Add(log.Description);
			foreach (LogItem thisLogItem in log.Items) {
				TreeNode thisLogItemNode = thisLogNode.Nodes.Add(thisLogItem.Details.Description);
				thisLogItemNode.Tag = thisLogItem;
				AddLogDetail(thisLogItemNode, thisLogItem.Details, thisLogItem);
				thisLogItemNode.BackColor = thisLogItem.Color;
			}
			thisLogNode.Expand();
			TvLog.SelectedNode = thisLogNode;
		}

		private void AddLogDetail(TreeNode logNode, LogDetail details, Object tag) {
			if (logNode.Tag is null) {
				logNode.Tag = details;
			}
			foreach (LogDetail detail in details.SubDetails) {
				TreeNode thisLogNode = logNode.Nodes.Add(detail.Description);
				AddLogDetail(thisLogNode, detail, tag);
			}
		}

		private void TvLog_AfterSelect(Object sender, TreeViewEventArgs e) {
			if (e.Node is not null) {
				if (e.Node.Tag is LogItem logItem) {
					logItem.ShowItem(SgBoard.Grid);
					SgBoard.Invalidate();
				} else if (e.Node.Tag is LogDetail logDetail) {
					logDetail.ShowItem(SgBoard.Grid);
					SgBoard.Invalidate();
				}
			}
		}

		private void BtnClearUserEdits_Click(Object sender, EventArgs e) {
			SgBoard.ClearUserEdits();
		}

		private void BtnCopyGraphic_Click(Object sender, EventArgs e) {
			SgBoard.CopyGraphic();
		}

		private void CbCopyRawBoard_CheckedChanged(Object sender, EventArgs e) {
			SgBoard.CopyRawBoardOnly = CbUserMode.Checked;
			SgBoard.Select();
		}
	}
}