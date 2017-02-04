using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FluentWindsor.Hawkeye.Appenders
{
    public class UdpAppender : AppenderSkeleton
    {
        private int localPort;
        private int remotePort;
        private UdpClient client;
        private IPAddress remoteAddress;
        private IPEndPoint remoteEndPoint;

        private Encoding encoding = Encoding.Default;

        public IPAddress RemoteAddress
        {
            get
            {
                return this.remoteAddress;
            }
            set
            {
                this.remoteAddress = value;
            }
        }

        public int RemotePort
        {
            get
            {
                return this.remotePort;
            }
            set
            {
                if (value < 0 || value > (int)ushort.MaxValue)
                    throw SystemInfo.CreateArgumentOutOfRangeException("value", (object)value, "The value specified is less than " + 0.ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + " or greater than " + ((int)ushort.MaxValue).ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + ".");
                else
                    this.remotePort = value;
            }
        }

        public int LocalPort
        {
            get
            {
                return this.localPort;
            }
            set
            {
                if (value != 0 && (value < 0 || value > (int)ushort.MaxValue))
                    throw SystemInfo.CreateArgumentOutOfRangeException("value", (object)value, "The value specified is less than " + 0.ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + " or greater than " + ((int)ushort.MaxValue).ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + ".");
                else
                    this.localPort = value;
            }
        }

        public Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value;
            }
        }

        protected UdpClient Client
        {
            get
            {
                return this.client;
            }
            set
            {
                this.client = value;
            }
        }

        protected IPEndPoint RemoteEndPoint
        {
            get
            {
                return this.remoteEndPoint;
            }
            set
            {
                this.remoteEndPoint = value;
            }
        }

        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (this.RemoteAddress == null)
                throw new ArgumentNullException("The required property 'Address' was not specified.");

            if (this.RemotePort < 0 || this.RemotePort > (int)ushort.MaxValue)
                throw SystemInfo.CreateArgumentOutOfRangeException("this.RemotePort", (object)this.RemotePort, "The RemotePort is less than " + 0.ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + " or greater than " + ((int)ushort.MaxValue).ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + ".");
            else if (this.LocalPort != 0 && (this.LocalPort < 0 || this.LocalPort > (int)ushort.MaxValue))
                throw SystemInfo.CreateArgumentOutOfRangeException("this.LocalPort", (object)this.LocalPort, "The LocalPort is less than " + 0.ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + " or greater than " + ((int)ushort.MaxValue).ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + ".");
            else
            {
                this.RemoteEndPoint = new IPEndPoint(this.RemoteAddress, this.RemotePort);
                this.InitializeClientConnection();
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                byte[] bytes = this.encoding.GetBytes(this.RenderLoggingEvent(loggingEvent).ToCharArray());
                this.Client.Send(bytes, bytes.Length, this.RemoteEndPoint);
            }
            catch (Exception ex)
            {
                this.ErrorHandler.Error("Unable to send logging event to remote host " + (object)this.RemoteAddress.ToString() + " on port " + (string)(object)this.RemotePort + ".", ex, ErrorCode.WriteFailure);
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (this.Client == null)
                return;
            this.Client.Close();
            this.Client = (UdpClient)null;
        }

        protected virtual void InitializeClientConnection()
        {
            try
            {
                if (this.LocalPort == 0)
                    this.Client = new UdpClient(this.RemoteAddress.AddressFamily);
                else
                    this.Client = new UdpClient(this.LocalPort, this.RemoteAddress.AddressFamily);
            }
            catch (Exception ex)
            {
                this.ErrorHandler.Error("Could not initialize the UdpClient connection on port " + this.LocalPort.ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + ".", ex, ErrorCode.GenericFailure);
                this.Client = (UdpClient)null;
            }
        }
    }
}