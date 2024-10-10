using RLMatrix.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPoleForTesting.TestingToolkit
{

    [RLMatrixEnvironment]
    public partial class IdentityToolTestCont // : should be ignored
    {
        private float myFloat1 = 0f;
        private float myFloat2 = 0f;
        private float myFloat3 = 0f;
        private float selectFloat1 = 0f;
        private float selectFloat2 = 0f;
        private float selectFloat3 = 0f;


        [RLMatrixObservation]
        public float ObserveFloat1() => myFloat1;

        [RLMatrixObservation]
        public float ObserveFloat2() => myFloat2;

        [RLMatrixObservation]
        public float ObserveFloat3() => myFloat3;

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
            myFloat1 = (float)random.NextDouble() * 2 - 1;
            myFloat2 = (float)random.NextDouble() * 2 - 1;
            myFloat3 = (float)random.NextDouble() * 2 - 1;
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
