using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Sudoku.Engine.Logging {
	public abstract class LogItem {
		protected readonly String[] setNumbering = {
			"single",
			"pair",
			"triple",
			"quadruple",
			"quintuple",
			"sextuple",
			"septuple",
			"octuple",
			"nonuple",
			"decuple",
			"undecuple",
			"duodecuple",
			"tredecuple",
			"quattuordecuple",
			"quindecuple",
			"sexdecuple",
			"septendecuple",
			"octodecuple",
			"novemdecuple",
			"vigintuple",
			"unvigintuple",
			"duovigintuple",
			"trevigintuple",
			"quattuorvigintuple",
			"quinvigintuple",
			"sexvigintuple",
			"septenvigintuple",
			"octovigintuple",
			"novemvigintuple",
			"trigintuple",
			"untrigintuple",
			"duotrigintuple"
		};
		public Color Color { get; set; }
		internal Grid _grid;
		public LogItem(Grid grid) {
			_grid = grid.Clone() as Grid;
			_grid.ClearHighlighting();
			Color = Color.White;
		}
		public abstract LogDetail Details { get; }
		public abstract void ApplyColoring();
		public override String ToString() {
			StringBuilder sb = new();
			PrintDetails(sb, Details);
			return sb.ToString();

		}
		internal String PrintDetails(StringBuilder sb, LogDetail detail, Int32 tabLevel = 0) {
			if (tabLevel > 0) {
				sb.Append(' ', tabLevel * 2);
			}
			sb.Append(detail.Description);
			sb.Append(Environment.NewLine);
			foreach (LogDetail subDetail in detail.SubDetails) {
				PrintDetails(sb, subDetail, tabLevel + 1);
			}
			return sb.ToString();
		}
		internal void ShowGrid(Grid grid) {
			foreach (Cell cell in grid.Cells) {
				Cell copyCell = _grid.Cells[cell.Index];
				cell.ClearCandidates();
				cell._value = copyCell._value;
				cell._color = copyCell._color;
				foreach (KeyValuePair<Char, Cell.CandidateData> copyCandidate in copyCell._candidates) {
					cell.AddCandidate(copyCandidate.Key);
					cell.SetCandidateColor(copyCandidate.Key, copyCandidate.Value.Color);
				}
			}
		}

		public void ShowItem(Grid grid) {
			ApplyColoring();
			ShowGrid(grid);
		}
	}
}