using System;
using System.ServiceProcess;
using Top4ever.NetServer;

namespace VechPosWinService
{
    public partial class VechPosService : ServiceBase
    {
        private SocketListener _socketListener = null;

        public VechPosService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        public void Start()
        {
            const int port = 5689;
            const int numConnections = 10;
            const int bufferSize = Int16.MaxValue;
            _socketListener = new SocketListener(numConnections, bufferSize);
            _socketListener.Start(port);
        }

        protected override void OnStop()
        {
            Close();
        }

        public void Close()
        {
            if (_socketListener != null)
            {
                _socketListener.Stop();
            }
        }
    }
}
