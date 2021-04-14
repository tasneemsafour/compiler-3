using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearingMachineModels;


namespace BearingMachineModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            n = 1;
            DelayTimeDistribution = new List<TimeDistribution>();
            BearingLifeDistribution = new List<TimeDistribution>();

            CurrentSimulationTable = new List<CurrentSimulationCase>(); //rows

            CurrentPerformanceMeasures = new PerformanceMeasures();

            ProposedSimulationTable = new List<ProposedSimulationCase>();
            ProposedPerformanceMeasures = new PerformanceMeasures();

            this.file = new List<string>();
            this.input_inform = new List<string>();
            this.DelayTimeDistribution = new List<TimeDistribution>();
            this.BearingLifeDistribution = new List<TimeDistribution>();
            read_from_file();
            calculate_CummProbability(DelayTimeDistribution, BearingLifeDistribution);
            calculate_CummProbability(DelayTimeDistribution, BearingLifeDistribution);

            /* SetValues();
             CostOfBearing();
             CostOfDelayTime();
             CostOfDownTime();
             RepairPerson();
             TotalCost();*/
        }
        public List<string> file { get; set; }
        public List<string> input_inform { get; set; }
        ///////////// INPUTS /////////////
        public int DowntimeCost { get; set; }
        public int RepairPersonCost { get; set; }
        public int BearingCost { get; set; }
        public int NumberOfHours { get; set; }
        public int NumberOfBearings { get; set; }
        public int RepairTimeForOneBearing { get; set; }
        public int RepairTimeForAllBearings { get; set; }
        public List<TimeDistribution> DelayTimeDistribution { get; set; }
        public List<TimeDistribution> BearingLifeDistribution { get; set; }

        ///////////// OUTPUTS /////////////
        public List<CurrentSimulationCase> CurrentSimulationTable { get; set; }
        public PerformanceMeasures CurrentPerformanceMeasures { get; set; }
        public List<ProposedSimulationCase> ProposedSimulationTable { get; set; }
        public PerformanceMeasures ProposedPerformanceMeasures { get; set; }

        ////////////////////////////additional Variable//////////////////////////////////////
        public int NumberOfBearChanged { get; set;}
        public int TotalDelayTime { get; set; }
        public int n { get; set; }

        public void read_from_file()
        {
            FileStream fs = new FileStream(@"C:\Users\TASNEEM\Downloads\[Students]_Template\BearingMachineSimulation\TestCases\TestCase2.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            while (sr.Peek() != -1)
            {
                file.Add(sr.ReadLine());
            }
            sr.Close();
            fs.Close();
            for (int i = 1; i < 21; i = i + 3)
            {
                input_inform.Add(file[i]);

            }
            DowntimeCost = int.Parse(input_inform[0]);

            RepairPersonCost = int.Parse(input_inform[1]);

            BearingCost = int.Parse(input_inform[2]);
            NumberOfHours = int.Parse(input_inform[3]);

            NumberOfBearings = int.Parse(input_inform[4]);

            RepairTimeForOneBearing = int.Parse(input_inform[5]);
            RepairTimeForAllBearings = int.Parse(input_inform[6]);

           
            for (int i = 22; i < 25; i++)
            {
                string[] arr = file[i].Split(',');
                DelayTimeDistribution.Add(new TimeDistribution() { Time = int.Parse(arr[0]), Probability = decimal.Parse(arr[1]) });


            }

            for (int i = 27; i < file.Count; i++)
            {

                string[] arr = file[i].Split(',');
                BearingLifeDistribution.Add(new TimeDistribution() { Time = int.Parse(arr[0]), Probability = decimal.Parse(arr[1]) });

            }

            calculate_CummProbability(DelayTimeDistribution, BearingLifeDistribution);
            calculate_RandomDigit(DelayTimeDistribution, BearingLifeDistribution);

        }

        public void calculate_CummProbability(List<TimeDistribution> DelayTimeDistribution, List<TimeDistribution> BearingLifeDistribution)
        {
            for (int i = 0; i < DelayTimeDistribution.Count; i++)
            {
                decimal sumDelay = 0.0m;
                for (int j = 0; j <= i; j++)
                {
                    sumDelay += DelayTimeDistribution[j].Probability;

                }
                DelayTimeDistribution[i].CummProbability = sumDelay;
            }

            for (int i = 0; i < BearingLifeDistribution.Count; i++)
            {

                decimal sumBearing = 0.0m;
                for (int j = 0; j <= i; j++)
                {

                    sumBearing += BearingLifeDistribution[j].Probability;
                }
                BearingLifeDistribution[i].CummProbability = sumBearing;
            }

        }
        public void calculate_RandomDigit(List<TimeDistribution> DelayTimeDistribution, List<TimeDistribution> BearingLifeDistribution)
        {
            DelayTimeDistribution[0].MinRange = 1;
            BearingLifeDistribution[0].MinRange = 1;
            Int16 range1 = Convert.ToInt16((DelayTimeDistribution[0].CummProbability * 100));
            Int16 range2 = Convert.ToInt16((BearingLifeDistribution[0].CummProbability * 100));
            DelayTimeDistribution[0].MaxRange = range1;
            BearingLifeDistribution[0].MaxRange = range2;

            for (int i = 1; i < DelayTimeDistribution.Count; i++)
            {
                if (DelayTimeDistribution[i].Probability == 0)
                {
                    DelayTimeDistribution[i].MinRange = DelayTimeDistribution[i - 1].MinRange;
                    int range = DelayTimeDistribution[i - 1].MaxRange;
                    DelayTimeDistribution[i].MaxRange = range;
                }
                else
                {
                    DelayTimeDistribution[i].MinRange = DelayTimeDistribution[i - 1].MaxRange + 1;

                    Int16 range = Convert.ToInt16((DelayTimeDistribution[i].CummProbability * 100));
                    DelayTimeDistribution[i].MaxRange = range;
                }
            }
            for (int i = 1; i < BearingLifeDistribution.Count; i++)
            {
                if (BearingLifeDistribution[i].Probability == 0)
                {
                    BearingLifeDistribution[i].MinRange = BearingLifeDistribution[i - 1].MinRange;
                    int range = BearingLifeDistribution[i - 1].MaxRange;
                    BearingLifeDistribution[i].MaxRange = range;
                }
                else
                {
                    BearingLifeDistribution[i].MinRange = BearingLifeDistribution[i - 1].MaxRange + 1;

                    Int16 range = Convert.ToInt16((BearingLifeDistribution[i].CummProbability * 100));
                    BearingLifeDistribution[i].MaxRange = range;
                }

            }
        }
        public void SetValues()
        {
            
            Random rd = new Random();
            Random rd1 = new Random();
            int i = 0;
            bool contt = true;
            bool accumelator_zero = true;
            for (int s = 1; s <= NumberOfBearings; s++)
            {
                contt = true;
                accumelator_zero = true;
                while (contt==true)
                {
                    CurrentSimulationTable.Add(new CurrentSimulationCase());
                    CurrentSimulationTable[i].Bearing = new Bearing();
                    CurrentSimulationTable[i].Bearing.RandomHours = rd.Next(1, 100); //1_c

                    CurrentSimulationTable[i].Bearing.Index = s;
                    for (int k = 0; k < BearingLifeDistribution.Count; k++) //2_c
                    {
                        if (CurrentSimulationTable[i].Bearing.RandomHours >= BearingLifeDistribution[k].MinRange
                            && CurrentSimulationTable[i].Bearing.RandomHours <= BearingLifeDistribution[k].MaxRange)
                        {
                            CurrentSimulationTable[i].Bearing.Hours = BearingLifeDistribution[k].Time;
                            break;
                        }
                    }
                   
                    if (accumelator_zero==true)
                    {
                        CurrentSimulationTable[i].AccumulatedHours = CurrentSimulationTable[i].Bearing.Hours;//3_c
                        accumelator_zero = false;
                    }
                    else
                    {
                        CurrentSimulationTable[i].AccumulatedHours = CurrentSimulationTable[i-1].AccumulatedHours+ CurrentSimulationTable[i].Bearing.Hours;//3_c
                    }

                    CurrentSimulationTable[i].RandomDelay = rd1.Next(1, 100); //4_c

                    for (int k = 0; k < DelayTimeDistribution.Count; k++) //5_c
                    {
                        if (CurrentSimulationTable[i].RandomDelay >= DelayTimeDistribution[k].MinRange
                            && CurrentSimulationTable[i].RandomDelay <= DelayTimeDistribution[k].MaxRange)
                        {
                            CurrentSimulationTable[i].Delay = DelayTimeDistribution[k].Time;
                            
                            TotalDelayTime += CurrentSimulationTable[i].Delay;
                            break;
                        }
                    }

                    if (CurrentSimulationTable[i].AccumulatedHours >= NumberOfHours)
                    {
                        
                        contt = false;
                        accumelator_zero = true;
                    }

                    i++;
                }
              
            }
            //NumberOfBearChanged = i;
        }
        public void CostOfBearing()
        {
            CurrentPerformanceMeasures.BearingCost = BearingCost* CurrentSimulationTable.Count;
        }
        public void CostOfDelayTime()
        {
            CurrentPerformanceMeasures.DelayCost = TotalDelayTime*DowntimeCost;
        }
        public void CostOfDownTime()
        {
          
            CurrentPerformanceMeasures.DowntimeCost = CurrentSimulationTable.Count * RepairTimeForOneBearing * DowntimeCost;
        }
        public void RepairPerson()
        {
           
            CurrentPerformanceMeasures.RepairPersonCost = CurrentSimulationTable.Count * RepairTimeForOneBearing * RepairPersonCost / 60.0m;
        }
        public void TotalCost()
        {
            CurrentPerformanceMeasures.TotalCost = CurrentPerformanceMeasures.BearingCost +
                CurrentPerformanceMeasures.DelayCost + CurrentPerformanceMeasures.DowntimeCost
                + CurrentPerformanceMeasures.RepairPersonCost;
        }

        int sum_Delay = 0;

        public void Proposed_table()
        {
            TotalDelayTime = 0;
            Random rd = new Random();
            Random rd2 = new Random();
            int k = 0;
            bool[] take = new bool[CurrentSimulationTable.Count];
            bool cont = true;
            int bearing_index = 1;
            bool find = false;
          /*  int[,] bearing_array = new int[CurrentSimulationTable.Count, NumberOfBearings];

            for (int g=0;g<CurrentSimulationTable.Count;g++)
            {
                for (int y=0 ; y < NumberOfBearings ; y++)
                {
                  if(CurrentSimulationTable[g].Bearing.Index==y+1)
                    {
                        bearing_array[g,y] = CurrentSimulationTable[g].Bearing.Hours;
                    }

                }
               
            }*/

            while (cont==true)
            {
                ProposedSimulationTable.Add(new ProposedSimulationCase());
                 bearing_index = 1;
                find = false;

                int min = 100000000;

                for (int i = 0; i < NumberOfBearings; i++)
                {
                    //ProposedSimulationTable[i].Bearings.Add(new Bearing());

                   find = false;
                   for (int y=0;y< CurrentSimulationTable.Count && find == false; y++)
                    {

                        if (CurrentSimulationTable[y].Bearing.Index == bearing_index && take[y]==false)
                        {

                            ProposedSimulationTable[k].Bearings.Add(CurrentSimulationTable[y].Bearing);
                            take[y] = true;
                            find = true;
                            bearing_index++;
                            break;
                        }

                    }

                   
                    if (find==false)
                    {
                        ProposedSimulationTable[k].Bearings.Add(new Bearing());
                        ProposedSimulationTable[k].Bearings[i].RandomHours = rd.Next(1, 100);
                        ProposedSimulationTable[k].Bearings[i].Index = bearing_index;
                        bearing_index++;
                       
                        for (int y = 0; y < BearingLifeDistribution.Count; y++)
                        {
                            if (BearingLifeDistribution[y].MinRange <= ProposedSimulationTable[k].Bearings[i].RandomHours && BearingLifeDistribution[y].MaxRange >= ProposedSimulationTable[k].Bearings[i].RandomHours)
                            {
                                ProposedSimulationTable[k].Bearings[i].Hours = BearingLifeDistribution[y].Time;
                                break;

                            }
                        }
                    }

                    if (ProposedSimulationTable[k].Bearings[i].Hours < min)
                        min = ProposedSimulationTable[k].Bearings[i].Hours;
                }

                ProposedSimulationTable[k].FirstFailure = min;
                if (k == 0)
                {
                    ProposedSimulationTable[k].AccumulatedHours = ProposedSimulationTable[k].FirstFailure;
                }
                else
                {
                    ProposedSimulationTable[k].AccumulatedHours = ProposedSimulationTable[k].FirstFailure + ProposedSimulationTable[k - 1].AccumulatedHours;
                }
                ProposedSimulationTable[k].RandomDelay = rd2.Next(1, 100); 

                for (int p = 0; p < DelayTimeDistribution.Count; p++)
                {
                    if (ProposedSimulationTable[k].RandomDelay >= DelayTimeDistribution[p].MinRange
                        && ProposedSimulationTable[k].RandomDelay <= DelayTimeDistribution[p].MaxRange)
                    {
                        ProposedSimulationTable[k].Delay = DelayTimeDistribution[p].Time;

                        TotalDelayTime += ProposedSimulationTable[k].Delay;
                        break;
                    }
                }

                if (ProposedSimulationTable[k].AccumulatedHours >= NumberOfHours)
                    cont = false;
                k++;
            }
        }

        public void perform_table2()
        {
            ProposedPerformanceMeasures.BearingCost = ProposedSimulationTable.Count *NumberOfBearings* BearingCost;
            ProposedPerformanceMeasures.DelayCost = TotalDelayTime * DowntimeCost;
            ProposedPerformanceMeasures.DowntimeCost = ProposedSimulationTable.Count * DowntimeCost * RepairTimeForAllBearings;
            ProposedPerformanceMeasures.RepairPersonCost = ProposedSimulationTable.Count * RepairTimeForAllBearings * RepairPersonCost / 60.0m;
            ProposedPerformanceMeasures.TotalCost = ProposedPerformanceMeasures.BearingCost + ProposedPerformanceMeasures.DelayCost + ProposedPerformanceMeasures.DowntimeCost + ProposedPerformanceMeasures.RepairPersonCost;

        }

    }
}