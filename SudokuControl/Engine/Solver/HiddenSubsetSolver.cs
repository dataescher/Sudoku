using Sudoku.Engine.Logging;
using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Solver {
	public class HiddenSubsetSolver : SolveEngine {
		public HiddenSubsetSolver(Grid grid) : base(grid) { }
		/// <summary>Find naked and hidden subsets</summary>
		/// <param name="logItems">The log items for the found subsets.</param>
		/// <returns>True if subset found, false otherwise.</returns>
		public override Int32 Solve(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			Int32 tacticsFound = 0;
			Int32 unitDimension = _grid._boxWidth * _grid._boxHeight;
			Dictionary<Unit, Dictionary<Char, List<Cell>>> candidateMap = [];
			foreach (Unit thisUnit in _grid.Units) {
				foreach (Char thisCandidate in _grid.Candidates) {
					foreach (Cell thisCell in thisUnit.Cells) {
						if (thisCell.Empty) {
							if (thisCell.Candidates.Contains(thisCandidate)) {
								if (!candidateMap.TryGetValue(thisUnit, out Dictionary<Char, List<Cell>> unitCandidateMap)) {
									candidateMap.Add(thisUnit, new Dictionary<Char, List<Cell>>() { { thisCandidate, new List<Cell>() { thisCell } } });
								} else if (!unitCandidateMap.TryGetValue(thisCandidate, out List<Cell> candidateCells)) {
									unitCandidateMap.Add(thisCandidate, [thisCell]);
								} else {
									candidateCells.Add(thisCell);
								}
							}
						}
					}
				}
			}
			foreach (KeyValuePair<Unit, Dictionary<Char, List<Cell>>> thisEntry in candidateMap) {
				// Transform the data to list
				List<Tuple<Char, List<Cell>>> thisUnitData = [];
				foreach (KeyValuePair<Char, List<Cell>> thisCellGroup in thisEntry.Value) {
					thisUnitData.Add(new Tuple<Char, List<Cell>>(thisCellGroup.Key, thisCellGroup.Value));
				}
				thisUnitData.Sort((a, b) => a.Item2.Count.CompareTo(b.Item2.Count));
				for (Int32 tryGrpCnt = 1; tryGrpCnt < (unitDimension - 1); tryGrpCnt++) {
					for (Int32 startGrpIdx = 0; startGrpIdx < thisUnitData.Count; startGrpIdx++) {
						// Check if there are enough remaining possibilities left
						Int32 lh = startGrpIdx + thisUnitData[startGrpIdx].Item2.Count;
						Int32 rh = thisUnitData.Count - tryGrpCnt;
						if ((startGrpIdx + thisUnitData[startGrpIdx].Item2.Count) < (thisUnitData.Count - tryGrpCnt)) {
							Int32[] grpIdxs = new Int32[tryGrpCnt];
							Int32 maxGrpIdx = thisUnitData.Count;
							for (Int32 initGrpIdxOffset = 1; initGrpIdxOffset < tryGrpCnt; initGrpIdxOffset++) {
								grpIdxs[initGrpIdxOffset] = maxGrpIdx - (tryGrpCnt - initGrpIdxOffset);
							}
							// grpIdxs needs to contain startGrpIdx, and all other indices in grpIdxs must be greater than startGrpIdx.
							// Additionally, all items in grpIdxs must be distinct and less than the number of groups found.
							grpIdxs[0] = startGrpIdx;

							// Now to iterate through this array
							Boolean isDone;
							do {
								List<Tuple<Cell, Char>> candidatesToRemove = [];
								List<Tuple<Char, List<Cell>>> groups = [];
								// Collect the groups
								for (Int32 thisGrpIdx = 0; thisGrpIdx < tryGrpCnt; thisGrpIdx++) {
									groups.Add(thisUnitData[grpIdxs[thisGrpIdx]]);
								}
								List<Cell> cellsInGrp = [];
								List<Char> candatesInGrp = [];

								// Loop through the groups
								for (Int32 thisGrpIdx = 0; thisGrpIdx < tryGrpCnt; thisGrpIdx++) {
									Char thisCellCandidate = thisUnitData[grpIdxs[thisGrpIdx]].Item1;
									List<Cell> grp = thisUnitData[grpIdxs[thisGrpIdx]].Item2;
									foreach (Cell thisGrpCell in grp) {
										if (!candatesInGrp.Contains(thisCellCandidate)) {
											candatesInGrp.Add(thisCellCandidate);
										}
										if (!cellsInGrp.Contains(thisGrpCell)) {
											cellsInGrp.Add(thisGrpCell);
										}
									}
								}
								Boolean hiddenSubsetFound = false;
								if (cellsInGrp.Count == tryGrpCnt) {
									// This appears to be a match. Iterate through all cells in the group and remove candidates
									// which were not evaluated with the group
									foreach (Cell thisCell in cellsInGrp) {
										foreach (Char thisCandidate in thisCell.Candidates) {
											if (!candatesInGrp.Contains(thisCandidate)) {
												// Found hidden subset tactic
												hiddenSubsetFound = true;
												candidatesToRemove.Add(new Tuple<Cell, Char>(thisCell, thisCandidate));
											}
										}
									}
								}
								if (hiddenSubsetFound) {
									tacticsFound++;
									// Log this hidden subset
									log?.Items.Add(new HiddenSubsetLog(_grid, thisEntry.Key, groups, candidatesToRemove));
									if (modifyGrid) {
										// Remove the candidates from the grid
										foreach (Tuple<Cell, Char> thisCandidateRemoval in candidatesToRemove) {
											thisCandidateRemoval.Item1.RemoveCandidate(thisCandidateRemoval.Item2);
										}
									}
									if (!solveMultiple) {
										return tacticsFound;
									}
								}

								// Now increment the indices appropriately
								isDone = true;
								for (Int32 grpIdx = 1; grpIdx < grpIdxs.Length; grpIdx++) {
									if (grpIdxs[grpIdx] <= (grpIdxs[grpIdx - 1] + 1)) {
										if (grpIdx >= (grpIdxs.Length - 1)) {
											isDone = true;
											break;
										}
									} else {
										grpIdxs[grpIdx]--;
										for (Int32 decGrpIdx = grpIdx - 1; decGrpIdx > 0; decGrpIdx--) {
											grpIdxs[decGrpIdx] = grpIdxs[decGrpIdx + 1] - 1;
										}
										isDone = false;
										break;
									}
								}
							} while (!isDone);
						}
					}
				}
			}
			return tacticsFound;
		}
	}
}
