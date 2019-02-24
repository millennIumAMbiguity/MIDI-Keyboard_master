﻿using System;
using System.Runtime.InteropServices;

namespace keyBordL.dataFolder
{
    public class InputPort
    {
        private NativeMethods.MidiInProc midiInProc;
        private NativeMethods.MidiOutProc midiOutProc;
        private IntPtr handle, handleOut;

        public int p = 0;
        public string pS = "";

        public InputPort()
        {
            midiInProc = new NativeMethods.MidiInProc(MidiProc);
            handle = IntPtr.Zero;

            midiOutProc = new NativeMethods.MidiOutProc(MidiProc);
            handleOut = IntPtr.Zero;
        }


        public int InputCount()
        {
            return NativeMethods.midiInGetNumDevs();
        }


        public bool Close()
        {
            bool result = NativeMethods.midiInClose(handle)
                == NativeMethods.MMSYSERR_NOERROR;
            handle = IntPtr.Zero;
            return result;
        }

        public bool Open(int id)
        {
            return NativeMethods.midiInOpen(
                out handle,
                id,
                midiInProc,
                IntPtr.Zero,
                NativeMethods.CALLBACK_FUNCTION)
                    == NativeMethods.MMSYSERR_NOERROR;
        }

        public bool Start()
        {
            return NativeMethods.midiInStart(handle)
                == NativeMethods.MMSYSERR_NOERROR;
        }

        public bool Stop()
        {
            return NativeMethods.midiInStop(handle)
                == NativeMethods.MMSYSERR_NOERROR;
        }

        private void MidiProc(
            IntPtr hMidiIn,
            int wMsg,
            IntPtr dwInstance,
            int dwParam1,
            int dwParam2)
        {

            pS = dwParam1.ToString("X").PadLeft(4, '0');  // Gives you hexadecimal
            p = dwParam1;

            Program.waitHandle.Set();

            // Receive messages here
        }


        public bool MidiOutMsg(byte pitch, byte velocity)
        {
            //hmidi is an IntPtr obtained via midiOutOpen or other means.

            byte[] data = new byte[4];
            data[0] = 0x90;//note on, channel 0
            data[1] = pitch;//pitch
            data[2] = velocity;//velocity
            int msg = BitConverter.ToInt32(data, 0);

            return (NativeMethods.midiOutShortMsg(handleOut, msg) )
                == NativeMethods.MMSYSERR_NOERROR;
        }

        public int OutputCount()
        {
            return NativeMethods.midiOutGetNumDevs();
        }

        public bool CloseOut()
        {
            bool result = NativeMethods.midiOutClose(handleOut)
                == NativeMethods.MMSYSERR_NOERROR;
            handleOut = IntPtr.Zero;
            return result;
        }

        public bool OpenOut(int id)
        {
            return NativeMethods.midiOutOpen(
                out handleOut,
                id,
                midiOutProc,
                IntPtr.Zero,
                NativeMethods.CALLBACK_FUNCTION)
                    == NativeMethods.MMSYSERR_NOERROR;
        }
        public bool midiOutReset()
        {
            return (NativeMethods.midiOutReset(handleOut)) 
                == NativeMethods.MMSYSERR_NOERROR;
        }

        


    }

    internal static class NativeMethods
    {
        internal const int MMSYSERR_NOERROR = 0;
        internal const int CALLBACK_FUNCTION = 0x00030000;

        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct MIDIINCAPS
        {
            public UInt16 wMid;
            public UInt16 wPid;
            public UInt32 vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public UInt32 dwSupport;
        }

        public static string midiInGetDevCaps(IntPtr uDeviceID)
        {
            NativeMethods.MIDIINCAPS caps;
             NativeMethods.midiInGetDevCaps(uDeviceID, out caps,
                (UInt32)Marshal.SizeOf(typeof(NativeMethods.MIDIINCAPS)));
            return caps.szPname;
        }

        public static string midiOutGetDevCaps(IntPtr uDeviceID)
        {
            NativeMethods.MIDIINCAPS caps;
            NativeMethods.midiOutGetDevCaps(uDeviceID, out caps,
               (UInt32)Marshal.SizeOf(typeof(NativeMethods.MIDIINCAPS)));
            return caps.szPname;
        }

        internal delegate void MidiInProc(
            IntPtr hMidiIn,
            int wMsg,
            IntPtr dwInstance,
            int dwParam1,
            int dwParam2);

        [DllImport("winmm.dll")]
        internal static extern int midiInGetNumDevs();

        [DllImport("winmm.dll", SetLastError = true)]
        internal static extern int midiInGetDevCaps(IntPtr uDeviceID, out MIDIINCAPS caps,
        UInt32 cbMidiInCaps);

        [DllImport("winmm.dll", SetLastError = true)]
        internal static extern int midiOutGetDevCaps(IntPtr uDeviceID, out MIDIINCAPS caps,
        UInt32 cbMidiInCaps);

        [DllImport("winmm.dll")]
        internal static extern int midiInClose(
            IntPtr hMidiIn);

        [DllImport("winmm.dll")]
        internal static extern int midiInOpen(
            out IntPtr lphMidiIn,
            int uDeviceID,
            MidiInProc dwCallback,
            IntPtr dwCallbackInstance,
            int dwFlags);

        [DllImport("winmm.dll")]
        internal static extern int midiInStart(
            IntPtr hMidiIn);

        [DllImport("winmm.dll")]
        internal static extern int midiInStop(
            IntPtr hMidiIn);



        internal delegate void MidiOutProc(
            IntPtr hMidiIn,
            int wMsg,
            IntPtr dwInstance,
            int dwParam1,
            int dwParam2);

        [DllImport("winmm.dll")]
        internal static extern int midiOutShortMsg(
            IntPtr hMidiOut,
            int dwMsg
            );

        [DllImport("winmm.dll")]
        internal static extern int midiOutGetNumDevs();

        [DllImport("winmm.dll")]
        internal static extern int midiOutOpen(
            out IntPtr lphMidiIn,
            int uDeviceID,
            MidiOutProc dwCallback,
            IntPtr dwCallbackInstance,
            int dwFlags);


        [DllImport("winmm.dll")]
        internal static extern int midiOutClose(
            IntPtr hMidiIn);

        [DllImport("winmm.dll")]
        internal static extern int midiOutReset(
            IntPtr hMidiIn);


    }
}