using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace transport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Tag = new List<ITransport>();
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private List<string> StringToList(string text)
        {
            List<string> list = new List<string>();
            string temp = "";
            foreach (char c in text)
            {
                if (c != '\n' && c != '\r')
                    temp += c;
                else if (c == '\n')
                {
                    list.Add(temp);
                    temp = "";
                }
            }
            if (temp != "")
                list.Add(temp);
            return list;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> stops = StringToList(textBox2.Text);
            if (stops.Count < 2)
                return;
            foreach(string s in stops)
            {
                if (s == "")
                    return;
            }
            stops = stops.Distinct().ToList();
            CityTransport T = new CityTransport(stops);
            this.Tag = T;
            comboBox1.DataSource = stops;
            comboBox1.SelectedIndex = 0;
            comboBox2.DataSource = new List<string>(stops);
            comboBox2.SelectedIndex = 0;

            button1.Visible = false;
            label1.Visible = false;
            textBox2.Visible = false;

            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            comboBox1.Visible = true;
            comboBox2.Visible = true;
            comboBox3.Visible = true;
            comboBox4.Visible = true;
            textBox1.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            listBox1.Visible = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ITransport> list = listBox1.Tag as List<ITransport>;
            if (list.Count == 0)
                return;
            ITransport T = list[listBox1.SelectedIndex];
            textBox1.Text = "";
            List<string> route = T.GetRoute();
            if (route == null)
                textBox1.Text = "No route";
            else
            {
                foreach (string temp in route)
                {
                    textBox1.Text += temp + Environment.NewLine;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
                return;
            string from, to, type;
            from = comboBox1.SelectedItem.ToString();
            to = comboBox2.SelectedItem.ToString();
            type = comboBox3.SelectedItem.ToString();
            if (to == from)
                return;
            CityTransport T = this.Tag as CityTransport;
            switch(type)
            {
                case "Дорога":
                    T.RoadMap.CreateRoad(from, to);
                    break;
                case "Дорога (троллейбус)":
                    T.RoadMap.CreateRoad(from, to, 2);
                    break;
                case "Тоннель метро":
                    T.Subway.CreateRoad(from, to);
                    break;
                case "ЖД":
                    T.Railways.CreateRoad(from, to);
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> route = StringToList(textBox4.Text);
            string name = textBox3.Text;
            if (name == "")
                return;
            if (comboBox4.SelectedItem == null)
                comboBox4.SelectedIndex = 0;
            string type = comboBox4.SelectedItem.ToString();

            CityTransport C = this.Tag as CityTransport;
            List<ITransport> list = listBox1.Tag as List<ITransport>;
            Transport T = null;
            switch(type)
            {
                case "Автобус":
                    T = new Bus(name, C);
                    break;
                case "Троллейбус":
                    T = new Trolleybus(name, C);
                    break;
                case "Трамвай":
                    T = new Tram(name, C);
                    break;
                case "Поезд метро":
                    T = new SubwayTrain(name, C);
                    break;
            }
            ITransport IT = T as ITransport;
            if (!IT.SetRoute(route))
                textBox4.Text = "Ошибка: некорректный маршрут";
            else
            {
                list.Add(IT);
                listBox1.DataSource = new List<string>();
                listBox1.DataSource = list.Select(t => t.Info()).ToList();
            }
        }
    }
}
