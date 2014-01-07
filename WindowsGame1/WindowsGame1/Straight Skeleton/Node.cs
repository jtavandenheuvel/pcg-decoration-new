/// <summary>
/// Represents a node
/// </summary>
/// <typeparam name="T"></typeparam>
using WindowsGame1.Straight_Skeleton;
public sealed class Node<T>
{
    /// <summary>
    /// Gets the Value
    /// </summary>
    public T Value { get; private set; }

    /// <summary>
    /// Gets next node
    /// </summary>
    public Node<T> Next { get; internal set; }

    /// <summary>
    /// Gets previous node
    /// </summary>
    public Node<T> Previous { get; internal set; }

    /// <summary>
    /// Initializes a new <see cref="Node"/> instance
    /// </summary>
    /// <param name="item">Value to be assigned</param>
    internal Node(T item)
    {
        this.Value = item;
    }

    public Node<T> NextActive()
    {
        Vertex ver = Next.Value as Vertex;
        if(ver != null) {
            if (ver.isActive())
            {
                return Next;
            }
            else
            {
                return Next.NextActive(Next);
            }
        }
        return null;
    }

    private Node<T> NextActive(Node<T> start)
    {
        Vertex ver = Next.Value as Vertex;
        if (ver != null)
        {
            if (start.Value.Equals(ver))
            {
                return start;
            }
            if (ver.isActive())
            {
                return Next;
            }
            else
            {
                return Next.NextActive(start);
            }
        }
        return null;
    }

    public Node<T> PrevActive()
    {
        Vertex ver = Previous.Value as Vertex;
        if (ver != null)
        {
            if (ver.isActive())
            {
                return Previous;
            }
            else
            {
                return Previous.PrevActive(Previous);
            }
        }
        return null;
    }

    private Node<T> PrevActive(Node<T> start)
    {
        Vertex ver = Previous.Value as Vertex;
        if (ver != null)
        {
            if (start.Value.Equals(ver))
            {
                return start;
            }
            if (ver.isActive())
            {
                return Previous;
            }
            else
            {
                return Previous.PrevActive(start);
            }
        }
        return null;
    }
}