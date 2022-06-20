using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Tetris_OOP;

namespace TetrisApp
{
    internal class Program
    {   //Settings
        static int TetrisRows = 20;
        static int TetrisCols = 10;
        static int InfoCols = 10;
        static int ConsoleRows = 1 + TetrisRows + 1;
        static int ConsoleCols = 1 + TetrisCols + 1 + InfoCols + 1;
        static List<bool[,]> TetrisFigures = new List<bool[,]>()
        {
            new bool[,]
            {
                {true,true,true,true }
            }, // ----
            new bool[,]
            {
                {true,true },
                { true,true}
            },  // O
            new bool[,]
            {
                {false,true,false },
                {true,true,true }
            }, //T
            new bool[,]
            {
                { false,true,true},
                { true,true,false}
            }, //S
            new bool[,]
            {
                {true,true,false},
                {false,true,true}
            }, //Z
            new bool[,]
            {
                {true,false,false},
                {true,true,true}
            }, //J
            new bool[,]
            {
                {false,false,true},
                {true,true,true}
            }, //L
                        
        };
        static int[] ScorePerLines = new int[] { 0, 40, 100, 300, 1200 };


        //Current State:
        static TetrisGameState State = new TetrisGameState(TetrisRows,TetrisCols);
       
        static Random RandomIndex = new Random();

        static void Main(string[] args)
        {

            MusicPlayer musicPlayer = new MusicPlayer();
            musicPlayer.Play();

            ScoreManager scoreManager = new ScoreManager("score.txt");
            State.HighScore = scoreManager.GetHighScore();

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.Title = "Tetris App 1.0";
            Console.WindowHeight = ConsoleRows + 1; //Dimentions of console.
            Console.WindowWidth = ConsoleCols;
            Console.BufferHeight = ConsoleRows + 1; //For scrows of console
            Console.BufferWidth = ConsoleCols;
            Console.CursorVisible = false;
            State.CurrentFigure = TetrisFigures[RandomIndex.Next(0, TetrisFigures.Count)];

            DrawBorder();


            while (true)
            {
                State.Frame++;
                State.UpdateMethod();
                
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey();
                    if (pressedKey.Key == ConsoleKey.Escape)
                    {
                        Console.SetCursorPosition(0, 0);
                        Write("_Thanks for playing", TetrisRows / 2, TetrisCols / 2);
                        return;
                    }
                    if (pressedKey.Key == ConsoleKey.LeftArrow)
                    {
                        if (State.CurrentFigureCol >= 1)  //If figure reaches left end it cant go more left
                        {
                            State.CurrentFigureCol--;
                        }
                    }
                    if (pressedKey.Key == ConsoleKey.RightArrow)  //If figure reaches right end it cant go more right
                    {
                        if (State.CurrentFigureCol < TetrisCols - State.CurrentFigure.GetLength(1))
                        {
                            State.CurrentFigureCol++;
                        }
                    }
                    if (pressedKey.Key == ConsoleKey.DownArrow)
                    {
                        State.Frame = 1;
                        State.Score += State.Level;
                        State.CurrentFigureRow++;

                    }
                    if (pressedKey.Key == ConsoleKey.Spacebar)
                    {
                        RotateCurrentFugure();
                    }

                }
                // user input



                //Update the game state
                if (State.Frame % (State.FramesToMoveFigure - State.Level) == 0)
                {
                    State.CurrentFigureRow++;
                    State.Frame = 0;

                }
                if (Collision(State.CurrentFigure))
                {
                    AddCurrentFigureToTetrisFIeld();
                    int lines = CheckForFullLines();
                    State.Score += ScorePerLines[lines] * State.Level;

                    State.CurrentFigure = TetrisFigures[RandomIndex.Next(0, TetrisFigures.Count)];  //If collision than make new random figure with row=0 & col=0
                    State.CurrentFigureCol = 0;
                    State.CurrentFigureRow = 0;
                    if (Collision(State.CurrentFigure)) //Game is over
                    {

                        scoreManager.Add(State.Score);
                        
                        string finalScore = State.Score.ToString();
                        finalScore += new string(' ', 5 - finalScore.Length);
                        Write("╔══════════════╗", 5, 5);
                        Write("║ GAME         ║", 6, 5);
                        Write("║   OVER!      ║", 7, 5);
                        Write("║     SCORE:   ║", 8, 5);
                        Write($"║         {finalScore}║", 9, 5);
                        Write("╚══════════════╝", 10, 5);

                        Thread.Sleep(100);
                        return;
                    }
                }


                //redraw UI
                DrawBorder();
                DrawInfo();

                DrawTetrisField();
                DrawCurrentFigure();

                Thread.Sleep(40);
            }
        }

       

        private static void RotateCurrentFugure()
        {
            var newFIgure = new bool[State.CurrentFigure.GetLength(1), State.CurrentFigure.GetLength(0)];
            for (int row = 0; row < State.CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < State.CurrentFigure.GetLength(1); col++)
                {
                    newFIgure[col, State.CurrentFigure.GetLength(0) - row - 1] = State.CurrentFigure[row, col];
                }
            }

