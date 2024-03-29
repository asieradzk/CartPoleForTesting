﻿using CartPoleForTesting;
using RLMatrix;
using RLMatrix.Agents.PPO.Variants;
using RLMatrix.WinformsChart;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var myChart = new WinformsChart();

//PPO
var optsppo = new PPOAgentOptions(
    batchSize: 1,           // Number of EPISODES agent interacts with environment before learning from its experience
    memorySize: 10000,       // Size of the replay buffer
    gamma: 0.99f,          // Discount factor for rewards
    gaeLambda: 0.95f,      // Lambda factor for Generalized Advantage Estimation
    lr: 1e-5f,            // Learning rate
    width: 1024,
    depth: 2,
    clipEpsilon: 0.2f,     // Clipping factor for PPO's objective function
    vClipRange: 0.2f,      // Clipping range for value loss
    cValue: 0.5f,          // Coefficient for value loss
    ppoEpochs: 5,            // Number of PPO epochs
    clipGradNorm: 0.5f, 
    entropyCoefficient: 0.1f,
    displayPlot: myChart,
    useRNN: true
   );

var envppo = new List<IEnvironment<float[]>> { new SequencePushEnv() };
var myAgentppo = new PPOAgent<float[]>(optsppo, envppo);

//var myenv2ppo = new List<IEnvironment<float[]>>{ new TwoNumbersGame1d() };
//var myAgent2ppo = new PPOForTesting<float[]>(optsppo, myenv2ppo);

for (int i = 0; i < 10000000; i++)
{
     myAgentppo.Step();
    //myAgent2ppo.Step();sss
}



Console.ReadLine();





//DQN
var opts = new DQNAgentOptions(batchSize: 32, memorySize: 10000, gamma: 0.99f, epsStart: 1f, epsEnd: 0.05f, epsDecay: 50f, tau: 0.005f, lr: 1e-4f, displayPlot: myChart);
var env = new List<IEnvironment<float[]>> { new CartPole(), new CartPole() };
var myAgent = new DQNAgent<float[]>(opts, env);

//var myenv2 = new TwoNumbersGame1d();
//var myAgent2 = new D2QNAgent<float[]>(opts, myenv2);





for (int i = 0; i < 10000; i++)
{
    myAgent.Step();
  // myAgent2.TrainEpisode();


}


Console.ReadLine();