using System.IO.Ports;

namespace TeamsStatusLight
{
    public class Indicator : IIndicator
    {
        SerialPort _serialPort;
        bool _continue;

        public Indicator(string portName) { 
            _serialPort = new SerialPort();
            _serialPort.PortName = portName;
            _serialPort.Open();
            _continue = true;

            Listen();
        }
        void IIndicator.SetIndicator(IIndicatorInstruction instruction)
        {
            Write(String.Join(",", instruction.transition, instruction.transitionDuration, instruction.effect, instruction.effectRate, instruction.r, instruction.g, instruction.b, instruction.r2, instruction.g2, instruction.b2, "-"));
        }

        public void SetIndicator(IIndicatorInstruction instruction)
        {
            Write(String.Join(",", Convert.ChangeType(instruction.transition, instruction.transition.GetTypeCode()), instruction.transitionDuration, Convert.ChangeType(instruction.effect, instruction.effect.GetTypeCode()), instruction.effectRate, instruction.r, instruction.g, instruction.b, instruction.r2, instruction.g2, instruction.b2, "-"));
        }

        void Listen()
        {
            Thread readThread = new Thread(Read);
            readThread.Start();
        }

        void Read()
        {
            while (_continue)
            {
                string input = _serialPort.ReadLine();
                Console.WriteLine(input);
            }
        }

        void Write(string input) {
            _serialPort.WriteLine(input);
        }

        void IIndicator.Write(string input)
        {
            throw new NotImplementedException();
        }
    }
}
