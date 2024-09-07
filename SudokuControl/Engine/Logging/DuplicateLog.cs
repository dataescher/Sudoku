using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Sudoku.Engine.Logging {
	public class DuplicateLog : LogItem {
		private readonly Dictionary<Unit, Dictionary<Char, List<Cell>>> _duplicates;

		public DuplicateLog(Grid grid, Dictionary<Unit, Dictionary<Char, List<Cell>>> duplicates) : base(grid) {
			_duplicates = null;
			if (duplicates is not null) {
				_duplicates = new();
				foreach (KeyValuePair<Unit, Dictionary<Char, List<Cell>>> thisDuplicateUnitEntry in duplicates) {
					Unit newUnit = _grid.Units[thisDuplicateUnitEntry.Key.UnitIndex - 1];
					Dictionary<Char, List<Cell>> thisUnitDuplicates = new();
					_duplicates.Add(newUnit, thisUnitDuplicates);
					foreach (KeyValuePair<Char, List<Cell>> thisUnitDuplicateData in thisDuplicateUnitEntry.Value) {
						List<Cell> duplicateCells = new();
						thisUnitDuplicates.Add(thisUnitDuplicateData.Key, duplicateCells);
						foreach (Cell thisUnitDuplicateCell in thisUnitDuplicateData.Value) {
							duplicateCells.Add(_grid.Cells[thisUnitDuplicateCell.Index]);
						}
					}
				}
			}
		}
		public override LogDetail Details {
			get {
				LogDetail details = new(this);
				Boolean completionCheckResult = (_duplicates is null);
				String status;
				if (completionCheckResult) {
					status = "PASSED";
					Color = Color.LightGreen;
				} else {
					status = "FAILED";
					Color = Color.Red;
				}
				details.Description = $"Checking for duplicate cell values in units -- {status}";
				if (_duplicates is not null) {
					Int32 duplicateCount = 0;
					LogDetail duplicateDetail = new(this);
					details.SubDetails.Add(duplicateDetail);
					IOrderedEnumerable<KeyValuePair<Unit, Dictionary<Char, List<Cell>>>> unitDuplicates = _duplicates.ToList().OrderBy(x => x.Key.Index);
					foreach (KeyValuePair<Unit, Dictionary<Char, List<Cell>>> thisUnitDuplicates in unitDuplicates) {
						LogDetail unitDetail = new UnitDetail(this, thisUnitDuplicates.Key, GameColors.LightYellow);
						duplicateDetail.SubDetails.Add(unitDetail);
						IOrderedEnumerable<KeyValuePair<Char, List<Cell>>> valueDuplicates = thisUnitDuplicates.Value.ToList().OrderBy(x => x.Key);
						foreach (KeyValuePair<Char, List<Cell>> thisValueDuplicates in valueDuplicates) {
							LogDetail valueDetail = new(this, $"Value {thisValueDuplicates.Key}");
							unitDetail.SubDetails.Add(valueDetail);
							IOrderedEnumerable<Cell> cellDuplicates = thisValueDuplicates.Value.OrderBy(x => x.Index);
							Boolean first = true;
							foreach (Cell thisDuplicate in cellDuplicates) {
								valueDetail.SubDetails.Add(new CellDetail(this, thisDuplicate, GameColors.LightRed));
								if (!first) {
									duplicateCount++;
								}
								first = false;
							}
						}
					}
					duplicateDetail.Description = $"Duplicate values found: {duplicateCount}";
				}

				return details;
			}
		}

		public override void ApplyColoring() {
			if (_duplicates is not null) {
				foreach (Dictionary<Char, List<Cell>> thisDuplicate in _duplicates.Values) {
					foreach (List<Cell> thisDuplicateCellGroup in thisDuplicate.Values) {
						foreach (Cell thisDuplicateCell in thisDuplicateCellGroup) {
							thisDuplicateCell.Color = GameColors.LightRed;
						}
					}
				}
			}
		}
	}
}
