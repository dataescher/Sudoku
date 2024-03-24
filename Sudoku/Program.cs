using System;
using System.Windows.Forms;

namespace Sudoku {
	internal static class Program {
		public static GameData Puzzles { get; private set; }

		static Program() {
			Puzzles = new();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main() {
			Puzzles = GameData.Load("sudoku.xml");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FrmSudoku());
		}
	}
}
