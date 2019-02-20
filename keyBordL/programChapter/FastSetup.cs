﻿
using System;
using System.Collections.Generic;
using System.IO;
using keyBordL.dataFolder;

namespace keyBordL
{
    class FastSetup
    {

        private static InputPort midi = new InputPort();

        public static bool FastSetup_(bool runFastSetup)
        {

            
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Warning: you will now overwrite the data.txt file! any old setups will be deleted.\n");
                Console.ResetColor();
                Console.WriteLine("what midi port do you whant to use \n(they start at 0 and count up from that, remember that some devices use multiple ports)");
                Console.Write("port: ");
                int resultat;
                int.TryParse(Console.ReadLine(), out resultat);
                Console.WriteLine("value set to " + resultat);
                int chanel = resultat;
                Console.WriteLine("\npress the midi keys you whant to use, and wen you're done press any key on your keybord\n");
                midi.Open(chanel);
                midi.Start();
                int old = 0;
                int value = 0;
                string valueHex = "";
                string hex4 = "0000";
                while (runFastSetup)
                {
                    if (Console.KeyAvailable)
                    {
                        runFastSetup = false;
                    }
                    value = midi.p;
                    if (old != value)
                    {
                        valueHex = midi.pS;
                        if (hex4 != valueHex.Substring(valueHex.Length - 4))
                        {
                            if (hex4.Substring(hex4.Length - 2) != "D0")
                            {
                                hex4 = valueHex.Substring(valueHex.Length - 4);

                                int hex4Con = int.Parse(hex4, System.Globalization.NumberStyles.HexNumber);

                                bool x2 = true;
                                using (IEnumerator<int[]> enumerator = Program.values.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        if (enumerator.Current[0] == hex4Con)
                                        {
                                            x2 = false;
                                            break;
                                        }
                                    }
                                }
                                if (x2)
                                {
                                    Program.values.Add(new int[]
                                    {
                                            hex4Con,
                                            0,
                                            Program.getTeken()
                                    });
                                }
                            }

                        }

                        if (valueHex.Substring(valueHex.Length - 2) == "D0")
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(valueHex.PadLeft(6, ' ').Substring(0, 4));
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("D0 ");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(value);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.Write(valueHex.PadLeft(6, ' ').Substring(0, 2));
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(hex4 + " ");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(value);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        old = value;
                    }
                }
                midi.Stop();
                using (StreamWriter file = new StreamWriter("data.txt"))
                {
                    file.WriteLine(chanel);
                    foreach (int[] line in Program.values)
                    {
                        file.WriteLine(string.Concat(new object[]
                        {
                                line[0],
                                ",",
                                line[1],
                                ",",
                                line[2]
                        }));
                    }
                }

            Program.teken = 0;

            return runFastSetup;
        }


    }
}