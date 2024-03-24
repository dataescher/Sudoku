using System;

namespace Sudoku {
	public enum GameColors {
		White,
		LightGray,
		DarkGray,
		LightRed,
		DarkRed,
		LightGreen,
		DarkGreen,
		LightBlue,
		DarkBlue,
		LightYellow,
		DarkYellow,
		LightCyan,
		DarkCyan,
		LightMagenta,
		DarkMagenta,
		LightOrange,
		DarkOrange
	}
	public static class GameColor {
		public static readonly Map<GameColors, Char> ColorMap;
		static GameColor() {
			ColorMap = new();
			ColorMap.Add(GameColors.White, 'w');
			ColorMap.Add(GameColors.LightGray, 'a');
			ColorMap.Add(GameColors.LightRed, 'r');
			ColorMap.Add(GameColors.LightGreen, 'g');
			ColorMap.Add(GameColors.LightBlue, 'b');
			ColorMap.Add(GameColors.LightYellow, 'y');
			ColorMap.Add(GameColors.LightCyan, 'c');
			ColorMap.Add(GameColors.LightMagenta, 'm');
			ColorMap.Add(GameColors.LightOrange, 'o');
		}
		public static System.Drawing.Color ToColor(GameColors cellColor) {
			return cellColor switch {
				GameColors.White => System.Drawing.Color.FromArgb(255, 255, 255),
				GameColors.LightGray => System.Drawing.Color.FromArgb(220, 220, 220),
				GameColors.DarkGray => System.Drawing.Color.FromArgb(120, 120, 120),
				GameColors.LightRed => System.Drawing.Color.FromArgb(255, 220, 220),
				GameColors.DarkRed => System.Drawing.Color.FromArgb(255, 120, 120),
				GameColors.LightGreen => System.Drawing.Color.FromArgb(220, 255, 220),
				GameColors.DarkGreen => System.Drawing.Color.FromArgb(120, 255, 120),
				GameColors.LightBlue => System.Drawing.Color.FromArgb(220, 220, 255),
				GameColors.DarkBlue => System.Drawing.Color.FromArgb(120, 120, 255),
				GameColors.LightYellow => System.Drawing.Color.FromArgb(255, 255, 220),
				GameColors.DarkYellow => System.Drawing.Color.FromArgb(255, 255, 120),
				GameColors.LightCyan => System.Drawing.Color.FromArgb(220, 255, 255),
				GameColors.DarkCyan => System.Drawing.Color.FromArgb(120, 255, 255),
				GameColors.LightMagenta => System.Drawing.Color.FromArgb(255, 220, 255),
				GameColors.DarkMagenta => System.Drawing.Color.FromArgb(255, 120, 255),
				GameColors.LightOrange => System.Drawing.Color.FromArgb(255, 220, 128),
				GameColors.DarkOrange => System.Drawing.Color.FromArgb(255, 120, 64),
				_ => System.Drawing.Color.DarkSlateGray
			};
		}
		public static Char ColorToLetter(GameColors cellColor) {
			if (!ColorMap.Forward.TryGetValue(cellColor, out Char value)) {
				throw new Exception($"Cannot translate the color {cellColor}.");
			}
			return value;
		}
		public static GameColors LetterToColor(Char letter) {
			if (!ColorMap.Reverse.TryGetValue(letter, out GameColors value)) {
				throw new Exception($"Cannot translate the letter {letter}.");
			}
			return value;
		}
	}
}
