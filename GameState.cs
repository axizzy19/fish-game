using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class FishState : GameObject
    {
        public int Direction;
        public double health = 100;
        public int maxHealth = 100;
        public int fishLength = 19;
        public static string[] ReversedFishArray = Fish.GetReversedFish(Fish.fishArray);
        private HealthBarState healthBarState;

        public FishState()
        {
            healthBarState = new HealthBarState(this);
        }
        public override void Draw()
        {
            //base.Draw(); 
            //int drawX = (int)Math.Round(X);
            //int drawY = (int)Math.Round(Y);

            //string[] currentArray = (Direction == -1) ? Fish.fishArray : ReversedFishArray;
            //for (int i = 0; i < currentArray.Length; i++)
            //{
            //    Console.SetCursorPosition(drawX, drawY + i);
            //    Console.Write(currentArray[i]);
            //}

            //healthBarState.Draw();

            int drawX = (int)Math.Round(X);
            int drawY = (int)Math.Round(Y);
            int prevDrawX = (int)Math.Round(previousX);
            int prevDrawY = (int)Math.Round(previousY);

            string[] currentArray = (Direction == -1) ? Fish.fishArray : ReversedFishArray;

            int minX = Math.Min(prevDrawX, drawX) - 2;
            int maxX = Math.Max(prevDrawX, drawX) + 30;
            int width = maxX - minX;

            if (minX < 0) { width += minX; minX = 0; }
            if (width <= 0) width = 1;

            char[] buffer = new char[width];

            for (int i = 0; i < currentArray.Length; i++)
            {
                Array.Fill(buffer, ' ');
                string fishLine = currentArray[i];
                int offset = drawX - minX;

                for (int j = 0; j < fishLine.Length && (offset + j) < buffer.Length; j++)
                {
                    if (offset + j >= 0)
                        buffer[offset + j] = fishLine[j];
                }

                Console.SetCursorPosition(minX, drawY + i);
                Console.Write(buffer);
            }

            if (drawY - 1 >= 0)
            {
                Array.Fill(buffer, ' ');
                Console.SetCursorPosition(minX, drawY - 1);
                Console.Write(buffer);

                healthBarState.Draw();
            }
        }
    }

    public class HealthBarState : GameObject
    {
        private FishState fish;
        public HealthBarState(FishState fish)
        {
            this.fish = fish;
            StringLength = 12;
            StringsAmount = 1;
        }
        public override void Draw()
        {
            X = fish.X + fish.fishLength - StringLength;
            Y = fish.Y - 1;

            previousX = fish.previousX + fish.fishLength - StringLength;
            previousY = fish.previousY - 1;

            base.Draw();

            int totalBlocks = 10;
            int currentBlocks = (int)(fish.health / fish.maxHealth * totalBlocks);
            if (currentBlocks > totalBlocks) currentBlocks = totalBlocks;
            string bar = new string('#', currentBlocks) + new string('-', totalBlocks - currentBlocks);

            Console.SetCursorPosition((int)Math.Round(X), (int)Math.Round(Y));
            Console.Write($"[{bar}]");
        }
    }

    public class FoodState : GameObject
    {
        private string food = "*";
        public override void Draw()
        {
            int drawX = (int)Math.Round(X);
            int drawY = (int)Math.Round(Y);
            int prevDrawY = (int)Math.Round(previousY);

            int minY = Math.Min(prevDrawY, drawY);
            int maxY = Math.Max(prevDrawY, drawY);

            for (int y = minY; y <= maxY; y++)
            {
                Console.SetCursorPosition(drawX, y);
                Console.Write(" ");
            }

            Console.SetCursorPosition(drawX, drawY);
            Console.Write(food);
        }
    }
    public class PlayerState : GameObject
    {
        private string arm = "XXX";
        public PlayerState() : base()
        {
            StringLength = arm.Length;
            StringsAmount = 1;
        }

        public override void Draw()
        {
            base.Draw();
            Console.SetCursorPosition((int)Math.Round(X), (int)Math.Round(Y));
            Console.Write(arm);
        }
    }
    public class GameState
    {
        public List<FishState> Fishes = new List<FishState>();
        public List<FoodState> Foods = new List<FoodState>();
        public PlayerState Player = new PlayerState();
    }
}
