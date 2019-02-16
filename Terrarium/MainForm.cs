﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Management;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;


namespace Terrarium
{

    public partial class MainForm : Form
    {
        private int panelSettingsWidth;
        private bool panelSettingsHiden;

        private bool panelMacroHiden;
        private MacroPanelWizard macroWizard = new MacroPanelWizard();
        private Properties.Settings ps = Properties.Settings.Default;
        private string[] serialPortList;
        private string com_portName;
        private int com_baudRate;
        private int com_baudRateCustome;
        private int com_dataBits;
        private Parity com_parity;
        private StopBits com_stopBits;
        private Handshake com_handshake;
        private SerialClient serClient;
        private int RxDataCounter = 0;
        private int TxDataCounter = 0;
        private bool IsOpenBtnClicked = false;

        public SerialClient SerClient { get => serClient; set => serClient = value; }

        public MainForm()
        {
            InitializeComponent();

            panelSettingsWidth = pnl_Settings.Width;
            SettingsGet();

            rb_baudRate_4800.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_9600.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_14400.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_19200.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_28800.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_38400.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_56000.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_57600.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_115200.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_128000.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_256000.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_460800.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);
            rb_baudRate_custome.CheckedChanged += new EventHandler(rb_baudRate_CheckedChanged);

            rb_dataBits_5.CheckedChanged += new EventHandler(rb_dataBits_CheckedChanged);
            rb_dataBits_6.CheckedChanged += new EventHandler(rb_dataBits_CheckedChanged);
            rb_dataBits_7.CheckedChanged += new EventHandler(rb_dataBits_CheckedChanged);
            rb_dataBits_8.CheckedChanged += new EventHandler(rb_dataBits_CheckedChanged);

            rb_parity_none.CheckedChanged += new EventHandler(rb_parity_CheckedChanged);
            rb_parity_odd.CheckedChanged += new EventHandler(rb_parity_CheckedChanged);
            rb_parity_even.CheckedChanged += new EventHandler(rb_parity_CheckedChanged);
            rb_parity_mark.CheckedChanged += new EventHandler(rb_parity_CheckedChanged);
            rb_parity_space.CheckedChanged += new EventHandler(rb_parity_CheckedChanged);

            rb_stopBits_1.CheckedChanged += new EventHandler(rb_stopBits_CheckedChanged);
            rb_stopBits_1_5.CheckedChanged += new EventHandler(rb_stopBits_CheckedChanged);
            rb_stopBits_2.CheckedChanged += new EventHandler(rb_stopBits_CheckedChanged);

            rb_handshake_none.CheckedChanged += new EventHandler(rb_handshake_CheckedChanged);
            rb_handshake_rts.CheckedChanged += new EventHandler(rb_handshake_CheckedChanged);
            rb_handshake_xon.CheckedChanged += new EventHandler(rb_handshake_CheckedChanged);
            rb_handshake_rts_xon.CheckedChanged += new EventHandler(rb_handshake_CheckedChanged);

            macroPannel.BtnSendClick += new EventHandler(btn_SerialSend_Click);
            macroPannel.ButtonSendEvent += new EventHandler(btn_SerialSend_Click); // need to catch presed Enter key 
            macroPannel.BtnMacroSettingsClick += new EventHandler(btn_MacroPanelWizard_Click);

