﻿using System;
using System.IO;
using System.Text;
using keyBordL.dataFolder;
using WindowsInput;
using WindowsInput.Native;

namespace keyBordL
{
    class Run
    {

        private static InputPort midi = new InputPort();
        private static InputSimulator IS = new InputSimulator();

        public static bool Run_(bool runPogram)
        {

            int chanel = 0;
            string[] array = new string[] { };

            if (Program.values.Count < 1)
            {
                Console.WriteLine("loading from file...");
                array = File.ReadAllLines("data.txt");
                chanel = int.Parse(array[0].Split(',')[0]);

                //color
                if (array[0].Split(',').Length > 1){
                    string[] ColorArray = array[0].Split(',');

                    InputPort midiOut = new InputPort();
                    midiOut.OpenOut(int.Parse(ColorArray[1]));

                    for (int i = 2; i+1 < ColorArray.Length; i += 2)
                    {
                        int x = int.Parse(ColorArray[i]);
                        midiOut.MidiOutMsg((Byte)x, (Byte)(x + 1));
                    }

                    midiOut.CloseOut();
                }


                foreach (string line2 in array)
                {
                    if (line2 != string.Concat(chanel))
                    {
                        int[] tempValues = new int[line2.Split(new char[]
                        {
                                    ','
                        }).Length];
                        for (int i = 0; i < tempValues.Length; i++)
                        {
                            string thiss = line2.Split(new char[]
                            {
                                        ','
                            })[i];
                            tempValues[i] = int.Parse(thiss);
                            Console.Write(thiss + " ");
                        }
                        Program.values.Add(tempValues);
                        Console.Write("\n");
                    }
                }
            }

            string[] valuesHex = new string[Program.values.Count];

            for (int i = 0; i < Program.values.Count; i++)
            {
                valuesHex[i] = Program.values[i][0].ToString("X").PadLeft(4, '0');
            }

            int old2 = 0;
            midi.Open(chanel);
            midi.Start();
            string valueHex = "", valueHex4 = "0000";
            Console.WriteLine("runing...   //press any key and then any active midi key to stop");
            while (runPogram)
            {
                if (Console.KeyAvailable)
                {
                    runPogram = false;
                }

                Program.waitHandle.WaitOne();

                int value = midi.p;
                if (old2 != value)
                {

                    valueHex = midi.pS;
                    valueHex4 = valueHex.Substring(valueHex.Length - 4);

                    if (valueHex.Substring(valueHex.Length - 2) != "D0")
                    {
                        int lenght_ = Program.values.Count;
                        for (int j = 0; j < lenght_; j++)
                        {
                            if (valuesHex[j] == valueHex4)
                            {
                                if (Program.values[j][1] == 0) // key
                                {

                                    StringBuilder sb = new StringBuilder();
                                    if (valueHex.Length > 4)
                                    {
                                        sb.Append("Key_Down '");
                                        IS.Keyboard.KeyDown((VirtualKeyCode)((ushort)Program.values[j][2]));
                                    }
                                    else
                                    {
                                        sb.Append("Key_Up '");
                                        IS.Keyboard.KeyUp((VirtualKeyCode)((ushort)Program.values[j][2]));
                                    }
                                    sb.Append(Program.values[j][2]);
                                    sb.Append("' on:");
                                    Console.WriteLine(sb);
                                    break;
                                }
                                else if (Program.values[j][1] == 2) // addonkey
                                {
                                    StringBuilder sb = new StringBuilder();
                                    if (valueHex.Length > 4)
                                    {
                                        sb.Append("Key_Down '");
                                        sb.Append(Program.values[j][2]);
                                        sb.Append(AddonButton(true));
                                        IS.Keyboard.KeyDown((VirtualKeyCode)((ushort)Program.values[j][2]));
                                    }
                                    else
                                    {
                                        sb.Append("Key_Up '");
                                        IS.Keyboard.KeyUp((VirtualKeyCode)((ushort)Program.values[j][2]));
                                        sb.Append(Program.values[j][2]);
                                        sb.Append(AddonButton(false));
                                    }
                                    sb.Append("' on:");
                                    Console.WriteLine(sb);
                                    break;
                                }
                                else if (Program.values[j][1] == 1 && valueHex.Length > 4) // macro
                                {
                                    StringBuilder sb = new StringBuilder("macro '");
                                    for (int k = 2; k < Program.values[j].Length; k++)
                                    {
                                        if (Program.values[j][k] > 0)
                                        {
                                            IS.Keyboard.KeyDown((VirtualKeyCode)((ushort)Program.values[j][k]));
                                            sb.Append("+" + Program.values[j][k] + "'");
                                        }
                                        else
                                        {
                                            IS.Keyboard.KeyUp((VirtualKeyCode)((ushort)(-(ushort)Program.values[j][k])));
                                            sb.Append(Program.values[j][k] + "'");
                                        }
                                    }
                                    sb.Append(" on:");
                                    Console.WriteLine(sb);
                                    break;
                                }
                            }

                        }
                    }
                    Console.WriteLine(valueHex + "\n");
                    old2 = value;


                }
                Program.waitHandle.Reset();
            }
            midi.Stop();
            midi.Close();
            Program.teken = 0;

            //color
            if (array[0].Split(',').Length > 1)
            {
                string[] ColorArray = array[0].Split(',');

                InputPort midiOut = new InputPort();
                midiOut.OpenOut(int.Parse(ColorArray[1]));

                for (int i = 2; i + 1 < ColorArray.Length; i += 2)
                {
                    int x = int.Parse(ColorArray[i]);
                    midiOut.MidiOutMsg((Byte)x, (Byte)(0));
                }

                midiOut.CloseOut();
            }

            return false;
        }


        private static bool addonButtonActive = false;
        private static VirtualKeyCode addonButtonKey = VirtualKeyCode.F24;
        private static string AddonButton(bool keyStatus)
        {
            if (addonButtonActive == keyStatus)
                return "";

            addonButtonActive = keyStatus;

            if (addonButtonActive)
            {
                IS.Keyboard.KeyDown(addonButtonKey);
            }
            else
            {
                IS.Keyboard.KeyUp(addonButtonKey);
            }
            return (" + " + addonButtonKey.ToString());
        }

    }
}
