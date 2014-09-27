using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace VechsoftPos
{
    public delegate void LDTCallBack(int port);

    public class LDT
    {
        [DllImport("Sandu.dll", EntryPoint = "Setup_Tel")]
        public static extern void Setup_Tel(LDTCallBack CallBack, int y);

        [DllImport("Sandu.dll", EntryPoint = "Begin_Tel")]
        public static extern int Begin_Tel(int Port, char Power);

        [DllImport("Sandu.dll", EntryPoint = "End_Tel")]
        public static extern int End_Tel(int Port);

        [DllImport("Sandu.dll", EntryPoint = "GetNumber_Tel")]
        public static extern StringBuilder GetNumber_Tel(int Port);

        [DllImport("Sandu.dll", EntryPoint = "DevCount_Tel")]
        public static extern int DevCount_Tel();

        [DllImport("Sandu.dll", EntryPoint = "Hookup_Tel")]
        public static extern int Hookup_Tel(int port, int flag);

        [DllImport("Sandu.dll", EntryPoint = "Plugin_Tel")]
        public static extern bool Plugin_Tel(int port);

        [DllImport("Sandu.dll", EntryPoint = "LineOffHook_Tel")]
        public static extern bool LineOffHook_Tel(int port);

        [DllImport("Sandu.dll", EntryPoint = "ISRing_Tel")]
        public static extern bool ISRing_Tel(int port);

        [DllImport("Sandu.dll", EntryPoint = "Check_State")]
        public static extern int Check_State(int port);
    }
}
