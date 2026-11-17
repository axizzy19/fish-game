using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    internal class Game
    {
        private Player player = new Player();
        private List<Food> foods = new List<Food>();
        private List<Fish> fishes = new List<Fish>();
        private int FPS = 60;
        public bool isGameOver = false;
        public double updateTotal;
        public double drawTotal;

        public GameState LastState = new GameState();

        private void Clean()
        {
            foreach (var fish in fishes)
            {
                if (fish.IsDead) fish.Erase();
            }
            fishes.RemoveAll(f => f.IsDead);

            foreach (var food in foods)
            {
                if (food.IsDead) food.Erase();
            }
            foods.RemoveAll(f => f.IsDead);
        }

        public void UpdateLoop()
        {
            Stopwatch timeTotalUpdate = new Stopwatch();
            Stopwatch gameTime = new Stopwatch();
            gameTime.Start();
            double previousTime = 0;

            while (true)
            {
                timeTotalUpdate.Restart();
                double currentTime = gameTime.Elapsed.TotalMilliseconds;
                double deltaTime = (currentTime - previousTime) / 1000;
                previousTime = currentTime;

                foreach (var fish in fishes)
                {
                    fish.Update(deltaTime);
                }

                foreach (var food in foods)
                {
                    food.Update(deltaTime);

                    foreach (var fish in fishes)
                    {
                        if (food.X >= fish.X && food.X <= fish.X + fish.fishLength &&
                            food.Y >= fish.Y && food.Y <= fish.Y + fish.fishHeight)
                        {
                            food.IsDead = true;
                            fish.health += 10;

                            if (fish.health > fish.maxHealth)
                            {
                                fish.health = fish.maxHealth;
                            }
                            break;
                        }
                    }
                }

                if (player.dropFood)
                {
                    var newFood = new Food();
                    newFood.X = player.X;
                    newFood.Y = player.Y + 1;
                    foods.Add(newFood);
                    player.dropFood = false;
                }

                Clean();

                if (fishes.Count == 0)
                {
                    isGameOver = true;
                    return;
                }

                var newState = new GameState();

                for (int i = 0; i < fishes.Count; i++)
                {
                    var fish = fishes[i];

                    double prevX = fish.X;
                    double prevY = fish.Y;

                    if (i < LastState.Fishes.Count)
                    {
                        prevX = LastState.Fishes[i].X;
                        prevY = LastState.Fishes[i].Y;
                    }

                    newState.Fishes.Add(new FishState
                    {
                        X = fish.X,
                        Y = fish.Y,
                        previousX = prevX,
                        previousY = prevY,
                        Direction = fish.direction,
                        health = fish.health
                    });
                }

                foreach (var food in foods)
                {
                    newState.Foods.Add(new FoodState
                    {
                        X = food.X,
                        Y = food.Y
                    });
                }

                newState.Player.X = player.X;
                newState.Player.Y = player.Y;

                LastState = newState;

                timeTotalUpdate.Stop();
                updateTotal = timeTotalUpdate.Elapsed.TotalMilliseconds;

                Thread.Sleep(1000 / FPS);
            }
        }

        public void DrawLoop()
        {
            Stopwatch timeTotalDraw = new Stopwatch();

            while (true)
            {
                timeTotalDraw.Restart();
                if (isGameOver) return;

                GameState stateToDraw = LastState;

                foreach (var fishState in stateToDraw.Fishes)
                {
                    fishState.Draw();
                }

                stateToDraw.Player.Draw();

                foreach (var foodState in stateToDraw.Foods)
                {
                    foodState.Draw();
                }

                timeTotalDraw.Stop();
                drawTotal = timeTotalDraw.Elapsed.TotalMilliseconds;

                //Console.SetCursorPosition(0, Console.WindowHeight - 2);
                //Console.WriteLine($"UPDATE всего - {updateTotal:F2}   ");
                //Console.SetCursorPosition(0, Console.WindowHeight - 4);
                //Console.WriteLine($"DRAW всего - {drawTotal:F2}   ");

                Thread.Sleep(1000 / FPS);
            }
        }

        public void HandleInput(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.LeftArrow)
            {
                player.MoveLeft();
            }
            if (key.Key == ConsoleKey.RightArrow)
            {
                player.MoveRight();
            }
            if (key.Key == ConsoleKey.Spacebar)
            {
                player.DropFood();
            }
        }

        public void Start()
        {
            Fish fish1 = new Fish();
            fish1.X = 30;
            fish1.Y = 4;
            fishes.Add(fish1);

            Fish fish2 = new Fish();
            fish2.X = 0;
            fish2.Y = 15;
            fishes.Add(fish2);

            player.X = 2;
            player.Y = 1;

            Thread updateThread = new Thread(UpdateLoop);
            Thread drawThread = new Thread(DrawLoop);
            updateThread.Start();
            drawThread.Start();
        }
    }
}