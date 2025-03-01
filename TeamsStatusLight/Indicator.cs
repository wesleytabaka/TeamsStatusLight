using System.IO.Ports;

namespace TeamsStatusLight
{
    public class Indicator : IIndicator
    {
        SerialPort _serialPort;
        bool _continue;

        public Indicator(string portName, int baudRate) { 
            _serialPort = new SerialPort();
            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;
            _serialPort.DtrEnable = true;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            StartSerial();
        }

        public void StartSerial() {
            try
            {
                _serialPort.Open();
                _continue = true;
            }
            catch (Exception ex) {
                _continue = false;
                Console.Error.WriteLine(ex.ToString());
            }
        }

        public void StopAllSerial() {
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }
            this._continue = false;
        }

        public void EnterSuspendState() {
            this.Write("SLEEP-");
        }

        public void EnterRunState() { 
            this.Write("WAKE-");
        }

        public void SetIndicator(IIndicatorInstruction instruction)
        {
            string tx = String.Join("", String.Join(",", Convert.ChangeType(instruction.transition, instruction.transition.GetTypeCode()), instruction.transitionDuration, Convert.ChangeType(instruction.effect, instruction.effect.GetTypeCode()), instruction.effectRate, instruction.r, instruction.g, instruction.b, instruction.r2, instruction.g2, instruction.b2), "-");
            Write(tx);
        }

        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string data = _serialPort.ReadExisting();
            DataReceivedCallback(data);
        }

        public void DataReceivedCallback(string message) {
            Console.WriteLine("RX: " + message);
        }

        public void Write(string input) {
            if (_continue)
            {
                try {
                    _serialPort.WriteLine(input);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                    Reconnect();
                }
            }
        }

        public void Reconnect() {
            StopAllSerial();
            StartSerial();
            EnterRunState();
        }

        public bool getIndicatorState() {
            return _continue && _serialPort.IsOpen;
        }
    }
}
