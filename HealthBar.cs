using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    internal class HealthBar : GameObject
    {
        private Fish fish;
        public HealthBar(Fish fish)
        {
            this.fish = fish;
            StringLength = 12;
            StringsAmount = 1;
        }
    }
}
