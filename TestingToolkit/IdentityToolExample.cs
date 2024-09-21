using OneOf;
using RLMatrix;
using RLMatrix.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPoleForTesting.TestingToolkit
{
    //[RLMatrixEnvironment]
    public partial class IdentityToolExample // : should be ignored
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

        //[RLMatrixObservation]
        public float ObserveNum1() => myNum1;

        //[RLMatrixObservation]
        public float ObserveNum2() => myNum2;

        //[RLMatrixObservation]
        public float ObserveNum3() => myNum3;

        //[RLMatrixObservation]
        public float ObserveFloat1() => myFloat1;

        //[RLMatrixObservation]
        public float ObserveFloat2() => myFloat2;

        //[RLMatrixObservation]
        public float ObserveFloat3() => myFloat3;

        //[RLMatrixActionDiscrete(2)]
        public void ActionDiscrete1(int action)
        {
            selectNum1 = action;
        }

        //[RLMatrixActionDiscrete(2)]
        public void ActionDiscrete2(int action)
        {
            selectNum2 = action;
        }

        //[RLMatrixActionDiscrete(2)]
        public void ActionDiscrete3(int action)
        {
            selectNum3 = action;
        }

        //[RLMatrixActionContinuous(float min = -1, float max = 1)]
        public void ActionContinuous1(float action)
        {
            selectFloat1 = action;
        }

        //[RLMatrixActionContinuous(float min = -1, float max = 1)]
        public void ActionContinuous2(float action)
        {
            selectFloat2 = action;
        }

        //[RLMatrixActionContinuous(float min = -1, float max = 1)]
        public void ActionContinuous3(float action)
        {
            selectFloat3 = action;
        }

        //[RLMatrixReward]
        public float RewardDiscrete()
        {
            float reward = 0;
            if (selectNum1 == myNum1) reward += 1;
            if (selectNum2 == myNum2) reward += 1;
            if (selectNum3 == myNum3) reward += 1;
            return reward - 1.5f; // Normalized to range [-1.5, 1.5]
        }

        //[RLMatrixReward]
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

        public void ResetMe()
        {
            myNum1 = random.Next(2);
            myNum2 = random.Next(2);
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

        //[RLMatrixDone]
        public bool AmIDoneDone()
        {
            return isDone;
        }
    }



    //What should be generated
    public partial class IdentityToolExample : IContinuousEnvironmentAsync<float[]>
    {
        private int _poolingRate;
        private RLMatrixPoolingHelper _poolingHelper;
        private List<IRLMatrixExtraObservationSource> _extraObservationSources;
        private int _stepsSoft;
        private int _stepsHard;
        private int _maxStepsHard;
        private int _maxStepsSoft;

        public OneOf<int, (int, int)> StateSize { get; set; }
        public int[] DiscreteActionSize { get; set; } = new int[] { 2, 2, 2 };
        public (float min, float max)[] ContinuousActionBounds { get; set; } = new (float min, float max)[]
        {
        (-1f, 1f),
        (-1f, 1f),
        (-1f, 1f)
        };
        private bool _rlMatrixEpisodeTerminated;

        private (Action<int> method, int maxValue)[] _discreteActionMethodsWithCaps;
        private Action<float>[] _continuousActionMethods;

        public IdentityToolExample(int poolingRate = 1, int maxStepsHard = 1000, int maxStepsSoft = 100, List<IRLMatrixExtraObservationSource> extraObservationSources = null)
        {
            _poolingRate = poolingRate;
            _maxStepsHard = maxStepsHard / poolingRate;
            _maxStepsSoft = maxStepsSoft / poolingRate;
            _extraObservationSources = extraObservationSources ?? new List<IRLMatrixExtraObservationSource>();

            _poolingHelper = new RLMatrixPoolingHelper(_poolingRate, DiscreteActionSize.Length + ContinuousActionBounds.Length, _GetAllObservations);

            int baseObservationSize = _GetBaseObservationSize();
            int extraObservationSize = _extraObservationSources.Sum(source => source.GetObservationSize());
            StateSize = _poolingRate * (baseObservationSize + extraObservationSize);

            _rlMatrixEpisodeTerminated = true;
            _InitializeObservations();

            _discreteActionMethodsWithCaps = new (Action<int>, int)[]
            {
            (ActionDiscrete1, 2),
            (ActionDiscrete2, 2),
            (ActionDiscrete3, 2)
            };

            _continuousActionMethods = new Action<float>[]
            {
            ActionContinuous1,
            ActionContinuous2,
            ActionContinuous3
            };
        }

        private void _InitializeObservations()
        {
            for (int i = 0; i < _poolingRate; i++)
            {
                float reward = RewardDiscrete() + RewardContinuous();
                _poolingHelper.CollectObservation(reward);
            }
        }

        public Task<float[]> GetCurrentState()
        {
            if (_rlMatrixEpisodeTerminated && _IsHardDone())
            {
                ResetMe();
                _poolingHelper.HardReset(_GetAllObservations);
                _rlMatrixEpisodeTerminated = false;
            }
            else if (_rlMatrixEpisodeTerminated && _IsSoftDone())
            {
                _stepsSoft = 0;
                _rlMatrixEpisodeTerminated = false;
            }

            return Task.FromResult(_poolingHelper.GetPooledObservations());
        }

        public Task Reset()
        {
            _stepsSoft = 0;
            _stepsHard = 0;
            ResetMe();
            _rlMatrixEpisodeTerminated = false;
            _poolingHelper.HardReset(_GetAllObservations);

            if (AmIDoneDone())
            {
                throw new Exception("Done flag still raised after reset - did you intend to reset?");
            }

            return Task.CompletedTask;
        }

        public Task<(float reward, bool done)> Step(int[] discreteActions, float[] continuousActions)
        {
            _stepsSoft++;
            _stepsHard++;

            for (int i = 0; i < _discreteActionMethodsWithCaps.Length; i++)
            {
                int cappedAction = Math.Min(discreteActions[i], _discreteActionMethodsWithCaps[i].maxValue - 1);
                _discreteActionMethodsWithCaps[i].method(cappedAction);
            }

            for (int i = 0; i < _continuousActionMethods.Length; i++)
            {
                _continuousActionMethods[i](continuousActions[i]);
            }

            float stepReward = RewardDiscrete() + RewardContinuous();
            _poolingHelper.CollectObservation(stepReward);

            float totalReward = _poolingHelper.GetAndResetAccumulatedReward();

            _rlMatrixEpisodeTerminated = _IsHardDone() || _IsSoftDone();

            _poolingHelper.SetAction(discreteActions.Select(a => (float)a).Concat(continuousActions).ToArray());

            return Task.FromResult((totalReward, _rlMatrixEpisodeTerminated));
        }

        private bool _IsHardDone()
        {
            return (_stepsHard >= _maxStepsHard || AmIDoneDone());
        }

        private bool _IsSoftDone()
        {
            return (_stepsSoft >= _maxStepsSoft);
        }

        public void GhostStep()
        {
            if (_IsHardDone() || _IsSoftDone())
                return;

            if (_poolingHelper.HasAction)
            {
                var actions = _poolingHelper.GetLastAction();
                for (int i = 0; i < _discreteActionMethodsWithCaps.Length; i++)
                {
                    int cappedAction = Math.Min((int)actions[i], _discreteActionMethodsWithCaps[i].maxValue - 1);
                    _discreteActionMethodsWithCaps[i].method(cappedAction);
                }
                for (int i = 0; i < _continuousActionMethods.Length; i++)
                {
                    _continuousActionMethods[i](actions[_discreteActionMethodsWithCaps.Length + i]);
                }
            }
            float reward = RewardDiscrete() + RewardContinuous();
            _poolingHelper.CollectObservation(reward);
        }

        private float[] _GetAllObservations()
        {
            var baseObservations = _GetBaseObservations();
            var extraObservations = _extraObservationSources.SelectMany(source => source.GetObservations()).ToArray();
            return baseObservations.Concat(extraObservations).ToArray();
        }

        private int _GetBaseObservationSize()
        {
            return _GetBaseObservations().Length;
        }

        private float[] _GetBaseObservations()
        {
            return new float[]
            {
            ObserveNum1(),
            ObserveNum2(),
            ObserveNum3(),
            ObserveFloat1(),
            ObserveFloat2(),
            ObserveFloat3()
            };
        }
    }

}
