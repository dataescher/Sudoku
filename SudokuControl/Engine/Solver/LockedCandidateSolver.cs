using Sudoku.Engine.Logging;
using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Solver {
	public class LockedCandidateSolver : SolveEngine {
		public LockedCandidateSolver(Grid grid) : base(grid) { }
		/// <summary>Find naked and hidden subsets</summary>
		/// <param name="logItems">The log items for the found subsets.</param>
		/// <returns>True if subset found, false otherwise.</returns>
		public override Int32 Solve(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			Int32 lockedCandidatesFound = 0;
			foreach (Unit lockedUnit in _grid.Units) {
				Dictionary<Char, List<Cell>> candidateCellMap = null;
				foreach (Cell thisCell in lockedUnit.Cells) {
					if (thisCell.Empty) {
						foreach (Char thisCandidate in thisCell.Candidates) {
							if (candidateCellMap is null) {
								candidateCellMap = new() { { thisCandidate, new List<Cell>() { thisCell } } };
							} else if (!candidateCellMap.TryGetValue(thisCandidate, out List<Cell> candidateCells)) {
								candidateCellMap.Add(thisCandidate, new List<Cell>() { thisCell });
							} else {
								candidateCells.Add(thisCell);
							}
						}
					}
				}
				if (candidateCellMap is not null) {
					List<Tuple<Char, List<Unit>>> intersectingUnitsWithCandidate = new();
					// Determine whether for any candidate, they all exist in one of the intersecting units
					foreach (Unit intersectingUnit in lockedUnit.IntersectingUnits) {
						if (intersectingUnit != lockedUnit) {
							foreach (KeyValuePair<Char, List<Cell>> thisCandidateCellList in candidateCellMap) {
								List<Cell> removeCells = new();
								List<Cell> lockedCells = new();
								Boolean isPossibleLockedCandidate = true;
								foreach (Cell thisCandidateCell in thisCandidateCellList.Value) {
									if (!intersectingUnit.Cells.Contains(thisCandidateCell)) {
										isPossibleLockedCandidate = false;
										break;
									}
								}
								if (isPossibleLockedCandidate) {
									//intersectingUnitsWithCandidate.Add(new Tuple<Char, List<Unit>>intersectingUnit);
									// Need to determine if there are any candidates to remove from the intersecting unit
									foreach (Cell possibleRemoveCell in intersectingUnit.Cells) {
										if (possibleRemoveCell.Candidates.Contains(thisCandidateCellList.Key)) {
											if (lockedUnit.Cells.Contains(possibleRemoveCell)) {
												// This is an intersecting cell. Keep the candidate.
												lockedCells.Add(possibleRemoveCell);
											} else {
												// This cell is in the intersecting unit but not the locked unit. Remove the candidate.
												removeCells.Add(possibleRemoveCell);
											}
										}
									}
								}
								if ((removeCells.Count > 0) && (lockedCells.Count > 0)) {
									lockedCandidatesFound++;
									if (log is not null) {
										log.Items.Add(new LockedCandidateLog(_grid, lockedUnit, intersectingUnit, thisCandidateCellList.Key, lockedCells, removeCells));
									}
									if (modifyGrid) {
										foreach (Cell thisRemoveCell in removeCells) {
											thisRemoveCell.RemoveCandidate(thisCandidateCellList.Key);
										}
									}
									if (!solveMultiple) {
										return lockedCandidatesFound;
									}
								}
							}
						}
					}
				}
			}
			return lockedCandidatesFound;
		}
	}
}