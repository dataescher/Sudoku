using Sudoku.Engine.Logging;
using Sudoku.Engine.Solver;
using System;

namespace Sudoku.Engine {
	public partial class Grid {
		internal Boolean CellHasZeroCandidates() {
			foreach (Cell thisCell in Cells) {
				if (thisCell._value == Cell.EmptyChar) {
					if (thisCell._candidates.Count == 0) {
						return true;
					}
				}
			}
			return false;
		}

		internal Int32 FindAdvancedTactics(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			Int32 tacticsFound = 0;
			Int32 tacticsFoundIteration;
			LockedCandidateSolver lockedCandidateSolver = new(this);

			_ = new PointingSubsetSolver(this);
			HiddenSubsetSolver hiddenSubsetSolver = new(this);

			do {
				tacticsFoundIteration = 0;
				tacticsFoundIteration += lockedCandidateSolver.Solve(solveMultiple, log, modifyGrid);
				if ((tacticsFoundIteration > 0) && !solveMultiple) {
					return tacticsFoundIteration;
				}
				tacticsFoundIteration += hiddenSubsetSolver.Solve(solveMultiple, log, modifyGrid);
				if ((tacticsFoundIteration > 0) && !solveMultiple) {
					return tacticsFoundIteration;
				}
				tacticsFound += tacticsFoundIteration;
			} while (tacticsFoundIteration > 0);
			return tacticsFound;
		}

		public Int32 Solve(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			Int32 totalCellsFilled = 0;
			Int32 totalTacticsFound = 0;
			NakedSinglesSolver nakedSinglesSolver = new(this);
			CandidateGenerator candidateGenerator = new(this);
			Int32 cellsFilledStep;
			_ = candidateGenerator.UpdateCandidates(true, log, true);
			do {
				Int32 tacticsFound = FindAdvancedTactics(solveMultiple, log, modifyGrid);
				totalTacticsFound += tacticsFound;
				if ((tacticsFound > 0) && !solveMultiple) {
					break;
				}
				cellsFilledStep = nakedSinglesSolver.Solve(solveMultiple, log, modifyGrid);
				if (cellsFilledStep > 0) {
					_ = candidateGenerator.UpdateCandidates(true, log, true);
					totalCellsFilled += cellsFilledStep;
				}
				if (!solveMultiple) {
					break;
				}
			} while (cellsFilledStep > 0);
			if (!CheckComplete(log)) {
				if (CellHasZeroCandidates()) {
					// Check if any of the cells has zero candidates
					// tODO: Log this
				}
			}
			if (log is not null) {
				log.Description = $"Tactical Solver - {totalTacticsFound} tactics found and {totalCellsFilled} cell(s) filled";
			}

			return totalCellsFilled;
		}
	}
}