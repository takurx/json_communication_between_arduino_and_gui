using System;
using System.IO.Ports;
using System.Diagnostics;
using System.Windows.Forms; // WinFormsの場合
using System.Text;
using System.Drawing.Text; // ★この行を追加★

namespace test_serial_port_WinFormsApp
{
    public partial class Form1 : Form
    {
        private SerialPort _serial_Port = null!; // Null許容の警告を抑制

        private void _serial_Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Debug.WriteLine("DataReceivedイベント発生");

            // 受信データの取得
            //string data = _serial_Port.ReadExisting();

            // 受信バッファのサイズを取得
            int bytesToRead = _serial_Port.BytesToRead;
            byte[] buffer = new byte[bytesToRead];

            // バイト配列としてデータを読み込む
            _serial_Port.Read(buffer, 0, bytesToRead);

            // デバッグ表示用にバイトデータを16進数文字列などに変換
            string receivedHex = BitConverter.ToString(buffer).Replace("-", " ");
            string receivedString = Encoding.ASCII.GetString(buffer); // ASCII文字列の場合の例

            // UIスレッドで表示（例: TextBoxに表示）
            Invoke(new Action(() =>
            {
                //MessageBox.Show($"受信: {data}");
                //MessageBox.Show($"受信データ(文字列変換): {receivedString}");
                Debug.Write(receivedString);
                richTextBox1.AppendText(receivedString);
            }));
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void connect_serial_port(string _port_Name, int _baud_rate)
        {
            _serial_Port = new SerialPort
            {
                PortName = _port_Name, //comboBox_port_number.Text, // 選択されたポート名を使用
                BaudRate = _baud_rate, //int.Parse(comboBox_baud_rate.Text), // 選択されたボーレートを使用
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                RtsEnable = false,
                DtrEnable = true,
                WriteTimeout = 500,
                ReadTimeout = 500,
                NewLine = "\r\n" // 改行コードの設定（必要に応じて変更）, \r, \n, \r\n など
            };
            _serial_Port.ReceivedBytesThreshold = 1; // 1バイト受信ごとにイベント発生
            _serial_Port.DataReceived += _serial_Port_DataReceived;

            try
            {
                _serial_Port.Open();
                _serial_Port.DiscardInBuffer();
                _serial_Port.DiscardOutBuffer();
                Debug.WriteLine("serial port open");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening serial port: {ex.Message}");
                MessageBox.Show($"Error opening serial port: {ex.Message}");
            }
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            connect_serial_port(comboBox_port_number.Text, int.Parse(comboBox_baud_rate.Text));
        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (_serial_Port != null && _serial_Port.IsOpen)
                {
                    _serial_Port.DataReceived -= _serial_Port_DataReceived; // イベントハンドラーの解除
                    _serial_Port.Close();
                    Debug.WriteLine("serial port closed");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error closing serial port: {ex.Message}");
            }
        }

        private void button_led_on_Click(object sender, EventArgs e)
        {
            // LED ON command
            // {"cmd": "power", "value": 1}
            // LED OFF command
            // {"cmd": "power", "value": 0}
            // データ送信例
            _serial_Port.WriteLine("{\"cmd\": \"power\", \"value\": 1}");
        }

        private void button_led_off_Click(object sender, EventArgs e)
        {
            // LED ON command
            // {"cmd": "power", "value": 1}
            // LED OFF command
            // {"cmd": "power", "value": 0}
            // データ送信例
            _serial_Port.WriteLine("{\"cmd\": \"power\", \"value\": 0}");
        }

        private void comboBox_port_number_DropDown(object sender, EventArgs e)
        {

            comboBox_port_number.Items.Clear();
            string[] _port_Names = SerialPort.GetPortNames();
            foreach (string _port_Name in _port_Names) {
                comboBox_port_number.Items.Add(_port_Name);
            }
        }
    }
}
