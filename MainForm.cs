using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Sudoku
{
    public partial class MainForm : Form
    {

        private const int k = 3;
        public int[,] matrix = new int[k * k, k * k];
        public int[,] matrixForChange = new int[k * k, k * k];

        GameLogic gameLogic = new GameLogic();

        public MainForm() {

            InitializeComponent();
            InitializeMatrix();

            string filename = "C:\\Users\\ilinv\\Desktop\\Курсовая по ООП\\Sudoku\\Для файла с правилами\\SudokuRules.txt";
            String line;

            try {
                System.IO.StreamReader sr = new System.IO.StreamReader(filename);
                line = sr.ReadLine();

                while (line != null) {
                    textBox1.Text += line;
                    textBox1.Text += Environment.NewLine;
                    line = sr.ReadLine();
                }

                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e) {
                Console.WriteLine("Exception: " + e.Message);
            }

            gameLogic.GenerateMatrix(matrix, k);
            gameLogic.RefreshMatrix(matrix, k);

            CreateMatrix(); 
        }

        private void CreateMatrix() {
            int tmp = 1;
            for (int i = 1; i < (k * k) + 1; i++) {
                for (int j = 1; j < (k * k) + 1; j++) {
                    panel1.Controls["button" + tmp].Text = matrix[i - 1, j - 1].ToString();
                    panel1.Controls["button" + tmp].Enabled = false;
                    panel1.Controls["button" + tmp].BackColor = Color.White;
                    tmp++;
                }
            }
        }
        
		private void NewGameButton_Click(object sender, EventArgs e) {

            gameLogic.GenerateMatrix(matrix, k);           
            gameLogic.RefreshMatrix(matrix, k);
            CreateMatrix();

            for (int i = 1; i < (k * k) + 1; i++) {
                for (int j = 1; j < (k * k) + 1; j++) {
                     matrixForChange[i - 1, j - 1] = matrix[i - 1, j - 1]; 
                }
            }

            if (comboBox1.SelectedItem != null) {
                gameLogic.HideButtonsSwitch(matrixForChange, k, gameLogic.KeyObtaining(comboBox1.SelectedItem.ToString()));                   
            }
            else {
                gameLogic.HideButtonsSwitch(matrixForChange, k, 0);
            }

            int tmp = 1;
            for (int i = 1; i < (k * k) + 1; i++) {
                for (int j = 1; j < (k * k) + 1; j++) {
                   if (matrixForChange[i - 1, j - 1] == 0) {
                        panel1.Controls["button" + tmp].Enabled = true;
                        panel1.Controls["button" + tmp].Text = ""; 
                    }
                    tmp++;
                }
            }
        }

        private void InitializeMatrix() {
            int tmp_G = 1;
            for (int i = 1; i < (k * k) + 1; i++) {
                for (int j = 1; j < (k * k) + 1; j++) {
                    panel1.Controls["button" + tmp_G].Click += gameLogic.PressedButton;
                    tmp_G++;
                }
            }
        }

		private void WinnerCheckButton_Click(object sender, EventArgs e) {

            int tmp_G = 1;
            for (int i = 1; i < (k * k) + 1; i++) {
                for (int j = 1; j < (k * k) + 1; j++) {
                    if (panel1.Controls["button" + tmp_G].Text != "") {
                        matrixForChange[i - 1, j - 1] = int.Parse(panel1.Controls["button" + tmp_G].Text);
                        tmp_G++;
                    }
                    else {
                        matrixForChange[i - 1, j - 1] = 0;
                        tmp_G++;
                    }  
                }
            }

            int tmp_G2 = 1;
            int countTmp = 0;

            for (int i = 1; i < (k * k) + 1; i++) {
                for (int j = 1; j < (k * k) + 1; j++) {

                    panel1.Controls["button" + tmp_G2].Enabled = false;

                    if (matrix[i - 1, j - 1] != matrixForChange[i - 1, j - 1]) {
                        panel1.Controls["button" + tmp_G2].Enabled = true;
                        panel1.Controls["button" + tmp_G2].BackColor = Color.Coral;
                        countTmp++;
                    }
                    tmp_G2++;
                }
            }

            if (countTmp != 0) {
                MessageBox.Show("Судоку решено неверно, но вы можете исправить ошибки");
            }
			else {
                MessageBox.Show("Поздравляю! Судоку решено верно");
            }
        }
	}
}
