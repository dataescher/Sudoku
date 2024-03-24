using System;
using System.Collections.Generic;

namespace Sudoku.Engine {
	public partial class Grid {
		public void FindXWing() {
			Int32 dimension = _boxWidth * _boxHeight;
			// First check columns
			Dictionary<Int32, Dictionary<Char, List<Int32>>> colCandidateDictionary = [];
			for (Int32 col = 0; col < dimension; col++) {
				for (Int32 row = 0; row < dimension; row++) {
					Cell thisCell = this[col, row];
					foreach (Char thisCandidate in thisCell.Candidates) {
						if (colCandidateDictionary.TryGetValue(col, out Dictionary<Char, List<Int32>> rowCandidateMap)) {
							if (rowCandidateMap.TryGetValue(thisCandidate, out List<Int32> rowCandidates)) {
								rowCandidates.Add(row);
							} else {
								rowCandidateMap.Add(thisCandidate, [row]);
							}
						} else {
							colCandidateDictionary.Add(col, new Dictionary<Char, List<Int32>>() { { thisCandidate, new List<Int32>() { row } } });
						}
					}
				}
			}
			// For each candidate, check if there are multiple columns which have the same row
			foreach (Char thisCandidate in Candidates) {
				for (Int32 col1 = 0; col1 < dimension; col1++) {
					if (colCandidateDictionary.TryGetValue(col1, out Dictionary<Char, List<Int32>> col1Dictionary)) {
						if (col1Dictionary.TryGetValue(thisCandidate, out List<Int32> col1RowList)) {
							List<Int32> matchingColumns = [col1];
							for (Int32 col2 = col1 + 1; col2 < dimension; col2++) {
								if (colCandidateDictionary.TryGetValue(col2, out Dictionary<Char, List<Int32>> col2Dictionary)) {
									if (col2Dictionary.TryGetValue(thisCandidate, out List<Int32> col2RowList)) {
										// Compare the two lists to see if they have the same row set
										if (col1RowList.Count == col2RowList.Count) {
											Boolean match = true;
											foreach (Int32 candidateRow in col1RowList) {
												if (!col2RowList.Contains(candidateRow)) {
													match = false;
													break;
												}
											}
											if (match) {
												matchingColumns.Add(col2);
											}
										}
									}
								}
							}
							if (matchingColumns.Count == col1RowList.Count) {
								// Can proceed to remove the values from all the other columns for the same rows
								for (Int32 col = 0; col < dimension; col++) {
									if (!matchingColumns.Contains(col)) {
										foreach (Int32 thisRow in col1RowList) {
											Cell thisCell = this[col, thisRow];
											thisCell.RemoveCandidate(thisCandidate);
										}
									}
								}
							}
						}
					}
				}
			}

			// Now check rows
			Dictionary<Int32, Dictionary<Char, List<Int32>>> rowCandidateDictionary = [];
			for (Int32 row = 0; row < dimension; row++) {
				for (Int32 col = 0; col < dimension; col++) {
					Cell thisCell = this[col, row];
					foreach (Char thisCandidate in thisCell.Candidates) {
						if (rowCandidateDictionary.TryGetValue(row, out Dictionary<Char, List<Int32>> colCandidateMap)) {
							if (colCandidateMap.TryGetValue(thisCandidate, out List<Int32> colCandidates)) {
								colCandidates.Add(col);
							} else {
								colCandidateMap.Add(thisCandidate, [col]);
							}
						} else {
							rowCandidateDictionary.Add(row, new Dictionary<Char, List<Int32>>() { { thisCandidate, new List<Int32>() { col } } });
						}
					}
				}
			}
			// For each candidate, check if there are multiple columns which have the same row
			foreach (Char thisCandidate in Candidates) {
				for (Int32 row1 = 0; row1 < dimension; row1++) {
					if (rowCandidateDictionary.TryGetValue(row1, out Dictionary<Char, List<Int32>> row1Dictionary)) {
						if (row1Dictionary.TryGetValue(thisCandidate, out List<Int32> row1ColList)) {
							List<Int32> matchingRows = [row1];
							for (Int32 row2 = row1 + 1; row2 < dimension; row2++) {
								if (rowCandidateDictionary.TryGetValue(row2, out Dictionary<Char, List<Int32>> row2Dictionary)) {
									if (row2Dictionary.TryGetValue(thisCandidate, out List<Int32> row2ColList)) {
										// Compare the two lists to see if they have the same row set
										if (row1ColList.Count == row2ColList.Count) {
											Boolean match = true;
											foreach (Int32 candidateCol in row1ColList) {
												if (!row2ColList.Contains(candidateCol)) {
													match = false;
													break;
												}
											}
											if (match) {
												matchingRows.Add(row2);
											}
										}
									}
								}
							}
							if (matchingRows.Count == row1ColList.Count) {
								// Can proceed to remove the values from all the other columns for the same rows
								for (Int32 col = 0; col < dimension; col++) {
									if (!matchingRows.Contains(col)) {
										foreach (Int32 thisCol in row1ColList) {
											Cell thisCell = this[col, thisCol];
											//thisCell.RemoveCandidate(thisCandidate);
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
}