using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using EasyModbus;
using System.Diagnostics;

namespace PAC_Characterstic_Recorder
{
    public class CharacteristicRecorder : INotifyPropertyChanged
    {
        private Characterstic _currentL1Characteristic = new Characterstic();
        private Characterstic _currentL2Characteristic = new Characterstic();
        private Characterstic _currentL3Characteristic = new Characterstic();
        private Characterstic _voltageL1NCharacteristic = new Characterstic();
        private Characterstic _voltageL2NCharacteristic = new Characterstic();
        private Characterstic _voltageL3NCharacteristic = new Characterstic();
        private Characterstic _voltageL1L2Characteristic = new Characterstic();
        private Characterstic _voltageL2L3Characteristic = new Characterstic();
        private Characterstic _voltageL3L1Characteristic = new Characterstic();
        private Characterstic _activePowerL1Characteristic = new Characterstic();
        private Characterstic _activePowerL2Characteristic = new Characterstic();
        private Characterstic _activePowerL3Characteristic = new Characterstic();
        private Characterstic _reactivePowerL1Characteristic = new Characterstic();
        private Characterstic _reactivePowerL2Characteristic = new Characterstic();
        private Characterstic _reactivePowerL3Characteristic = new Characterstic();
        private Characterstic _totalActivePowerCharacteristic = new Characterstic();
        private Characterstic _totalReactivePowerCharacteristic = new Characterstic();


        public void ClearAll()
        {
            _currentL1Characteristic.Clear();
            _currentL2Characteristic.Clear();
            _currentL3Characteristic.Clear();
            _voltageL1NCharacteristic.Clear();
            _voltageL2NCharacteristic.Clear();
            _voltageL3NCharacteristic.Clear();
            _voltageL1L2Characteristic.Clear();
            _voltageL2L3Characteristic.Clear();
            _voltageL3L1Characteristic.Clear();
            _activePowerL1Characteristic.Clear();
            _activePowerL2Characteristic.Clear();
            _activePowerL3Characteristic.Clear();
            _reactivePowerL1Characteristic.Clear();
            _reactivePowerL2Characteristic.Clear();
            _reactivePowerL3Characteristic.Clear();
            _totalActivePowerCharacteristic.Clear();
            _totalReactivePowerCharacteristic.Clear();
        }
        public void SetStartTimeAll(DateTime time)
        {
            _currentL1Characteristic.SetStartTime(time);
            _currentL2Characteristic.SetStartTime(time);
            _currentL3Characteristic.SetStartTime(time);
            _voltageL1NCharacteristic.SetStartTime(time);
            _voltageL2NCharacteristic.SetStartTime(time);
            _voltageL3NCharacteristic.SetStartTime(time);
            _voltageL1L2Characteristic.SetStartTime(time);
            _voltageL2L3Characteristic.SetStartTime(time);
            _voltageL3L1Characteristic.SetStartTime(time);
            _activePowerL1Characteristic.SetStartTime(time);
            _activePowerL2Characteristic.SetStartTime(time);
            _activePowerL3Characteristic.SetStartTime(time);
            _reactivePowerL1Characteristic.SetStartTime(time);
            _reactivePowerL2Characteristic.SetStartTime(time);
            _reactivePowerL3Characteristic.SetStartTime(time);
            _totalActivePowerCharacteristic.SetStartTime(time);
            _totalReactivePowerCharacteristic.SetStartTime(time);
        }

        /// <summary>
        /// Method for converting data to value of variable
        /// </summary>
        /// <param name="Data">Data to convert</param
        /// <param name="Offset">Data offset</param>
        /// <returns>Variable value</returns>
        protected double ConvertDataToValue(Int32[] Data, Int32 offset)
        {
            var firstRegisterBytes = BitConverter.GetBytes(Data[offset]);
            var secondRegisterBytes = BitConverter.GetBytes(Data[offset + 1]);

            var allBytes = new Byte[] { secondRegisterBytes[0], secondRegisterBytes[1], firstRegisterBytes[0], firstRegisterBytes[1] };

            return Convert.ToDouble(BitConverter.ToSingle(allBytes, 0));
        }

        public CharacteristicRecorder()
        {
            this.Timer = new Timer();
            this.Timer.Elapsed += Timer_Elapsed;
            this.Timer.Interval = 100;
        }

