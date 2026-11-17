using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    internal class Fish : GameObject
    {
        public int direction = -1;
        public int fishLength = 19;
        public static string[] fishArray = ["      /`·.¸        ", "     /¸...¸`:·     ", " ¸.·´  ¸   `·.¸.·´)", ": © ):´;      ¸  { ", " `·.¸ `·  ¸.·´\\`·¸) ", "     `\\\\´´\\¸.·´    "];
        public int fishHeight = fishArray.Length;
        public static string[] reversedFishArray = GetReversedFish(fishArray);
        public int maxHealth = 100;
        public double health = 100;
        private double healthDecreasedSpeed = 10;
        private HealthBar healthBar;
        private double fishSpeed = 18;

        public Fish()
        {
            StringLength = fishLength;
            StringsAmount = fishHeight;
            healthBar = new HealthBar(this);
            Speed = fishSpeed;
        }
        public override void Update(double deltaTime)
        {
            X = X + direction*Speed*deltaTime;
            if (X < 0)
            {
                direction = 1;
                X = 0;
            }
            if (X > Console.WindowWidth - StringLength)
            {
                direction = -1;
            }
            health -= healthDecreasedSpeed*deltaTime;
            if (health < 0)
            {
                IsDead = true;
            }
        }

        public static string[] GetReversedFish(string[] fishArray)
        { 
            return fishArray.Select(line =>
            {
                var reversed = new string(line.Reverse().ToArray());
                var mirrored = reversed.Select(c =>
                {
                    if (c == '/') return '\\';
                    if (c == '\\') return '/';
                    if (c == '(') return ')';
                    if (c == ')') return '(';
                    if (c == '{') return '}';
                    if (c == '}') return '{';
                    return c;
                }).ToArray();

                return new string(mirrored);
            }).ToArray();
        }

    }
}
