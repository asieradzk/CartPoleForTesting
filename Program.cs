using RLMatrix;
using RLMatrix.Agents.Common;
using RLMatrix.WinformsChart;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var myChart = new WinformsChart();


var optsppo = new PPOAgentOptions(
    batchSize: 12,        // Nu8mber of EPISODES agent interacts with environment before learning from its experience
    memorySize: 10000,       // Size of the replay buffer
    gamma: 0.99f,          // Discount factor for rewards
    gaeLambda: 0.95f,      // Lambda factor for Generalized Advantage Estimation
    lr: 1e-4f,            // Learning rate
    width: 512,
    depth: 2,
    clipEpsilon: 0.2f,     // Clipping factor for PPO's objective function
    vClipRange: 0.2f,      // Clipping range for value loss
    cValue: 0.5f,          // Coefficient for value loss
    ppoEpochs: 3,            // Number of PPO epoch
    clipGradNorm: 0.5f,
    entropyCoefficient: 0.005f,
    useRNN: false
   );

var optsdqn = new DQNAgentOptions(numAtoms: 51,
    batchedActionProcessing: true,
    boltzmannExploration: true,
    prioritizedExperienceReplay: true,
    nStepReturn: 200, duelingDQN: true,
    doubleDQN: true, noisyLayers: true,
    noisyLayersScale: 0.02f,
    categoricalDQN: true,
    batchSize: 128,
    memorySize: 10000,
    gamma: 0.99f,
    epsStart: 1f,
    epsEnd: 0.05f,
    epsDecay: 150f,
    tau: 0.005f,
    lr: 5e-3f,
    width: 512,
    depth: 2);



var env = new List<IContinuousEnvironmentAsync<float[]>> { new TrivialContinuousEnvironmentAsync(), new TrivialContinuousEnvironmentAsync(), new TrivialContinuousEnvironmentAsync(), new TrivialContinuousEnvironmentAsync(), };
//----------------------------------can use PPO options \/ or DQN options
var myAgent = new LocalContinuousRolloutAgent<float[]>(optsppo, env, myChart);

/*
var env = new List<IEnvironmentAsync<float[]>> { new CartPoleAsync(), new CartPoleAsync(), new CartPoleAsync(), new CartPoleAsync(), };
//----------------------------------can use PPO options \/ or DQN options
var myAgent = new LocalDiscreteRolloutAgent<float[]>(optsppo, env, myChart);
*/

//var savePath = @"C:\temp";
//await myAgent.Load(savePath);
for (int i = 0; i < 1200; i++)
{
   await myAgent.Step();
}
//await myAgent.Save(savePath);

Console.WriteLine("doneTraining");

for (int i = 0; i < 800; i++)
{
    await myAgent.Step(false);

}

Console.WriteLine("doneTesting");


Console.ReadLine();