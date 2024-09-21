using RLMatrix.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPoleForTesting.TestingToolkit
{

    [RLMatrixEnvironment]
    public partial class IdentityToolTestDiscrete // : should be ignored
    {
        private int myNum1 = 0;
        private int myNum2 = 0;
        private int myNum3 = 0;
        private int selectNum1 = 0;
        private int selectNum2 = 0;
        private int selectNum3 = 0;

        [RLMatrixObservation]
        public float ObserveNum1() => myNum1;

        [RLMatrixObservation]
        public float ObserveNum2() => myNum2;

        [RLMatrixObservation]
        public float ObserveNum3() => myNum3;


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


        [RLMatrixReward]
        public float RewardDiscrete()
        {
            float reward = 0;
            isDone = true;
            if (selectNum1 == myNum1) reward += 1;
            if (selectNum2 == myNum2) reward += 1;
            if (selectNum3 == myNum3) reward += 1;
            return reward - 1.5f;
        }


        Random random = new Random();
        bool isDone = false;

        [RLMatrixReset]
        public void ResetMe()
        {
            myNum1 = random.Next(2);
            myNum2 = random.Next(3);
            myNum3 = random.Next(2);
            selectNum1 = 0;
            selectNum2 = 0;
            selectNum3 = 0;
            isDone = false;
        }

        [RLMatrixDone]
        public bool AmIDoneDone()
        {
            return isDone;
        }
    }
}