        private ModbusClient _mbClient;

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Recording)
            {
                try
                {
                    _RefreshData();
                }
                catch (Exception exception)
                {
                    this.StopRecording();
                    MessageBox.Show(exception.Message, "Błąd podczas rejestracji", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
        }

        public void _RefreshData()
        {
            DateTime timestamp = DateTime.Now;
            int[] buffer = _mbClient.ReadHoldingRegisters(1, 72);
            var voltageL1N = ConvertDataToValue(buffer,0);
            var voltageL2N = ConvertDataToValue(buffer, 2);
            var voltageL3N = ConvertDataToValue(buffer, 4);
            var voltageL1L2 = ConvertDataToValue(buffer, 6);
            var voltageL2L3 = ConvertDataToValue(buffer, 8);
            var voltageL3L1 = ConvertDataToValue(buffer, 10);
            var currentL1 = ConvertDataToValue(buffer, 12);
            var currentL2 = ConvertDataToValue(buffer, 14);
            var currentL3 = ConvertDataToValue(buffer, 16);
            var activePowerL1 = ConvertDataToValue(buffer, 24);
            var activePowerL2 = ConvertDataToValue(buffer, 26);
            var activePowerL3 = ConvertDataToValue(buffer, 28);
            var reactivePowerL1 = ConvertDataToValue(buffer, 30);
            var reactivePowerL2 = ConvertDataToValue(buffer, 32);
            var reactivePowerL3 = ConvertDataToValue(buffer, 34);
            var totalActivePower = ConvertDataToValue(buffer, 64);
            var totalReactivePower = ConvertDataToValue(buffer, 66);

            _currentL1Characteristic.AddPoint(timestamp, currentL1);
            _currentL2Characteristic.AddPoint(timestamp, currentL2);
            _currentL3Characteristic.AddPoint(timestamp, currentL3);
            _voltageL1NCharacteristic.AddPoint(timestamp, voltageL1N);
            _voltageL2NCharacteristic.AddPoint(timestamp, voltageL2N);
            _voltageL3NCharacteristic.AddPoint(timestamp, voltageL3N);
            _voltageL1L2Characteristic.AddPoint(timestamp, voltageL1L2);
            _voltageL2L3Characteristic.AddPoint(timestamp, voltageL2L3);
            _voltageL3L1Characteristic.AddPoint(timestamp, voltageL3L1);
            _activePowerL1Characteristic.AddPoint(timestamp, activePowerL1);
            _activePowerL2Characteristic.AddPoint(timestamp, activePowerL2);
            _activePowerL3Characteristic.AddPoint(timestamp, activePowerL3);
            _reactivePowerL1Characteristic.AddPoint(timestamp, reactivePowerL1);
            _reactivePowerL2Characteristic.AddPoint(timestamp, reactivePowerL2);
            _reactivePowerL3Characteristic.AddPoint(timestamp, reactivePowerL3);
            _totalActivePowerCharacteristic.AddPoint(timestamp, totalActivePower);
            _totalReactivePowerCharacteristic.AddPoint(timestamp, totalReactivePower);
        }

        private Int32 _portNumber = 502;
        public Int32 PortNumber
        {
            get
            {
                return this._portNumber;
            }

            set
            {
                this._portNumber = value;
                OnPropertyChanged("PortNumber");
            }
        }

        private String _ipAddress = "192.168.0.10";
        public String IPAddress
        {
            get
            {
                return this._ipAddress;
            }

            set
            {
                this._ipAddress = value;
                OnPropertyChanged("IPAddress");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }

            private set
            {
                _startTime = value;
            }
        }

        private Timer _timer;
        public Timer Timer
        {
            get
            {
                return _timer;
            }

            private set
            {
                this._timer = value;
            }
        }

        public void StartRecording()
        {
            try
            {
                ReadyToSave = false;
                ClearAll();
                this.StartTime = DateTime.Now;
                _mbClient = new ModbusClient(IPAddress,PortNumber);
                _mbClient.Connect();
                DateTime startTime = DateTime.Now;
                SetStartTimeAll(startTime);
                this.Timer.Start();
                OnPropertyChanged("Recording");
                OnPropertyChanged("NotRecording");
            }
            catch (Exception exception)
            {
                this.StopRecording();
                MessageBox.Show(exception.Message, "Błąd podczas rejestracji", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        public void StopRecording()
        {
            this.Timer.Stop();
            ReadyToSave = true;
            OnPropertyChanged("Recording");
            OnPropertyChanged("NotRecording");
        }

        public Boolean Recording
        {
            get
            {
                return Timer.Enabled;
            }

        }

        public Boolean NotRecording
        {
            get
            {
                return !Timer.Enabled;
            }

        }

        private Boolean _readyToSave = false;
        public Boolean ReadyToSave
        {
            get
            {
                return this._readyToSave;
            }

            private set
            {
                this._readyToSave = value;
                OnPropertyChanged("ReadyToSave");
            }

        }


        public String GetCSVData()
        {
            StringBuilder content = new StringBuilder();

            content.AppendLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17}", "Czas [s]", "Prąd L1 [A]", "Prąd L2 [A]", "Prąd L3 [A]", "Napięcie L1-N [V]", "Napięcie L2-N [V]", "Napięcie L3-N [V]", "Napięcie L1-L2 [V]", "Napięcie L2-L3 [V]", "Napięcie L3-L1 [V]", "Moc czynna L1 [W]", "Moc czynna L2 [W]", "Moc czynna L3 [W]", "Moc bierna L1 [var]", "Moc bierna L2 [var]", "Moc bierna L3 [var]", "Moc czynna całkowita [W]", "Moc bierna całkowita [var]"));

            for (int i = 0; i < _currentL1Characteristic.Points.Count; i++)
            {
                content.AppendLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17}", _currentL1Characteristic.Points[i].X, _currentL1Characteristic.Points[i].Y, _currentL2Characteristic.Points[i].Y, _currentL3Characteristic.Points[i].Y, _voltageL1NCharacteristic.Points[i].Y, _voltageL2NCharacteristic.Points[i].Y, _voltageL3NCharacteristic.Points[i].Y, _voltageL1L2Characteristic.Points[i].Y, _voltageL2L3Characteristic.Points[i].Y, _voltageL3L1Characteristic.Points[i].Y, _activePowerL1Characteristic.Points[i].Y, _activePowerL2Characteristic.Points[i].Y, _activePowerL3Characteristic.Points[i].Y, _reactivePowerL1Characteristic.Points[i].Y, _reactivePowerL2Characteristic.Points[i].Y, _reactivePowerL3Characteristic.Points[i].Y, _totalActivePowerCharacteristic.Points[i].Y, _totalReactivePowerCharacteristic.Points[i].Y));
            }

            return content.ToString();
        }
    }
}
