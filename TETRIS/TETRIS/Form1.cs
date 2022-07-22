using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TETRIS
{
    public partial class Form1 : Form
    {
        Shape currentShape;
        Shape nextShape;
        Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();
            loadCanvas();

            currentShape = getRandomShapeWithCenterAligned();
            nextShape = getNextShape();

            timer.Tick += Timer_Tick;
            timer.Interval = 500;
            timer.Start();

            this.KeyDown += Form1_KeyDown;
        }

        Bitmap canvasBitmap;
        Graphics canvasGraphics;
        int canvasWidth = 15;
        int canvasHeight = 20;
        int[,] canvasDotArray;
        int dotSize = 20;

        //Step2
        private void loadCanvas()
        {
            // Rezise the picture box based on the dotsize and canvas size
            pictureBox1.Width = canvasWidth * dotSize;
            pictureBox1.Height = canvasHeight * dotSize;

            //Create Bitmap with picture box's size
            canvasBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            canvasGraphics = Graphics.FromImage(canvasBitmap);

            canvasGraphics.FillRectangle(Brushes.LightGray, 0, 0, canvasBitmap.Width, canvasBitmap.Height);

            // Load bitmap into picture box
            pictureBox1.Image = canvasBitmap;

            // Initialize canvas dot array. By default all elements are zero
            canvasDotArray = new int[canvasWidth, canvasHeight];

        }
        int currentX;
        int currentY;
        private Shape getRandomShapeWithCenterAligned()
        {
            var shape = ShapesHandler.GetRandomShape();

            // Calculate the X and Y values as if the shape lies in the center
            currentX = 7;
            currentY = -shape.Height;

            return shape;
        }
        Bitmap workingBitmap;
        Bitmap workingGraphics;

        private Timer_Tick(object sender, EventArgs e)
        {
            var isMoveSucces = moveShapeIfPossible(moveDown: 1);

            // If shape reached bottom or touched any other shapes
            if (!isMoveSucces)
            {

                // Copy working image into canvas image
                canvasBitmap = new Bitmap(workingBitmap);

                updateCanvasDotArrayWithCurrentShape();

                // Get next shape
                currentShape = nextShape;
                nextShape = getNextShape();

                clearFilledRowsAndUpdateScrore();
               
            }

        }
        /// <summary>
        /// https://github.com/learnfromanver/tetris-brick-game/blob/master/Tetris/Tetris/Form1.cs
        /// </summary>
        private void updateCanvasDotArrayWithCurrentShape()
        {
            for (int i = 0; i < currentShape.Widht; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Dots[j, i] == 1)
                    {
                        checkIfGameOver();

                        canvasDotArray[currentX + i, currentY + j] = 1;
                    }
                }
            }
        }
        private void checkIfGameOver()
        {
            if (currentY < 0)
            {
                timer.Stop();
                MessageBox.Show("Game Over");
                Application.Restart();
            }
        }
        // Return if it reaches the bottom or touches any other blocks
        private bool moveShapeIfPossible(int moveDown = 0, int moveSide = 0)
        {
            var newX = currentX + moveSide;
            var newY = currentY + moveDown;

            //Check if it reaches the bottom or side bar
            if (newX < 0 || newX + currentShape.Width > canvasWidth || newY + currentShape.Height > canvasHeight)
                return false;
            // Check if it touches any other blocks
            for (int i = 0; i < currentShape.Width; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (newY + j > 0 && canvasDotArray[newX + 1, newY + j] == 1 && currentShape.Dots[j, i] == 1)
                        return false;
                }
            }
            currentX = newX;
            currentY = newY;

            drawShape();

            return true;
        }
        private void drawShape()
        {
            workingBitmap = new Bitmap(canvasBitmap);
            workingGraphics = Graphics.FromImage(workingBitmap);

            for (int i = 0; i < currentShape.Widht; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Dots[j, i] == 1)
                        workingGraphics.FillRectangle(Brushes.Black, (currentX + 1) * dotSize, (currentY + j) * dotSize, dotSize, dotSize);
                }
            }
            pictureBox1.Image = workingBitmap;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var verticalMove = 0;
            var horizontalMove = 0;

            // Calculate the vertical and horizontal move values
            // Based on the key pressed
            switch (e.KeyCode)
            {
                // Move shape left
                case Keys.Left:
                    verticalMove--;
                    break;
                // Move shape right
                case Keys.Right:
                    verticalMove++;
                    break;
                // Move shape down faster
                case Keys.Down:
                    horizontalMove++;
                    break;
                // Rotate the shape clockwise
                case Keys.Up:
                    currentShape.turn();
                    break;
                default:
                    return;
            }
            var usMoveSucces = moveShapeIfPossible(horizontalMove, verticalMove);

            // If the player was trying to rotate the shape, but
            // that move was not possible - rollback the shape

            if (!isMoveSucces && e.KeyCode == Keys.Up)
                currentShape.rollBack();
        }
        // line 201
        int score;

        public void clearFilledRowsAndUpdateScore()
        {
            // Check through each row
            for (int i = 0; i < canvasHeight; i++)
            {
                int j;
                for(j = canvasWidth - 1; j >= 0; j--)
                {
                    if (canvasDotArray[j, i] == 0)
                    {
                        break;
                    }
                    if (j == 1)
                    {
                        // Update score and level values and labels
                        score++;
                        label1.Text = "Score: " + score;
                        label2.Text = "Level: " + score / 10;

                        // Increase the speed
                        timer.Interval -= 10;

                        // Update the dot array based on the check
                        for (j = 0; j < canvasWidth; j++)
                        {
                            for(int k = i; k > 0; k--)
                            {
                                canvasDotArray[j, k] = canvasDotArray[j, k - 1];
                            }
                            canvasDotArray[j, 0] = 0;
                        }
                    }
                }
                // 236
            }
        }

    }
}