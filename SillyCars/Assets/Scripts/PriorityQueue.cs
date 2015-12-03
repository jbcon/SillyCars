using System;
using System.Collections.Generic;

public class PriorityQueue<T>
	where T : IComparable<T>, IEquatable<T>
{
	private List<T> heap;

	public PriorityQueue()
	{
		heap = new List<T>();
	}

	public void Push(T val)
	{
		heap.Add(val);
		BubbleUp(heap.Count - 1);
	}

	public T Pop()
	{
		if (heap.Count == 0)
			throw new InvalidOperationException("Empty queue");

		T val = heap[0];
		heap[0] = heap[heap.Count - 1];
		heap.RemoveAt(heap.Count - 1);
		TrickleDown(0);

		return val;
	}

	public T Peek()
	{
		return heap[0];
	}

	public void Clear()
	{
		heap.Clear();
	}

	private void BubbleUp(int n)
	{
		T val = heap[n];

		for (int i = n / 2; n > 0 && val.CompareTo(heap[i]) > 0; n = i, i /= 2)
			heap[n] = heap[i];

		heap[n] = val;
	}

	private void TrickleDown(int n)
	{
		T val = heap[n];
		for (int i = n * 2; i < heap.Count; n = i, i *= 2)
		{
			if (i + 1 < heap.Count && heap[i + 1].CompareTo(heap[i]) > 0)
				i++;

			if (val.CompareTo(heap[i]) >= 0)
				break;

			heap[n] = heap[i];
		}

		heap[n] = val;
	}
}
