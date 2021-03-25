using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SpecToolGUI
{
    public partial class Form1 : Form
    {
        //Name of the template flyer
        //Normally this is set automatically from template.config when ReadConfig() is called
        string FlyerTemplateName = "";

        Scanner scan;
        public Form1()
        {
            InitializeComponent();
            scan = new Scanner();
            //Read config and load template from its content
            ReadConfig();
        }
        /// <summary>
        /// Reads template.config
        /// Navigate Browser to the .htm in template.config
        /// extract directory 
        /// set directory textbox to extracted directory
        /// </summary>
        void ReadConfig() 
        {
            StreamReader reader = new StreamReader("template.config");
            string input = reader.ReadToEnd();
            reader.Close();
            webBrowser1.Navigate(input);
            textBox12.Text = ExtractDirectoryFromFullFilePath(input);

        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                listBox2.Items[listBox2.SelectedIndex] = textBox3.Text;
            }
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                listBox3.Items[listBox3.SelectedIndex] = textBox8.Text;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;
            }
        }


        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                listBox6.Items[listBox6.SelectedIndex] = textBox9.Text;
            }
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
                textBox9.Text = listBox6.Items[listBox6.SelectedIndex].ToString();
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
             textBox1.Text  = listBox1.Items[listBox1.SelectedIndex].ToString();
        }


        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
                textBox8.Text = listBox3.Items[listBox3.SelectedIndex].ToString();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
                textBox3.Text = listBox2.Items[listBox2.SelectedIndex].ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            listBox3.Items.Clear();
            scan.ScanForGPU(ref textBox10);
            scan.ScanForOS(ref textBox4);
            scan.ScanForModel(ref textBox5, ref textBox11);
            scan.ScanForStorageDevices(ref listBox2, checkBox6.Checked, checkBox5.Checked);
            scan.ScanForCPU(ref textBox2);
            progressBar1.Value = 25;
            scan.ScanForNetworkDevices(ref listBox3);
            scan.ScanForTouchScreen(ref listBox3);
            scan.ScanForBluetooth(ref listBox3);
            progressBar1.Value = 50;
            scan.ScanForRam(ref listBox1, checkBox1.Checked, checkBox2.Checked);
            progressBar1.Value = 100;
            //  progressBar1.Value = 0;
            //     Console.ReadLine();
        }


        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox3.Items.Add("Feature");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("Ram");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add("Storage Device");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
        }

        private void button8_Click(object sender, EventArgs e)
        {


            Dictionary<string, string> toreplace = new Dictionary<string, string>();
            var date = DateTime.Now;
            var datee = date.Date;
            toreplace.Add("DATE", datee.ToShortDateString());


            toreplace.Add("CPU_TEXT", textBox2.Text);

            string dir = textBox12.Text;
            string flyernamehtm = textBox11.Text;
            string flyernamehtm_temp = textBox11.Text; flyernamehtm_temp += "_temp.htm";

            flyernamehtm += ".htm";
            //   EditorialResponse(dir, "Publication7.htm","CPU_TEXT",textBox2.Text, flyernamehtm);
            //      EditorialResponse(dir, flyernamehtm, "OS_TEXT", textBox4.Text, flyernamehtm_temp);
            toreplace.Add("OS_TEXT", textBox4.Text);
            string ram_text = "";
            foreach (string objDisk in listBox1.Items)
            {
                ram_text += " - ";
                ram_text += objDisk;
            }
            toreplace.Add("PROGRAM_TEXT", textBox9.Text);
            //     EditorialResponse(dir, flyernamehtm_temp, " RAM_TEXT", ram_text, flyernamehtm);
            toreplace.Add("RAM_TEXT", ram_text);
            //    EditorialResponse(dir, flyernamehtm, "MODEL_HERE", textBox5.Text, flyernamehtm_temp);
            toreplace.Add("MODEL_HERE", textBox5.Text);
            //    EditorialResponse(dir, flyernamehtm_temp, "PROGRAM_TEXT",textBox9.Text, flyernamehtm);


            string feature_text = "";
            int i = 0;
            foreach (string objDisk in listBox3.Items)
            {
                feature_text += objDisk;

                feature_text += ", ";

                ++i;
            }
            //    EditorialResponse(dir, flyernamehtm, "FEATURE_TEXT", feature_text, flyernamehtm_temp);
            toreplace.Add("FEATURE_TEXT", feature_text);
            //      EditorialResponse(dir, flyernamehtm_temp, "USED", textBox7.Text, flyernamehtm);
            if (radioButton3.Checked)
                toreplace.Add("USED", textBox7.Text);
            if (radioButton1.Checked)
                toreplace.Add("USED", "NEW");
            //     EditorialResponse(dir, flyernamehtm, "PRICE", textBox6.Text, flyernamehtm_temp);
            toreplace.Add("PRICE", textBox6.Text);
            string storage_text = "";

            foreach (string objDisk in listBox2.Items)
            {
                storage_text += objDisk;

                storage_text += ", ";

            }
            //   EditorialResponse(dir, flyernamehtm_temp, "STORAGE_TEXT", storage_text, flyernamehtm);
            toreplace.Add("STORAGE_TEXT", storage_text);
            //   EditorialResponse(dir, flyernamehtm, "GPU_TEXT", textBox10.Text, flyernamehtm_temp);
            toreplace.Add("GPU_TEXT", textBox10.Text);
            EditorialResponse(dir, FlyerTemplateName, toreplace, flyernamehtm);
            string str = dir; str += flyernamehtm;//flyernamehtm
            webBrowser1.Navigate(str);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox12.Text = ExtractDirectoryFromFullFilePath(openFileDialog1.FileName);
            OverwriteTemplateConfigFile(openFileDialog1.FileName);
        }
        private void button10_Click(object sender, EventArgs e)
        {//RegisteredApplications
            foreach (string s in Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("RegisteredApplications").GetValueNames())
            {
                StreamReader reader = new StreamReader("ProgramIgnoreList.Config");
                string input = reader.ReadToEnd();
                bool ignore = false;
                string[] strarr = input.Split("\r\n".ToCharArray());
                foreach (var str in strarr)
                {
                    string final = str.Replace("\r\n", "");
                    final = final.Replace("\n", "");
                    final = final.Replace("\r", "");
                    if (final != "")
                        Console.Write(final);
                    Console.Write(" ");
                    Console.Write(str);
                    Console.Write("\n");
                    if (final.Contains(s))
                    {
                        ignore = true;
                    }
                }
                if (!ignore)
                    listBox6.Items.Add(s);

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            listBox6.Items.Add("Program");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
                listBox6.Items.RemoveAt(listBox6.SelectedIndex);
            listBox6.Refresh();
        }

      private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
                textBox7.Enabled = true;
            else
                textBox7.Enabled = false;
        }
 


        public void EditorialResponse(string directory, string fileName, Dictionary<string,string> toreplace, string saveFileName)
        {
            StreamReader reader = new StreamReader(directory + fileName);
            string input = reader.ReadToEnd();

            using (StreamWriter writer = new StreamWriter(directory + saveFileName, false))
            {
                {
                    string output = input;
                    foreach (KeyValuePair<string,string> val in toreplace) 
                    {
                        if (output.Contains(val.Key))
                        {
                            output = output.Replace(val.Key, val.Value);

                       
                        }

                    }
                    writer.Write(output);

                }
                writer.Close();
                reader.Close();
            }
        }
        public void OverwriteTemplateConfigFile(string filename)
        {
   

            using (StreamWriter writer = new StreamWriter("template.config", false))
            {
                {
               
                    writer.Write(filename);
                }
                writer.Close();
     
            }
        }
        public void EditorialResponse(string directory, string fileName, string word, string replacement, string saveFileName)
        {
            StreamReader reader = new StreamReader(directory + fileName);
            string input = reader.ReadToEnd();

            using (StreamWriter writer = new StreamWriter(directory + saveFileName, true))
            {
                {
                    string output = input.Replace(word, replacement);
                    writer.Write(output);
                }
                writer.Close();
                reader.Close();
            }
        }


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            webBrowser1.Navigate(openFileDialog1.FileName);
        }

        string ExtractDirectoryFromFullFilePath(string s) 
        {
         //   OverwriteTemplateConfigFile(s);
            FlyerTemplateName = "";
            var carray = s.ToCharArray();
            Array.Reverse(carray);
            s = new string(carray);

            string t = "";
            bool found_directory = false;
            bool is_slash = false;
            foreach (char c in s)
            {
                is_slash = false;

                if (c.Equals('\\'))
                {
                    found_directory = true;
                    is_slash = true;

                }

                if (found_directory)
                {
                    if (is_slash)
                        t += '/';
                    else
                        t += c;
                }
                else
                {
                    FlyerTemplateName += c;
                }

            }
            var tarray = t.ToCharArray();
            Array.Reverse(tarray);
            t = new string(tarray);

            var FTNarray = FlyerTemplateName.ToCharArray();
            Array.Reverse(FTNarray);
            FlyerTemplateName = new string(FTNarray);
            return t;

        }
    
  

    }
}
