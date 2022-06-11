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

    public class GameLogic {

        public void GenerateMatrix(int[,] matrix, int k) {

            for (int i = 0; i < (k * k); i++) {
                for (int j = 0; j < (k * k); j++) {
                    matrix[i, j] = ((i * k) + (i / k) + j) % (k * k) + 1;
                }
            }
        }

        public void TranspositionMatrix(int[,] matrix, int k) {

            int[,] tMatrix = new int[k * k, k * k];

            for (int i = 0; i < (k * k); i++) {
                for (int j = 0; j < (k * k); j++) {
                    tMatrix[i, j] = matrix[j, i];
                }
            }

			for (int i = 0; i < (k * k); i++) {
				for (int j = 0; j < (k * k); j++) {
					matrix[i, j] = tMatrix[i, j];
				}
			}
        }

        public void SwapRowsInBlock(int[,] matrix, int k) {

            Random r = new Random(Guid.NewGuid().GetHashCode());

            var block = r.Next(0, k);
            var row1 = r.Next(0, k);
            var row2 = r.Next(0, k);
            while (row1 == row2) {
                row2 = r.Next(0, k);
            }

            var line1 = block * k + row1;
            var line2 = block * k + row2;
            
            for (int i = 0; i < k * k; i++) {
                var tmp = matrix[line1, i];
                matrix[line1, i] = matrix[line2, i];
                matrix[line2, i] = tmp;
			}
        }

        public void SwapColumnsInBlock(int[,] matrix, int k) {

            Random r = new Random(Guid.NewGuid().GetHashCode());

            var block = r.Next(0, k);
            var row1 = r.Next(0, k);
            var row2 = r.Next(0, k);
            while (row1 == row2) {
                row2 = r.Next(0, k);
            }

            var line1 = block * k + row1;
            var line2 = block * k + row2;

            for (int i = 0; i < k * k; i++) {
                var tmp = matrix[i, line1];
                matrix[i, line1] = matrix[i, line2];
                matrix[i, line2] = tmp;
            }
        }

        public void SwapBlocksInRow(int[,] matrix, int k) {

            Random r = new Random(Guid.NewGuid().GetHashCode());

            var block1 = r.Next(0, k);
            var block2 = r.Next(0, k);
            
            while (block1 == block2) {
                block2 = r.Next(0, k);
            }

            block1 *= k;            
            block2 *= k;            

            for (int i = 0; i < k * k; i++) {
                var blockTmp = block2;
                for (int j = block1; j < block1 + k; j++) {
                    var tmp = matrix[j, i];
                    matrix[j, i] = matrix[blockTmp, i];
                    matrix[blockTmp, i] = tmp;
					blockTmp++;
				}
			}
        }

        public void SwapBlocksInColumn(int[,] matrix, int k) {

            Random r = new Random(Guid.NewGuid().GetHashCode());

            var block1 = r.Next(0, k);
            var block2 = r.Next(0, k);

            while (block1 == block2) {
                block2 = r.Next(0, k);
            }

            block1 *= k;            
            block2 *= k;            

            for (int i = 0; i < k * k; i++) {
                var blockTmp = block2;
                for (int j = block1; j < block1 + k; j++) {
                    var tmp = matrix[i, j];
                    matrix[i, j] = matrix[i, blockTmp];
                    matrix[i, blockTmp] = tmp;
                    blockTmp++;
                }
            }          
        }

        public void RefreshMatrixSwitch(int i, int[,] matrix, int k) {

			switch (i) {

                case 0:
                    TranspositionMatrix(matrix, k);
                    break;

                case 1:
                    SwapRowsInBlock(matrix, k);
                    break;

                case 2:
                    SwapColumnsInBlock(matrix, k);
                    break;

                case 3:
                    SwapBlocksInRow(matrix, k);
                    break;

                case 4:
                    SwapBlocksInColumn(matrix, k);
                    break;

                default:
                    TranspositionMatrix(matrix, k);
                    break;
            }
		}

        public void RefreshMatrix(int[,] matrix, int k) {

            Random r = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < 30; i++) {
                RefreshMatrixSwitch(r.Next(0,5), matrix, k);
			}
        }

        public void PressedButton(object sender, EventArgs e) {

            Button pressedButton = sender as Button;
            string buttonText = pressedButton.Text;

            if (string.IsNullOrEmpty(buttonText)) {
                pressedButton.Text = "1";
			}
            else {
                int num = int.Parse(buttonText);
                num++;

                if(num == 10) {
                    num = 1;
				}
                pressedButton.Text = num.ToString();
			}
		}
        
        public void HideButtonsSwitch(int[,] matrix, int k, int key) {

            switch(key) {
                case 0:
                    HideButtons(matrix, k, 30);
                    break;

                case 1:
                    HideButtons(matrix, k, 40);
                    break;

                case 2:
                    HideButtons(matrix, k, 50);
                    break;
                case 3:
                    HideButtons(matrix, k, 5);
                    break;
                default:
                    HideButtons(matrix, k, 30);
                    break;
            }
		}

        public void HideButtons(int[,] matrix, int k, int difficultValue) {

            Random r = new Random(Guid.NewGuid().GetHashCode());
            int difficultValueTmp = difficultValue;

            while (difficultValueTmp > 0) {
                for (int i = 0; i < (k * k); i++) {
                    for (int j = 0; j < (k * k); j++) {

                        int aNull = r.Next(0,3);

                        if (aNull == 0) {
                            matrix[i, j] = 0;
                            difficultValueTmp--;
                        }

						if (difficultValueTmp == 0) {
                            break;
						}
                    }

                    if (difficultValueTmp == 0) {
                        break;
                    }
                }
            }
        }

        public int KeyObtaining(string difficult) {

            switch(difficult) {
                case "Средний":
                    return 0;

                case "Сложный":
                    return 1;

                case "Профессионал":
                    return 2;

                case "Тренировочный":
                    return 3;

                default:
                    return 0;
            }
		}
    }
}
