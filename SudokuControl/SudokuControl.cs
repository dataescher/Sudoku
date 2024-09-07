using Sudoku.Engine;
using Sudoku.Engine.Logging;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Sudoku {
	public partial class SudokuControl : UserControl {
		public Grid Grid { get; private set; }
		public Boolean CandidatesMode { get; set; }
		public Boolean UserMode { get; set; }
		public Boolean CopyRawBoardOnly {
			get => Grid.CopyRawBoardOnly;
			set => Grid.CopyRawBoardOnly = value;
		}

		[Bindable(true), Category(""), Description("")]
		public override Font Font {
			get => Grid.Font;
			set {
				base.Font = value;
				Grid.Font = value;
			}
		}

		public delegate void LogAddedEventHandler(Object sender, LogItemGroup log);
		internal LogAddedEventHandler _logItemAdded;
		public event LogAddedEventHandler LogItemAdded {
			add {
				_logItemAdded += value;
			}
			remove {
				_logItemAdded -= value;
			}
		}
		protected void AddLogItemGroup(LogItemGroup log) {
			_logItemAdded?.Invoke(this, log);
		}
		public SudokuControl() {
			Grid = new() {
				Font = base.Font
			};
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			InitializeComponent();
		}

		/// <summary>The bitmap.</summary>
		internal Bitmap _bmp;

		protected override void OnPaint(PaintEventArgs pevent) {
			_bmp = new Bitmap(Width, Height);
			Graphics gfx = Graphics.FromImage(_bmp);
			// Reset the control graphic
			foreach (Box thisBox in Grid.Boxes) {
				thisBox.Draw(gfx);
			}
			gfx.Dispose();
			if (_bmp is not null) {
				pevent.Graphics.DrawImage(_bmp, new Rectangle(0, 0, _bmp.Width, _bmp.Height));
			}
			base.OnPaint(pevent);
		}

		public Boolean CheckComplete() {
			LogItemGroup log = new("Completion Check");
			Boolean result = Grid.CheckComplete(log);
			AddLogItemGroup(log);
			return result;
		}

		internal void Redraw() {
			Int32 minDimension = Math.Min(Width, Height);
			Grid.Size = new(minDimension, minDimension);
			Grid.PositionCells();
			Invalidate();
		}

		internal void Grid_Resize(Object sender, EventArgs e) {
			Redraw();
		}

		public override String ToString() {
			return Grid.ToString();
		}
		public void FromString(String boardString) {
			Grid.FromString(boardString);
			UserMode = true;
			CandidateGenerator candidateGenerator = new(Grid);
			candidateGenerator.Solve(true, null, true);
			Invalidate();
		}

		internal void Grid_MouseDown(Object sender, MouseEventArgs e) {
			SizeF cellSize = new() {
				Width = Grid.Size.Width / (Grid.BoxWidth * Grid.BoxHeight),
				Height = Grid.Size.Height / (Grid.BoxWidth * Grid.BoxHeight)
			};
			// Determine which box the mouse down event occurred in
			Point location = new((Int32)(e.X / cellSize.Width), (Int32)(e.Y / cellSize.Height));
			// Select the appropriate cell
			if (location.X < Grid.Columns.Count) {
				if (location.Y < Grid.Columns.Count) {
					Grid[location.X, location.Y].Select();
					Invalidate();
				}
			}
		}
		public void CopyGraphic() {
			using (Bitmap bmp = new(Width, Height)) {
				DrawToBitmap(bmp, new Rectangle(new Point(0, 0), new Size(Width, Height)));
				Clipboard.SetImage(bmp);
			}
		}

		public void ClearUserEdits() {
			Grid.ClearUserEdits();
			CandidateGenerator candidateGenerator = new(Grid);
			candidateGenerator.Solve(true, null, true);
			foreach (Cell thisCell in Grid.Cells) {
				thisCell.Color = GameColors.White;
			}
			Grid.PushUndoItem();
			Invalidate();
		}

		public void GenerateCandidates() {
			LogItemGroup log = new("Resetting candidates");
			CandidateGenerator candidateGenerator = new(Grid);
			candidateGenerator.Solve(true, log, true);
			AddLogItemGroup(log);
			foreach (Cell thisCell in Grid.Cells) {
				thisCell.Color = GameColors.White;
			}
			Grid.PushUndoItem();
			Invalidate();
		}

		internal void Grid_KeyDown(Object sender, KeyEventArgs e) {
			if (Grid.SelectedCell is not null) {
				Point position = Grid.SelectedCell.Position;
				// Get the character typed, and check if it is a valid cell character
				switch (e.KeyCode) {
					case Keys.Left: {
						Grid.SelectPosition(new Point(position.X - 1, position.Y));
						e.Handled = true;
						Invalidate();
						break;
					}
					case Keys.Right: {
						Grid.SelectPosition(new Point(position.X + 1, position.Y));
						e.Handled = true;
						Invalidate();
						break;
					}
					case Keys.Up: {
						Grid.SelectPosition(new Point(position.X, position.Y - 1));
						e.Handled = true;
						Invalidate();
						break;
					}
					case Keys.Down: {
						Grid.SelectPosition(new Point(position.X, position.Y + 1));
						e.Handled = true;
						Invalidate();
						break;
					}
					case Keys.Back:
					case Keys.Delete: {
						if (Grid.SelectedCell is not null) {
							if (Grid.SelectedCell.UserCell || !UserMode) {
								Grid.SelectedCell.Value = Cell.EmptyChar;
								e.Handled = true;
								Invalidate();
								Grid.PushUndoItem();
							}
						}
						break;
					}
					case Keys.Escape: {
						Grid.SelectedCell = null;
						Invalidate();
						break;
					}
					default: {
						if (e.Modifiers.HasFlag(Keys.Shift)) {
							if (e.KeyCode != Keys.ShiftKey) {
								Char thisChar = Char.ToLower((Char)e.KeyValue);
								if (GameColor.ColorMap.Reverse.ContainsKey(thisChar)) {
									if (Grid.SelectedCell is not null) {
										e.Handled = true;
										Grid.SelectedCell.Color = GameColor.ColorMap.Reverse[thisChar];
										Invalidate();
										Grid.PushUndoItem();
									}
								}
							}
						} else if (e.Modifiers.HasFlag(Keys.Control)) {
							if (e.KeyCode != Keys.ControlKey) {
								switch ((Char)e.KeyValue) {
									case 'Y': {
										// Redo
										e.Handled = true;
										Grid.Redo();
										break;
									}
									case 'Z': {
										// Undo
										e.Handled = true;
										Grid.Undo();
										break;
									}
								}
							}
						}
						break;
					}
				}
			}
		}

		public void ClearAll() {
			Grid.ClearAll();
			Invalidate();
		}

		public void HighlightCandidate(Char candidate) {
			Grid.ClearHighlighting();
			Grid.HighlightCandidate(candidate, GameColors.DarkYellow);
			Grid.PushUndoItem();
			Invalidate();
		}

		public void Solve(Boolean solveMultiple = true) {
			LogItemGroup log = new();
			Grid.Solve(solveMultiple, log, true);
			AddLogItemGroup(log);
			Grid.PushUndoItem();
			Invalidate();
		}

		public void GeneratePuzzle() {
			Grid newGrid = Grid.GeneratePuzzle(Grid._boxWidth, Grid._boxHeight);
			newGrid._font = Grid._font;
			newGrid._size = Grid._size;
			newGrid.PencilMarkFont = Grid.PencilMarkFont;
			newGrid.CellCharSize = Grid.CellCharSize;
			newGrid.PencilMarkCharSize = Grid.PencilMarkCharSize;
			Grid = newGrid;
			Redraw();
			Grid.ClearUndoHistory();
			Invalidate();
		}

		public void SolveBruteForce() {
			LogItemGroup log = new();
			BruteForceSolveEngine bruteForceSolveEngine = new(Grid, 100, -1);
			bruteForceSolveEngine.Solve(true, log, true);
			AddLogItemGroup(log);
			Grid.PushUndoItem();
			Invalidate();
		}

		internal void Grid_KeyPress(Object sender, KeyPressEventArgs e) {
			if (Grid.SelectedCell is not null) {
				Char thisChar = Char.ToUpper(e.KeyChar);
				if (UserMode && (Grid.SelectedCell.UserCell || Grid.SelectedCell.Empty)) {
					if (CandidatesMode) {
						if (Grid.Candidates.Contains(thisChar.ToString())) {
							// Toggle the candidate
							if (Grid.SelectedCell.HasCandidate(thisChar)) {
								Grid.SelectedCell.RemoveCandidate(thisChar);
							} else {
								Grid.SelectedCell.AddCandidate(thisChar);
							}
							Invalidate();
							Grid.PushUndoItem();
						}
					} else {
						// User not allowed to overwrite cells which are given
						Grid.SelectedCell.UserCell = true;
						Grid.SelectedCell.Value = thisChar;
						Invalidate();
						Grid.PushUndoItem();
					}
				} else if (!(UserMode || CandidatesMode)) {
					// User not allowed to overwrite cells which are given
					Grid.SelectedCell.UserCell = false;
					Grid.SelectedCell.Value = thisChar;
					Invalidate();
					Grid.PushUndoItem();
				}
			}
		}

		internal void Grid_PreviewKeyDown(Object sender, PreviewKeyDownEventArgs e) {
			switch (e.KeyCode) {
				case Keys.Left:
				case Keys.Right:
				case Keys.Up:
				case Keys.Down:
				case Keys.Back:
				case Keys.Delete: {
					e.IsInputKey = true;
					break;
				}
				default: {
					e.IsInputKey = false;
					if (e.Modifiers.HasFlag(Keys.Shift)) {
						if (e.KeyCode != Keys.ControlKey) {
							e.IsInputKey = true;
						}
					} else if (e.Modifiers.HasFlag(Keys.Control)) {
						if (e.KeyCode != Keys.ControlKey) {
							e.IsInputKey = true;
						}
					}
					break;
				}
			}
		}
	}
}
