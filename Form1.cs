using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.IO;

namespace TheEndGame2
{
    public partial class DataBase : Form
    {
        Excel.Application excelApp;
        Excel.Workbook workBook;
        Excel.Worksheet workSheet;
        int freeRow;
        int columnIndex;
        bool flagOfRowHeaderValue = false;
        string password;
        bool flag = false;
        bool isCreateBook = false;




        public DataBase()
        {
            InitializeComponent();
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel file (*.xls)|*.xls";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (excelApp == null)
                {
                    excelApp = new Excel.Application();
                }
                workBook = excelApp.Workbooks.Open(openFileDialog1.FileName);
                workSheet = workBook.Worksheets.get_Item(1);


                freeRow = workSheet.Cells[workSheet.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row + 1;

                dataGridView1.ReadOnly = true;
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                if (flag == true)
                    dataGridView1.ReadOnly = false;
                toolStripTextBox1.Enabled = true;
                PrizesCountryToolStripMenuItem.Enabled = true;
                MedalsCountryToolStripMenuItem.Enabled = true;
                PrizesSportToolStripMenuItem.Enabled = true;
                Column8.Visible = true;
                dataGridView2.Visible = false;
                SaveFileButton.Enabled = true;
                SaveAsFileButton.Enabled = true;
                toolStripTextBox3.Enabled = true;
                isCreateBook = false;
                f5.Enabled = true;
                for (int i = 2; i < freeRow; i++)
                {
                    dataGridView1.Rows.Add(workSheet.Cells[i, 1].Text, workSheet.Cells[i, 2].Text, workSheet.Cells[i, 3].Text, workSheet.Cells[i, 4].Text, workSheet.Cells[i, 5].Text, workSheet.Cells[i, 6].Text, workSheet.Cells[i, 7].Text);
                }

                this.flagOfRowHeaderValue = true;
            }
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    if (dataGridView1[j - 1, i].Value != null)
                        workSheet.Cells[dataGridView1.Rows[i].HeaderCell.Value, j] = dataGridView1[j - 1, i].Value.ToString();
                }
            }
            excelApp.ActiveWorkbook.Save();
        }

        private void SaveAsFileButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    if (dataGridView1[j - 1, i].Value != null)
                        workSheet.Cells[dataGridView1.Rows[i].HeaderCell.Value, j] = dataGridView1[j - 1, i].Value.ToString();
                }
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel file (*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                excelApp.ActiveWorkbook.SaveAs(saveFileDialog1.FileName, Excel.XlSaveAsAccessMode.xlNoChange);
            }
        }

        private void CreateNewButton_Click(object sender, EventArgs e)
        {
            if (excelApp == null)
            {
                excelApp = new Excel.Application();
                workBook = excelApp.Workbooks.Add();
                workSheet = workBook.Worksheets.get_Item(1);
            }
            


            //Заполняем основные поля
            workSheet.Cells[1, 1] = "Номер";
            workSheet.Cells[1, 2] = "Фамилия";
            workSheet.Cells[1, 3] = "Имя";
            workSheet.Cells[1, 4] = "Отчество";
            workSheet.Cells[1, 5] = "Страна";
            workSheet.Cells[1, 6] = "Дисциплина";
            workSheet.Cells[1, 7] = "Место";
            //--------------------------------------

            excelApp.Columns.ColumnWidth = 30; // Указываем ширину ячейки

            freeRow = workSheet.Cells[workSheet.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row + 1;
           
            SaveAsFileButton.Enabled = true;
            dataGridView1.Rows.Clear();            
            dataGridView1.ReadOnly = false;
            isCreateBook = true;
            toolStripTextBox3.Enabled = PrizesCountryToolStripMenuItem.Enabled = PrizesSportToolStripMenuItem.Enabled = MedalsCountryToolStripMenuItem.Enabled = false;
            this.flagOfRowHeaderValue = true;
            f5.Enabled = false;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (flagOfRowHeaderValue == true)
            {
                object head = this.dataGridView1.Rows[e.RowIndex].HeaderCell.Value;
                if (head == null || !head.Equals((e.RowIndex + 2).ToString()))
                    this.dataGridView1.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 2).ToString();
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
        } 

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (columnIndex == 0 || columnIndex == 6)
            {
                if (Char.IsDigit(e.KeyChar) || e.KeyChar == (char)8) return;
                else e.Handled = true;
            }
            else if (columnIndex > 1 || columnIndex < 6)
            {
                if (Char.IsLetter(e.KeyChar) || e.KeyChar == (char)8) return;
                else e.Handled = true;
            }
            else if (columnIndex == 1)
            {
                if (Char.IsLetter(e.KeyChar) || e.KeyChar == (char)8 || e.KeyChar == '-') return;
                else e.Handled = true;
            }
        }        

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.columnIndex = e.ColumnIndex;            
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = "";
            toolStripTextBox1.ForeColor = System.Drawing.Color.Black;
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || e.KeyChar == (char)8 || e.KeyChar == '-') return;
            else e.Handled = true;
            if (e.KeyChar == (char)13)
            {
                this.flagOfRowHeaderValue = false;
                int index = 0;
                dataGridView1.Rows.Clear();
                dataGridView2.Visible = false;
                for (int i = 2; i < freeRow; i++)
                { 
                   if (workSheet.Cells[i, 2].Text.ToLower() == toolStripTextBox1.Text.ToLower())
                   {
                        dataGridView1.Rows.Add(workSheet.Cells[i, 1].Text, workSheet.Cells[i, 2].Text, workSheet.Cells[i, 3].Text, workSheet.Cells[i, 4].Text, workSheet.Cells[i, 5].Text, workSheet.Cells[i, 6].Text, workSheet.Cells[i, 7].Text);
                        dataGridView1.Rows[index].HeaderCell.Value = i.ToString();
                        index++;
                   }
                }               
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (flag == true)
            {
                if (e.ColumnIndex == 7)
                {
                    workSheet.Rows[dataGridView1.Rows[e.RowIndex].HeaderCell.Value].Delete();
                    dataGridView1.Rows.Clear();
                    this.flagOfRowHeaderValue = true;
                    freeRow--;
                    for (int i = 2; i < freeRow; i++)
                    {
                        dataGridView1.Rows.Add(workSheet.Cells[i, 1].Text, workSheet.Cells[i, 2].Text, workSheet.Cells[i, 3].Text, workSheet.Cells[i, 4].Text, workSheet.Cells[i, 5].Text, workSheet.Cells[i, 6].Text, workSheet.Cells[i, 7].Text);
                    }
                }
            }
        }

        private void PrizesSportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flagOfRowHeaderValue = false;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Visible = false;
            List<Excel.Range> winner = new List<Excel.Range>();
            List<string> sportNames = new List<string>();
            List<int> index = new List<int>();
            int indexOfRows = 0;
            for (int i = 2; i < freeRow; i++)
            {
                if (!sportNames.Contains(workSheet.Cells[i, 6].Text))
                {
                    sportNames.Add(workSheet.Cells[i, 6].Text);
                }
                if (workSheet.Cells[i, 7].Text == "1" || workSheet.Cells[i, 7].Text == "2" || workSheet.Cells[i, 7].Text == "3")
                {
                    winner.Add(workSheet.Rows[i]);
                    index.Add(i);
                }
            }
            for (int i = 0; i < sportNames.Count; i++)
            {
                for (int j = 0; j < winner.Count; j++)
                {
                        if (sportNames[i] == winner[j].Cells[1, 6].Text)
                        {
                            dataGridView1.Rows.Add(winner[j].Cells[1, 1].Text, winner[j].Cells[1, 2].Text, winner[j].Cells[1, 3].Text, winner[j].Cells[1, 4].Text, winner[j].Cells[1, 5].Text, winner[j].Cells[1, 6].Text, winner[j].Cells[1, 7].Text);
                            dataGridView1.Rows[indexOfRows].HeaderCell.Value = index[0].ToString();
                            index.RemoveAt(0);
                            indexOfRows++;
                        }
                }
            }

        }

        private void MedalsCountryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int gold = 0;
            int silver = 0;
            int bronze = 0;
            int leave = 0;
            dataGridView2.Visible = true;                       
            List<string> country = new List<string>();
            List<Excel.Range> winner = new List<Excel.Range>();
            for (int i = 2; i < freeRow; i++)
            {
                if (!country.Contains(workSheet.Cells[i, 5].Text))
                    country.Add(workSheet.Cells[i, 5].Text);
                if (workSheet.Cells[i, 7].Text == "1" || workSheet.Cells[i, 7].Text == "2" || workSheet.Cells[i, 7].Text == "3")
                    winner.Add(workSheet.Rows.Range["E" + i.ToString(), "G" + i.ToString()]);
            }
            for (int i = 0; i < country.Count; i++)
            {
                gold = silver = bronze = 0;
                for (int j = 0; j < winner.Count; j++)
                {
                    string t = winner[j].Cells[1, 3].Text;
                    if (winner[j].Cells[1, 1].Text == country[i])
                        gold += t == "1" ? 1 : t == "2" ? ++silver - silver : t == "3" ? ++bronze - bronze : ++leave - leave;
                }
                dataGridView2.Rows.Add(country[i], gold, silver, bronze);
            }
        }

        private void toolStripTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (Char.IsLetter(e.KeyChar) || e.KeyChar == (char)8) return;
            else e.Handled = true;
            if (e.KeyChar == (char)13)
            {
                List<int> index = new List<int>();
                this.flagOfRowHeaderValue = false;
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView2.Visible = false;
                string country = toolStripTextBox2.Text;
                List<Excel.Range> array = new List<Excel.Range>();
                for (int i = 2; i < freeRow; i++)
                    if (workSheet.Cells[i, 5].Text.ToLower() == country.ToLower() && (workSheet.Cells[i, 7].Text == "1" || workSheet.Cells[i, 7].Text == "2" || workSheet.Cells[i, 7].Text == "3"))
                    {
                        array.Add(workSheet.Rows[i]);
                        index.Add(i);
                    }
                if (array.Count != 0)
                    for (int i = 0; i < array.Count; i++)
                    {
                        dataGridView1.Rows.Add(array[i].Cells[1,1].Text, array[i].Cells[1, 2].Text, array[i].Cells[1, 3].Text, array[i].Cells[1, 4].Text, array[i].Cells[1, 5].Text, array[i].Cells[1, 6].Text, array[i].Cells[1, 7].Text);
                        dataGridView1.Rows[i].HeaderCell.Value = index[0].ToString();
                        index.RemoveAt(0);
                    }
                else MessageBox.Show("Эта страна не участвовала или не заработала медалей");

            }

        }

        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {
            toolStripTextBox3.Text = "";
            toolStripTextBox3.ForeColor = System.Drawing.Color.Black;
        }

        private void toolStripTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (toolStripTextBox3.Text == password)
                {
                    dataGridView1.ReadOnly = false;
                    toolStripTextBox3.Enabled = false;
                    flag = true;
                    toolStripTextBox3.Text = "";
                    MessageBox.Show("Разрешение на редактирование данных получено");
                }
                else MessageBox.Show("Не верный пароль!");
            }
        }

        private void DataBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                workBook.Close(false);
                excelApp.Quit();
            }
            catch
            {
                
            }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            for (int i = 2; i < freeRow; i++)
            {
                dataGridView1.Rows.Add(workSheet.Cells[i, 1].Text, workSheet.Cells[i, 2].Text, workSheet.Cells[i, 3].Text, workSheet.Cells[i, 4].Text, workSheet.Cells[i, 5].Text, workSheet.Cells[i, 6].Text, workSheet.Cells[i, 7].Text);
            }
            this.flagOfRowHeaderValue = false;

        }

        private void pass_Click(object sender, EventArgs e)
        {
            pass.Text = "";
            pass.ForeColor = System.Drawing.Color.Black;
        }

        private void pass1_Click(object sender, EventArgs e)
        {
            pass1.Text = "";
            pass1.ForeColor = System.Drawing.Color.Black;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
                if (pass.Text == "")
                    MessageBox.Show("Введите текущий пароль!");
                else if (pass1.Text == "")
                    MessageBox.Show("Введите новый пароль!");
                else if (pass.Text != "" && pass1.Text != "")
                    if (password == pass.Text)
                    {
                        using (StreamWriter sw = new StreamWriter("pass.txt", false))
                        {
                            password = pass1.Text;
                            sw.WriteLine(pass1.Text);                            
                        }
                        MessageBox.Show("Вы сменили пароль");
                    }
        }

        private void DataBase_Load(object sender, EventArgs e)
        {
            using (StreamReader sr = new StreamReader("pass.txt"))
            {
                password = sr.ReadLine();
            }
        }
    }
}
