﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{
    public partial class UserControlDay : UserControl
    {
        public UserControlDay()
        {
            InitializeComponent();
        }

        private void UserControlDay_Load(object sender, EventArgs e)
        {

        }

        public void dayNumber(int number) { 
            labelDay.Text = number.ToString();
        }

        public void holidayName(String name)
        {
            labelHolidayName.Text = name;
        }
    }
}
