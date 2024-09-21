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
    public partial class IdentityToolExampleDiscrete // : should be ignored
    {
        private int myNum = 0;
        private int selectNum = 0;
        private int myNum2 = 0;
        private int selectNum2 = 0;
        private int myNum3 = 0;
        private int selectNum3 = 0;



        //[RLMatrixObservation]
        public float ObserveNum()
        {
            return myNum;
        }

        //[RLMatrixObservation]
        public float ObserveNum2()
        {
            return myNum2;
        }
        //[RLMatrixObservation]
        public float ObserveNum3()
        {
            return myNum3;
        }

        //[RLMatrixActionDiscrete(2)]
        public void ActionDiscrete(int someActionNameTest)
        {
            switch (someActionNameTest)
            {
                case 0:
                    selectNum = 0;
                    break;
                case 1:
                    selectNum = 1;
                    break;
            }
        }

        //[RLMatrixActionDiscrete(3)]
        public void ActionDiscrete2(int someActionNameTest)
        {
            switch (someActionNameTest)
            {
                case 0:
                    selectNum2 = 0;
                    break;
                case 1:
                    selectNum2 = 1;
                    break;
                case 2:
                    selectNum2 = 2;
                    break;
            }
        }
        //[RLMatrixActionDiscrete(2)]
        public void ActionDiscrete3(int someActionNameTest)
        {
            switch (someActionNameTest)
            {
                case 0:
                    selectNum3 = 0;
                    break;
                case 1:
                    selectNum3 = 1;
                    break;
            }
        }


        //[RLMatrixReward]
        public float RewardOne()
        {
            Console.WriteLine($"SelectNum: {selectNum}, MyNum: {myNum}");


            if (selectNum == myNum)
            {
                return 1;
            }
            else
            {
                return -5;
            }
        }

        //[RLMatrixReward]
        public float RewardTwo()
        {
            Console.WriteLine($"SelectNum2: {selectNum2}, MyNum2: {myNum2}");

            if (selectNum2 == myNum2)
            {
                return 1;
            }
            else
            {
                return -5;
            }

        }

        //[RLMatrixReward]
        public float RewardThree()
        {
            Console.WriteLine($"SelectNum3: {selectNum3}, MyNum3: {myNum3}");
            isDone = true;
            if (selectNum3 == myNum3)
            {
                return 1;
            }
            else
            {
                return -5;
            }
           

        }


        Random random = new Random();
        bool isDone = false;
        public void ResetMe()
        {
            myNum = random.Next(2);
            myNum2 = random.Next(3);
            myNum3 = random.Next(2);
            selectNum = 0;
            selectNum2 = 0;
            selectNum3 = 0;
            isDone = false;
        }

        //[RLMatrixDone]
        public bool AmIDoneDone()
        {
            return isDone;
        }




    }




    //What should be generated
    public partial class IdentityToolExampleDiscrete : IEnvironmentAsync<float[]>
    {
        private int _poolingRate;
        private RLMatrixPoolingHelper _poolingHelper;
        private List<IRLMatrixExtraObservationSource> _extraObservationSources;
        private int _stepsSoft;
        private int _stepsHard;
        private int _maxStepsHard;
        private int _maxStepsSoft;

        public OneOf<int, (int, int)> stateSize { get; set; }
        public int[] actionSize { get; set; } = new int[] { 3, 3, 3 };
        private bool _rlMatrixEpisodeTerminated;

        private (Action<int> method, int maxValue)[] _actionMethodsWithCaps;

        public IdentityToolExampleDiscrete(int poolingRate = 1, int maxStepsHard = 1000, int maxStepsSoft = 100, List<IRLMatrixExtraObservationSource> extraObservationSources = null)
        {
            _poolingRate = poolingRate;
            _maxStepsHard = maxStepsHard / poolingRate;
            _maxStepsSoft = maxStepsSoft / poolingRate;
            _extraObservationSources = extraObservationSources ?? new List<IRLMatrixExtraObservationSource>();

            _poolingHelper = new RLMatrixPoolingHelper(_poolingRate, actionSize.Length, _GetAllObservations);

            int baseObservationSize = _GetBaseObservationSize();
            int extraObservationSize = _extraObservationSources.Sum(source => source.GetObservationSize());
            stateSize = _poolingRate * (baseObservationSize + extraObservationSize);

            _rlMatrixEpisodeTerminated = true;
            _InitializeObservations();

            _actionMethodsWithCaps = new (Action<int>, int)[]
            {
            (ActionDiscrete, 2),
            (ActionDiscrete2, 3),
            (ActionDiscrete3, 2)
            };
        }

        private void _InitializeObservations()
        {
            for (int i = 0; i < _poolingRate; i++)
            {
                float reward = RewardOne() + RewardTwo() + RewardThree();
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

        public Task<(float, bool)> Step(int[] actionsIds)
        {
            _stepsSoft++;
            _stepsHard++;

            for (int i = 0; i < _actionMethodsWithCaps.Length; i++)
            {
                int cappedAction = Math.Min(actionsIds[i], _actionMethodsWithCaps[i].maxValue - 1);
                _actionMethodsWithCaps[i].method(cappedAction);
            }

            float stepReward = RewardOne() + RewardTwo() + RewardThree();
            _poolingHelper.CollectObservation(stepReward);

            float totalReward = _poolingHelper.GetAndResetAccumulatedReward();

            _rlMatrixEpisodeTerminated = _IsHardDone() || _IsSoftDone();

            _poolingHelper.SetAction(actionsIds.Select(a => (float)a).ToArray());

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
                for (int i = 0; i < _actionMethodsWithCaps.Length; i++)
                {
                    int cappedAction = Math.Min((int)actions[i], _actionMethodsWithCaps[i].maxValue - 1);
                    _actionMethodsWithCaps[i].method(cappedAction);
                }
            }
            float reward = RewardOne() + RewardTwo() + RewardThree();
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
            ObserveNum(),
            ObserveNum2(),
            ObserveNum3()
            };
        }
    }


}
