using System;
using System.Threading.Tasks;
using OneOf;
using RLMatrix;

public class TrivialEnvironmentAsync : IEnvironmentAsync<float[]>
{
    public const float CorrectAnswerReward = 1;
    public const float WrongAnswerPenalty = -1;

    public float[] state;
    public int stepCounter { get; set; }
    public int maxSteps { get; set; } = 10;
    public bool isDone { get; set; }
    public OneOf<int, (int, int)> stateSize { get; set; } = 1;
    public int[] actionSize { get; set; } = new int[] { 2 };

    public TrivialEnvironmentAsync()
    {
        InitialiseAsync();
    }

    public Task<float[]> GetCurrentState()
    {
        if (isDone)
            Reset().Wait(); // Reset if done

        return Task.FromResult(state);
    }

    public void InitialiseAsync()
    {
        Reset();
    }

    public Task Reset()
    {
        state = new float[1] { RandomValue() };
        stepCounter = 0;
        isDone = false;
        return Task.CompletedTask;
    }

    public Task<(float, bool)> Step(int[] actionsIds)
    {
        if (isDone)
            Reset().Wait(); // Reset if done

        float input = state[0];
        float output = actionsIds[0];
        state[0] = RandomValue();

        if (stepCounter++ >= maxSteps)
        {
            isDone = true;
        }

        float reward = input == output ? CorrectAnswerReward : WrongAnswerPenalty;
        return Task.FromResult((reward, isDone));
    }

    private static int RandomValue()
    {
        return Random.Shared.Next(2);
    }
}