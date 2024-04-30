using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

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
        static string ScoresFileName = "score.txt";


        //Current State:
        static int HighScore = 0;
        static int Level = 0;
        static int score = 0;
        static bool[,] TetrisField = new bool[TetrisRows, TetrisCols];
        static int Frame = 0;
        static int FramesToMoveFigure = 15;
        static int CurrentFigureRow = 0;
        static int CurrentFigureCol = 0;
        static bool[,] CurrentFigure = null; 
        static Random RandomIndex = new Random();

        static void Main(string[] args)
        {
           


            if (File.Exists(ScoresFileName))
            {
                var allScores = File.ReadAllLines(ScoresFileName);
                foreach (var score  in allScores)
                {
                    var match = Regex.Match(score, @"=> (?<score>[0-9]+)");
                    HighScore = Math.Max(HighScore, int.Parse(match.Groups["score"].Value));
                }

            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.Title = "Tetris App 1.0";
            Console.WindowHeight = ConsoleRows + 1; //Dimentions of console.
            Console.WindowWidth = ConsoleCols;
            Console.BufferHeight = ConsoleRows + 1; //For scrows of console
            Console.BufferWidth = ConsoleCols;
            Console.CursorVisible = false;
            CurrentFigure = TetrisFigures[RandomIndex.Next(0, TetrisFigures.Count)];

            DrawBorder();


            while (true)
            {
                Frame++;
                UpdateMethod();
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
                        if (CurrentFigureCol >= 1)  //If figure reaches left end it cant go more left
                        {
                            CurrentFigureCol--;
                        }
                    }
                    if (pressedKey.Key == ConsoleKey.RightArrow)  //If figure reaches right end it cant go more right
                    {
                        if (CurrentFigureCol < TetrisCols - CurrentFigure.GetLength(1))
                        {
                            CurrentFigureCol++;
                        }
                    }
                    if (pressedKey.Key == ConsoleKey.DownArrow)
                    {
                        Frame = 1;
                        score += Level;
                        CurrentFigureRow++;
                        
                    }
                    if (pressedKey.Key == ConsoleKey.Spacebar)
                    {
                        RotateCurrentFugure();
                    }

                }
                // user input



                //Update the game state
                if (Frame % (FramesToMoveFigure-Level) == 0)
                {
                    CurrentFigureRow++;
                    Frame = 0;

                }
                if (Collision(CurrentFigure))
                {
                    AddCurrentFigureToTetrisFIeld();
                    int lines = CheckForFullLines();
                    score += ScorePerLines[lines] * Level;

                    CurrentFigure = TetrisFigures[RandomIndex.Next(0, TetrisFigures.Count)];  //If collision than make new random figure with row=0 & col=0
                    CurrentFigureCol = 0;
                    CurrentFigureRow = 0;
                    if (Collision(CurrentFigure))
                    {

                        
                        File.AppendAllLines(ScoresFileName, new List<string>
                        {
                            $"[{DateTime.Now.ToString()}]{Environment.UserName} => {score}"
                        });
                        string finalScore = score.ToString();
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

        static void UpdateMethod()
        {
            if (score <= 0)
            {
                Level = 1;
                return;
            }
            Level = (int)Math.Log10(score) - 1;
            if (Level < 1)
            {
                Level = 1;
            }
            if (Level > 10)
            {
                Level = 10;
            }

        }

        private static void RotateCurrentFugure()
        {
            var newFIgure = new bool[CurrentFigure.GetLength(1), CurrentFigure.GetLength(0)];
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    newFIgure[col, CurrentFigure.GetLength(0) - row - 1] = CurrentFigure[row, col];
                }
            }

            if (!Collision(newFIgure))
            {
                CurrentFigure = newFIgure;
            }
        }

        static int CheckForFullLines() // 0, 1, 2, 3, 4
        {
            int lines = 0;
            for (int row = 0; row < TetrisField.GetLength(0); row++)  //Check if line is full
            {
                bool rowIsFull = true;
                for (int col = 0; col < TetrisField.GetLength(1); col++)
                {
                    if (TetrisField[row, col] == false)
                    {
                        rowIsFull = false;
                        break;
                    }

                }
                if (rowIsFull)
                {
                    for (int rowToMove = row; rowToMove >= 1; rowToMove--)  //If line is full than delete it and move remaining lines down
                    {
                        for (int col = 0; col < TetrisField.GetLength(1); col++)
                        {
                            TetrisField[rowToMove, col] = TetrisField[rowToMove - 1, col];
                        }
                    }
                    lines++;
                }
            }

            return lines;  //returns how many lines we have deleted!
        }

        static void DrawTetrisField()
        {
            for (int row = 0; row < TetrisField.GetLength(0); row++)
            {
                for (int col = 0; col < TetrisField.GetLength(1); col++)
                {
                    if (TetrisField[row, col] == true)
                    {
                        Write("█", row + 1, col + 1);
                    }
                }
            }
        }

        static void AddCurrentFigureToTetrisFIeld()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row, col] == true)
                    {
                        TetrisField[CurrentFigureRow + row, CurrentFigureCol + col] = true;
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
            if (score>HighScore)
            {
                HighScore = score;
            }

            Write("Level:", 1, 1 + TetrisCols + 1 + 1);
            Write(Level.ToString(), 2, 1 + TetrisCols + 1 + 1);

            Write("Score:", 4, 1 + TetrisCols + 1 + 1);
            Write(score.ToString(), 5, 1 + TetrisCols + 1 + 1);

            Write("BEST:", 7, 1 + TetrisCols + 1 + 1);
            Write(HighScore.ToString(), 8, 1 + TetrisCols + 1 + 1);

            Write("Frame:", 10, 1 + TetrisCols + 1 + 1);
            Write(Frame.ToString() + "/" + (FramesToMoveFigure-Level), 11, 1 + TetrisCols + 1 + 1);

            Write("Position", 13, 3 + TetrisCols);
            Write($"{CurrentFigureRow},{CurrentFigureCol}", 14, 3 + TetrisCols);

            Write("Keys:", 16, 3 + TetrisCols);
            Write("Space", 17, 3 + TetrisCols);
            Write("<-↓->", 18, 3 + TetrisCols);
        }
        static void DrawCurrentFigure()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row, col] == true)
                    {
                        Write("█", row + 1 + CurrentFigureRow, col + 1 + CurrentFigureCol);
                    }
                }
            }
        }

        static bool Collision(bool[,] figure)
        {

            if (CurrentFigureCol > TetrisCols - figure.GetLength(1))
            {
                return true;
            }
            if (CurrentFigureRow + figure.GetLength(0) == TetrisRows) // If it hits bottom stop the currentfigute
            {
                return true;
            }

            for (int row = 0; row < figure.GetLength(0); row++)  //If it collides with another figure return true and stop
            {
                for (int col = 0; col < figure.GetLength(1); col++)
                {
                    if (figure[row, col] && TetrisField[CurrentFigureRow + row + 1, CurrentFigureCol + col])
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
