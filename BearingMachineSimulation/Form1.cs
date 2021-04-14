using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BearingMachineModels;
using BearingMachineTesting;

namespace BearingMachineSimulation
{
    public partial class Form1 : Form
    {
        public SimulationSystem sm = new SimulationSystem();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            sm.SetValues();
            sm.CostOfBearing();
            sm.CostOfDelayTime();
            sm.CostOfDownTime();
            sm.RepairPerson();
            sm.TotalCost();

            for (int i = 0; i < sm.CurrentSimulationTable.Count; i++)
            {
                string index = sm.CurrentSimulationTable[i].Bearing.Index.ToString();

                string RandomHours = sm.CurrentSimulationTable[i].Bearing.RandomHours.ToString();

                string Hours = sm.CurrentSimulationTable[i].Bearing.Hours.ToString();

                string AccumulatedHours = sm.CurrentSimulationTable[i].AccumulatedHours.ToString();

                string RandomDelay = sm.CurrentSimulationTable[i].RandomDelay.ToString();

                string Delay = sm.CurrentSimulationTable[i].Delay.ToString();
                if (sm.CurrentSimulationTable[i].RandomDelay == 100)
                    RandomDelay = "0";
                if (dataGridView1.ColumnCount == 0)
                {
                    dataGridView1.Columns.Add("index ", "index ");
                    dataGridView1.Columns.Add("RandomDigits", "RandomDigits");
                    dataGridView1.Columns.Add("Hours", "Hours");
                    dataGridView1.Columns.Add("AccumulatedHours", "AccumulatedHours");
                    dataGridView1.Columns.Add("RandomDelay", "RandomDelay");
                    dataGridView1.Columns.Add("Delay", "Delay");
                }
                dataGridView1.Rows.Add(new string[] { index, RandomHours, Hours, AccumulatedHours, RandomDelay, Delay });
            }

                sm.Proposed_table();
                sm.perform_table2();
            for (int i = 0; i < sm.ProposedSimulationTable.Count; i++)
            {
                string DayNo = (i + 1).ToString();
                string[] Bearinglife = new string[sm.NumberOfBearings];
                for (int n = 0; n < sm.NumberOfBearings; n++)
                {

                    Bearinglife[n] = sm.ProposedSimulationTable[i].Bearings[n].Hours.ToString();
                }
                string Failer = sm.ProposedSimulationTable[i].FirstFailure.ToString();
                string acc = sm.ProposedSimulationTable[i].AccumulatedHours.ToString();
                string random = sm.ProposedSimulationTable[i].RandomDelay.ToString();
                string delay = sm.ProposedSimulationTable[i].Delay.ToString();

                if (dataGridView2.ColumnCount == 0)
                {
                    //dataGridView1.Columns.Add(" ", " ");
                    for (int n = 0; n < sm.NumberOfBearings; n++)
                    {
                        dataGridView2.Columns.Add("Bearing", "Bearing");
                    }
                    dataGridView2.Columns.Add("FirstFailure", "FirstFailure");
                    dataGridView2.Columns.Add("AccumulatedHours", "AccumulatedHours");
                    dataGridView2.Columns.Add("RandomDelay", "RandomDelay");
                    dataGridView2.Columns.Add("Delay", "Delay");
                }
                string[] s = new string[] { Failer, acc, random, delay };
                string[] res = new string[] { };
                res = Bearinglife.Concat(s).ToArray();


                dataGridView2.Rows.Add(res);
            }
                    string testingResult = TestingManager.Test(sm, Constants.FileNames.TestCase2);
                    MessageBox.Show(testingResult);
                
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

