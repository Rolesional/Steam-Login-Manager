using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Steam_Login_Manager
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			this.InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			new Thread(delegate ()
			{
				Thread.Sleep(666);
				MessageBox.Show("The data you have entered will not be transmitted to us.\nYour data is saved in the steamlogin.txt file in the C disk.", "Steam Login Manager", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}).Start();
			Control.CheckForIllegalCrossThreadCalls = false;
			if (!File.Exists(this.path))
			{
				StreamWriter streamWriter = File.CreateText(this.path);
				streamWriter.Flush();
				streamWriter.Dispose();
			}
			foreach (string text in File.ReadAllLines(this.path).ToList<string>())
			{
				string[] array = text.Split(new char[]
				{
					','
				});
				Form1.user item = new Form1.user(array[0], array[1]);
				this.comboBox1.Items.Add(array[0]);
				this.userlist.Add(item);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			try
			{
				List<string> list = File.ReadAllLines(this.path).ToList<string>();
				Form1.user item = new Form1.user(this.textBox1.Text, this.textBox2.Text);
				this.userlist.Add(item);
				list.Add(item.username + "," + item.password);
				File.WriteAllLines(this.path, list);
				MessageBox.Show("User Saved Succesfully!", "Steam Login Manager", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			catch
			{
				MessageBox.Show("An error has occurred, data cannot be written if the txt file is open.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			this.comboBox1.Items.Clear();
			foreach (Form1.user user in this.userlist)
			{
				this.comboBox1.Items.Add(user.username);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Process[] processesByName = Process.GetProcessesByName("steam");
			for (int i = 0; i < processesByName.Length; i++)
			{
				processesByName[i].Kill();
			}
			processesByName = Process.GetProcessesByName("csgo");
			for (int i = 0; i < processesByName.Length; i++)
			{
				processesByName[i].Kill();
			}
			Process.Start(new ProcessStartInfo
			{
				FileName = (string)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "Steamexe", "null"),
				Arguments = " -login " + this.userlist[this.comboBox1.SelectedIndex].username + " " + this.userlist[this.comboBox1.SelectedIndex].password
			});
		}

		private void button3_Click(object sender, EventArgs e)
		{
			base.Hide();
		}

		private string path = "C:\\steamlogin.txt";

		private List<Form1.user> userlist = new List<Form1.user>();

		private struct user
		{
			public user(string user, string pass)
			{
				this.username = user;
				this.password = pass;
			}

			public string username { get; }

			public string password { get; }
		}

    }
}
