using Sudoku.Engine.History;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace Sudoku.Engine {
	public partial class Grid : ICloneable {
		internal static readonly Int32 _defaultBoxWidth = 3;
		internal static readonly Int32 _defaultBoxHeight = 3;
		internal static readonly String _possibleCandidates = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
		public String Candidates { get; internal set; }
		public List<Cell> Cells { get; }
		public List<Box> Boxes { get; }
		public List<Row> Rows { get; }
		public List<Unit> Units { get; }
		public List<Column> Columns { get; }
		public PointF CellSize { get; }
		public UndoHistory UndoHistory { get; internal set; }
		public UndoItem CurrentUndoItem { get; internal set; }
		public Boolean CopyRawBoardOnly { get; set; }

		internal Size _size;
		public Size Size {
			get => _size;
			set {
				_size = value;
				AdjustFontSize();
			}
		}
		public Cell SelectedCell { get; set; }

		internal Font _font;

		public Font PencilMarkFont { get; internal set; }

		internal static Font GetAdjustedFont(Graphics g, String graphicString, Font originalFont, Size containerSize, Int32 maxFontSize, Int32 minFontSize, Boolean smallestOnFail, out SizeF newSize) {
			Font testFont = null;
			// We utilize MeasureString which we get via a control instance           
			for (Int32 adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--) {
				testFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);

				// Test the string with the new size
				newSize = g.MeasureString(graphicString, testFont);

				if (
					(containerSize.Width > Convert.ToInt32(newSize.Width)) &&
					(containerSize.Height > Convert.ToInt32(newSize.Height))
				) {
					// Good font, return it
					return testFont;
				}
			}

			// If you get here there was no fontsize that worked
			// return minimumSize or original?
			Font retval;
			if (smallestOnFail) {
				retval = testFont;
			} else {
				retval = originalFont;
			}
			newSize = g.MeasureString(graphicString, retval);
			return retval;
		}

		internal void AdjustFontSize() {
			using (Bitmap bmp = new(1, 1)) {
				using (Graphics gfx = Graphics.FromImage(bmp)) {
					Int32 dimension = _boxWidth * _boxHeight;
					Size expectedFontSize = new(_size.Width / dimension, _size.Height / dimension);
					Size expectedPencilMarkFontSize = new(_size.Width / (dimension * _boxWidth), _size.Height / (dimension * _boxHeight));
					gfx.PageUnit = GraphicsUnit.Pixel;
					_font = GetAdjustedFont(gfx, "0", _font, expectedFontSize, 80, 10, true, out SizeF cellCharSize);
					// Scale the font down for pencil marks for candidates
					PencilMarkFont = GetAdjustedFont(gfx, "0", _font, expectedPencilMarkFontSize, 30, 4, true, out SizeF pencilMarkCharSize);
					CellCharSize = cellCharSize;
					PencilMarkCharSize = pencilMarkCharSize;
				}
			}
		}

		public Font Font {
			get => _font;
			set {
				_font = value;
				AdjustFontSize();
			}
		}

		public SizeF CellCharSize { get; internal set; }
		public SizeF PencilMarkCharSize { get; internal set; }

		internal Int32 _boxWidth;
		public Int32 BoxWidth {
			get => _boxWidth;
			set {
				if (value > 1) {
					if (value != _boxWidth) {
						_boxWidth = value;
						CreateBoard();
					}
				}
			}
		}
		internal Int32 _boxHeight;
		public Int32 BoxHeight {
			get => _boxHeight;
			set {
				if (value > 1) {
					if (value != _boxHeight) {
						_boxHeight = value;
						CreateBoard();
					}
				}
			}
		}

		#region Constructors
		public Grid() : this(_defaultBoxWidth, _defaultBoxHeight) { }

		public Grid(Int32 boxWidth, Int32 boxHeight) {
			_boxWidth = boxWidth;
			_boxHeight = boxHeight;
			Font = new(FontFamily.GenericSerif, 16f, FontStyle.Bold);
			Size = new(500, 500);
			Cells = new();
			Boxes = new();
			Columns = new();
			Rows = new();
			Units = new();
			CreateBoard();
		}

		public Grid(Grid other) {
			_boxWidth = other._boxWidth;
			_boxHeight = other._boxHeight;
			_font = other._font;
			_size = other._size;
			PencilMarkFont = other.PencilMarkFont;
			CellCharSize = other.CellCharSize;
			PencilMarkCharSize = other.PencilMarkCharSize;
			Cells = new();
			Boxes = new();
			Columns = new();
			Rows = new();
			Units = new();
			foreach (Row thisRow in other.Rows) {
				Row duplicateRow = new(this, thisRow.Index, thisRow.Position);
				Rows.Add(duplicateRow);
				Units.Add(duplicateRow);
			}
			foreach (Column thisCol in other.Columns) {
				Column duplicateCol = new(this, thisCol.Index, thisCol.Position);
				Columns.Add(duplicateCol);
				Units.Add(duplicateCol);
			}
			foreach (Box thisBox in other.Boxes) {
				Box duplicateBox = new(this, thisBox.Index, thisBox.Position);
				Boxes.Add(duplicateBox);
				Units.Add(duplicateBox);
			}
			foreach (Cell cell in other.Cells) {
				Cell duplicateCell = new(this, cell.Position);
				Cells.Add(duplicateCell);
				foreach (KeyValuePair<Char, Cell.CandidateData> thisCandidate in cell._candidates) {
					duplicateCell._candidates.Add(thisCandidate.Key, thisCandidate.Value.Clone() as Cell.CandidateData);
				}
				duplicateCell.Location = cell.Location;
				duplicateCell.Size = cell.Size;
				duplicateCell._value = cell._value;
				duplicateCell.UserCell = cell.UserCell;
				Rows[cell.Row.Index - 1].Cells.Add(duplicateCell);
				Columns[cell.Column.Index - 1].Cells.Add(duplicateCell);
				Boxes[cell.Box.Index - 1].Cells.Add(duplicateCell);
			}
			Candidates = other.Candidates;
			PositionCells();
		}
		#endregion

		public Cell this[Int32 col, Int32 row] => Rows[row].Cells[col];

		internal Box GetBox(Int32 col, Int32 row) {
			return Boxes[(row * BoxHeight) + col];
		}

		public Cell this[Int32 index] {
			get {
				Int32 dimension = _boxWidth * _boxHeight;
				return this[index % dimension, index / dimension];
			}
		}

		public void PositionCells() {
			Int32 dimension = _boxWidth * _boxHeight;
			SizeF cellSize = new(Size.Width / dimension, Size.Height / dimension);
			foreach (Cell cell in Cells) {
				cell.Location = new(cell.Position.X * cellSize.Width, cell.Position.Y * cellSize.Height);
				cell.Size = cellSize;
			}
		}

		internal void CreateBoard() {
			Int32 dimension = _boxHeight * _boxWidth;
			Int32 area = dimension * dimension;
			// Clear all previous game data
			Cells.Clear();
			Boxes.Clear();
			Rows.Clear();
			Columns.Clear();
			Units.Clear();
			// Create and re-populate the unit lists. There is an equal amount of units of each type on the grid.
			for (Int32 unitIdx = 0; unitIdx < dimension; unitIdx++) {
				Rows.Add(new Row(this, unitIdx + 1, new Point(0, unitIdx)));
				Columns.Add(new Column(this, unitIdx + 1, new Point(unitIdx, 0)));
				Boxes.Add(new Box(this, unitIdx + 1, new Point(unitIdx % _boxHeight, unitIdx / _boxHeight)));
			}
			foreach (Row row in Rows) {
				Units.Add(row);
			}
			foreach (Column col in Columns) {
				Units.Add(col);
			}
			foreach (Box box in Boxes) {
				Units.Add(box);
			}

			// Re-populate the cells and unit lists
			for (Int32 cellIdx = 0; cellIdx < area; cellIdx++) {
				Point cellPosition = new(cellIdx % dimension, cellIdx / dimension);
				Cell thisCell = new(this, cellPosition);
				Cells.Add(thisCell);
				// The box height is the same as the number of boxes in a row, and
				// the box width is the same as the number of boxes in a column
				// Therefore, _boxWidth can be used to describe either
				Point gridPosition = new(cellPosition.X / _boxWidth, cellPosition.Y / _boxHeight);
				Int32 boxIdx = gridPosition.Y * _boxHeight + gridPosition.X;
				Rows[cellPosition.Y].Cells.Add(thisCell);
				Columns[cellPosition.X].Cells.Add(thisCell);
				Boxes[boxIdx].Cells.Add(thisCell);
			}
			PositionCells();
			Int32 numCandidates = _boxWidth * _boxHeight;
			if (numCandidates < 10) {
				// Make the grid characters 1-based
				Candidates = _possibleCandidates.Substring(1, numCandidates);
			} else {
				// Make the grid characters 0-based
				Candidates = _possibleCandidates.Substring(0, numCandidates);
			}
			// Clear any undo history
			UndoHistory = new();
			CurrentUndoItem = new();
		}

		public void GetText(StringBuilder sb) {
			Boolean useWhitespace = false;
			if ((_boxWidth != _defaultBoxWidth) || (_boxHeight != _defaultBoxHeight)) {
				sb.Append($"{_boxWidth}-{_boxHeight}:");
			}
			if (useWhitespace) {
				sb.Append(Environment.NewLine);
			}
			Int32 dimension = _boxHeight * _boxWidth;
			for (Int32 row = 0; row < dimension; row++) {
				for (Int32 col = 0; col < dimension; col++) {
					this[col, row].GetText(sb);
					if (useWhitespace) {
						if (col < (dimension - 1)) {
							sb.Append('\t');
						}
					}
				}
				if (useWhitespace) {
					if (row < (dimension - 1)) {
						sb.Append(Environment.NewLine);
					}
				}
			}
		}
		public void ClearUserEdits() {
			foreach (Box thisBox in Boxes) {
				foreach (Cell thisCell in thisBox.Cells) {
					if (thisCell.UserCell) {
						thisCell.Value = Cell.EmptyChar;
					}
					thisCell.ClearCandidates();
				}
			}
			PushUndoItem();
		}
		public void ClearAll() {
			foreach (Box thisBox in Boxes) {
				foreach (Cell thisCell in thisBox.Cells) {
					thisCell.Value = Cell.EmptyChar;
					thisCell.ClearCandidates();
				}
			}
			PushUndoItem();
		}
		public void ClearHighlighting() {
			foreach (Box thisBox in Boxes) {
				foreach (Cell thisCell in thisBox.Cells) {
					thisCell.Color = GameColors.White;
					foreach (Char thisCandidate in thisCell.Candidates) {
						thisCell.SetCandidateColor(thisCandidate, GameColors.White);
					}
				}
			}
			PushUndoItem();
		}
		public void HighlightCandidate(Char candidate, GameColors color) {
			foreach (Box thisBox in Boxes) {
				foreach (Cell thisCell in thisBox.Cells) {
					if (thisCell._value == Cell.EmptyChar) {
						if (thisCell.HasCandidate(candidate)) {
							thisCell.Color = color;
						}
					}
				}
			}
			PushUndoItem();
		}
		public void FromString(String boardString) {
			Regex regex = new(@"^(?<bw>[0-9]+)\s*-\s*(?<bh>[0-9]+)\s*:");
			Match match = regex.Match(boardString);
			Int32 gridStartPos;
			if (match.Success) {
				Int32 boxWidth = Int32.Parse(match.Groups["bw"].Value);
				Int32 boxHeight = Int32.Parse(match.Groups["bh"].Value);
				Int32 dimension = _boxHeight * _boxWidth;
				if (dimension < 2) {
					throw new Exception("Invalid grid setting. Cannot have zero dimension grid.");
				}
				if (dimension > _possibleCandidates.Length) {
					throw new Exception("Invalid grid setting. Not enough letters and numbers to cover any grid solutions.");
				}
				_boxWidth = boxWidth;
				_boxHeight = boxHeight;
				gridStartPos = match.Length;
			} else {
				// No grid dimensions provided
				_boxWidth = _defaultBoxWidth;
				_boxHeight = _defaultBoxHeight;
				gridStartPos = 0;
			}
			AdjustFontSize();
			CreateBoard();
			// Now parse the digits
			Int32 cellIdx = 0;
			Int32 gridDimension = _boxWidth * _boxHeight;
			Int32 gridArea = gridDimension * gridDimension;
			for (Int32 charPos = gridStartPos; charPos < boardString.Length; charPos++) {
				Char thisChar = boardString[charPos];
				if (Char.IsWhiteSpace(thisChar)) {
					// Ignore any whitespace characters
					continue;
				}
				if (cellIdx >= gridArea) {
					throw new Exception("Number of characters exceeds number of cells.");
				}
				Cell thisCell = this[cellIdx % gridDimension, cellIdx / gridDimension];
				// Ignore the character if white space
				if (thisChar == '.') {
					thisCell.Value = Cell.EmptyChar;
					thisCell.UserCell = true;
				} else if (!Candidates.Contains(thisChar.ToString())) {
					throw new Exception($"Unexpected character encountered at position {charPos}: {boardString[charPos]}");
				} else {
					thisCell.Value = thisChar;
					thisCell.UserCell = false;
				}
				thisCell.Color = GameColors.White;
				while (charPos < (boardString.Length - 1)) {
					// Look ahead to the next character and determine if it is a normal grid square or a qualifier
					Char nextChar = boardString[charPos + 1];
					if (_possibleCandidates.Contains(nextChar.ToString()) || (nextChar == '.')) {
						break;
					} else if (!Char.IsWhiteSpace(nextChar)) {
						thisCell.ProcessQualifier(nextChar);
					}
					charPos++;
				}
				cellIdx++;
			}
			while (cellIdx < gridArea) {
				Cell thisCell = this[cellIdx % gridDimension, cellIdx / gridDimension];
				thisCell.UserCell = true;
				thisCell.Color = GameColors.White;
				cellIdx++;
			}
			UndoHistory = new();
			CurrentUndoItem = new();
		}

		public void SelectPosition(Point position) {
			if ((position.X >= 0) && (position.Y >= 0)) {
				if ((position.X < (BoxHeight * BoxWidth)) && (position.Y < (BoxWidth * BoxHeight))) {
					Point selectedBoxPosition = new(position.X / BoxHeight, position.Y / BoxWidth);
					Int32 selectedBoxIndex = selectedBoxPosition.Y * BoxHeight + selectedBoxPosition.X;
					Point selectedCellPosition = new(position.X - selectedBoxPosition.X * BoxHeight, position.Y - selectedBoxPosition.Y * BoxWidth);
					Int32 selectedCellIndex = selectedCellPosition.Y * BoxWidth + selectedCellPosition.X;
					SelectedCell = Boxes[selectedBoxIndex].Cells[selectedCellIndex];
				}
			}
		}

		public override String ToString() {
			StringBuilder sb = new();
			GetText(sb);
			return sb.ToString();
		}

		#region Undo history routines
		public void PushUndoItem() {
			if (CurrentUndoItem != null) {
				if (CurrentUndoItem.Count > 0) {
					UndoHistory.Add(CurrentUndoItem);
					CurrentUndoItem = new();
				}
			}
		}

		public void ClearUndoHistory() {
			UndoHistory = new();
			CurrentUndoItem = new();
		}
		public void Undo() {
			UndoHistory.Undo();
			CurrentUndoItem = new();
		}
		public void Redo() {
			UndoHistory.Redo();
			CurrentUndoItem = new();
		}

		#endregion

		#region ICloneable Interface
		public Object Clone() {
			return new Grid(this);
		}
		#endregion
	}
}