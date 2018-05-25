using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //string[] tesst = new string[200];
        IList<string> tesst = new List<string>();



        //for print
        PaperSize paperSize = new PaperSize("papersize", 150, 500);//set the paper size
        int totalnumber = 0;//this is for total number of items of the list or array
        int itemperpage = 0;//this is for no of item per page 



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string test = "\u0670";
             string text = System.IO.File.ReadAllText(@"amar.txt");
            
            //string text = " إِنَّ الَّذِينَ ";
            //label1.Text = text;
            text = text.Replace(" ", "");
            text = Regex.Replace(text,@"\([0-9]*\)", "");
            Console.WriteLine(text);
            char[] characters = text.ToCharArray();
            Console.WriteLine(characters.Length);
            Boolean flag = false;
            string stringToCompare = null;
            IList<PickedNode> pickedNodes = new List<PickedNode>();
            for (int i = 0; i < characters.Length; i++)
            {
                if (pickedNodes.Count == 0)
                {
                    pickedNodes.Add(new PickedNode(characters[i].ToString()));
                }
                else
                {
                    for (int j = 0; j < pickedNodes.Count; j++)
                    {
                        stringToCompare = characters[i].ToString();
                        if (stringToCompare.Equals("\u0651") || stringToCompare.Equals("\u0670") || stringToCompare.Equals("\u064E") || stringToCompare.Equals("\u0650") || stringToCompare.Equals("\u064F") || stringToCompare.Equals("\u0652") || stringToCompare.Equals("\u064B") || stringToCompare.Equals("\u064D") || stringToCompare.Equals("\u064C"))
                        {
                            if (characters[i - 1].ToString().Equals("\u0651"))
                            {
                                stringToCompare = characters[i - 2].ToString() + characters[i].ToString();
                            }
                            else
                            {
                                stringToCompare = characters[i - 1].ToString() + characters[i].ToString();
                            }
                        }
                        if (pickedNodes[j].Character.Equals(stringToCompare))
                        {
                            pickedNodes[j].Count++;
                            flag = true;
                        }
                    }
                    if (flag == false)
                    {
                        pickedNodes.Add(new PickedNode(stringToCompare));
                    }
                    flag = false;
                }
            }




            for (int i = 0; i < pickedNodes.Count; i++)
            {
                richTextBox1.Text += pickedNodes[i].Character + " " + pickedNodes[i].Count + "\n\n";
                tesst.Add( pickedNodes[i].Character + " " + pickedNodes[i].Count + "\n\n");
            }
            //label1.Text = test;
            //richTextBox1.Text = text;
            
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //e.Graphics.DrawString(tesst , new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 150, 125 );

            //Graphics graphic = e.Graphics;
            //SolidBrush brush = new SolidBrush(Color.Black);

            //Font font = new Font("Courier New", 18);

            //e.PageSettings.PaperSize = new PaperSize("A4", 850, 1100);

            //float pageWidth = e.PageSettings.PrintableArea.Width;
            //float pageHeight = e.PageSettings.PrintableArea.Height;

            //float fontHeight = font.GetHeight();
            //int startX = 40;
            //int startY = 30;
            //int offsetY = 40;

            ////for (int j = 0; j < tesst.Length/30; j++)
            ////{
            //    for (int i = 0; i < 30; i++)
            //    {
            //        graphic.DrawString("Line " + i + ":   " + tesst[i], font, brush, startX, startY + offsetY);
            //        offsetY += (int)fontHeight;

            //        if (offsetY >= pageHeight)
            //        {
            //            e.HasMorePages = true;
            //            offsetY = 0;
            //            return; // you need to return, then it will go into this function again
            //        }
            //        else
            //        {
            //            e.HasMorePages = false;
            //        }
            //    }
            //}




            float currentY = 40;// declare  one variable for height measurement
            Font font = new Font("B Nazanin", 18);
            e.Graphics.DrawString("تعداد تکرار حروف به تفکیک اعراب", font, Brushes.Black, 80, currentY);//this will print one heading/title in every page of the document
            currentY += 40;


            while (totalnumber < tesst.Count) // check the number of items
            {
                e.Graphics.DrawString(tesst[totalnumber], font, Brushes.Black, 80, currentY);//print each item
                currentY += 50; // set a gap between every item
                totalnumber += 1; //increment count by 1
                if (itemperpage < 17) // check whether  the number of item(per page) is more than 20 or not
                {
                    itemperpage += 1; // increment itemperpage by 1
                    e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added

                }

                else // if the number of item(per page) is more than 20 then add one page
                {
                    itemperpage = 0; //initiate itemperpage to 0 .
                    e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                    return;//It will call PrintPage event again

                }
            }




        }

        private void print_btn_Click(object sender, EventArgs e)
        {
            //printDialog1.Document = printDocument1;
            //if (printDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    printDocument1.Print();
            //}


            itemperpage = totalnumber = 0;
            printDialog1.Document = printDocument1;
            printDocument1.DefaultPageSettings.PaperSize = paperSize;
            //printDialog1.ShowDialog();
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }

        }
    }
    class PickedNode
    {
        string character;
        int count = 1;
        public int Count { get => count; set => count = value; }
        public string Character { get => character; set => character = value; }

        public PickedNode(string character)
        {
            this.character = character;
        }
    }
}
