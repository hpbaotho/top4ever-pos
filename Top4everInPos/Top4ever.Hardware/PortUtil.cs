using System;
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

        private readonly PortType _portType;
        private bool _isOpen;
        /// <summary>
        /// 串口
        /// </summary>
        private readonly SerialPort _serialPort;
        /// <summary>
        /// USB端口
        /// </summary>
        private readonly UsbDeviceFinder _usbFinder;
        private readonly WriteEndpointID _endpointId;
        private UsbDevice _usbDevice;
        private UsbEndpointWriter _usbWriter;
        /// <summary>
        /// 网口
        /// </summary>
        private readonly IPEndPoint _ipEndPoint;
        private Socket _socket;

        public PortUtil(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            _portType = PortType.COM;
        }

        public PortUtil(string usbVid, string usbPid, string endpointId)
        {
            Int32 vid = Int32.Parse(usbVid, NumberStyles.HexNumber);
            Int32 pid = Int32.Parse(usbPid, NumberStyles.HexNumber);
            _usbFinder = new UsbDeviceFinder(vid, pid);
            _endpointId = GetEndpointId(endpointId);
            _portType = PortType.USB;
        }

        public PortUtil(string ip, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            _ipEndPoint = new IPEndPoint(ipAddress, port);
            _portType = PortType.EtherNet;
        }

        public void Open()
        {
            if (_portType == PortType.COM)
            {
                _serialPort.Open();
                _isOpen = true;
            }
            if (_portType == PortType.USB)
            {
                // Find and open the usb device.
                _usbDevice = UsbDevice.OpenUsbDevice(_usbFinder);
                // If the device is open and ready
                if (_usbDevice == null) throw new Exception("Device Not Found.");

                // If this is a "whole" usb device (libusb-win32, linux libusb)
                // it will have an IUsbDevice interface. If not (WinUSB) the 
                // variable will be null indicating this is an interface of a 
                // device.
                IUsbDevice wholeUsbDevice = _usbDevice as IUsbDevice;
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
                _usbWriter = _usbDevice.OpenEndpointWriter(_endpointId);
                _isOpen = true;
            }
            if (_portType == PortType.EtherNet)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(_ipEndPoint);
                _socket.SendTimeout = 1000;
                _isOpen = true;
            }
        }

        public bool IsOpen 
        {
            get { return _isOpen; }
        }

        public void CnWrite(string data)
        {
            byte[] sendData = Encoding.GetEncoding("gb18030").GetBytes(data);
            Write(sendData);
        }

        public void Write(string data)
        {
            byte[] sendData = Encoding.Default.GetBytes(data);
            Write(sendData);
        }

        public void Write(byte[] data)
        {
            if (_portType == PortType.COM)
            {
                _serialPort.Write(data, 0, data.Length);
            }
            if (_portType == PortType.USB)
            {
                int bytesWritten;
                ErrorCode ec = _usbWriter.Write(data, 2000, out bytesWritten);
                if (ec != ErrorCode.None) throw new Exception(UsbDevice.LastErrorString);
            }
            if (_portType == PortType.EtherNet)
            {
                _socket.Send(data, data.Length, 0);
            }
        }

        public void Close()
        {
            if (_portType == PortType.COM)
            {
                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                    }
                }
            }
            if (_portType == PortType.USB)
            {
                if (_usbDevice != null)
                {
                    if (_usbDevice.IsOpen)
                    {
                        _usbDevice.Close();
                        _usbDevice = null;
                    }
                }
            }
            if (_portType == PortType.EtherNet)
            {
                if (_socket != null)
                {
                    _socket.Close();
                    _socket = null;
                }
            }
        }

        public void Dispose()
        {
            if (_portType == PortType.COM)
            {
                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                    }
                }
            }
            if (_portType == PortType.USB)
            {
                if (_usbDevice != null)
                {
                    if (_usbDevice.IsOpen)
                    {
                        // If this is a "whole" usb device (libusb-win32, linux libusb-1.0)
                        // it exposes an IUsbDevice interface. If not (WinUSB) the 
                        // 'wholeUsbDevice' variable will be null indicating this is 
                        // an interface of a device; it does not require or support 
                        // configuration and interface selection.
                        IUsbDevice wholeUsbDevice = _usbDevice as IUsbDevice;
                        if (!ReferenceEquals(wholeUsbDevice, null))
                        {
                            // Release interface #0.
                            wholeUsbDevice.ReleaseInterface(0);
                        }
                        _usbDevice.Close();
                    }
                    _usbDevice = null;
                    // Free usb resources
                    UsbDevice.Exit();
                }
            }
            if (_portType == PortType.EtherNet)
            {
                if (_socket != null)
                {
                    _socket.Close();
                    _socket = null;
                }
            }
        }

        private WriteEndpointID GetEndpointId(string endpointId)
        {
            WriteEndpointID writeEndpointId;
            switch (endpointId)
            {
                case "1":
                case "01":
                    writeEndpointId = WriteEndpointID.Ep01;
                    break;
                case "2":
                case "02":
                    writeEndpointId = WriteEndpointID.Ep02;
                    break;
                case "3":
                case "03":
                    writeEndpointId = WriteEndpointID.Ep03;
                    break;
                case "4":
                case "04":
                    writeEndpointId = WriteEndpointID.Ep04;
                    break;
                case "5":
                case "05":
                    writeEndpointId = WriteEndpointID.Ep05;
                    break;
                case "6":
                case "06":
                    writeEndpointId = WriteEndpointID.Ep06;
                    break;
                case "7":
                case "07":
                    writeEndpointId = WriteEndpointID.Ep07;
                    break;
                case "8":
                case "08":
                    writeEndpointId = WriteEndpointID.Ep08;
                    break;
                case "9":
                case "09":
                    writeEndpointId = WriteEndpointID.Ep09;
                    break;
                case "10":
                    writeEndpointId = WriteEndpointID.Ep10;
                    break;
                case "11":
                    writeEndpointId = WriteEndpointID.Ep11;
                    break;
                case "12":
                    writeEndpointId = WriteEndpointID.Ep12;
                    break;
                case "13":
                    writeEndpointId = WriteEndpointID.Ep13;
                    break;
                case "14":
                    writeEndpointId = WriteEndpointID.Ep14;
                    break;
                case "15":
                    writeEndpointId = WriteEndpointID.Ep15;
                    break;
                default:
                    writeEndpointId = WriteEndpointID.Ep02;
                    break;
            }
            return writeEndpointId;
        }
    }
}
