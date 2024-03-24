using System;
using System.Collections.Generic;
using System.Drawing;

namespace Sudoku.Engine {
	public class Row : Unit {
		public Row(Grid grid, Int32 idx, Point pos) : base(grid, idx, pos) {
			_intersectingUnits = null;
			_boxes = null;
			_cols = null;
		}
		public override String ToString() {
			return $"Row[{Index}]";
		}
		internal List<Column> _cols;
		public List<Column> Columns {
			get {
				if (_cols is null) {
					_cols = new();
					for (Int32 col = 0; col < (Grid.BoxWidth * Grid.BoxHeight); col++) {
						_cols.Add(Grid.Columns[col]);
					}
				}
				return _cols;
			}
		}
		internal List<Box> _boxes;
		public List<Box> Boxes {
			get {
				if (_boxes is null) {
					_boxes = new();
					for (Int32 box = 0; box < Grid.BoxHeight; box++) {
						_boxes.Add(Grid.Boxes[box + Grid.BoxHeight * (Position.Y / Grid.BoxHeight)]);
					}
				}
				return _boxes;
			}
		}
		internal List<Unit> _intersectingUnits;
		public override List<Unit> IntersectingUnits {
			get {
				if (_intersectingUnits is null) {
					// Do a one-time computation of the intersecting units
					_intersectingUnits = new();
					foreach (Column thisCol in Columns) {
						_intersectingUnits.Add(thisCol);
					}
					foreach (Box thisBox in Boxes) {
						_intersectingUnits.Add(thisBox);
					}
				}
				return _intersectingUnits;
			}
		}
	}
}
