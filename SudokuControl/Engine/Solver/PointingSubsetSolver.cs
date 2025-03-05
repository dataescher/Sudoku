using Sudoku.Engine.Logging;
using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Solver {
	public class PointingSubsetSolver : SolveEngine {
		public PointingSubsetSolver(Grid grid) : base(grid) { }

		public Object PointingSubsetLog { get; private set; }

		public override Int32 Solve(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			// Next tactic: Pointing Pair In Cells
			// Find boxes with a particular candiate which is only one row or one column, and eliminate that
			// candiate from the same row or column for all the other boxes
			Int32 tacticsFound = 0;
			foreach (Box thisBox in _grid.Boxes) {
				Dictionary<Char, List<Cell>> candidates = new();
				foreach (Cell thisCell in thisBox.Cells) {
					foreach (Char thisCandidate in thisCell.Candidates) {
						if (candidates.TryGetValue(thisCandidate, out List<Cell> thesePossibilities)) {
							thesePossibilities.Add(thisCell);
						} else {
							candidates.Add(thisCandidate, new() { thisCell });
						}
					}
				}
				// Now, go through the characters and determine if any of them are all in the same row or column
				foreach (KeyValuePair<Char, List<Cell>> candidate in candidates) {
					// Loop through the possibilities and determine if the columns are all the same or the rows are all the same
					List<Cell> evaluatedCells = new();
					List<Column> seenCols = new();
					List<Row> seenRows = new();
					// Multiple squares have this number as a candidate. Figure out if they're all either in the same row or column.
					foreach (Cell thisCell in candidate.Value) {
						evaluatedCells.Add(thisCell);
						if (!seenCols.Contains(thisCell.Column)) {
							seenCols.Add(thisCell.Column);
						}
						if (!seenRows.Contains(thisCell.Row)) {
							seenRows.Add(thisCell.Row);
						}
					}
					if (seenCols.Count == 1) {
						List<Cell> eliminatedCandidateCells = null;
						List<Cell> pointingCandidateCells = null;
						Column thisSeenCol = seenCols[0];
						// Remove the candidate for all cells in the same column in other boxes
						foreach (Box selectBox in thisSeenCol.Boxes) {
							List<Cell> selectCells = selectBox.IntersectingCells(thisSeenCol);
							foreach (Cell thisSelectCell in selectCells) {
								if (thisSelectCell.Candidates.Contains(candidate.Key)) {
									if (thisSelectCell.Candidates.Contains(candidate.Key)) {
										if (thisBox != selectBox) {
											if (eliminatedCandidateCells == null) {
												eliminatedCandidateCells = new() { thisSelectCell };
											} else {
												eliminatedCandidateCells.Add(thisSelectCell);
											}
										} else if (pointingCandidateCells == null) {
											pointingCandidateCells = new() { thisSelectCell };
										} else {
											pointingCandidateCells.Add(thisSelectCell);
										}
									}
								}
							}
						}
						if (eliminatedCandidateCells is not null) {
							tacticsFound++;
							if (log is not null) {
								log.Items.Add(new PointingSubsetLog(_grid, thisBox, thisSeenCol, candidate.Key, evaluatedCells, eliminatedCandidateCells));
							}
							if (modifyGrid) {
								foreach (Cell thisCell in eliminatedCandidateCells) {
									thisCell.RemoveCandidate(candidate.Key);
								}
							}
							if (!solveMultiple) {
								return tacticsFound;
							}
						}
					}
					if (seenRows.Count == 1) {
						List<Cell> eliminatedCandidateCells = null;
						List<Cell> pointingCandidateCells = null;
						Row thisSeenRow = seenRows[0];
						// Remove the candidate for all cells in the same row in other boxes
						foreach (Box selectBox in thisSeenRow.Boxes) {
							List<Cell> selectCells = selectBox.IntersectingCells(thisSeenRow);
							foreach (Cell thisSelectCell in selectCells) {
								if (thisSelectCell.Candidates.Contains(candidate.Key)) {
									if (thisBox != selectBox) {
										if (eliminatedCandidateCells == null) {
											eliminatedCandidateCells = new() { thisSelectCell };
										} else {
											eliminatedCandidateCells.Add(thisSelectCell);
										}
									} else if (pointingCandidateCells == null) {
										pointingCandidateCells = new() { thisSelectCell };
									} else {
										pointingCandidateCells.Add(thisSelectCell);
									}
								}
							}
						}
						if (eliminatedCandidateCells is not null) {
							tacticsFound++;
							if (log is not null) {
								log.Items.Add(new PointingSubsetLog(_grid, thisBox, thisSeenRow, candidate.Key, pointingCandidateCells, eliminatedCandidateCells));
							}
							if (modifyGrid) {
								foreach (Cell thisCell in eliminatedCandidateCells) {
									thisCell.RemoveCandidate(candidate.Key);
								}
							}
							if (!solveMultiple) {
								return tacticsFound;
							}
						}
					}
				}
			}
			return tacticsFound;
		}
	}
}