using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class newMH
{
    public List<PathfindingNode> heap = Enumerable.Repeat(default(PathfindingNode), 2000).ToList();
    protected int IndexCount = -1;
    public int listMax
    {
        get { return IndexCount; }
    }
    void swap(int x, int y)
    {
        PathfindingNode temp = heap[x];
        heap[x] = heap[y];
        heap[x].HeapPos = heap[y].HeapPos;
        heap[y] = temp;
        heap[y].HeapPos = temp.HeapPos;

    }
    public void UpShift(int n)
    {
        while (n > 0 && (heap[n].Fcost < heap[(n - 1) / 2].Fcost || (heap[n].Fcost == heap[(n - 1) / 2].Fcost && heap[n].Hcost < heap[(n - 1) / 2].Hcost)))
        {
            swap(n, (n - 1) / 2);
            n = (n - 1) / 2;
        }
    }
    public void Insert(PathfindingNode n)
    {
        IndexCount += 1;
        heap[IndexCount] = n;
        n.inopen = true;
        n.HeapPos = IndexCount;
        UpShift(IndexCount);

    }
    void DownShift(int n)
    {
        int M = n;
        int left = (n * 2) + 1;
        int right = (n * 2) + 2;
        if (left <= IndexCount && heap[left].Fcost < heap[M].Fcost)
        {
            M = left;
        }
        if (right <= IndexCount && heap[right].Fcost < heap[M].Fcost)
        {
            M = right;
        }
        if (n != M)
        {
            swap(n, M);
            DownShift(M);
        }
    }
    public void Remove(int n)
    {
        heap[n].inopen = false;
        heap[n] = heap[IndexCount];
        heap[IndexCount] = null;
        IndexCount -= 1;
        DownShift(n);
    }

    public void clear()
    {
        for(int x = 0; x <= IndexCount; x++)
        {
            Debug.Log(heap[x].passable);
        }
    }
    public PathfindingNode Takemin()
    {
        PathfindingNode min = heap[0];
        min.inopen = false;
        heap[0] = heap[IndexCount];
        heap[0].HeapPos = 0;
        heap[IndexCount] = null;
        IndexCount -= 1;
        DownShift(0);
        return min;
    }
}
