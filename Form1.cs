using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadPoolTest
{
    public partial class frmMain : Form
    {

        bool bStart = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!bStart)
            {
                bStart = true;
                btnStart.BackColor = Color.LightSalmon;
                btnStart.Text = "중지";

                ThreadPoolTest.Options options = new ThreadPoolTest.Options();
                options.IsTime = chkTime.Checked;
                options.IsString = chkString.Checked;
                options.IsNumber = chkNumber.Checked;

                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessStart), options);
            }
            else
            {
                bStart = false;
                btnStart.BackColor = SystemColors.Control;
                btnStart.Text = "시작";
            }
        }

        private void ProcessStart(object argument)
        {
            while (bStart)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(MainProcess), argument);
                Thread.Sleep(1000);
            }
        }

        private void MainProcess(object argument)
        {
            ThreadPoolTest.Options options = argument as ThreadPoolTest.Options;
            string strMessage = "";// $"시간 : {options.IsTime.ToString()}, 랜덤문자 : {options.IsString.ToString()}, 숫자 : {options.IsNumber.ToString()}";
            
            if (options.IsTime)
            {
                strMessage = $"시간 : {options.IsTime.ToString()}";
                Log(strMessage);
            }

            if (options.IsString)
            {
                strMessage = $"랜덤문자 : {options.IsString.ToString()}";
                Log(strMessage);
            }

            if (options.IsNumber)
            {
                strMessage = $"숫자 : {options.IsNumber.ToString()}";
                Log(strMessage);
            }

        }

        private void Log(string message)
        {
            string log = $"* [{DateTime.Now}] : {message}";
            string preText = textBox1.Text;
            if (!string.IsNullOrWhiteSpace(preText))
                log = log + Environment.NewLine + preText;

            Invoke(new MethodInvoker(
                    delegate ()
                    {
                        textBox1.Text = log;
                    }
                ));
        }

        private void CheckThreadPool()
        {
            // CancellationTokenSource 클래스 검토해 보자
        }

    }
}
