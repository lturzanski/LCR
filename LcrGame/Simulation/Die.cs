using System;

namespace LcrGame.Simulation
{
    public class Die
    {
        /// <summary>
        /// Called when the die is rolled
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public int Roll(Random random)
        {
            RolledValue = random.Next(0, 6);

            return RolledValue;
        }

        /// <summary>
        /// The die's rolled value
        /// </summary>
        public int RolledValue { get; private set; } = 0;
    }
}
