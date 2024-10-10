using CartPoleForTesting.TestingToolkit;
using RLMatrix;
using RLMatrix.Agents.Common;
using RLMatrix.Environments;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var optsPPO = new PPOAgentOptions(
   batchSize: 128,        // Number of experiences used per training step
   memorySize: 10000,     // Total experiences stored for training
   gamma: 0.99f,          // Reduces importance of future rewards
   gaeLambda: 0.95f,      // Balances bias-variance in advantage estimation
   lr: 1e-3f,             // Step size for gradient descent
   width: 128,            // Neurons per hidden layer
   depth: 2,              // Number of hidden layers
   clipEpsilon: 0.2f,     // Limits policy update size
   vClipRange: 0.2f,      // Limits value function update
   cValue: 0.5f,          // Adjusts importance of value function loss
   ppoEpochs: 3,          // Training iterations per batch
   clipGradNorm: 0.5f,    // Prevents extreme gradient updates
   entropyCoefficient: 0.005f,  // Encourages exploration
   useRNN: true           // Enables memory for temporal dependencies
);

var optsDQN = new DQNAgentOptions(
   numAtoms: 51,                     // Discretization for value distribution
   batchedActionProcessing: false,   // Single vs. multiple action selection
   boltzmannExploration: false,      // Temperature-based action selection
   prioritizedExperienceReplay: true,  // Focuses on important experiences
   nStepReturn: 100,                   // Future steps considered for updates
   duelingDQN: true,                 // Separate value and advantage streams
   doubleDQN: true,                  // Reduces overestimation of Q-values
   noisyLayers: true,                // Adds adaptive exploration
   noisyLayersScale: 0.02f,          // Controls exploration via noise
   categoricalDQN: false,            // Learns value distribution
   batchSize: 32,                    // Experiences used per training step
   memorySize: 10000,                // Total experiences stored for training
   gamma: 0.99f,                     // Reduces importance of future rewards
   epsStart: 1f,                     // Initial exploration rate
   epsEnd: 0.05f,                    // Final exploration rate
   epsDecay: 150f,                   // Speed of exploration decay
   tau: 0.005f,                      // Rate of target network update
   lr: 5e-3f,                        // Step size for gradient descent
   width: 128,                       // Neurons per hidden layer
   depth: 2                          // Number of hidden layers
);


var env = new List<IEnvironmentAsync<float[]>> { new IdentityToolTestVectorObsDiscrete().RLInit(), new IdentityToolTestVectorObsDiscrete().RLInit(), new IdentityToolTestVectorObsDiscrete().RLInit(), new IdentityToolTestVectorObsDiscrete().RLInit(), };
//----------------------------------can use PPO options \/ or DQN options
var myAgent = new LocalDiscreteRolloutAgent<float[]>(optsPPO, env);



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