            if (!Collision(newFIgure))
            {
                State.CurrentFigure = newFIgure;
            }
        }

        static int CheckForFullLines() // 0, 1, 2, 3, 4
        {
            int lines = 0;
            for (int row = 0; row < State.TetrisField.GetLength(0); row++)  //Check if line is full
            {
                bool rowIsFull = true;
                for (int col = 0; col < State.TetrisField.GetLength(1); col++)
                {
                    if (State.TetrisField[row, col] == false)
                    {
                        rowIsFull = false;
                        break;
                    }

                }
                if (rowIsFull)
                {
                    for (int rowToMove = row; rowToMove >= 1; rowToMove--)  //If line is full than delete it and move remaining lines down
                    {
                        for (int col = 0; col < State.TetrisField.GetLength(1); col++)
                        {
                            State.TetrisField[rowToMove, col] = State.TetrisField[rowToMove - 1, col];
                        }
                    }
                    lines++;
                }
            }

            return lines;  //returns how many lines we have deleted!
        }

        static void DrawTetrisField()
        {
            for (int row = 0; row < State.TetrisField.GetLength(0); row++)
            {
                for (int col = 0; col < State.TetrisField.GetLength(1); col++)
                {
                    if (State.TetrisField[row, col] == true)
                    {
                        Write("█", row + 1, col + 1);
                    }
                }
            }
        }

        static void AddCurrentFigureToTetrisFIeld()
        {
            for (int row = 0; row < State.CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < State.CurrentFigure.GetLength(1); col++)
                {
                    if (State.CurrentFigure[row, col] == true)
                    {
                        State.TetrisField[State.CurrentFigureRow + row, State.CurrentFigureCol + col] = true;
                    }
                }
            }
            //currentfigure
            //TetrisField
        }

        static void DrawBorder()
        {
            Console.SetCursorPosition(0, 0);
            string line = "╔";
            line += new string('═', TetrisCols);
            line += "╦";
            line += new string('═', InfoCols);
            line += "╗";

            Console.WriteLine(line);
            for (int i = 0; i < TetrisRows; i++)
            {
                string midLine = "║";
                midLine += new string(' ', TetrisCols);
                midLine += "║";
                midLine += new string(' ', InfoCols);
                midLine += "║";
                Console.Write(midLine);
            }

            string endLine = "╚";
            endLine += new string('═', TetrisCols);
            endLine += "╩";
            endLine += new string('═', InfoCols);
            endLine += "╝";
            Console.Write(endLine);
        }

        static void DrawInfo()
        {
            if (State.Score > State.HighScore)
            {
                State.HighScore = State.Score;
            }

            Write("Level:", 1, 1 + TetrisCols + 1 + 1);
            Write(State.Level.ToString(), 2, 1 + TetrisCols + 1 + 1);

            Write("Score:", 4, 1 + TetrisCols + 1 + 1);
            Write(State.Score.ToString(), 5, 1 + TetrisCols + 1 + 1);

            Write("BEST:", 7, 1 + TetrisCols + 1 + 1);
            Write(State.HighScore.ToString(), 8, 1 + TetrisCols + 1 + 1);

            Write("Frame:", 10, 1 + TetrisCols + 1 + 1);
            Write(State.Frame.ToString() + "/" + (State.FramesToMoveFigure - State.Level), 11, 1 + TetrisCols + 1 + 1);

            Write("Position", 13, 3 + TetrisCols);
            Write($"{ State.CurrentFigureRow},{ State.CurrentFigureCol}", 14, 3 + TetrisCols);

            Write("Keys:", 16, 3 + TetrisCols);
            Write("Space", 17, 3 + TetrisCols);
            Write("<-↓->", 18, 3 + TetrisCols);
        }
        static void DrawCurrentFigure()
        {
            for (int row = 0; row < State.CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < State.CurrentFigure.GetLength(1); col++)
                {
                    if (State.CurrentFigure[row, col] == true)
                    {
                        Write("█", row + 1 + State.CurrentFigureRow, col + 1 + State.CurrentFigureCol);
                    }
                }
            }
        }

        static bool Collision(bool[,] figure)
        {

            if (State.CurrentFigureCol > TetrisCols - figure.GetLength(1))
            {
                return true;
            }
            if (State.CurrentFigureRow + figure.GetLength(0) == TetrisRows) // If it hits bottom stop the currentfigute
            {
                return true;
            }

            for (int row = 0; row < figure.GetLength(0); row++)  //If it collides with another figure return true and stop
            {
                for (int col = 0; col < figure.GetLength(1); col++)
                {
                    if (figure[row, col] && State.TetrisField[State.CurrentFigureRow + row + 1, State.CurrentFigureCol + col])
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        static void Write(string text, int row = 0, int col = 0)
        {
            Console.SetCursorPosition(col, row);
            Console.Write(text);

        }
    }
}
