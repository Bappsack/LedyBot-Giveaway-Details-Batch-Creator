using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using PKHeX.Core;

namespace LedyBot_Giveaway_Details_Batch_Creator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] Files = Directory.GetFiles(textBox1.Text, "*.pk7", SearchOption.AllDirectories);
            WriteXML(Files,textBox1.Text);
        }


        private void WriteXML(string[] Files,string Dir)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = true
            };

            richTextBox1.AppendText("Open Folder: " + Dir + " with " + Files.Count() + " Files\n");

            XmlWriter xml = XmlWriter.Create(Directory.GetCurrentDirectory() + @"\GiveawayDetails.xml", settings);

            xml.WriteProcessingInstruction("xml", "version='1.0' standalone='yes'");

            xml.WriteStartElement("DocumentElement");

            List<int> DexList = new List<int>();
            int Count = 1;
            foreach (var file in Files)
            {
                if (file.EndsWith(".pk7"))
                {
                   
                    PKM pk7 = PKMConverter.GetPKMfromBytes(File.ReadAllBytes(file));
                    if (DexList.Contains(pk7.Species))
                    {
                        continue;
                    }
                    DexList.Add(pk7.Species);
                    xml.WriteStartElement("Giveaway_x0020_Details");
                    xml.WriteElementString("Dex_x0020_Number", pk7.Species.ToString());
                    xml.WriteElementString("Specific_x0020_Path", file);
                    xml.WriteElementString("Optional_x0020_Path", file.Replace(Path.GetFileNameWithoutExtension(file) + ".pk7",""));
                    xml.WriteElementString("Gender_x0020_Index", GetGenderIndex(pk7.Gender).ToString());
                    xml.WriteElementString("Level_x0020_Index", GetLevelIndex(pk7.CurrentLevel).ToString());
                    xml.WriteElementString("Count", "-1");
                    xml.WriteElementString("Traded", "0");
                    xml.WriteEndElement();

                    Count++;

                    richTextBox1.AppendText("Processing File(" + Count + "): " + file + "\n");
                    richTextBox1.ScrollToCaret();
                }


            }
            xml.WriteEndDocument();
            xml.Close();
            richTextBox1.AppendText("\n\nDone, added " + Count.ToString() + " Pokemon to Giveaway Details.\n GiveawayDetails.xml is located here: \n\n" + Directory.GetCurrentDirectory() + @"\GiveawayDetails.xml");
            richTextBox1.ScrollToCaret();
            
        }



        private int GetGenderIndex(int Gender)
        {
            if (Gender == 0) { Gender = 1; }
            else if (Gender == 1) { Gender = 2; }
            else if (Gender == 2) { Gender = 1; }

            return Gender;
        }

        private int GetLevelIndex(int Level)
        {

            if (Level == 100) { Level = 0; }
            else if (Level >= 91 && Level != 100) { Level = 10; }
            else if (Level >= 81 && Level < 91) { Level = 9; }
            else if (Level >= 71 && Level < 81) { Level = 8; }
            else if (Level >= 61 && Level < 71) { Level = 7; }
            else if (Level >= 51 && Level < 61) { Level = 6; }
            else if (Level >= 41 && Level < 51) { Level = 5; }
            else if (Level >= 31 && Level < 41) { Level = 4; }
            else if (Level >= 21 && Level < 31) { Level = 3; }
            else if (Level >= 11 && Level < 21) { Level = 2; }
            else if (Level >= 0 && Level < 11) { Level = 1; }
            return Level;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (fbd.SelectedPath != "")
                {
                    textBox1.Text = fbd.SelectedPath;
                }
            }

        }
    }
}
