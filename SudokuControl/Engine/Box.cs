using System;
using System.Collections.Generic;
using System.Drawing;

namespace Sudoku.Engine {
	public partial class Box : Unit {
		public Box(Grid grid, Int32 idx, Point pos) : base(grid, idx, pos) {
			_intersectingUnits = null;
			_rows = null;
			_cols = null;
		}
		public Cell this[Int32 index] => Cells[index];
		public Cell this[Int32 col, Int32 row] => Cells[col + (row * Grid.BoxWidth)];
		public void Draw(Graphics gfx) {
			gfx.DrawRectangle(new(Color.Black, 4.0f), Location.X, Location.Y, Grid.Size.Width / Grid.BoxHeight, Grid.Size.Height / Grid.BoxWidth);
			foreach (Cell thisCell in Cells) {
				thisCell.Draw(gfx);
			}
		}
		public override String ToString() {
			return $"Box[{Index}]";
		}
		internal List<Unit> _intersectingUnits;
		public override List<Unit> IntersectingUnits {
			get {
				if (_intersectingUnits is null) {
					_intersectingUnits = new();
					foreach (Row thisRow in Rows) {
						_intersectingUnits.Add(thisRow);
					}
					foreach (Column thisCol in Columns) {
						_intersectingUnits.Add(thisCol);
					}
				}
				return _intersectingUnits;
			}
		}
		internal List<Row> _rows;
		public List<Row> Rows {
			get {
				if (_rows is null) {
					_rows = new();
					for (Int32 row = 0; row < Grid.BoxHeight; row++) {
						_rows.Add(Grid.Rows[row + Position.Y * Grid.BoxHeight]);
					}
				}
				return _rows;
			}
		}
		internal List<Column> _cols;
		public List<Column> Columns {
			get {
				if (_cols is null) {
					_cols = new();
					for (Int32 col = 0; col < Grid.BoxWidth; col++) {
						_cols.Add(Grid.Columns[col + Position.X * Grid.BoxWidth]);
					}
				}
				return _cols;
			}
		}
	}
}
