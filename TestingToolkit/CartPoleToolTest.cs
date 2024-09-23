using System;
using System.Threading.Tasks;
using Gym.Environments.Envs.Classic;
using Gym.Rendering.WinForm;
using RLMatrix.Toolkit;

namespace RLMatrix.Environments
{
    [RLMatrixEnvironment]
    public partial class CartPoleToolTest
    {
        private CartPoleEnv myEnv;
        private float[] myState;
        private int stepCounter;
        private const int MaxSteps = 100000;
        private bool isDone;

        public CartPoleToolTest()
        {
            InitialiseAsync();
        }


        private void InitialiseAsync()
        {
            myEnv = new CartPoleEnv(WinFormEnvViewer.Factory);
            stepCounter = 0;
            myEnv.Reset();
            isDone = false;
            myState = [0, 0, 0, 0];
        }

        [RLMatrixObservation]
        public float GetCartPosition() => myState[0];

        [RLMatrixObservation]
        public float GetCartVelocity() => myState[1];

        [RLMatrixObservation]
        public float GetPoleAngle() => myState[2];

        [RLMatrixObservation]
        public float GetPoleAngularVelocity() => myState[3];

        [RLMatrixActionDiscrete(2)]
        public void ApplyForce(int action)
        {
            if (isDone)
                ResetEnvironment();

            var (observation, reward, done, _) = myEnv.Step(action);
            myEnv.Render();
            myState = observation.ToFloatArray();
            isDone = done;
            stepCounter++;

            if (stepCounter > MaxSteps)
                isDone = true;
        }

        [RLMatrixReward]
        public float CalculateReward()
        {
            return isDone ? 0 : 1;
        }

        [RLMatrixDone]
        public bool IsEpisodeFinished()
        {
            return isDone;
        }

        [RLMatrixReset]
        public void ResetEnvironment()
        {
            myEnv.Reset();
            myState = [0, 0, 0, 0];
            isDone = false;
            stepCounter = 0;
        }
    }
}