using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Sudoku.Engine {
	public abstract class Unit {
		public Grid Grid { get; }
		public List<Cell> Cells { get; }
		public Int32 Index { get; }
		public Int32 UnitIndex => Grid.Units.IndexOf(this) + 1;
		public Point Position { get; }
		public PointF Location => Cells[0].Location;
		public SizeF Size {
			get {
				Cell firstCell = Cells.First();
				Cell lastCell = Cells.Last();
				return new SizeF() {
					Width = lastCell.Position.X - firstCell.Position.X + lastCell.Size.Width,
					Height = lastCell.Position.Y - firstCell.Position.Y + lastCell.Size.Height
				};
			}
		}
		public Unit(Grid grid, Int32 index, Point pos) {
			Cells = [];
			Grid = grid;
			Index = index;
			Position = pos;
		}
		public abstract List<Unit> IntersectingUnits { get; }
		public List<Cell> IntersectingCells(Unit other) {
			List<Cell> result = [];
			foreach (Cell cell in Cells) {
				if (other.Cells.Contains(cell)) {
					result.Add(cell);
				}
			}
			return result;
		}
	}
}
