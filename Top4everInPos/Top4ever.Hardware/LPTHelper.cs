using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Top4ever.Hardware
{
    public class LPTHelper : IDisposable
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, int lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        private IntPtr iHandle;
        private FileStream fs;
        private StreamWriter m_StreamWriter;

        public LPTHelper(string LPTName)
        {
            try
            {
                iHandle = CreateFile(LPTName, 0x40000000, 0, 0, 3, 0, 0);
                if (iHandle.ToInt32() == -1)
                {
                    //log 没有连接打印机或者打印机端口未打开!
                }
                else
                {
                    SafeFileHandle handle = new SafeFileHandle(iHandle, true);
                    fs = new FileStream(handle, FileAccess.ReadWrite);
                    m_StreamWriter = new StreamWriter(fs, Encoding.Default);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LPTHelper(string LPTName, Encoding encoding)
        {
            try
            {
                iHandle = CreateFile(LPTName, 0x40000000, 0, 0, 3, 0, 0);
                if (iHandle.ToInt32() == -1)
                {
                    //log 没有连接打印机或者打印机端口未打开!
                }
                else
                {
                    SafeFileHandle handle = new SafeFileHandle(iHandle, true);
                    fs = new FileStream(handle, FileAccess.ReadWrite);
                    m_StreamWriter = new StreamWriter(fs, encoding);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Write(string data)
        {
            m_StreamWriter.Write(data);
        }

        public void WriteLine(string data)
        {
            m_StreamWriter.WriteLine(data);
        }

        public void Close()
        {
            m_StreamWriter.Close();
            fs.Close();
            CloseHandle(iHandle);
        }

        public void Dispose()
        {
            m_StreamWriter.Close();
            fs.Close();
            CloseHandle(iHandle);
        }
    }
}
