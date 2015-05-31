//-----------------------------------------------------------------------
// <copyright file="WatchdogThread.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WatchedManagedApp
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;

    internal sealed class WatchdogThread : IDisposable
    {
        private readonly NativeMethods.WatchdogSafeHandle handle;

        public WatchdogThread(string signalName)
        {
            int error = NativeMethods.InitializeWatchdog(signalName, out this.handle);
            if (error != 0)
            {
                throw new Win32Exception(error);
            }
        }

        public static void Signal(string signalName)
        {
            int error = NativeMethods.SignalWatchdog(signalName);
            if (error != 0)
            {
                throw new Win32Exception(error);                
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (this.handle)
                {
                }
            }
        }

        private static class NativeMethods
        {
            private const string WatchdogLib = "WatchdogThread.dll";

            [DllImport(WatchdogLib, ExactSpelling = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
            public static extern int InitializeWatchdog([MarshalAs(UnmanagedType.LPWStr)] string name, out WatchdogSafeHandle handle);

            [DllImport(WatchdogLib, ExactSpelling = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
            public static extern int SignalWatchdog([MarshalAs(UnmanagedType.LPWStr)] string name);

            [DllImport(WatchdogLib, ExactSpelling = true)]
            public static extern int DestroyWatchdog(IntPtr handle);

            public sealed class WatchdogSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
            {
                public WatchdogSafeHandle()
                    : base(true)
                {
                }

                protected override bool ReleaseHandle()
                {
                    return NativeMethods.DestroyWatchdog(this.handle) == 0;
                }
            }
        }
    }
}
