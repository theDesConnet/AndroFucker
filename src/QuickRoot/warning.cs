/*
 * TheEye V1.0
 * c0d9d by DesConnet
 * 
 * Warning Form
 * 
 * https://youtube.com/c/DesConnet
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace QuickRoot
{
    public partial class warning : Form
    {
        public warning()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit(); //Выход
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = checkBox1.Checked; //Ставим значение кнопки такое-же как и checkbox'а
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }
    }
}
