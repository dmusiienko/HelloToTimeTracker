using System;
using System.Runtime.InteropServices;


namespace HelloToTimeTracker
{
    /// <summary>
    /// <para>This Program is used for preventing system going into Sleep state.</para>
    /// 
    /// <para>kernel32.dll - Core library of the Windows OS, connected dynamically.
    /// It presents the basic operations in Windows OS, like data synchronization, mamory distribution, IO handling, etc.</para>
    /// </summary>
    internal class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        #region Console Window handling

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

        #endregion

        /// <summary>
        /// Types of sleep:
        /// 
        ///  <para>- disable monitor sleep:</para>
        ///     <para>SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);</para>
        ///     
        ///  <para>- disable monitor sleep and keep system awake:</para>
        ///     <para>SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED);</para>
        ///     
        ///  <para>- disable monitor sleep and keep system awake and prevent idle to sleep:</para>
        ///     <para>SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);</para>
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Hello TimeTracker!");
            ShowWindow(ThisConsole, MINIMIZE);

            Console.WriteLine("Preventing Sleep!");
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);

            // Wait for user
            Console.WriteLine("\npress any key to continue...\n");
            Console.ReadKey();

            // Allow sleeping
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }

        /// <summary>
        /// Represents the system states.
        /// More detailed information - https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-setthreadexecutionstate
        /// </summary>
        [Flags]
        private enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040, // Performs background work even if the computer appears to be sleeping
            ES_CONTINUOUS = 0x80000000, // Remain the same state until the next call
            ES_DISPLAY_REQUIRED = 0x00000002, // Resetting the display idle timer
            ES_SYSTEM_REQUIRED = 0x00000001 // Resetting the system idle timer
        }
    }
}
