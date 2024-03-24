using Sudoku.Engine.History;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sudoku.Engine {
	public class Cell {
		internal class CandidateData : ICloneable {
			public GameColors Color { get; set; }
			public CandidateData() {
				Color = GameColors.White;
			}

			public Object Clone() {
				return new CandidateData() {
					Color = Color
				};
			}
		}
		public static readonly Char EmptyChar = '\0';
		internal readonly Dictionary<Char, CandidateData> _candidates;
		public Boolean HasCandidate(Char candidate) {
			return _candidates.ContainsKey(candidate);
		}

		public void RemoveCandidate(Char candidate) {
			if (HasCandidate(candidate)) {
				Grid.CurrentUndoItem?.Add(new AddRemoveCandidateAction(this, candidate, true));
				_ = _candidates.Remove(candidate);
			}
		}
		public void AddCandidate(Char candidate) {
			if (!HasCandidate(candidate)) {
				if (Grid.Candidates.Contains(candidate)) {
					Grid.CurrentUndoItem?.Add(new AddRemoveCandidateAction(this, candidate, false));
					_candidates.Add(candidate, new CandidateData());
				}
			}
		}
		public void SetCandidateColor(Char candidate, GameColors color) {
			if (_candidates.TryGetValue(candidate, out CandidateData candidateData)) {
				candidateData.Color = color;
			}
		}
		public Int32 CandidateCount => _candidates.Count;
		public void ClearCandidates() {
			_candidates.Clear();
		}
		public List<Char> Candidates => _candidates.Keys.ToList();
		internal Box _box;
		public Box Box {
			get {
				_box ??= Grid.Boxes[(Position.X / Grid.BoxWidth) + (Grid.BoxHeight * (Position.Y / Grid.BoxHeight))];
				return _box;
			}
		}
		internal Column _col;
		public Column Column {
			get {
				_col ??= Grid.Columns[Position.X];
				return _col;
			}
		}
		internal Row _row;
		public Row Row {
			get {
				_row ??= Grid.Rows[Position.Y];
				return _row;
			}
		}
		internal List<Unit> _units;
		public List<Unit> Units {
			get {
				_units ??= [Row, Column, Box];
				return _units;
			}
		}
		internal Grid Grid { get; }
		public PointF Location { get; set; }
		public Point Position { get; }
		public SizeF Size { get; set; }
		internal Char _value;
		public Char Value {
			get => _value;
			set {
				if (value != _value) {
					if (value == EmptyChar) {
						Grid.CurrentUndoItem?.Add(new ChangeCellValueAction(this, _value, value));
						_value = value;
						UserCell = true;
					} else if (Grid.Candidates.Contains(value.ToString())) {
						Grid.CurrentUndoItem?.Add(new ChangeCellValueAction(this, _value, value));
						_value = value;
					}
				}
			}
		}
		public Boolean Empty => _value == EmptyChar;
		public Int32 Index => Position.X + (Position.Y * Grid.BoxWidth * Grid.BoxHeight);
		public Cell(Grid grid, Point position) {
			_row = null;
			_col = null;
			_box = null;
			_units = null;
			_candidates = [];
			Position = position;
			Grid = grid;
			_value = EmptyChar;
			UserCell = true;
		}
		public void Draw(Graphics gfx) {
			Brush fontBrush;
			if (_value == EmptyChar) {
				fontBrush = new SolidBrush(System.Drawing.Color.Gray);
			} else if (UserCell) {
				fontBrush = new SolidBrush(System.Drawing.Color.Blue);
			} else {
				fontBrush = new SolidBrush(System.Drawing.Color.Black);
			}
			// Reset the control graphic by coloring in the highlight color
			gfx.FillRectangle(new SolidBrush(GameColor.ToColor(Color)), Location.X, Location.Y, Size.Width, Size.Height);
			if (this == Grid.SelectedCell) {
				// Draw the cell selection box, since this box is currently selected
				Pen selectPen = new(System.Drawing.Color.LightGreen, 6.0f);
				gfx.DrawRectangle(selectPen, Location.X + 5, Location.Y + 5, Size.Width - 9, Size.Height - 9);
			}
			gfx.DrawRectangle(new(System.Drawing.Color.Black), Location.X, Location.Y, Size.Width, Size.Height);
			if (_value != EmptyChar) {
				gfx.DrawString(_value.ToString(), Grid.Font, fontBrush, new PointF(Location.X + ((Size.Width - Grid.CellCharSize.Width) / 2), Location.Y + ((Size.Height - Grid.CellCharSize.Height) / 2)));
			} else {
				foreach (KeyValuePair<Char, CandidateData> candidate in _candidates) {
					Int32 charPos = Grid.Candidates.IndexOf(candidate.Key);
					Point position = new(charPos % Grid.BoxWidth, charPos / Grid.BoxWidth);
					PointF location = new(
						Location.X + (Size.Width / Grid.BoxWidth * position.X) + (Size.Width / Grid.BoxWidth / 2) - (Grid.PencilMarkCharSize.Width / 2),
						Location.Y + (Size.Height / Grid.BoxHeight * position.Y) + (Size.Height / Grid.BoxHeight / 2) - (Grid.PencilMarkCharSize.Height / 2)
					);
					if ((candidate.Value.Color != Color) && (candidate.Value.Color != GameColors.White)) {
						// Draw a box for the candidate
						//gfx.FillRectangle(
						//	new SolidBrush(GameColor.ToColor(candidate.Value.Color)),
						//	location.X,// - Grid.PencilMarkCharSize.Width / 2,
						//	location.Y,// - Grid.PencilMarkCharSize.Height / 2,
						//	Grid.PencilMarkCharSize.Width,
						//	Grid.PencilMarkCharSize.Height
						//);
						gfx.FillEllipse(
							new SolidBrush(GameColor.ToColor(candidate.Value.Color)),
							location.X,// - Grid.PencilMarkCharSize.Width / 2,
							location.Y,// - Grid.PencilMarkCharSize.Height / 2,
							Grid.PencilMarkCharSize.Width,
							Grid.PencilMarkCharSize.Height
						);
						gfx.DrawEllipse(
							new Pen(System.Drawing.Color.DimGray),
							location.X,// - Grid.PencilMarkCharSize.Width / 2,
							location.Y,// - Grid.PencilMarkCharSize.Height / 2,
							Grid.PencilMarkCharSize.Width,
							Grid.PencilMarkCharSize.Height
						);
						fontBrush = new SolidBrush(System.Drawing.Color.DimGray);
					} else {
						fontBrush = new SolidBrush(System.Drawing.Color.Gray);
					}
					gfx.DrawString(candidate.Key.ToString(), Grid.PencilMarkFont, fontBrush, location);
				}
			}
		}

		public void ProcessQualifier(Char qualifier) {
			if (qualifier == '*') {
				UserCell = true;
			} else {
				Color = GameColor.LetterToColor(qualifier);
			}
		}
		internal GameColors _color;
		public GameColors Color {
			get => _color;
			set {
				Grid.CurrentUndoItem?.Add(new ChangeCellColorAction(this, _color, value));
				_color = value;
			}
		}
		public Boolean UserCell { get; set; } = false;

		public void GetText(StringBuilder sb) {
			if (_value == EmptyChar) {
				_ = sb.Append('.');
			} else {
				_ = sb.Append(_value);
			}
			if (!Grid.CopyRawBoardOnly) {
				if ((UserCell && (Value != EmptyChar)) || (Color != GameColors.White)) {
					if (UserCell && (Value != EmptyChar)) {
						_ = sb.Append('*');
					}
					if (GameColor.ColorMap.Forward.ContainsKey(Color) && (Color != GameColors.White)) {
						_ = sb.Append(GameColor.ColorToLetter(Color));
					}
				}
			}
		}
		public void Select() {
			Grid.SelectedCell = this;
		}
		public override String ToString() {
			return $"Cell[{Position.X},{Position.Y}]";
		}
	}
}
