namespace test_serial_port_WinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_connect = new Button();
            richTextBox1 = new RichTextBox();
            comboBox_port_number = new ComboBox();
            comboBox_baud_rate = new ComboBox();
            button_disconnect = new Button();
            button_led_on = new Button();
            button_led_off = new Button();
            label_port_number = new Label();
            label_baud_rate = new Label();
            SuspendLayout();
            // 
            // button_connect
            // 
            button_connect.Location = new Point(145, 68);
            button_connect.Name = "button_connect";
            button_connect.Size = new Size(75, 23);
            button_connect.TabIndex = 0;
            button_connect.Text = "Connect";
            button_connect.UseVisualStyleBackColor = true;
            button_connect.Click += button_connect_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(70, 144);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(671, 277);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // comboBox_port_number
            // 
            comboBox_port_number.FormattingEnabled = true;
            comboBox_port_number.Location = new Point(145, 27);
            comboBox_port_number.Name = "comboBox_port_number";
            comboBox_port_number.Size = new Size(121, 23);
            comboBox_port_number.TabIndex = 2;
            comboBox_port_number.DropDown += comboBox_port_number_DropDown;
            // 
            // comboBox_baud_rate
            // 
            comboBox_baud_rate.FormattingEnabled = true;
            comboBox_baud_rate.Location = new Point(401, 28);
            comboBox_baud_rate.Name = "comboBox_baud_rate";
            comboBox_baud_rate.Size = new Size(121, 23);
            comboBox_baud_rate.TabIndex = 3;
            comboBox_baud_rate.Text = "115200";
            // 
            // button_disconnect
            // 
            button_disconnect.Location = new Point(282, 68);
            button_disconnect.Name = "button_disconnect";
            button_disconnect.Size = new Size(75, 23);
            button_disconnect.TabIndex = 4;
            button_disconnect.Text = "Disconnect";
            button_disconnect.UseVisualStyleBackColor = true;
            button_disconnect.Click += button_disconnect_Click;
            // 
            // button_led_on
            // 
            button_led_on.Location = new Point(145, 106);
            button_led_on.Name = "button_led_on";
            button_led_on.Size = new Size(75, 23);
            button_led_on.TabIndex = 5;
            button_led_on.Text = "LED ON";
            button_led_on.UseVisualStyleBackColor = true;
            button_led_on.Click += button_led_on_Click;
            // 
            // button_led_off
            // 
            button_led_off.Location = new Point(282, 106);
            button_led_off.Name = "button_led_off";
            button_led_off.Size = new Size(75, 23);
            button_led_off.TabIndex = 6;
            button_led_off.Text = "LED OFF";
            button_led_off.UseVisualStyleBackColor = true;
            button_led_off.Click += button_led_off_Click;
            // 
            // label_port_number
            // 
            label_port_number.AutoSize = true;
            label_port_number.Location = new Point(66, 31);
            label_port_number.Name = "label_port_number";
            label_port_number.Size = new Size(73, 15);
            label_port_number.TabIndex = 7;
            label_port_number.Text = "Port number";
            // 
            // label_baud_rate
            // 
            label_baud_rate.AutoSize = true;
            label_baud_rate.Location = new Point(338, 31);
            label_baud_rate.Name = "label_baud_rate";
            label_baud_rate.Size = new Size(57, 15);
            label_baud_rate.TabIndex = 8;
            label_baud_rate.Text = "Baud rate";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label_baud_rate);
            Controls.Add(label_port_number);
            Controls.Add(button_led_off);
            Controls.Add(button_led_on);
            Controls.Add(button_disconnect);
            Controls.Add(comboBox_baud_rate);
            Controls.Add(comboBox_port_number);
            Controls.Add(richTextBox1);
            Controls.Add(button_connect);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_connect;
        private RichTextBox richTextBox1;
        private ComboBox comboBox_port_number;
        private ComboBox comboBox_baud_rate;
        private Button button_disconnect;
        private Button button_led_on;
        private Button button_led_off;
        private Label label_port_number;
        private Label label_baud_rate;
    }
}
