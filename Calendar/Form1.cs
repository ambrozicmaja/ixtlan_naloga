using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{
    public partial class Form1 : Form
    {
        private List<Holiday> holidays = new List<Holiday>();

        public Form1()
        {
            InitializeComponent();
            InitializeCalendar();
        }

        //Setting the default values to the month and year we are currently in
        public void InitializeCalendar() {
            comboBoxMonth.SelectedIndex = DateTime.Now.Month - 1;
            textBoxYear.Text = DateTime.Now.Year.ToString();

            buttonUpdate.Click += ButtonUpdate_Click;

            GetHolidays("holidays.txt");
            UpdateCalendar();
        }

        //When button Update is clicked it updates the calendar
        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            UpdateCalendar();
        }

        //Reading the file with holidays and saving them into list
        private void GetHolidays(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] components = line.Split(',');
                        DateTime date = DateTime.Parse(components[0].Trim());
                        String name = components[1].Trim();
                        bool isRecurring = components[2].Trim() == "R";

                        holidays.Add(new Holiday { Date = date, Name = name, IsRecurring = isRecurring });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading the file: " + ex.Message);
            }
        }

        //Updating the calender to specific month and year
        private void UpdateCalendar()
        {
            //First we check if the specific date was set
            if (textBoxDate.Text != "DD/MM/YYYY")
            {
                if (DateTime.TryParse(textBoxDate.Text, out DateTime date))
                {
                    comboBoxMonth.SelectedIndex = date.Month - 1;
                    textBoxYear.Text = date.Year.ToString();
                }
                else
                {
                    //If the format of the specific date is not correct the program shows a message
                    MessageBox.Show("Please enter a valid date (e.g., 01/01/2024).");
                }
            }

            if (int.TryParse(textBoxYear.Text, out int year) && comboBoxMonth.SelectedIndex >= 0 && year > 0) {
                //Calculating variables for current month
                int month = comboBoxMonth.SelectedIndex + 1;
                var firstDayOfMonth = new DateTime(year, month, 1);
                var daysInMonth = DateTime.DaysInMonth(year, month);

                flowLayoutPanel.Controls.Clear();
                labelCurrentMonth.Text = firstDayOfMonth.ToString("MMMM");
                labelCurrentYear.Text = year.ToString();

                //Setting up days before the current month
                int daysBefore = Convert.ToInt32(firstDayOfMonth.DayOfWeek.ToString("d"));
                int monthBefore = month - 1 > 0 ? month - 1 : 12;
                int daysInMonthBefore = DateTime.DaysInMonth(year, monthBefore);
                for (int i = daysInMonthBefore - daysBefore + 1; i <= daysInMonthBefore; i++)
                {
                    UserControlBlank blank = new UserControlBlank();
                    blank.dayNumber(i);
                    flowLayoutPanel.Controls.Add(blank);
                }

                //Setting up days of the current month
                for (int i = 0; i < daysInMonth; i++)
                {
                    var date = firstDayOfMonth.AddDays(i);

                    UserControlDay day = new UserControlDay();
                    day.dayNumber(i + 1);
                    day.BackColor = date.DayOfWeek == DayOfWeek.Sunday ? Color.Tan : Color.White;

                    //Checking it the current date is a holiday. If yes, we make it noticable.
                    var holiday = holidays.FirstOrDefault(h =>
                        (h.IsRecurring && h.Date.Month == month && h.Date.Day == date.Day) ||
                        (!h.IsRecurring && h.Date == date));

                    if (holiday != null)
                    {
                        day.holidayName(holiday.Name);
                        day.BackColor = Color.LightBlue;
                    }

                    flowLayoutPanel.Controls.Add(day);
                }

                //Setting up days after the current month
                for (int i = 1; i <= 42 - daysBefore - daysInMonth + 1; i++)
                {
                    UserControlBlank blank = new UserControlBlank();
                    blank.dayNumber(i);
                    flowLayoutPanel.Controls.Add(blank);
                }
            }

            //Putting default values to our elements
            textBoxYear.Text = "";
            comboBoxMonth.SelectedIndex = -1;
            textBoxDate.Text = "DD/MM/YYYY";
        }
    }

    public class Holiday
    {
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public bool IsRecurring { get; set; }
    }
    }