            macroPannel.BtnM1Click += new EventHandler(btn_m1_Click);
            macroPannel.BtnM2Click += new EventHandler(btn_m2_Click);
            macroPannel.BtnM3Click += new EventHandler(btn_m3_Click);
            macroPannel.BtnM4Click += new EventHandler(btn_m4_Click);
            macroPannel.BtnM5Click += new EventHandler(btn_m5_Click);
            macroPannel.BtnM6Click += new EventHandler(btn_m6_Click);
            macroPannel.BtnM7Click += new EventHandler(btn_m7_Click);
            macroPannel.BtnM8Click += new EventHandler(btn_m8_Click);
            macroPannel.BtnM9Click += new EventHandler(btn_m9_Click);
            macroPannel.BtnM10Click += new EventHandler(btn_m10_Click);
            macroPannel.BtnM11Click += new EventHandler(btn_m11_Click);
            macroPannel.BtnM12Click += new EventHandler(btn_m12_Click);
            macroPannel.BtnM13Click += new EventHandler(btn_m13_Click);
            macroPannel.BtnM14Click += new EventHandler(btn_m14_Click);
            macroPannel.BtnM15Click += new EventHandler(btn_m15_Click);
            macroPannel.BtnM16Click += new EventHandler(btn_m16_Click);
            macroPannel.BtnM17Click += new EventHandler(btn_m17_Click);
            macroPannel.BtnM18Click += new EventHandler(btn_m18_Click);
            macroPannel.BtnM19Click += new EventHandler(btn_m19_Click);
            macroPannel.BtnM20Click += new EventHandler(btn_m20_Click);

            macroWizard.BtnM1TextChange += new EventHandler(btn_m1_TextChange);
            macroWizard.BtnM2TextChange += new EventHandler(btn_m2_TextChange);
            macroWizard.BtnM3TextChange += new EventHandler(btn_m3_TextChange);
            macroWizard.BtnM4TextChange += new EventHandler(btn_m4_TextChange);
            macroWizard.BtnM5TextChange += new EventHandler(btn_m5_TextChange);
            macroWizard.BtnM6TextChange += new EventHandler(btn_m6_TextChange);
            macroWizard.BtnM7TextChange += new EventHandler(btn_m7_TextChange);
            macroWizard.BtnM8TextChange += new EventHandler(btn_m8_TextChange);
            macroWizard.BtnM9TextChange += new EventHandler(btn_m9_TextChange);
            macroWizard.BtnM10TextChange += new EventHandler(btn_m10_TextChange);
            macroWizard.BtnM11TextChange += new EventHandler(btn_m11_TextChange);
            macroWizard.BtnM12TextChange += new EventHandler(btn_m12_TextChange);
            macroWizard.BtnM13TextChange += new EventHandler(btn_m13_TextChange);
            macroWizard.BtnM14TextChange += new EventHandler(btn_m14_TextChange);
            macroWizard.BtnM15TextChange += new EventHandler(btn_m15_TextChange);
            macroWizard.BtnM16TextChange += new EventHandler(btn_m16_TextChange);
            macroWizard.BtnM17TextChange += new EventHandler(btn_m17_TextChange);
            macroWizard.BtnM18TextChange += new EventHandler(btn_m18_TextChange);
            macroWizard.BtnM19TextChange += new EventHandler(btn_m19_TextChange);
            macroWizard.BtnM20TextChange += new EventHandler(btn_m20_TextChange);

            macroWizard.BtnM1Click += new EventHandler(btn_m1_Click);
            macroWizard.BtnM2Click += new EventHandler(btn_m2_Click);
            macroWizard.BtnM3Click += new EventHandler(btn_m3_Click);
            macroWizard.BtnM4Click += new EventHandler(btn_m4_Click);
            macroWizard.BtnM5Click += new EventHandler(btn_m5_Click);
            macroWizard.BtnM6Click += new EventHandler(btn_m6_Click);
            macroWizard.BtnM7Click += new EventHandler(btn_m7_Click);
            macroWizard.BtnM8Click += new EventHandler(btn_m8_Click);
            macroWizard.BtnM9Click += new EventHandler(btn_m9_Click);
            macroWizard.BtnM10Click += new EventHandler(btn_m10_Click);
            macroWizard.BtnM11Click += new EventHandler(btn_m11_Click);
            macroWizard.BtnM12Click += new EventHandler(btn_m12_Click);
            macroWizard.BtnM13Click += new EventHandler(btn_m13_Click);
            macroWizard.BtnM14Click += new EventHandler(btn_m14_Click);
            macroWizard.BtnM15Click += new EventHandler(btn_m15_Click);
            macroWizard.BtnM16Click += new EventHandler(btn_m16_Click);
            macroWizard.BtnM17Click += new EventHandler(btn_m17_Click);
            macroWizard.BtnM18Click += new EventHandler(btn_m18_Click);
            macroWizard.BtnM19Click += new EventHandler(btn_m19_Click);
            macroWizard.BtnM20Click += new EventHandler(btn_m20_Click);




        }




