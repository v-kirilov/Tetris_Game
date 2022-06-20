using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_OOP
{
    internal class TetrisGameState
    {
        public TetrisGameState(int tetrisRows, int tetrisColumns)
        {
            this.HighScore = 0;  //The highest score which we read from file
            this.Score = 0;  //The current score from the current game
            this.Frame = 0;
            this.Level = 1;
            this.FramesToMoveFigure = 16;
            this.CurrentFigure = null;
            this.CurrentFigureRow = 0;
            this.CurrentFigureCol = 0;
            this.TetrisField = new bool[tetrisRows, tetrisColumns];
        }

        public int HighScore { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }
        public bool[,] TetrisField { get;private set; }
        public int Frame { get; set; }
        public int FramesToMoveFigure { get; private set; }
        public int CurrentFigureRow { get; set; }
        public int CurrentFigureCol { get; set; }
        public bool[,] CurrentFigure { get; set; }

        public void UpdateMethod()
        {
            if (this.Score <= 0)
            {
                this.Level = 1;
                return;
            }
            this.Level = (int)Math.Log10(this.Score) - 1;
            if (this.Level < 1)
            {
                this.Level = 1;
            }
            if (this.Level > 10)
            {
                this.Level = 10;
            }

        }
    }
    
}
