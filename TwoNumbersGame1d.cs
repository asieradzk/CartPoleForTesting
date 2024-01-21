using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneOf;
using OneOf.Types;
using RLMatrix;

namespace CartPoleForTesting
{
    public class TwoNumbersGame1d : IEnvironment<float[]>
    {
        public int stepCounter { get; set; }
        public int maxSteps { get; set; }
        public bool isDone { get; set; }
        public OneOf<int, (int, int)> stateSize { get; set; }
        public int[] actionSize { get; set; }

        private float[] myState;

        public TwoNumbersGame1d()
        {
            Initialise();
        }

        public float[] GetCurrentState()
        {
            return myState;
        }

        private float num1;
        private float num2;
        private float currentNum1;
        private float currentNum2;

        public void Initialise()
        {
            isDone = false;
            stepCounter = 0;
            maxSteps = 40;
            actionSize = new int[] { 8 }; // from 0 to 7
            stateSize = 42;
            myState = new float[42];
            //set all to 9f
            for (int i = 0; i < 42; i++)
            {
                myState[i] = 9f;
            }
            //num1 = 0.2f * 10f; 
            num1 = pickRandoSmallNumber()*10f;
            //num2 = 5000f/1000f; 
            num2 = pickRandoLargeNumber()/1000f;
            currentNum1 = 0f;
            currentNum2 = 0f;
            myState[0] = num1;
            myState[1] = num2;

        }

        public void Reset()
        {
            Initialise();
        }

        public float Step(int[] actionsIds)
        {
            var actionId = actionsIds[0];

            if(isDone)
            {
                throw new Exception("Cannot take action when environment is done");
            }

            

            if(actionId == 7)
            {
                isDone = true;
                var rewrd = RewardFunction();
                Console.WriteLine("Reward: " + rewrd);
                return rewrd;
            }
            if(stepCounter >= maxSteps)
            {
                isDone = true;
                var rewrd = RewardFunction();
                Console.WriteLine("Reward: " + rewrd);
                return rewrd;
            }
            myState[stepCounter + 2] = actionId*0.01f;
            takeAction(actionId);
            stepCounter++;

            return 0f;
            
        }










        private float pickRandoSmallNumber()
        {
           //picks a random number between -1.5f and 1.5f;
            Random rnd = new Random();
            float num = (float)rnd.NextDouble();
            num = num * 3;
            num = num - 1.5f;
            return num;
        }

        private float pickRandoLargeNumber()
        {
            //picks a random number between 1000 and 20000
            Random rnd = new Random();
            float num = (float)rnd.NextDouble();
            num = num * 19000;
            num = num + 1000;
            return num;
        }

        private void takeAction(int action)
        {
            switch (action)
            {
                case 0:
                    currentNum2 += 500f;
                    break;
                case 1:
                    currentNum1 += 0.1f;
                    break;
                case 2:
                    currentNum1 -= 0.1f;
                    break;
                case 3:
                    currentNum2 += 5000f;
                    break;
                case 4:
                    currentNum1 += 0.5f;
                    break;
                case 5:
                    currentNum1 -= 0.5f;
                    break;
                case 6:
                    currentNum2 += 100f;
                    break;
                case 7:
                    break;
                default:
                    throw new Exception("Invalid action");
                    break;
            }
        }

        private float RewardFunction()
        {
            // Calculate exponential rewards based on differences
            float reward1 = 100f - (Math.Abs(num1 - currentNum1) * 400f);
            float reward2 = 100f - (Math.Abs(num2 - currentNum2) / 20);

            return reward1 + reward2;
        }


    }
}
