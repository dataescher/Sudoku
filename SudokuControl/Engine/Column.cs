using System;
using System.Collections.Generic;
using System.Drawing;

namespace Sudoku.Engine {
	public class Column : Unit {
		public Column(Grid grid, Int32 idx, Point pos) : base(grid, idx, pos) {
			_intersectingUnits = null;
			_boxes = null;
			_rows = null;
		}
		public override String ToString() {
			return $"Col[{Index}]";
		}
		internal List<Row> _rows;
		public List<Row> Rows {
			get {
				if (_rows is null) {
					_rows = [];
					for (Int32 row = 0; row < (Grid.BoxWidth * Grid.BoxHeight); row++) {
						_rows.Add(Grid.Rows[row]);
					}
				}
				return _rows;
			}
		}
		internal List<Box> _boxes;
		public List<Box> Boxes {
			get {
				if (_boxes is null) {
					_boxes = [];
					for (Int32 box = 0; box < Grid.BoxWidth; box++) {
						_boxes.Add(Grid.Boxes[(box * Grid.BoxHeight) + (Position.X / Grid.BoxWidth)]);
					}
				}
				return _boxes;
			}
		}
		internal List<Unit> _intersectingUnits;
		public override List<Unit> IntersectingUnits {
			get {
				// Do a one-time computation of the intersecting units
				_intersectingUnits ??= [.. Rows, .. Boxes];
				return _intersectingUnits;
			}
		}
	}
}
