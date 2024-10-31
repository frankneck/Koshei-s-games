using System.Collections.Generic;

public class UICommandQueue
{
    private readonly Queue<IUICommand> queue = new();

    public bool TryEnqueueCommand(IUICommand ñommand)
    {
        queue.Enqueue(ñommand);
        return true;
    }

    public bool TryDequeueCommand(out IUICommand ñommand)
    {
        if (queue.Count > 0)
        {
            ñommand = queue.Dequeue();
            return true;
        }

        ñommand = default;
        return false;
    }
}
