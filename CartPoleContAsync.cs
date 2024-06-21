using System;
using System.Threading.Tasks;
using Gym.Environments.Envs.Classic;
using Gym.Rendering.WinForm;
using OneOf;
using RLMatrix;

public class CartPoleContAsync : IContinuousEnvironmentAsync<float[]>
{
    public int stepCounter { get; set; }
    public int maxSteps { get; set; }
    public bool isDone { get; set; }
    public OneOf<int, (int, int)> StateSize { get; set; }
    public int[] DiscreteActionSize { get; set; }
    (float min, float max)[] IContinuousEnvironmentAsync<float[]>.ContinuousActionBounds { get => new (float min, float max)[0];
 set => throw new NotImplementedException(); }

    private CartPoleEnv myEnv;
    private float[] myState;

    public CartPoleContAsync()
    {
        Task.Run(async () => await InitialiseAsync()).Wait(); // Initialize in constructor asynchronously
    }

    public Task<float[]> GetCurrentState()
    {
        if (isDone)
            Reset().Wait(); // Reset if done

        return Task.FromResult(myState ?? new float[4] { 0, 0, 0, 0 });
    }

    public async Task InitialiseAsync()
    {
        myEnv = new CartPoleEnv(WinFormEnvViewer.Factory);
        stepCounter = 0;
        maxSteps = 100000;
        StateSize = myEnv.ObservationSpace.Shape.Size;
        DiscreteActionSize = new int[] { myEnv.ActionSpace.Shape.Size };
        await Task.Run(() => myEnv.Reset()); // Assuming Reset is not async; wrap in Task.Run
        isDone = false;
        myState = new float[4]; // Initialize state with default values
    }

    public Task Reset()
    {
        return Task.Run(() =>
        {
            myEnv.Reset(); // Assuming Reset is not async; wrap in Task.Run
            myState = new float[4] { 0, 0, 0, 0 };
            isDone = false;
            stepCounter = 0;
        });
    }

    Task<(float reward, bool done)> IContinuousEnvironmentAsync<float[]>.Step(int[] discreteActions, float[] continuousActions)
    {
        return Task.Run(() =>
        {
            if (isDone)
                Reset().Wait(); // Reset if done

            var actionId = discreteActions[0];
            var (observation, reward, done, information) = myEnv.Step(actionId);
            SixLabors.ImageSharp.Image img = myEnv.Render();
            myState = observation.ToFloatArray();
            isDone = done;

            stepCounter++;
            if (stepCounter > maxSteps)
                isDone = true;

            if (isDone)
                reward = 0;

            return (reward, isDone);
        });
    }
}
