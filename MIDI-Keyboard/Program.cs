﻿using System;
using System.Collections.Generic;
using System.Threading;
using MIDIKeyboard.Miscellaneous;

namespace MIDIKeyboard
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		public static readonly EventWaitHandle waitHandle = new AutoResetEvent(false);

		private static bool runForm = true;
		private static bool runFastSetup;
		private static bool runProgram;
		private static bool runViewer;
		private static bool runOutViewer;
		private static bool runOutViewerVisual;

		public static IList<int[]> values = new List<int[]>();

		public static int teken;


		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static void Main(string[] args)
		{
			Console.Title = "MIDI Keyboard";

			if (!Settings.Read())
				Settings.Save();

			if (args.Length > 0 && args[0] == "run") runProgram = true;

			while (runForm) {
				Console.Clear();
				if (runFastSetup) FastSetup.FastSetup_(ref runFastSetup);
				if (runSetup) Setup.Setup_(ref runSetup);
				if (runProgram) Run.Run.Run_(ref runProgram);
				if (runViewer) Viewer.Viewer_(ref runViewer);
				if (runOutViewer) {
					OutViewer.OutViewer_(runOutViewer);
					runOutViewerVisual = false;
				}

				if (runOutViewerVisual) {
					OutViewerVisual.OutViewerVisual_(runOutViewerVisual);
					runOutViewerVisual = false;
				}

				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine(
					" ___ __ __    ________  ______    ________      ___   ___   ______   __  __   ______      \n/__//_//_/\\  /_______/\\/_____/\\  /_______/\\    /___/\\/__/\\ /_____/\\ /_/\\/_/\\ /_____/\\     \n\\::\\| \\| \\ \\ \\__.::._\\/\\:::_ \\ \\ \\__.::._\\/    \\::.\\ \\\\ \\ \\\\::::_\\/_\\ \\ \\ \\ \\\\::::_\\/_    \n \\:.      \\ \\   \\::\\ \\  \\:\\ \\ \\ \\   \\::\\ \\      \\:: \\/_) \\ \\\\:\\/___/\\\\:\\_\\ \\ \\\\:\\/___/\\   \n  \\:.\\-/\\  \\ \\  _\\::\\ \\__\\:\\ \\ \\ \\  _\\::\\ \\__    \\:. __  ( ( \\::___\\/_\\::::_\\/ \\_::._\\:\\  \n   \\. \\  \\  \\ \\/__\\::\\__/\\\\:\\/.:| |/__\\::\\__/\\    \\: \\ )  \\ \\ \\:\\____/\\ \\::\\ \\   /____\\:\\ \n    \\__\\/ \\__\\/\\________\\/ \\____/_/\\________\\/     \\__\\/\\__\\/  \\_____\\/  \\__\\/   \\_____\\/ \n");
				Console.ResetColor();
				Console.Write("1. fast setup\n2. setup\n");
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("3. visual setup");
				Console.ResetColor();
				Console.Write("\n4. run\n5. viewer\n6. viewer output\n7. viewer output visual\n8. quit\n");
				switch (Console.ReadKey().Key) {
					case ConsoleKey.D1:
						runFastSetup = true;
						break;
					case ConsoleKey.D2:
						runSetup = true;
						break;
					case ConsoleKey.D3:
						runVisualSetup = true;
						break;
					case ConsoleKey.D4:
						runProgram = true;
						break;
					case ConsoleKey.D5:
						runViewer = true;
						break;
					case ConsoleKey.D6:
						runOutViewer = true;
						break;
					case ConsoleKey.D7:
						runOutViewerVisual = true;
						break;
					case ConsoleKey.D8:
						runForm = false;
						break;
				}
			}
		}


		public static int GetTeken()
		{
			teken++;
			switch (teken) {
				case 1:
					return 9;
				case 2:
					return 13;
				case 3:
					return 16;
				case 4:
					return 17;
				case 5:
					return 32;
				case 6:
					return 35;
				case 7:
					return 36;
				case 8:
					return 37;
				case 9:
					return 38;
				case 10:
					return 39;
				case 11:
					return 40;
				case 12:
					return 45;
				case 13:
					return 46;
				case 14:
					return 48;
				case 15:
					return 49;
				case 16:
					return 50;
				case 17:
					return 51;
				case 18:
					return 52;
				case 19:
					return 53;
				case 20:
					return 54;
				case 21:
					return 55;
				case 22:
					return 56;
				case 23:
					return 57;
				case 24:
					return 65;
				case 25:
					return 66;
				case 26:
					return 67;
				case 27:
					return 68;
				case 28:
					return 69;
				case 29:
					return 70;
				case 30:
					return 71;
				case 31:
					return 72;
				case 32:
					return 73;
				case 33:
					return 74;
				case 34:
					return 75;
				case 35:
					return 76;
				case 36:
					return 77;
				case 37:
					return 78;
				case 38:
					return 79;
				case 39:
					return 80;
				case 40:
					return 81;
				case 41:
					return 82;
				case 42:
					return 83;
				case 43:
					return 84;
				case 44:
					return 85;
				case 45:
					return 86;
				case 46:
					return 87;
				case 47:
					return 88;
				case 48:
					return 89;
				case 49:
					return 90;
				case 50:
					return 90;
				case 51:
					return 96;
				case 52:
					return 97;
				case 53:
					return 98;
				case 54:
					return 99;
				case 55:
					return 100;
				case 56:
					return 102;
				case 57:
					return 103;
				case 58:
					return 104;
				case 59:
					return 105;
				case 61:
					return 106;
				case 62:
					return 107;
				case 63:
					return 108;
				case 64:
					return 109;
				case 65:
					return 110;
				case 66:
					return 111;
				case 67:
					return 160;
				case 68:
					return 161;
				case 69:
					return 162;
				case 70:
					return 163;
				case 71:
					return 186;
				case 72:
					return 187;
				case 73:
					return 188;
				case 74:
					return 189;
				case 75:
					return 190;
				case 76:
					return 191;
				case 77:
					return 192;
				case 78:
					return 219;
				case 79:
					return 220;
				case 80:
					return 221;
				case 81:
					return 222;
				case 82:
					return 223;
				case 83:
					return 226;
			}

			return 0;
		}
		#pragma warning disable IDE0052 // Remove unread private members
		private static bool runSetup;
		private static bool runVisualSetup;
		#pragma warning restore IDE0052 // Remove unread private members
	}
}