        private void rb_baudRate_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    if (rb_baudRate_4800.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_4800.Text);
                    if (rb_baudRate_9600.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_9600.Text);
                    if (rb_baudRate_14400.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_14400.Text);
                    if (rb_baudRate_19200.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_19200.Text);
                    if (rb_baudRate_28800.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_28800.Text);
                    if (rb_baudRate_38400.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_38400.Text);
                    if (rb_baudRate_56000.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_56000.Text);
                    if (rb_baudRate_57600.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_57600.Text);
                    if (rb_baudRate_115200.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_115200.Text);
                    if (rb_baudRate_128000.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_128000.Text);
                    if (rb_baudRate_256000.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_256000.Text);
                    if (rb_baudRate_460800.Checked) com_baudRate = Convert.ToInt32(rb_baudRate_460800.Text);
                    if (rb_baudRate_custome.Checked) com_baudRate = Convert.ToInt32(tb_baudRateCustome.Text);

                    SerClient?.SetBaudrate(com_baudRate);
                }
            }
        }

        private void rb_dataBits_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    if (rb_dataBits_5.Checked) com_dataBits = Convert.ToInt32(rb_dataBits_5.Text);
                    if (rb_dataBits_6.Checked) com_dataBits = Convert.ToInt32(rb_dataBits_6.Text);
                    if (rb_dataBits_7.Checked) com_dataBits = Convert.ToInt32(rb_dataBits_7.Text);
                    if (rb_dataBits_8.Checked) com_dataBits = Convert.ToInt32(rb_dataBits_8.Text);

                    SerClient?.SetDataBits(com_dataBits);
                }
            }
        }

        private void rb_parity_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    if (rb_parity_none.Checked) com_parity = Parity.None;
                    if (rb_parity_odd.Checked) com_parity = Parity.Odd;
                    if (rb_parity_even.Checked) com_parity = Parity.Even;
                    if (rb_parity_mark.Checked) com_parity = Parity.Mark;
                    if (rb_parity_space.Checked) com_parity = Parity.Space;

                    SerClient?.SetParity(com_parity);
                }
            }
        }

        private void rb_stopBits_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    if (rb_stopBits_1.Checked) com_stopBits = StopBits.One;
                    if (rb_stopBits_1_5.Checked) com_stopBits = StopBits.OnePointFive;
                    if (rb_stopBits_2.Checked) com_stopBits = StopBits.Two;

                    SerClient?.SetStopBits(com_stopBits);
                }
            }
        }

        private void rb_handshake_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    if (rb_handshake_none.Checked) com_handshake = Handshake.None;
                    if (rb_handshake_rts.Checked) com_handshake = Handshake.RequestToSend;
                    if (rb_handshake_xon.Checked) com_handshake = Handshake.XOnXOff;
                    if (rb_handshake_rts_xon.Checked) com_handshake = Handshake.RequestToSendXOnXOff;
                    if (SerClient != null) SerClient.SetHandshake(com_handshake);
                }
            }
        }

        protected void SerialPortError(object sender, EventArgs e)
        {
            SerClient.Close();

            SetTxtToStatusLable("PORT ERROR", Color.Red);
            this.Invoke(new Action(() =>
            {
                btn_SerialConnect.Image = Terrarium.Properties.Resources.icons8_Disconnected_32px;
                cmb_SerialPortList.Enabled = true;
                btn_SerialConnect.Enabled = true;
                IsOpenBtnClicked = false;

            }));


            //cmb_SerialPortList.Enabled = true;
            //btn_SerialConnect.Enabled = true;
            IsOpenBtnClicked = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FillControlValues();

            int portCount = SerialPortScan();
            if (portCount == 0) SetTxtToStatusLable("NO PORTS FOUND", Color.WhiteSmoke);
            else SetTxtToStatusLable("FOUND PORTS " + portCount, Color.WhiteSmoke);

            if (com_portName == null) com_portName = "COM1";

            SerClient = new SerialClient(com_portName, com_baudRate, com_dataBits, com_parity, com_stopBits, com_handshake);
            SerClient.OnReceiving += new EventHandler<DataStreamEventArgs>(ReceiveHandler);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SettingsSave();
            SerClient.Close();

            SerClient.OnReceiving -= new EventHandler<DataStreamEventArgs>(ReceiveHandler);
            SerClient.serialError -= new EventHandler(SerialPortError);

            SerClient.Dispose();
        }

        private void ReceiveHandler(object sender, DataStreamEventArgs e)
        {
            SetText(e.Response);
        }

        delegate void SetTextCallback(byte[] data);

        private void SetText(byte[] data)
        {
            if (nrtb_Rx.RichTextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { data });
            }
            else
            {
                if (cb_Rx_Hex.Checked)
                {
                    if (cb_Sort.Checked == true)
                    {
                        nrtb_Rx.RichTextBox.Text += "\n";

                        nrtb_Rx.AppendHex(data);
                    }
                    else
                    {
                        nrtb_Rx.AppendHex(data);
                    }
                }
                else
                {
                    if (cb_Sort.Checked == true)
                    {
                        nrtb_Rx.AppendText(Encoding.ASCII.GetString(data));
                    }
                    else
                    {
                        nrtb_Rx.AppendText(Encoding.ASCII.GetString(data));
                    }
                }

                nrtb_Rx.NumStripAutoscroll = cb_RxAutoscroll.Checked ? true : false;
                RxDataCounter += data.Length;
                lbl_RxCounter.Text = "Rx: " + RxDataCounter.ToString();
            }
        }

        private void FillControlValues()
        {
            tb_baudRateCustome.Text = ps.SerialBaudCustome.ToString();

            switch (ps.SerialPortBoude)
            {
                case 0:                                     //custome value
                    rb_baudRate_custome.Checked = true;
                    break;
                case 4800:
                    rb_baudRate_4800.Checked = true;
                    break;
                case 9600:
                    rb_baudRate_9600.Checked = true;
                    break;
                case 14400:
                    rb_baudRate_14400.Checked = true;
                    break;
                case 19200:
                    rb_baudRate_19200.Checked = true;
                    break;
                case 28800:
                    rb_baudRate_28800.Checked = true;
                    break;
                case 38400:
                    rb_baudRate_38400.Checked = true;
                    break;
                case 56000:
                    rb_baudRate_56000.Checked = true;
                    break;
                case 57600:
                    rb_baudRate_57600.Checked = true;
                    break;
                case 115200:
                    rb_baudRate_115200.Checked = true;
                    break;
                case 128000:
                    rb_baudRate_128000.Checked = true;
                    break;
                case 256000:
                    rb_baudRate_256000.Checked = true;
                    break;
                case 460800:
                    rb_baudRate_460800.Checked = true;
                    break;
                default:
                    break;
            }

            switch (ps.SerialDataBits)
            {
                case 5:
                    rb_dataBits_5.Checked = true;
                    break;
                case 6:
                    rb_dataBits_6.Checked = true;
                    break;
                case 7:
                    rb_dataBits_7.Checked = true;
                    break;
                case 8:
                    rb_dataBits_8.Checked = true;
                    break;
                default:
                    break;
            }

            switch (ps.SerialPortParity)
            {
                case "None":
                    rb_parity_none.Checked = true;
                    break;
                case "Odd":
                    rb_parity_odd.Checked = true;
                    break;
                case "Even":
                    rb_parity_even.Checked = true;
                    break;
                case "Mark":
                    rb_parity_mark.Checked = true;
                    break;
                case "Space":
                    rb_parity_space.Checked = true;
                    break;
                default:
                    break;
            }

            switch (ps.SerialStopBits)
            {
                case "One":
                    rb_stopBits_1.Checked = true;
                    break;
                case "Two":
                    rb_stopBits_2.Checked = true;
                    break;
                case "OnePointFive":
                    rb_stopBits_1_5.Checked = true;
                    break;
                default:
                    break;
            }

            switch (ps.SerialHandshake)
            {
                case "None":
                    rb_handshake_none.Checked = true;
                    break;
                case "XOnXOff":
                    rb_handshake_xon.Checked = true;
                    break;
                case "RequestToSend":
                    rb_handshake_rts.Checked = true;
                    break;
                case "RequestToSendXOnXOff":
                    rb_handshake_rts_xon.Checked = true;
                    break;
                default:
                    break;
            }

            if (ps.sidePannelHide == true) pnl_Settings.Width = 0;

            cb_RxAutoscroll.Checked = ps.rtb_Rx_AutoScroll;
            cb_Rx_Hex.Checked = ps.cb_Rx_Hex;
            cb_Sort.Checked = ps.cb_Sort;
            nmn_ByteSort.Value = ps.nmn_ByteSort;
        }

        private void SettingsSave()
        {
            ps.SerialPortName = com_portName;
            ps.SerialPortBoude = com_baudRate;
            ps.SerialBaudCustome = com_baudRateCustome;
            ps.SerialDataBits = com_dataBits;
            ps.SerialStopBits = Convert.ToString(com_stopBits);
            ps.SerialHandshake = Convert.ToString(com_handshake);
            ps.SerialPortParity = Convert.ToString(com_parity);
            ps.sidePannelHide = panelSettingsHiden;
            ps.panelMacroStatus = panelMacroHiden;
            ps.rtb_Rx_AutoScroll = cb_RxAutoscroll.Checked;
            ps.cb_Rx_Hex = cb_Rx_Hex.Checked;
            ps.cb_Sort = cb_Sort.Checked;
            ps.nmn_ByteSort = nmn_ByteSort.Value;
            ps.Save();
        }

        private void SettingsGet()
        {
            com_portName = ps.SerialPortName;
            com_baudRate = ps.SerialPortBoude;
            com_dataBits = ps.SerialDataBits;
            com_baudRateCustome = ps.SerialBaudCustome;
            com_stopBits = (StopBits)Enum.Parse(typeof(StopBits), ps.SerialStopBits);
            com_handshake = (Handshake)Enum.Parse(typeof(Handshake), ps.SerialHandshake);
            com_parity = (Parity)Enum.Parse(typeof(Parity), ps.SerialPortParity);
            panelSettingsHiden = ps.sidePannelHide;
            panelMacroHiden = ps.panelMacroStatus;
        }

        private void tb_baudRateCustome_TextChanged(object sender, EventArgs e)   //prevent from entering chars instead numbers
        {
            if (TextHelper.IsNumberEntered(tb_baudRateCustome.Text))
            {
                MessageBox.Show("Please enter only numbers.");
                tb_baudRateCustome.Text = tb_baudRateCustome.Text.Remove(tb_baudRateCustome.Text.Length - 1);
            }
            else
            {
                com_baudRateCustome = Convert.ToInt32(tb_baudRateCustome.Text);
            }
        }

        private void btn_Settings_Click(object sender, EventArgs e) => tmr_MenuSlide.Start();

        private void tmr_MenuSlide_Tick(object sender, EventArgs e)
        {
            if (panelSettingsHiden == true)
            {
                pnl_Settings.Width += 50;
                if (pnl_Settings.Width >= panelSettingsWidth)
                {
                    pnl_Settings.Width = panelSettingsWidth;
                    tmr_MenuSlide.Stop();
                    panelSettingsHiden = false;
                    this.Refresh();
                }
            }
            else
            {
                pnl_Settings.Width -= 50;
                if (pnl_Settings.Width <= 0)
                {
                    pnl_Settings.Width = 0;
                    tmr_MenuSlide.Stop();
                    panelSettingsHiden = true;
                    this.Refresh();
                }
            }
        }

        private void btn_SerialConnect_Click(object sender, EventArgs e)
        {
            if (serialPortList.Length == 0)
            {
                SetTxtToStatusLable("CHOSE PORT FIRST", Color.Red);
                return;
            }

            if (IsOpenBtnClicked == false)
            {
                SerClient.SetPortName(com_portName);
                SerClient.SetBaudrate(com_baudRate);
                SerClient.SetParity(com_parity);
                SerClient.SetDataBits(com_dataBits);
                SerClient.SetStopBits(com_stopBits);
                SerClient.SetHandshake(com_handshake);

                SerClient = new SerialClient(com_portName, com_baudRate, com_dataBits, com_parity, com_stopBits, com_handshake);
                SerClient.OnReceiving += new EventHandler<DataStreamEventArgs>(ReceiveHandler);
                SerClient.serialError += new EventHandler(SerialPortError);

                if (SerClient.Open() == true)
                {
                    SetTxtToStatusLable("SERIAL OPENED", Color.Aqua);
                    btn_SerialConnect.Image = Terrarium.Properties.Resources.icons8_Connected_32px;
                    this.Text = "Terrarium " + (string)cmb_SerialPortList.SelectedItem;

                    cmb_SerialPortList.Enabled = false;
                    btn_SerialConnect.Enabled = true;
                }
                else
                {
                    SerClient.Close();

                    SetTxtToStatusLable("SERIAL ERROR", Color.Red);
                    cmb_SerialPortList.Enabled = true;
                    btn_SerialConnect.Enabled = true;
                    IsOpenBtnClicked = false;
                    return;
                }
            }
            else
            {
                SerClient.Close();
                SetTxtToStatusLable("SERIAL CLOSED", Color.Aqua);
                btn_SerialConnect.Image = Terrarium.Properties.Resources.icons8_Disconnected_32px;
                this.Text = "Terrarium ";
                cmb_SerialPortList.Enabled = true;
                btn_SerialConnect.Enabled = true;
            }
            IsOpenBtnClicked ^= true;
        }

        private void btn_CleanTxField_Click(object sender, EventArgs e)
        {
            rtb_Tx.Clear();
            macroPannel.tb_Tx.Clear();
        }

        private void btn_CleanRxField_Click(object sender, EventArgs e) => nrtb_Rx.RichTextBox.Clear();

        private int SerialPortScan()
        {
            serialPortList = SerialPort.GetPortNames();
            Array.Sort(serialPortList, (x, y) => x.CompareTo(y));

            string tmpPortName = com_portName;

            if (serialPortList.Contains(tmpPortName))
            {
                cmb_SerialPortList.Items.Clear();
                foreach (string item in serialPortList)
                {
                    cmb_SerialPortList.Items.Add(item);
                    cmb_SerialPortList.SelectedIndex = cmb_SerialPortList.Items.IndexOf(tmpPortName);
                }
            }
            else
            {
                cmb_SerialPortList.Items.Clear();
                foreach (string item in serialPortList)
                {
                    cmb_SerialPortList.Items.Add(item);
                    cmb_SerialPortList.SelectedIndex = 0;
                }
            }
            com_portName = (string)cmb_SerialPortList.SelectedItem;
            return serialPortList.Length;
        }

        private void btn_SerialPortRefresh_Click(object sender, EventArgs e)
        {
            if (SerClient.IsOpen() == false)
            {
                int portCount = SerialPortScan();
                if (portCount == 0) SetTxtToStatusLable("NO PORTS FOUND", Color.WhiteSmoke);
                else SetTxtToStatusLable("FOUND PORTS " + portCount, Color.WhiteSmoke);
            }
        }

        delegate void SetTextStatusLableCallback(string text, Color color);

        public void SetTxtToStatusLable(string data, Color color)
        {
            if (lbl_Status.InvokeRequired)
            {
                SetTextStatusLableCallback d = new SetTextStatusLableCallback(SetTxtToStatusLable);
                this.Invoke(d, new object[] { data, color });
            }
            else
            {
                lbl_Status.ForeColor = color;
                lbl_Status.Text = data;
            }
        }

        private void cmb_SerialPortList_SelectedValueChanged(object sender, EventArgs e)
        {
            com_portName = (string)cmb_SerialPortList.SelectedItem;
        }

        private void rtb_Tx_KeyPress(object sender, KeyPressEventArgs e)  //used for greping chars from rtb_Tx
        {
            char c = e.KeyChar;

            byte[] buff = new byte[1];
            buff[0] = Convert.ToByte(c);
            if (SerClient != null && SerClient.IsOpen() == true)
            {
                SerClient.Transmit(buff);
                TxDataCounter += buff.Length;
                lbl_TxCounter.Text = "Tx: " + TxDataCounter.ToString();
            }
        }

        private void btn_SerialSend_Click(object sender, EventArgs e) => SerialSendHelper(macroPannel.tb_Tx.Text, cb_Tx_Hex.Checked);



        public void SerialSendHelper(string tbData, bool cbHex)
        {
            if (SerClient.IsOpen() == true)
            {
                byte[] buff;
                if (cbHex == true)
                {
                    if (TextHelper.IsHexEntered(tbData))
                    {
                        MessageBox.Show("Please enter only numbers in HEX format XX");
                        return;
                    }
                    buff = TextHelper.StringToHex(tbData);
                }
                else
                {
                    buff = Encoding.UTF8.GetBytes(tbData);
                }
                SerClient.Transmit(buff);
                TxDataCounter += buff.Length;
                lbl_TxCounter.Text = "Tx: " + TxDataCounter.ToString();
            }
            else
            {
                SetTxtToStatusLable("OPEN PORT FIRST", Color.Red);
            }
        }


        private void rtb_Rx_TextChanged(object sender, EventArgs e)
        {
            //rtb_Rx.SelectionStart = rtb_Rx.Text.Length;
            //rtb_Rx.ScrollToCaret();
        }

        private void lbl_RxCounter_DoubleClick(object sender, EventArgs e)
        {
            RxDataCounter = 0;
            lbl_RxCounter.Text = "Rx: " + RxDataCounter;
        }

        private void lbl_TxCounter_DoubleClick(object sender, EventArgs e)
        {
            TxDataCounter = 0;
            lbl_TxCounter.Text = "Tx: " + TxDataCounter;
        }

        private void cb_LinesNum_CheckedChanged(object sender, EventArgs e)
        {
            nrtb_Rx.NumStripVisible ^= true;
        }

        private void cb_Sort_CheckedChanged(object sender, EventArgs e)
        {
            nmn_ByteSort.Enabled ^= true;
        }

        private void cb_TxMacroSend_CheckedChanged(object sender, EventArgs e)
        {
            macroPannel.VisibleMacroButtons ^= true;
            if (macroPannel.VisibleMacroButtons == true)
            {
                tableLayoutPanel1.RowStyles[2].SizeType = SizeType.Absolute;
                tableLayoutPanel1.RowStyles[2].Height = 25F;
            }
            else
            {
                tableLayoutPanel1.RowStyles[2].SizeType = SizeType.Absolute;
                tableLayoutPanel1.RowStyles[2].Height = 100F;
            }
        }

        private void btn_MacroPanelWizard_Click(object sender, EventArgs e) => macroWizard.Show();


        private void btn_m1_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP1_Text, macroWizard.MP1_HexMode);
        private void btn_m2_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP2_Text, macroWizard.MP2_HexMode);
        private void btn_m3_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP3_Text, macroWizard.MP3_HexMode);
        private void btn_m4_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP4_Text, macroWizard.MP4_HexMode);
        private void btn_m5_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP5_Text, macroWizard.MP5_HexMode);
        private void btn_m6_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP6_Text, macroWizard.MP6_HexMode);
        private void btn_m7_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP7_Text, macroWizard.MP7_HexMode);
        private void btn_m8_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP8_Text, macroWizard.MP8_HexMode);
        private void btn_m9_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP9_Text, macroWizard.MP9_HexMode);
        private void btn_m10_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP10_Text, macroWizard.MP10_HexMode);
        private void btn_m11_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP11_Text, macroWizard.MP11_HexMode);
        private void btn_m12_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP12_Text, macroWizard.MP12_HexMode);
        private void btn_m13_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP13_Text, macroWizard.MP13_HexMode);
        private void btn_m14_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP14_Text, macroWizard.MP14_HexMode);
        private void btn_m15_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP15_Text, macroWizard.MP15_HexMode);
        private void btn_m16_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP16_Text, macroWizard.MP16_HexMode);
        private void btn_m17_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP17_Text, macroWizard.MP17_HexMode);
        private void btn_m18_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP18_Text, macroWizard.MP18_HexMode);
        private void btn_m19_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP19_Text, macroWizard.MP19_HexMode);
        private void btn_m20_Click(object sender, EventArgs e) => SerialSendHelper(macroWizard.MP20_Text, macroWizard.MP20_HexMode);


        private void btn_m1_TextChange(object sender, EventArgs e) => macroPannel.btn_m1.Text = macroWizard.MP1_ButtonText;
        private void btn_m2_TextChange(object sender, EventArgs e) => macroPannel.btn_m2.Text = macroWizard.MP2_ButtonText;
        private void btn_m3_TextChange(object sender, EventArgs e) => macroPannel.btn_m3.Text = macroWizard.MP3_ButtonText;
        private void btn_m4_TextChange(object sender, EventArgs e) => macroPannel.btn_m4.Text = macroWizard.MP4_ButtonText;
        private void btn_m5_TextChange(object sender, EventArgs e) => macroPannel.btn_m5.Text = macroWizard.MP5_ButtonText;
        private void btn_m6_TextChange(object sender, EventArgs e) => macroPannel.btn_m6.Text = macroWizard.MP6_ButtonText;
        private void btn_m7_TextChange(object sender, EventArgs e) => macroPannel.btn_m7.Text = macroWizard.MP7_ButtonText;
        private void btn_m8_TextChange(object sender, EventArgs e) => macroPannel.btn_m8.Text = macroWizard.MP8_ButtonText;
        private void btn_m9_TextChange(object sender, EventArgs e) => macroPannel.btn_m9.Text = macroWizard.MP9_ButtonText;
        private void btn_m10_TextChange(object sender, EventArgs e) => macroPannel.btn_m10.Text = macroWizard.MP10_ButtonText;
        private void btn_m11_TextChange(object sender, EventArgs e) => macroPannel.btn_m11.Text = macroWizard.MP11_ButtonText;
        private void btn_m12_TextChange(object sender, EventArgs e) => macroPannel.btn_m12.Text = macroWizard.MP12_ButtonText;
        private void btn_m13_TextChange(object sender, EventArgs e) => macroPannel.btn_m13.Text = macroWizard.MP13_ButtonText;
        private void btn_m14_TextChange(object sender, EventArgs e) => macroPannel.btn_m14.Text = macroWizard.MP14_ButtonText;
        private void btn_m15_TextChange(object sender, EventArgs e) => macroPannel.btn_m15.Text = macroWizard.MP15_ButtonText;
        private void btn_m16_TextChange(object sender, EventArgs e) => macroPannel.btn_m16.Text = macroWizard.MP16_ButtonText;
        private void btn_m17_TextChange(object sender, EventArgs e) => macroPannel.btn_m17.Text = macroWizard.MP17_ButtonText;
        private void btn_m18_TextChange(object sender, EventArgs e) => macroPannel.btn_m18.Text = macroWizard.MP18_ButtonText;
        private void btn_m19_TextChange(object sender, EventArgs e) => macroPannel.btn_m19.Text = macroWizard.MP19_ButtonText;
        private void btn_m20_TextChange(object sender, EventArgs e) => macroPannel.btn_m20.Text = macroWizard.MP20_ButtonText;

    }
}



