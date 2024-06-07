using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gym.Observations;
using OneOf;
using RLMatrix;

namespace CartPoleForTesting
{
    public class SequenceGuessingGame : IEnvironment<float[]> { 

        public int stepCounter { get; set; }
        public int maxSteps { get; set; }
        public bool isDone { get; set; }
        public OneOf<int, (int, int)> stateSize { get; set; }
        public int[] actionSize { get; set; }

        public SequenceGuessingGame()
        {
            Initialise();
        }

        float state;
        int randomLength;
        Random random = new Random();
        public float[] GetCurrentState()
        {

            return new float[] { state/10f };
        }

        public void Initialise()
        {
            maxSteps = 50;
            isDone = false;
            stateSize = 1;
            actionSize = new int[] { 5 };
            randomLength = random.Next(1, 47);
            stepCounter =1;
            state = randomLength * 100f;
        }

        public void Reset()
        {
            Initialise();
        }

        public float Step(int[] actionsIds)
        {
            //stepping
            stepCounter++;

            if (actionsIds[0] == 1)
            {
                //Finished
                var reward = 100 - 20 * Math.Abs(stepCounter - randomLength);

                Console.WriteLine($"Finished with reward {reward} at {stepCounter} steps");
                isDone = true;
                return reward;
            }
            else
            {
                
                if(stepCounter > 3)
                {
                    state = stepCounter;
                }
                

                if(stepCounter >= maxSteps)
                {
                    var reward = -20 * Math.Abs(stepCounter - randomLength);

                    Console.WriteLine($"Finished with reward {reward} at {stepCounter} steps");
                    isDone = true;
                    return reward;
                }
                

                return 0.1f;
            }
        }
    }
}
