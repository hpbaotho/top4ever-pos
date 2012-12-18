using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Net;
using System.Net.Sockets;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace Top4ever.Hardware
{
    public class PortUtil : IDisposable
    {
        private enum PortType { COM, USB, EtherNet }

        private PortType m_PortType;
        private bool m_IsOpen;
        /// <summary>
        /// 串口
        /// </summary>
        private SerialPort m_SerialPort;
        /// <summary>
        /// USB端口
        /// </summary>
        private UsbDeviceFinder m_UsbFinder;
        private UsbDevice m_UsbDevice;
        private UsbEndpointWriter m_Writer;
        /// <summary>
        /// 网口
        /// </summary>
        private IPEndPoint m_IPEndPoint;
        private Socket m_Socket;

        public PortUtil(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            m_SerialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            m_PortType = PortType.COM;
        }

        public PortUtil(string usbVID, string usbPID)
        {
            Int32 VID = Int32.Parse(usbVID, NumberStyles.HexNumber);
            Int32 PID = Int32.Parse(usbPID, NumberStyles.HexNumber);
            m_UsbFinder = new UsbDeviceFinder(VID, PID);
            m_PortType = PortType.USB;
        }

        public PortUtil(string ip, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            m_IPEndPoint = new IPEndPoint(ipAddress, port);
            m_PortType = PortType.EtherNet;
        }

        public void Open()
        {
            if (m_PortType == PortType.COM)
            {
                try
                {
                    m_SerialPort.Open();
                    m_IsOpen = true;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            if (m_PortType == PortType.USB)
            {
                try
                {
                    // Find and open the usb device.
                    m_UsbDevice = UsbDevice.OpenUsbDevice(m_UsbFinder);
                    // If the device is open and ready
                    if (m_UsbDevice == null) throw new Exception("Device Not Found.");

                    // If this is a "whole" usb device (libusb-win32, linux libusb)
                    // it will have an IUsbDevice interface. If not (WinUSB) the 
                    // variable will be null indicating this is an interface of a 
                    // device.
                    IUsbDevice wholeUsbDevice = m_UsbDevice as IUsbDevice;
                    if (!ReferenceEquals(wholeUsbDevice, null))
                    {
                        // This is a "whole" USB device. Before it can be used, 
                        // the desired configuration and interface must be selected.

                        // Select config #1
                        wholeUsbDevice.SetConfiguration(1);

                        // Claim interface #0.
                        wholeUsbDevice.ClaimInterface(0);
                    }
                    // open write endpoint 1.
                    m_Writer = m_UsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);
                    m_IsOpen = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (m_PortType == PortType.EtherNet)
            {
                try
                {
                    m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_Socket.Connect(m_IPEndPoint);
                    m_Socket.SendTimeout = 1000;
                    m_IsOpen = true;
                }
                catch (ArgumentNullException e1)
                {
                    throw e1;
                }
                catch (SocketException e2)
                {
                    throw e2;
                }
            }
        }

        public bool IsOpen 
        {
            get { return m_IsOpen; }
        }

        public void CNWrite(string data)
        {
            byte[] send_data = Encoding.GetEncoding("gb18030").GetBytes(data);
            Write(send_data);
        }

        public void Write(string data)
        {
            byte[] send_data = Encoding.Default.GetBytes(data);
            Write(send_data);
        }

        public void Write(byte[] data)
        {
            try
            {
                if (m_PortType == PortType.COM)
                {
                    m_SerialPort.Write(data, 0, data.Length);
                }
                if (m_PortType == PortType.USB)
                {
                    ErrorCode ec = ErrorCode.None;
                    int bytesWritten;
                    ec = m_Writer.Write(data, 2000, out bytesWritten);
                    if (ec != ErrorCode.None) throw new Exception(UsbDevice.LastErrorString);
                }
                if (m_PortType == PortType.EtherNet)
                {
                    m_Socket.Send(data, data.Length, 0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Close()
        {
            if (m_PortType == PortType.COM)
            {
                if (m_SerialPort != null)
                {
                    if (m_SerialPort.IsOpen)
                    {
                        m_SerialPort.Close();
                    }
                }
            }
            if (m_PortType == PortType.USB)
            {
                if (m_UsbDevice != null)
                {
                    if (m_UsbDevice.IsOpen)
                    {
                        m_UsbDevice.Close();
                        m_UsbDevice = null;
                    }
                }
            }
            if (m_PortType == PortType.EtherNet)
            {
                if (m_Socket != null)
                {
                    m_Socket.Close();
                    m_Socket = null;
                }
            }
        }

        public void Dispose()
        {
            if (m_PortType == PortType.COM)
            {
                if (m_SerialPort != null)
                {
                    if (m_SerialPort.IsOpen)
                    {
                        m_SerialPort.Close();
                    }
                }
            }
            if (m_PortType == PortType.USB)
            {
                if (m_UsbDevice != null)
                {
                    if (m_UsbDevice.IsOpen)
                    {
                        // If this is a "whole" usb device (libusb-win32, linux libusb-1.0)
                        // it exposes an IUsbDevice interface. If not (WinUSB) the 
                        // 'wholeUsbDevice' variable will be null indicating this is 
                        // an interface of a device; it does not require or support 
                        // configuration and interface selection.
                        IUsbDevice wholeUsbDevice = m_UsbDevice as IUsbDevice;
                        if (!ReferenceEquals(wholeUsbDevice, null))
                        {
                            // Release interface #0.
                            wholeUsbDevice.ReleaseInterface(0);
                        }
                        m_UsbDevice.Close();
                    }
                    m_UsbDevice = null;
                    // Free usb resources
                    UsbDevice.Exit();
                }
            }
            if (m_PortType == PortType.EtherNet)
            {
                if (m_Socket != null)
                {
                    m_Socket.Close();
                    m_Socket = null;
                }
            }
        }
    }
}
