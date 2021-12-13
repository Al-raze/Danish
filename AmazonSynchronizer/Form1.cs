using CsvHelper;
using Inventory.Synchronizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazonSynchronizer
{
    
    public partial class Form1 : Form
    {
        delegate void LabelWriteDelegate(string value);
        delegate void EnableButton(bool value);
        AmazonSync sync; 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            dateTimeFrom.Value = dateTimeFrom.Value.AddDays(-1);

            sync = new AmazonSync(Settings.SellerIndex);
            sync.InsertOrderEvent += sync_InsertOrderEvent;
            AppendConsoleText(string.Format("Install Location...{0}", System.Reflection.Assembly.GetExecutingAssembly().Location)); 
            AppendConsoleText(string.Format("Current UTC Time {0}",DateTime.Now.ToUniversalTime()));
        }

        private void btnSynchronize_Click(object sender, EventArgs e)
        {
            var value = dateTimeTo.Value.Subtract(dateTimeFrom.Value);

            if (value.Days < 0)
            {
                MessageBox.Show("Please select a valid date range,To Date cannot be lesser than the from date");
                return;
            }

            if (value.Days > 3)
            {
                MessageBox.Show("You can only select from and to date in the range of 3 days");
                return;
            }
            
            AppendConsoleText(string.Format("Started Synchronizing...."));

            List<string> marketPlace = Settings.MarketPlaceList.Split(',').ToList();

            Task asyncTask = new Task(() => sync.AmazonSynchronization(dateTimeFrom.Value.ToShortDateString(), dateTimeTo.Value.ToShortDateString(), marketPlace));

            asyncTask.ContinueWith((t) =>
            {
                if (!t.IsFaulted)
                {
                    AppendConsoleText(string.Format("Orders Synchronized Successfully"));
                    
                }
                else
                {
                    AppendConsoleText(t.Exception.InnerException.Message);              
                }
            });

            asyncTask.Start();

            
        }

        int sync_InsertOrderEvent(string details)
        {
            AppendConsoleText(details);
            return 0;
        }

        public void AppendConsoleText(string value)
        {
            if (InvokeRequired)
                Invoke(new LabelWriteDelegate(AppendConsoleText), value);
            else
            {
                txtMessages.AppendText(value);
                txtMessages.AppendText(Environment.NewLine);
            }
        }

        private void btnSynchronizeImages_Click(object sender, EventArgs e)
        {
            AppendConsoleText(string.Format("Started Synchronizing Images...."));

            Task asyncTask = new Task(() => sync.AmazonSynchronizeImages());

            asyncTask.ContinueWith((t) =>
            {
                if (!t.IsFaulted)
                {
                    AppendConsoleText(string.Format("Images Synchronized Successfully"));
                }
                else
                {
                    AppendConsoleText(t.Exception.InnerException.Message);
                }
            });

            asyncTask.Start();
        }

        private void txtMessages_TextChanged(object sender, EventArgs e)
        {

        }


        private void SyncTodaysOrders()
        {
            var value = dateTimeTo.Value.Subtract(dateTimeFrom.Value);

            if (!Directory.Exists("/Reports"))
            {
                Directory.CreateDirectory("Reports");
            }

            if (value.Days < 0)
            {
                MessageBox.Show("Please select a valid date range,To Date cannot be lesser than the from date");
                return;
            }

            if (value.Days > 15)
            {
                MessageBox.Show("You can only select from and to date in the range of 15 days");
                return;
            }

            Task asyncTask = new Task(() => sync.DownloadOrderReport(dateTimeFrom.Value, dateTimeTo.Value));

            asyncTask.ContinueWith((t) =>
            {
                if (!t.IsFaulted)
                {
                    AppendConsoleText(string.Format("Sales Commission Synchronized Successfully"));
                }
                else
                {
                    AppendConsoleText(t.Exception.InnerException.Message);
                }

            });

            asyncTask.Start();
        }


        private void btnReportSync_Click(object sender, EventArgs e)
        {

           
        }

        private void button1_Click(object sender, EventArgs e)
        {

            List<string> marketPlace = Settings.MarketPlaceList.Split(',').ToList();

            openFileDialog1.Filter = "txt files (*.txt)|*.txt";

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtOrderNumber.Text = openFileDialog1.FileName;
                
            }

            AppendConsoleText(string.Format("Started Synchronizing Order File ", txtOrderNumber.Text));

            Task asyncTask = new Task(() => sync.ParseOrderFile(txtOrderNumber.Text));

            asyncTask.ContinueWith((t) =>
            {
                if (!t.IsFaulted)
                {
                    AppendConsoleText(string.Format("Order File Synchronized Successfully"));
                }
                else
                {
                    AppendConsoleText(t.Exception.InnerException.Message);
                }
            });

            asyncTask.Start();
        }

        private void btnSyncOrder_Click(object sender, EventArgs e)
        {
            List<string> orderNumber = new List<string>() { txtOrderNum.Text };

            sync.AmazonSyncOrderNo(orderNumber);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtOrderNumber.Text = openFileDialog1.FileName;

            }

            AppendConsoleText(string.Format("Started Synchronizing Order File ", txtOrderNumber.Text));

            Task asyncTask = new Task(() => sync.ParseOrderFile(txtOrderNumber.Text,true));

            asyncTask.ContinueWith((t) =>
            {
                if (!t.IsFaulted)
                {
                    AppendConsoleText(string.Format("Order File Synchronized Successfully"));
                }
                else
                {
                    AppendConsoleText(t.Exception.InnerException.Message);
                }
            });

            asyncTask.Start();
        }
    }
}
