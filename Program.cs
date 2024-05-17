using CartPoleForTesting;
using RLMatrix;
using RLMatrix.Agents.DQN.Variants;
using RLMatrix.Agents.PPO.Variants;
using RLMatrix.WinformsChart;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var myChart = new WinformsChart();
var mychart2 = new WinformsChart();

//PPO
var optsppo = new PPOAgentOptions(
    batchSize: 8,           // Number of EPISODES agent interacts with environment before learning from its experience
    memorySize: 10000,       // Size of the replay buffer
    gamma: 0.99f,          // Discount factor for rewards
    gaeLambda: 0.95f,      // Lambda factor for Generalized Advantage Estimation
    lr: 3e-4f,            // Learning rate
    width: 256,
    depth: 2,
    clipEpsilon: 0.2f,     // Clipping factor for PPO's objective function
    vClipRange: 0.2f,      // Clipping range for value loss
    cValue: 0.5f,          // Coefficient for value loss
    ppoEpochs: 1,            // Number of PPO epochs
    clipGradNorm: 0.5f, 
    entropyCoefficient: 0.1f,
    displayPlot: myChart,
    useRNN: false
   );

var envppo = new List<IEnvironment<float[]>> { new CartPole(), new CartPole()};
var myAgentppo = new PPOAgent<float[]>(optsppo, envppo);

//var myenv2ppo = new List<IEnvironment<float[]>>{ new TwoNumbersGame1d() };
//var myAgent2ppo = new PPOForTesting<float[]>(optsppo, myenv2ppo);

for (int i = 0; i < 10000000; i++)
{
     myAgentppo.Step();
    //myAgent2ppo.Step();sss
}



//Console.ReadLine();






//DQN
var opts = new DQNAgentOptions(numAtoms: 51, prioritizedExperienceReplay: true, nStepReturn: 30, duelingDQN: false, doubleDQN: true, noisyLayers: true, noisyLayersScale: 0.01f, categoricalDQN: true, batchSize: 128, memorySize: 10000, gamma: 0.99f, epsStart: 1f, epsEnd: 0.05f, epsDecay: 150f, tau: 0.005f, lr: 1e-3f, displayPlot: myChart, width: 512, depth: 2);
var env = new List<IEnvironmentAsync<float[,]>> { new CartPole2dAsync(), new CartPole2dAsync()};
var myAgent = new BaseRolloutAgent<float[,]>(opts, env);


for (int i = 0; i < 6000; i++)
{
   await myAgent.Step();
}

Console.WriteLine("doneTraining");

for (int i = 0; i < 6000; i++)
{
    await myAgent.Step(false);
    //myAgentOld.Step(false);

}

Console.WriteLine("doneTesting");


Console.ReadLine();