using CartPoleForTesting.TestingToolkit;
using RLMatrix;
using RLMatrix.Agents.Common;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var optsppo = new PPOAgentOptions(
    batchSize: 128,        // Nu8mber of EPISODES agent interacts with environment before learning from its experience
    memorySize: 10000,       // Size of the replay buffer
    gamma: 0.99f,          // Discount factor for rewards
    gaeLambda: 0.95f,      // Lambda factor for Generalized Advantage Estimation
    lr: 1e-3f,            // Learning rate
    width: 128,
    depth: 2,
    clipEpsilon: 0.2f,     // Clipping factor for PPO's objective function
    vClipRange: 0.2f,      // Clipping range for value loss
    cValue: 0.5f,          // Coefficient for value loss
    ppoEpochs: 3,            // Number of PPO epoch
    clipGradNorm: 0.5f,
    entropyCoefficient: 0.005f,
    useRNN: true
   );

var optsdqn = new DQNAgentOptions(numAtoms: 51,
            batchedActionProcessing: false,
            boltzmannExploration: false,
            prioritizedExperienceReplay: true,
            nStepReturn: 1, duelingDQN: true,
            doubleDQN: true, noisyLayers: true,
            noisyLayersScale: 0.02f,
            categoricalDQN: false,
            batchSize: 32,
            memorySize: 10000,
            gamma: 0.99f,
            epsStart: 1f,
            epsEnd: 0.05f,
            epsDecay: 150f,
            tau: 0.005f,
            lr: 5e-3f,
            width: 512,
            depth: 2);


var env = new List<IEnvironmentAsync<float[]>> { new IdentityToolTestDiscrete(), new IdentityToolTestDiscrete(), new IdentityToolTestDiscrete(), new IdentityToolTestDiscrete(), };
//----------------------------------can use PPO options \/ or DQN options
var myAgent = new LocalDiscreteRolloutAgent<float[]>(optsdqn, env);



/*
var env = new List<IContinuousEnvironmentAsync<float[]>> { new IdentityToolTest(), new IdentityToolTest(), };
//----------------------------------can use PPO options \/ or DQN options
var myAgent = new LocalContinuousRolloutAgent<float[]>(optsppo, env);
*/

//var savePath = @"C:\temp";
//await myAgent.Load(savePath);
for (int i = 0; i < 320000; i++)
{
    await myAgent.Step();
}
//await myAgent.Save(savePath);

Console.WriteLine("doneTraining");

for (int i = 0; i < 8000; i++)
{
    await myAgent.Step(false);

}

Console.WriteLine("doneTesting");


Console.ReadLine();