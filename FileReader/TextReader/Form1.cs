using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;


namespace TextReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            int numberOfLines = 0, numberOfWords = 0, numberOfPage = 0;
            string[] file;
            ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
            
            file = this.txtPath.Text.Split('.');

            if (!File.Exists(this.txtPath.Text))
                MessageBox.Show("File doesn't exist.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (!file[file.Length - 1].Equals("pdf"))
                MessageBox.Show("File should be in PDF format.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                using (PdfReader reader = new PdfReader(this.txtPath.Text))
                {
                    //StringBuilder text = new StringBuilder();
                    numberOfPage = reader.NumberOfPages;
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        string thePage = PdfTextExtractor.GetTextFromPage(reader, i, its);
                        char[] splitter = new char[] { ' ', ',', '/', '.', '\t' };
                        string[] theLines = thePage.Split('\n');
                        string[] theWords = thePage.Split(' ');

                        numberOfLines = numberOfLines + theLines.Length;

                        for (int j = 0; j < theWords.Length; j++)
                        {
                            if (!theWords[j].Equals(""))
                            {
                                byte[] asciiBytes = Encoding.ASCII.GetBytes(theWords[j]);
                                bool flag = true;

                                for(int k = 0; k < asciiBytes.Length; k++)
                                {
                                    //Check if letter
                                    if (!((asciiBytes[k] >= 65 && asciiBytes[k] <= 90) || (asciiBytes[k] >= 97 && asciiBytes[k] <= 122)))
                                    {
                                        //If value is not a letter and found in last position of a word 
                                        if (k != asciiBytes.Length - 1)
                                        {
                                            flag = false;
                                            k = asciiBytes.Length;
                                        }
                                    }
                                }

                                if(flag)
                                    numberOfWords++;
                            }
                                
                        }
                    }
                }
                this.lblNumberOfPages.Text = numberOfPage.ToString();
                this.lblNumberOfLines.Text = numberOfLines.ToString();
                this.lblNumberOfWords.Text = numberOfWords.ToString();
            }  
        }
    }
}
