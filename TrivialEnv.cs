using OneOf;
using RLMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPoleForTesting
{
    public class TrivialEnvironment : IEnvironment<float[]>
    {
        public const float CorrectAnswerReward = 1;
        public const float WrongAnswerPenalty = -1;

        public float[] state;

        public static int RandomValue()
        {
            return Random.Shared.Next(2);
        }

        public int stepCounter { get; set; }
        public int maxSteps { get; set; } = 10;
        public bool isDone { get; set; }
        public OneOf<int, (int, int)> stateSize { get; set; } = 1;
        public int[] actionSize { get; set; } = [2];

        public float[] GetCurrentState() => state;

        public TrivialEnvironment() => Initialise();

        public void Initialise() => Reset();

        public void Reset()
        {
            state = new float[1] { RandomValue() };
            stepCounter = 0;
            isDone = false;
        }

        public float Step(int[] actionsIds)
        {
            float input = state[0];
            float output = actionsIds[0];

            state[0] = RandomValue();

            if (stepCounter++ >= maxSteps)
            {
                isDone = true;
            }

            return input == output ? CorrectAnswerReward : WrongAnswerPenalty;
        }
    }
}
