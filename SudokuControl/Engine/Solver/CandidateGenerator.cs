using Sudoku.Engine.Logging;
using Sudoku.Engine.Logging.Sudoku.Engine.Logging;
using Sudoku.Engine.Solver;
using System;
using System.Collections.Generic;

namespace Sudoku.Engine {
	public class CandidateGenerator : SolveEngine {
		public CandidateGenerator(Grid grid) : base(grid) { }

		internal static void EliminateCandidate(Unit unit, Cell filledCell, Cell emptyCell, ref Dictionary<Unit, Dictionary<Cell, List<Cell>>> candidatesEliminated) {
			if ((!filledCell.Empty) && emptyCell.Empty) {
				if (emptyCell.HasCandidate(filledCell.Value)) {
					// Generate report for this candidate removal
					if (candidatesEliminated is null) {
						candidatesEliminated = new() { { unit, new Dictionary<Cell, List<Cell>>() { { filledCell, new List<Cell>() { emptyCell } } } } };
					} else if (!candidatesEliminated.TryGetValue(unit, out Dictionary<Cell, List<Cell>> candidateEliminationMap)) {
						candidatesEliminated.Add(unit, new Dictionary<Cell, List<Cell>>() { { filledCell, new List<Cell>() { emptyCell } } });
					} else if (!candidateEliminationMap.TryGetValue(filledCell, out List<Cell> candidateEliminationList)) {
						candidateEliminationMap.Add(filledCell, new List<Cell>() { emptyCell });
					} else {
						candidateEliminationList.Add(emptyCell);
					}
				}
			}
		}
		internal Int32 EliminateCandidates(Unit unit, ref Dictionary<Unit, Dictionary<Cell, List<Cell>>> candidatesEliminated) {
			Int32 candidatesEliminatedCnt = 0;
			for (Int32 idx1 = 0; idx1 < unit.Cells.Count; idx1++) {
				Cell cell1 = unit.Cells[idx1];
				for (Int32 idx2 = idx1 + 1; idx2 < unit.Cells.Count; idx2++) {
					Cell cell2 = unit.Cells[idx2];
					if (!cell1.Empty) {
						if (cell2.Empty) {
							EliminateCandidate(unit, cell1, cell2, ref candidatesEliminated);
							candidatesEliminatedCnt++;
						}
					} else if (!cell2.Empty) {
						EliminateCandidate(unit, cell2, cell1, ref candidatesEliminated);
						candidatesEliminatedCnt++;
					}
				}
			}
			return candidatesEliminatedCnt;
		}
		internal Int32 ResetCandidates(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			List<Tuple<Cell, Char>> candidatesReset = null;
			Int32 candidateResetCnt = 0;
			foreach (Cell cell in _grid.Cells) {
				if (cell.Empty) {
					foreach (Char thisValue in _grid.Candidates) {
						if (!cell.HasCandidate(thisValue)) {
							candidateResetCnt++;
							if (candidatesReset is null) {
								candidatesReset = new List<Tuple<Cell, Char>>() { new Tuple<Cell, Char>(cell, thisValue) };
							} else {
								candidatesReset.Add(new Tuple<Cell, Char>(cell, thisValue));
							}
							if (!solveMultiple) {
								break;
							}
						}
					}
				}
				if (!solveMultiple && (candidatesReset.Count > 0)) {
					break;
				}
			}
			if (candidatesReset is not null) {
				if (modifyGrid) {
					foreach (Tuple<Cell, Char> thisCandidateReset in candidatesReset) {
						thisCandidateReset.Item1.AddCandidate(thisCandidateReset.Item2);
					}
				}
			}
			if ((log is not null) && (candidatesReset is not null) && (candidatesReset.Count > 0)) {
				log.Items.Add(new CandidateResetLog(_grid, candidatesReset));
			}
			return candidateResetCnt;
		}
		public Int32 UpdateCandidates(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			Dictionary<Unit, Dictionary<Cell, List<Cell>>> candidatesEliminated = null;
			Int32 candidatesEliminatedCnt = 0;
			foreach (Unit unit in _grid.Units) {
				candidatesEliminatedCnt += EliminateCandidates(unit, ref candidatesEliminated);
				if ((candidatesEliminatedCnt > 0) && !solveMultiple) {
					break;
				}
			}
			if (candidatesEliminated is not null) {
				if (log is not null) {
					log.Items.Add(new UpdateCandidatesLog(_grid, candidatesEliminated));
				}
				if (modifyGrid) {
					foreach (KeyValuePair<Unit, Dictionary<Cell, List<Cell>>> thisCandidateEliminated in candidatesEliminated) {
						foreach (KeyValuePair<Cell, List<Cell>> thisElimination in thisCandidateEliminated.Value) {
							foreach (Cell thisCell in thisElimination.Value) {
								thisCell.RemoveCandidate(thisElimination.Key.Value);
							}
						}
					}
				}
			}
			return candidatesEliminatedCnt;
		}
		public Int32 GenerateCandidates(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			// Eliminate candidates for rows/columns/boxes
			Int32 candidatesReset = ResetCandidates(solveMultiple, log, modifyGrid);
			Int32 candidatesUpdated = 0;
			if ((candidatesReset == 0) || solveMultiple) {
				candidatesUpdated = UpdateCandidates(solveMultiple, log, modifyGrid);
			}
			return candidatesReset + candidatesUpdated;
		}
		public override Int32 Solve(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			// Eliminate candidates for rows/columns/boxes
			return GenerateCandidates(solveMultiple, log, modifyGrid);
		}
	}
}