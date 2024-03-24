using System;
using System.Collections.Generic;

namespace Sudoku.Engine.History {
	public class UndoHistory {
		public Int32 UndoLevel { get; set; }
		internal List<UndoItem> UndoItems { get; set; }
		public UndoHistory() {
			UndoLevel = 0;
			UndoItems = new();
		}
		public Int32 Count => UndoItems.Count;
		public void Add(UndoItem item) {
			if (UndoLevel < UndoItems.Count) {
				UndoItems.RemoveRange(UndoLevel, UndoItems.Count - UndoLevel);
			}
			UndoItems.Add(item);
			UndoLevel++;
		}
		public void Undo() {
			if (UndoLevel > 0) {
				UndoLevel--;
				UndoItems[UndoLevel].Undo();
			}
		}
		public void Redo() {
			if (UndoLevel < UndoItems.Count) {
				UndoItems[UndoLevel].Redo();
				UndoLevel++;
			}
		}
	}
}
