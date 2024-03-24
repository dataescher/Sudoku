using Sudoku.Engine.Logging;
using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Solver {
	public class EmptyCellCheck : SolveEngine {
		public EmptyCellCheck(Grid grid) : base(grid) { }
		public override Int32 Solve(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			List<Cell> emptyCells = null;
			Int32 emptyCellCnt = 0;
			foreach (Cell thisCell in _grid.Cells) {
				if (thisCell.Empty) {
					if (emptyCells is null) {
						emptyCells = [thisCell];
					} else {
						emptyCells.Add(thisCell);
					}
					emptyCellCnt++;
					if (!solveMultiple) {
						break;
					}
				}
			}
			log?.Items.Add(new EmptyCellCheckLog(_grid, emptyCells));
			return emptyCellCnt;
		}
	}
}
