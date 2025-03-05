using Sudoku.Engine.Logging;
using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Solver {
	public class NakedSinglesSolver : SolveEngine {
		public NakedSinglesSolver(Grid grid) : base(grid) { }
		public override Int32 Solve(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			// Check for cells which only have one possibility, and fill in the value
			List<Tuple<Char, Cell>> candidatesFilled = new();
			foreach (Cell cell in _grid.Cells) {
				if (cell.Empty) {
					if (cell.CandidateCount == 1) {
						candidatesFilled.Add(new Tuple<Char, Cell>(cell.Candidates[0], cell));
						if (!solveMultiple) {
							break;
						}
					}
				}
			}
			if (log is not null) {
				if (candidatesFilled.Count > 0) {
					log.Items.Add(new NakedSingleLog(_grid, candidatesFilled));
				}
			}
			if (modifyGrid) {
				foreach (Tuple<Char, Cell> thisFillCell in candidatesFilled) {
					thisFillCell.Item2.Value = thisFillCell.Item1;
					thisFillCell.Item2.UserCell = true;
				}
			}
			return candidatesFilled.Count;
		}
	}
}