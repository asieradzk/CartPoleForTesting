using RLMatrix.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPoleForTesting.TestingToolkit
{

    [RLMatrixEnvironment]
    public partial class IdentityToolTestVectorObs // : should be ignored
    {
        private int myNum1 = 0;
        private int myNum2 = 0;
        private int myNum3 = 0;
        private int selectNum1 = 0;
        private int selectNum2 = 0;
        private int selectNum3 = 0;
        private float myFloat1 = 0f;
        private float myFloat2 = 0f;
        private float myFloat3 = 0f;
        private float selectFloat1 = 0f;
        private float selectFloat2 = 0f;
        private float selectFloat3 = 0f;


        [RLMatrixObservation]
        public float[] ObserveAllNums()
        {
            return new float[] { myNum1, myNum2, myNum3 };
        }

        [RLMatrixObservation]
        public float ObserveFloat1() => myFloat1;

        [RLMatrixObservation]
        public float ObserveFloat2() => myFloat2;

        [RLMatrixObservation]
        public float ObserveFloat3() => myFloat3;

        [RLMatrixActionDiscrete(2)]
        public void ActionDiscrete1(int action)
        {
            selectNum1 = action;
        }

        [RLMatrixActionDiscrete(3)]
        public void ActionDiscrete2(int action)
        {
            selectNum2 = action;
        }

        [RLMatrixActionDiscrete(2)]
        public void ActionDiscrete3(int action)
        {
            selectNum3 = action;
        }

        [RLMatrixActionContinuous]
        public void ActionContinuous1(float action)
        {
            selectFloat1 = action;
        }

        [RLMatrixActionContinuous(-1, 1)]
        public void ActionContinuous2(float action)
        {
            selectFloat2 = action;
        }

        [RLMatrixActionContinuous(-1, 1)]
        public void ActionContinuous3(float action)
        {
            selectFloat3 = action;
        }

        [RLMatrixReward]
        public float RewardDiscrete()
        {
            float reward = 0;
            if (selectNum1 == myNum1) reward += 1;
            if (selectNum2 == myNum2) reward += 1;
            if (selectNum3 == myNum3) reward += 1;
            return reward - 1.5f;
        }

        [RLMatrixReward]
        public float RewardContinuous()
        {
            float reward = 3 - (Math.Abs(selectFloat1 - myFloat1) +
                                Math.Abs(selectFloat2 - myFloat2) +
                                Math.Abs(selectFloat3 - myFloat3));
            isDone = true;
            return reward;
        }

        Random random = new Random();
        bool isDone = false;

        [RLMatrixReset]
        public void ResetMe()
        {
            myNum1 = random.Next(2);
            myNum2 = random.Next(3);
            myNum3 = random.Next(2);
            myFloat1 = (float)random.NextDouble() * 2 - 1;
            myFloat2 = (float)random.NextDouble() * 2 - 1;
            myFloat3 = (float)random.NextDouble() * 2 - 1;
            selectNum1 = 0;
            selectNum2 = 0;
            selectNum3 = 0;
            selectFloat1 = 0f;
            selectFloat2 = 0f;
            selectFloat3 = 0f;
            isDone = false;
        }

        [RLMatrixDone]
        public bool AmIDoneDone()
        {
            return isDone;
        }
    }
}
