using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tetris_OOP
{
    public class ScoreManager
    {
        private readonly string highScoreFile;
        public ScoreManager(string highScoreFile)
        {
            this.highScoreFile = highScoreFile;
        }
        public int GetHighScore()
        {
            int highScore = 0;
            if (File.Exists(this.highScoreFile))
            {
                string[] allScores = File.ReadAllLines(this.highScoreFile);
                foreach (var score in allScores)
                {
                    var match = Regex.Match(score, @"=> (?<score>[0-9]+)");
                    highScore = Math.Max(highScore, int.Parse(match.Groups["score"].Value));
                }

            }

            return highScore;
        }

        public void Add(int score)
        {
            File.AppendAllLines(this.highScoreFile, new List<string>
                        {
                            $"[{DateTime.Now.ToString()}]{Environment.UserName} => {score}"
                        });
        }

    }
}
