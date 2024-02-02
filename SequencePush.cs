using OneOf;
using RLMatrix;

public class SequencePushEnv : IEnvironment<float[]>
{

    public int stepCounter { get; set; }
    public int maxSteps { get; set; }
    public bool isDone { get; set; }
    public OneOf<int, (int, int)> stateSize { get; set; }
    public int[] actionSize { get; set; }

    public SequencePushEnv()
    {
        Initialise();
    }

    float state;
    int direction;
    Random random = new Random();
    public float[] GetCurrentState()
    {

        return new float[] { stepCounter, state};
    }
    float previousStep;

    public void Initialise()
    {
        maxSteps = 50;
        isDone = false;
        stateSize = 2;
        actionSize = new int[] { 3 };
        direction = random.Next(0, 3);
        Console.WriteLine($"Direction is {direction}");
        stepCounter = 1;
        state = (direction + 2)*10f;
        previousStep = 0f;
    }

    public void Reset()
    {
        Initialise();
    }

    public float Step(int[] actionsIds)
    {
        //stepping
        stepCounter++;
        previousStep = actionsIds[0];
        if(stepCounter > 4)
        {
            state = -1f;
        }

        if(stepCounter > maxSteps)
        {
            isDone = true;
        }

        if (actionsIds[0] == direction )
        {
            return 1f;
        }
        else
        {
            return -1f;
        }
    }
}


