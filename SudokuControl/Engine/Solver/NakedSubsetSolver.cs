using Sudoku.Engine.Logging;
using Sudoku.Engine.Solver;
using System;
using System.Collections.Generic;

namespace Sudoku.Engine {
	public partial class Grid {
		/// <summary>Find naked pairs within a box.</summary>
		public void FindNakedPairs_Box() {
			foreach (Box thisBox in Boxes) {
				for (Int32 row = 0; row < _boxHeight; row++) {
					List<Cell> rowEmptyCells = new();
					List<Char> rowCandidates = new();
					for (Int32 col = 0; col < _boxWidth; col++) {
						Cell thisCell = thisBox[col, row];
						if (thisCell.Empty) {
							rowEmptyCells.Add(thisCell);
							foreach (Char thisCandidate in thisCell.Candidates) {
								if (!rowCandidates.Contains(thisCandidate)) {
									rowCandidates.Add(thisCandidate);
								}
							}
						}
					}
					if (rowEmptyCells.Count == rowCandidates.Count) {
						// We can remove all the discovered candidates from all other boxes in the same row
						for (Int32 boxX = 0; boxX < _boxHeight; boxX++) {
							if (boxX != thisBox.Position.X) {
								Box otherBox = GetBox(boxX, thisBox.Position.Y);
								for (Int32 boxCol = 0; boxCol < _boxWidth; boxCol++) {
									Cell thisCell = otherBox[boxCol, row];
									foreach (Char thisRowCandidate in rowCandidates) {
										thisCell.RemoveCandidate(thisRowCandidate);
									}
								}
							}
						}
					}
				}
				for (Int32 col = 0; col < _boxWidth; col++) {
					List<Cell> colEmptyCells = new();
					List<Char> colCandidates = new();
					for (Int32 row = 0; row < _boxHeight; row++) {
						Cell thisCell = thisBox[col, row];
						if (thisCell.Empty) {
							colEmptyCells.Add(thisCell);
							foreach (Char thisCandidate in thisCell.Candidates) {
								if (!colCandidates.Contains(thisCandidate)) {
									colCandidates.Add(thisCandidate);
								}
							}
						}
					}
					if (colEmptyCells.Count == colCandidates.Count) {
						// We can remove all the discovered candidates from all other boxes in the same row
						for (Int32 boxY = 0; boxY < _boxHeight; boxY++) {
							if (boxY != thisBox.Position.Y) {
								Box otherBox = GetBox(thisBox.Position.X, boxY);
								for (Int32 boxRow = 0; boxRow < _boxHeight; boxRow++) {
									Cell thisCell = otherBox[col, boxRow];
									foreach (Char thisRowCandidate in colCandidates) {
										thisCell.RemoveCandidate(thisRowCandidate);
									}
								}
							}
						}
						// TODO: Can we also remove all the discovered candidates from all the other cells in the box?
					}
				}
			}
		}

		internal Boolean CheckComplete(LogItemGroup log) {
			DuplicateCheck duplicateCheck = new(this);
			EmptyCellCheck emptyCellCheck = new(this);
			return ((duplicateCheck.Solve(true, log, true) == 0) && (emptyCellCheck.Solve(true, log, true) == 0));
		}

		public void FindNakedPairs_RowsColumns() {
			// Similar to naked pairs, but expand to rows and columns instead of just in boxes
			// The pairs/triples are on the same row or column, not necessarily in the same box
			Int32 dimension = _boxWidth * _boxHeight;
			// First do columns
			for (Int32 col = 0; col < dimension; col++) {
				for (Int32 row1 = 0; row1 < dimension; row1++) {
					Cell cell1 = this[col, row1];
					if (cell1.CandidateCount == 0) {
						continue;
					}
					List<Cell> colMatches = new() { cell1 };
					for (Int32 row2 = row1 + 1; row2 < dimension; row2++) {
						Cell cell2 = this[col, row2];
						if (cell2.CandidateCount == 0) {
							continue;
						}
						if (cell1.CandidateCount == cell2.CandidateCount) {
							Boolean match = true;
							foreach (Char cellCandidate in cell1.Candidates) {
								if (!cell2.HasCandidate(cellCandidate)) {
									match = false;
									break;
								}
							}
							if (match) {
								// Appears there is an exact match.
								colMatches.Add(cell2);
							}
						}
					}
					// Check if there is a naked pair
					if (colMatches.Count > 1) {
						if (colMatches.Count == colMatches[0].CandidateCount) {
							// We can proceed to remove the candidates from the rest of the rows
							for (Int32 thisRow = 0; thisRow < dimension; thisRow++) {
								Cell thisCell = this[col, thisRow];
								if (!colMatches.Contains(thisCell)) {
									foreach (Char thisCandidate in colMatches[0].Candidates) {
										thisCell.RemoveCandidate(thisCandidate);
									}
								}
							}
						}
					}
					// At this point, we can check if they're all in the same box and if so, remove
					// the candidates from all other cells in the box, but there already is a different routine written for that.
				}
			}
			// Now do rows
			for (Int32 row = 0; row < dimension; row++) {
				for (Int32 col1 = 0; col1 < dimension; col1++) {
					Cell cell1 = this[col1, row];
					if (cell1.CandidateCount == 0) {
						continue;
					}
					List<Cell> rowMatches = new() { cell1 };
					for (Int32 col2 = col1 + 1; col2 < dimension; col2++) {
						Cell cell2 = this[col2, row];
						if (cell2.CandidateCount == 0) {
							continue;
						}
						if (cell1.CandidateCount == cell2.CandidateCount) {
							Boolean match = true;
							foreach (Char cellCandidate in cell1.Candidates) {
								if (!cell2.HasCandidate(cellCandidate)) {
									match = false;
									break;
								}
							}
							if (match) {
								// Appears there is an exact match.
								rowMatches.Add(cell2);
							}
						}
					}
					// Check if there is a naked pair
					if (rowMatches.Count > 1) {
						if (rowMatches.Count == rowMatches[0].CandidateCount) {
							// We can proceed to remove the candidates from the rest of the rows
							for (Int32 thisCol = 0; thisCol < dimension; thisCol++) {
								Cell thisCell = this[thisCol, row];
								if (!rowMatches.Contains(thisCell)) {
									foreach (Char thisCandidate in rowMatches[0].Candidates) {
										thisCell.RemoveCandidate(thisCandidate);
									}
								}
							}
						}
					}
				}
			}
		}
	}
}