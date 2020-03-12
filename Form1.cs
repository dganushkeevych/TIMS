using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace MathStatistics
{
    public partial class from1 : Form
    {
        private List<Interval> intervals = new List<Interval>();
        private List<double> xi = new List<double>();
        private List<int> ni = new List<int>();

        // 0 -> Desc, 1 -> Uninterrupt
        private int type = 0;

        private Random random = new Random();

        public from1()
        {
            InitializeComponent();
            ReadUninterrupted();
            Uninterrupted();
        }

        public void Desc()
        {
            type = 0;
            dataGridView1.Columns[0].HeaderText = "xi";
            dataGridView1.Columns[1].HeaderText = "ni";
            FillDataGrid(xi, ni);
            СalculateСharacteristics(xi, ni);
            F();
            Poligon();
        }

        public void ReadDesc()
        {
            clear();
            string[] lines = File.ReadAllLines("../../readDesc.txt");
            xi = lines[0].Split(' ').Select(s => Convert.ToDouble(s)).ToList();
            ni = lines[1].Split(' ').Select(s => Convert.ToInt32(s)).ToList();
        }

        public void RandomDesc(int volume, int a, int b)
        {
            resultTextBox.Clear();

            xi.Clear();
            ni.Clear();

            int counter = 0;

            List<double> elements = new List<double> { };
            for (int i = 0; i < volume; i++)
            {
                elements.Add(random.Next(a, b));
                textBox3.Text += $"{elements.Last()} ";
            }
            elements.Sort();
            foreach (double e in elements)
            {
                resultTextBox.Text += $"{e} ";
            }
            xi = elements.Distinct().ToList();
            for (int i = 0; i < xi.Count; i++)
            {
                for (int j = 0; j < elements.Count; j++)
                {
                    if(xi[i] == elements[j])
                    {
                        counter++;
                    }
                }
                ni.Add(counter);
                counter = 0;
            }
        }

        public void RandomUniterrupted(int n, int a, int b)
        {
            resultTextBox.Clear();
            xi.Clear();
            ni.Clear();
            intervals.Clear();

            List<double> vybirka = new List<double> { };
            int counter2 = 0;
            double v;
            for (int i = 0; i < n; i++)
            {
                v = Math.Round(random.NextDouble() * (b - a) + a, 1);
                vybirka.Add(v);
                textBox3.Text += $"{v} ";
            }
            vybirka.Sort();
            double rozmah = vybirka.Last() - vybirka.First();
            int r = Convert.ToInt32(Math.Floor(Math.Log(n, 2)));
            double interval = rozmah/(r+1);
            double begin = vybirka.First();
            for (int i = 0; i < r+1; i++)
            {
                intervals.Add(new Interval { A = begin, B = begin + interval });
                resultTextBox.Text += $"{intervals.Last()} ";
                xi.Add(begin + interval / 2);
                for (int l = 0; l < vybirka.Count; l++)
                {
                    if(begin <= vybirka[l] && vybirka[l] < begin + interval)
                    {
                        counter2++;
                    }
                }
                if(i == r)
                {
                    counter2 = n - ni.Sum();
                }
               
                ni.Add(counter2);
                counter2 = 0;
                begin += interval;

            }
        }

        public void clear()
        {
            label17.Text = "";
            label18.Text = "";
            label24.Text = "";
            textBox3.Text = "";
            resultTextBox.Text = "";
            interquartileTextBox.Text = "null";
            interoktileTextBox.Text = "null";
            interdeclineTextBox.Text = "null";
            intercellularTextBox.Text = "null";
            intermediateTextBox.Text = "null";
            m.Text = "";
        }

        public void СalculateСharacteristics(List<double> xi, List<int> ni)
        {
            double median = Median(xi, ni);
            medianTextBox.Text = median.ToString();

            List<double> mode = Mode(xi, ni);
            modeTextBox.Text = MakeStringFromList(mode);

            double averageSelective = AverageSelective(xi, ni);
            averageSelectiveTextBox.Text = Math.Round(averageSelective, 3).ToString();

            double scope = xi.Last() - xi.First();
            scopeTextBox.Text = scope.ToString();

            double dev = Dev(xi, ni);
            devTextBox.Text = Math.Round(dev, 3).ToString();

            double variansa = Variansa(xi, ni);
            variansaTextBox.Text = Math.Round(variansa, 3).ToString();

            double standart = Standart(xi, ni);
            standartTextBox.Text = Math.Round(standart, 3).ToString();

            double dispersion = Dispersion(xi, ni);
            dispersionTextBox.Text = Math.Round(dispersion, 3).ToString();

            double variation = Variation(xi, ni);
            varioationTextBox.Text = Math.Round(variation, 3).ToString();

            double asymmetry = Asymmetry(xi, ni);
            asymmetryTextBox.Text = Math.Round(asymmetry, 3).ToString();

            double excess = Excess(xi, ni);
            excessTextBox.Text = Math.Round(excess, 3).ToString();


            if (type == 0)
            {
                int n = Volume(ni);

                if (n % 4 == 0)
                {
                    double interquartile = Interquartile(xi, ni, 3) - Interquartile(xi, ni, 1);
                    interquartileTextBox.Text = Math.Round(interquartile, 3).ToString();
                    label17.Text = $"Q1={Interquartile(xi, ni, 1)}, Q2={Interquartile(xi, ni, 2)}, Q3={Interquartile(xi, ni, 3)}";
                }
                else
                {
                    interquartileTextBox.Text = "null";
                }

                if (n % 8 == 0)
                {
                    double interoktile = Interoktile(xi, ni, 7) - Interoktile(xi, ni, 1);
                    interoktileTextBox.Text = Math.Round(interoktile, 3).ToString();
                    string res = "";
                    for (int i = 1; i < 8; i++)
                    {
                        res += $"O{i}={Interoktile(xi, ni, i)} ";
                    }
                    label24.Text = res;
                }
                else
                {
                    interoktileTextBox.Text = "null";
                }

                if (n % 10 == 0)
                {
                    double interdecline = Interdecline(xi, ni, 9) - Interdecline(xi, ni, 1);
                    interdeclineTextBox.Text = Math.Round(interdecline, 3).ToString();
                    string res = "";
                    for (int i = 1; i < 10; i++)
                    {
                        res += $"D{i}={Interdecline(xi, ni, i)} ";
                    }
                    label18.Text = res;
                }
                else
                {
                    interdeclineTextBox.Text = "null";
                }

                if (n % 100 == 0)
                {
                    double intercellular = Intercellular(xi, ni, 99) - Intercellular(xi, ni, 1);
                    intercellularTextBox.Text = Math.Round(intercellular, 3).ToString();
                }
                else
                {
                    intercellularTextBox.Text = "null";
                }

                if (n % 1000 == 0)
                {
                    double intermediate = Intermediate(xi, ni, 999) - Intermediate(xi, ni, 1);
                    intermediateTextBox.Text = Math.Round(intermediate, 3).ToString();
                }
                else
                {
                    intermediateTextBox.Text = "null";
                }
            }

        }

        public void Uninterrupted()
        {
            dataGridView1.Columns[0].HeaderText = "zi";
            dataGridView1.Columns[1].HeaderText = "ni";
            FillDataGrid(xi, ni);
            СalculateСharacteristics(xi, ni);
            F();
            Histogram();
        }

        public void F()
        {
            foreach (var item in chart2.Series)
            {
                item.Points.Clear();
                item.BorderWidth = 4;
                chart2.Series[0].ChartType = SeriesChartType.Line;
                chart2.Series[0].Points.AddXY(xi[0] - 2, 0);
            }

            double result = 0;
            double prevResult = 0;
            int n = Volume(ni);
            for (int i = 0; i < ni.Count; i++)
            {
                result *= n;
                result += ni[i];
                result /= n;
                if (i + 1 >= chart2.Series.Count)
                {
                    chart2.Series.Add(new Series { BorderWidth = 4, ChartType = SeriesChartType.Line });
                }
                chart2.Series[i].Points.AddXY(xi[i], prevResult);
                chart2.Series[i + 1].Points.AddXY(xi[i], result);
                prevResult = result;
            }

            chart2.Series.Last().Points.AddXY(xi.Last(), 1);
            chart2.Series.Last().Points.AddXY(xi.Last() + 3, 1);
        }

        public void Poligon()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[0].BorderWidth = 2;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            for (int i = 0; i < ni.Count; i++)
            {
                chart1.Series[0].Points.AddXY(xi[i], ni[i]);
            }
        }

        public void Histogram()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            for (int i = 0; i < ni.Count; i++)
            {
                chart1.Series[0].Points.AddXY(intervals[i].ToString(), ni[i]);
            }
        }

        public void ReadUninterrupted()
        {
            type = 1;
            clear();
            string[] lines = File.ReadAllLines("../../readUninterrupted.txt");
            intervals = lines[0].Split(' ').Select(s =>
            {
                string[] interval = s.Split('-');
                return new Interval { A = Convert.ToDouble(interval[0]), B = Convert.ToDouble(interval[1]) };
            }).ToList();

            xi = intervals.Select(i => (i.A + i.B) / 2).ToList();
            ni = lines[1].Split(' ').Select(s => Convert.ToInt32(s)).ToList();
        }

        public void FillDataGrid(List<double> c1, List<int> c2)
        {
            for (int i = 0; i < c1.Count; i++)
            {
                dataGridView1.Rows.Add(c1[i], c2[i]);
            }
        }

        public double AverageSelective(List<double> xi, List<int> ni)
        {
            double result = 0;
            for (int i = 0; i < ni.Count; i++)
            {
                result += xi[i] * ni[i];
            }

            result = result / Volume(ni);
            return result;
        }

        public double Dev(List<double> xi, List<int> ni)
        {
            double result = 0;
            double averageSelective = AverageSelective(xi, ni);
            for (int i = 0; i < ni.Count; i++)
            {
                result += ni[i] * Math.Pow(xi[i] - averageSelective, 2);
            }

            return result;
        }

        public double Variansa(List<double> xi, List<int> ni)
        {
            double dev = Dev(xi, ni);
            double result = dev / (Volume(ni) - 1);

            return result;
        }

        public double Standart(List<double> xi, List<int> ni)
        {
            double result = Math.Sqrt(Variansa(xi, ni));

            return result;
        }

        public double Dispersion(List<double> xi, List<int> ni)
        {
            double result = Dev(xi, ni) / Volume(ni);

            return result;
        }

        public double Variation(List<double> xi, List<int> ni)
        {
            double result = Standart(xi, ni) / AverageSelective(xi, ni);

            return result;
        }

        public double Median(List<double> xi, List<int> ni)
        {
            int n = Volume(ni);
            int k = 0;
            if (IsOdd(n))
            {
                k = n / 2 + 1;
                int sum = 0;
                for (int i = 0; i < ni.Count; i++)
                {
                    sum += ni[i];
                    if (sum >= k)
                    {
                        return xi[i];
                    }
                }
            }
            else
            {
                k = n / 2;
                int sum = 0;
                for (int i = 0; i < ni.Count; i++)
                {
                    sum += ni[i];
                    if (sum == k)
                    {
                        return (xi[i] + xi[i + 1]) / 2;
                    }
                    else if (sum > k)
                    {
                        return xi[i];
                    }
                }
            }
            throw new ArgumentException();
        }

        public bool IsOdd(int x)
        {
            return x % 2 != 0;
        }

        public int Volume(List<int> ni)
        {
            int result = 0;
            for (int i = 0; i < ni.Count; i++)
            {
                result += ni[i];
            }

            return result;
        }

        public List<double> Mode(List<double> xi, List<int> ni)
        {
            int max = ni.Max();
            List<double> result = new List<double>();

            for (int i = 0; i < ni.Count; i++)
            {
                if (ni[i] == max)
                {
                    result.Add(xi[i]);
                }
            }
            return result;
        }

        public double Moment(List<double> xi, List<int> ni, int k)
        {
            double result = 0;
            double n = ni.Sum();
            double averageSelective = AverageSelective(xi, ni);
            for (int i = 0; i < ni.Count; i++)
            {
                result += ni[i] * Math.Pow(xi[i] - averageSelective, k);
            }
            return result/n;
        }

        public double Asymmetry(List<double> xi, List<int> ni)
        {
            double result = Moment(xi, ni, 3) / Math.Pow(Moment(xi, ni, 2), 1.5);

            return result;
        }

        public double Excess(List<double> xi, List<int> ni)
        {
            double result = Moment(xi, ni, 4) / Math.Pow(Moment(xi, ni, 2), 2) - 3;

            return result;
        }

        public double Intermediate(List<double> xi, List<int> ni, int j)
        {
            int amount = j * Volume(ni) / 1000;
            int sum = 0;
            for (int i = 0; i < ni.Count; i++)
            {
                sum += ni[i];
                if (sum >= amount)
                {
                    return xi[i];
                }
            }

            throw new ArgumentException();
        }

        public double Intercellular(List<double> xi, List<int> ni, int j)
        {
            int amount = j * Volume(ni) / 100;
            int sum = 0;
            for (int i = 0; i < ni.Count; i++)
            {
                sum += ni[i];
                if (sum >= amount)
                {
                    return xi[i];
                }
            }

            throw new ArgumentException();
        }

        public double Interdecline(List<double> xi, List<int> ni, int j)
        {
            int amount = j * Volume(ni) / 10;
            int sum = 0;
            for (int i = 0; i < ni.Count; i++)
            {
                sum += ni[i];
                if (sum >= amount)
                {
                    return xi[i];
                }
            }

            throw new ArgumentException();
        }

        public double Interoktile(List<double> xi, List<int> ni, int j)
        {
            int amount = j * Volume(ni) / 8;
            int sum = 0;
            for (int i = 0; i < ni.Count; i++)
            {
                sum += ni[i];
                if (sum >= amount)
                {
                    return xi[i];
                }
            }

            throw new ArgumentException();
        }

        public double Interquartile(List<double> xi, List<int> ni, int j)
        {
            int amount = j * Volume(ni) / 4;
            int sum = 0;
            for (int i = 0; i < ni.Count; i++)
            {
                sum += ni[i];
                if (sum >= amount)
                {
                    return xi[i];
                }
            }

            throw new ArgumentException();
        }

        public string MakeStringFromList(List<double> list)
        {
            string result = string.Empty;
            foreach (var item in list)
            {
                result += $"{item}; ";
            }

            return result;
        }

        private void uninterruptedButton2_Click(object sender, EventArgs e)
        {
            clear();
            dataGridView1.Rows.Clear();
            ReadUninterrupted();
            Uninterrupted();
            type = 1;
        }

        private void DescButton_Click(object sender, EventArgs e)
        {
            clear();
            dataGridView1.Rows.Clear();
            ReadDesc();
            Desc();
            type = 0;
        }

        private void randomButtom_Click(object sender, EventArgs e)
        {
            clear();
            int n;
            int a;
            int b;
            if (n_volume.Text != String.Empty)
            {
                n = Convert.ToInt32(n_volume.Text);
            }
            else
            {
                n = 20;
            }

            if (textBox1.Text != String.Empty)
            {
                a = Convert.ToInt32(textBox1.Text);
            }
            else
            {
                a = 2;
            }

            if (textBox2.Text != String.Empty)
            {
                b = Convert.ToInt32(textBox2.Text);
            }
            else
            {
                b = 10;
            }

            dataGridView1.Rows.Clear();
            if (type == 0)
            {
                RandomDesc(n, a, b);
                Desc();
            }
            else if (type == 1)
            {
                RandomUniterrupted(n, a, b);
                Uninterrupted();
            }
        }

        private void momentGenerate_Click(object sender, EventArgs e)
        {
            int num;
            try
            {
                num = Convert.ToInt32(StartMomentTextBox.Text);
                m.Text = Math.Round(Moment(xi, ni, num), 4).ToString();
            }
            catch
            {
                StartMomentTextBox.Focus();
            }
        }

        private void StartMomentTextBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && (e.KeyChar <= '9'))
            {
                return;
            }

            if (Char.IsControl(e.KeyChar))
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    m.Focus();
                }
                return;

            }
            e.Handled = true;
        }

        private int n_TextChanged(object sender, EventArgs e)
        {
            int n;
            n = Convert.ToInt32(n_volume.Text);
            return n;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

//public void RandomDesc()
//{
//    resultTextBox.Clear();
//    int amount = 20;
//    int[] f = new int[amount];
//    xi.Clear();
//    ni.Clear();
//    for (int i = 0; i < amount; i++)
//    {
//        int item = random.Next(1, 30);
//        while (xi.Contains(item))
//        {
//            item = random.Next(1, 30);
//        }
//        xi.Add(item);
//        ni.Add(random.Next(1, 10));
//        f[i] = ni.Last();
//    }

//    int c = 0;
//    xi = xi.OrderBy(x => x).ToList();
//    foreach (int x in xi)
//    {
//        for (int i = 0; i < f[c]; i++)
//        {
//            resultTextBox.Text += $"{x} ";
//        }
//        c++;
//    }
//}


//public void RandomDesc(int volume)
//{
//    //int a = 5;
//    //int b = 10;
//    resultTextBox.Clear();
//    int amount = volume;
//    int[] f = new int[volume];
//    xi.Clear();
//    ni.Clear();
//    int ind = 0;
//    for (int i = 0; i < amount; i++)
//    {
//        int item = random.Next(1, volume);
//        while (xi.Contains(item))
//        {
//            item = random.Next(1, volume);
//        }
//        xi.Add(item);
//        int count = random.Next(1, 10);
//        amount -= count;
//        if (amount > 0)
//        {
//            ni.Add(count);
//            f[ind] = ni.Last();
//            ind++;
//        }
//        else
//        {
//            ni.Add(count + amount);
//            f[ind] = ni.Last();
//            ind++;
//        }
//        i--;

//    }
//    int c = 0;
//    xi = xi.OrderBy(x => x).ToList();
//    foreach (int x in xi)
//    {
//        for (int i = 0; i < f[c]; i++)
//        {
//            resultTextBox.Text += $"{x} ";
//        }
//        c++;
//    }
//}
//resultTextBox.Clear();
//int amount = random.Next(3, 10);
//double interval = Math.Round(random.NextDouble() * (99.9-0.01) + 0.01, 3);
//double begin = random.Next(1, 20);
//xi.Clear();
//ni.Clear();
//intervals.Clear();
//for (int i = 0; i < amount; i++)
//{
//    xi.Add(begin);
//    intervals.Add(new Interval { A = begin - interval, B = begin + interval });
//    resultTextBox.Text += $"{intervals.Last()} ";
//    ni.Add(random.Next(1, 100));
//    begin += 2 * interval;
//}