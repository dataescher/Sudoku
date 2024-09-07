using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Sudoku.Engine.Logging {
	public class EmptyCellCheckLog : LogItem {
		private readonly List<Cell> _emptyCells;

		public EmptyCellCheckLog(Grid grid, List<Cell> emptyCells) : base(grid) {
			_emptyCells = null;
			if (emptyCells is not null) {
				_emptyCells = new();
				foreach (Cell thisEmptyCell in emptyCells) {
					_emptyCells.Add(_grid.Cells[thisEmptyCell.Index]);
				}
			}
		}
		public override LogDetail Details {
			get {
				LogDetail details = new(this);
				Boolean completionCheckResult = (_emptyCells is null);
				String status;
				if (completionCheckResult) {
					status = "PASSED";
					Color = Color.LightGreen;
				} else {
					status = "FAILED";
					Color = Color.Red;
				}
				details.Description = $"Checking for empty cells -- {status}";
				if (_emptyCells is not null) {
					LogDetail emptyCellDetail = new(this, $"Empty cells found: {_emptyCells.Count}");
					details.SubDetails.Add(emptyCellDetail);
					foreach (Cell emptyCell in _emptyCells.OrderBy(x => x.Index)) {
						emptyCellDetail.SubDetails.Add(new CellDetail(this, emptyCell));
					}
				}

				return details;
			}
		}

		public override void ApplyColoring() {
			if (_emptyCells is not null) {
				foreach (Cell thisCell in _emptyCells) {
					thisCell.Color = GameColors.LightYellow;
				}
			}
		}
	}
}
