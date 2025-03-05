using Sudoku.Engine.Logging;
using Sudoku.Engine.Solver;
using System;
using System.Collections.Generic;

namespace Sudoku.Engine {
	public class BruteForceSolveEngine : SolveEngine {
		private readonly Int32 _maxSolutions;
		private readonly Int32 _maxTries;
		public BruteForceSolveEngine(Grid grid, Int32 maxSolutions, Int32 maxTries) : base(grid) {
			_maxSolutions = maxSolutions;
			_maxTries = maxTries;
		}

		public override Int32 Solve(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			DuplicateCheck duplicateCheck = new(_grid);
			EmptyCellCheck emptyCellCheck = new(_grid);
			List<String> solutions = new();
			if (log is not null) {
				log.Description = "Brute force solve engine";
			}
			if (!(duplicateCheck.Solve(true, log, true) == 0)) {
				return 0;
			}
			if (emptyCellCheck.Solve(true, log, true) == 0) {
				return 0;
			}
			Int32 maxTries = _maxTries;
			TrySolveBruteForce(ref solutions, _maxSolutions, ref maxTries);
			if (solutions.Count > 0) {
				if ((solutions.Count > 0) && (log is not null)) {
					log.Items.Add(new BruteForceSolveLog(_grid, solutions));
				}
				_grid.FromString(solutions[0]);
			}
			return solutions.Count;
		}
		internal Boolean TrySolveBruteForce(ref List<String> solutions, Int32 maxSolutions, ref Int32 maxTries) {
			NakedSinglesSolver nakedSinglesSolver = new(_grid);
			CandidateGenerator candidateGenerator = new(_grid);
			if (maxTries == 0) {
				return false;
			}
			if (maxTries > 0) {
				maxTries--;
			}
			if (solutions.Count >= maxSolutions) {
				return false;
			}
			candidateGenerator.Solve(true, null, true);
			_grid.FindAdvancedTactics(true, null, true);
			// Eliminate any cells which have only 1 candidate
			nakedSinglesSolver.Solve(true, null, true);
			candidateGenerator.Solve(true, null, true);
			_grid.FindAdvancedTactics(true, null, true);
			if (_grid.CellHasZeroCandidates()) {
				// Check if any of the cells has zero candidates
				return false;
			}
			// Locate the cell with the fewest candidates, and evaluate that particular cell
			Cell cellToEvaluate = null;
			foreach (Cell thisCell in _grid.Cells) {
				if (thisCell.Empty) {
					if (cellToEvaluate is null) {
						cellToEvaluate = thisCell;
					} else if (cellToEvaluate.CandidateCount > thisCell.CandidateCount) {
						cellToEvaluate = thisCell;
					}
					if (cellToEvaluate.CandidateCount <= 2) {
						break;
					}
				}
			}
			if (cellToEvaluate is null) {
				// All cells have been filled
				if (_grid.CheckComplete(null)) {
					String thisSolution = _grid.ToString();
					if (!solutions.Contains(thisSolution)) {
						solutions.Add(thisSolution);
					}
					return true;
				}
				return false;
			}
			// Test all possibilities with the cell
			Boolean tryResult = false;
			foreach (Char thisCandidate in cellToEvaluate.Candidates) {
				Grid testGrid = _grid.Clone() as Grid;
				testGrid.Cells[cellToEvaluate.Index].Value = thisCandidate;
				BruteForceSolveEngine bruteForceSolveEngine = new(testGrid, maxSolutions, maxTries);
				tryResult = bruteForceSolveEngine.TrySolveBruteForce(ref solutions, maxSolutions, ref maxTries);
			}
			return tryResult;
		}
	}
}