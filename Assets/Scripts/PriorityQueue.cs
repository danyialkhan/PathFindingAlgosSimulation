using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriorityQueue<T> where T : IComparable<T>
{
    List<T> data;

    public int Count { get { return data.Count; } }

    public PriorityQueue()
    {
        this.data = new List<T>();
    }

    public void Enqueue(T item)
    {
        data.Add(item);

        int childIndex = data.Count - 1;

        while(childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            
            if(data[childIndex].CompareTo(data[parentIndex]) >= 0) // if the priority of child is greator than the priority of parent
            {
                return;// then stop the loop no need to further sort
            }
            // else swap
            T temp = data[childIndex];
            data[childIndex] = data[parentIndex];
            data[parentIndex] = temp;
            // set child index = parent index
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        int lastIndex = data.Count - 1;

        T front = data[0];
        data[0] = data[lastIndex];

        data.RemoveAt(lastIndex);
        lastIndex--;
        int parentIndex = 0;

        while (true)
        {
            int childIndex = parentIndex * 2 + 1;
            if(childIndex > lastIndex)
            {
                break;
            }

            int rightChild = childIndex + 1;

            if(rightChild <= lastIndex && data[rightChild].CompareTo(data[childIndex]) < 0)// if the priority of the right child is less then the priority of left child
            {
                childIndex = rightChild; // then we will choose the right child as the childIndex
            }

            if(data[parentIndex].CompareTo(data[childIndex]) <= 0)
            {
                break;// they are already in the correct order
            }

            // else swap the items
            T temp = data[parentIndex];
            data[parentIndex] = data[childIndex];
            data[childIndex] = temp;

            parentIndex = childIndex;
        }

        return front;
    }

    public T Peek()
    {
        T item = data[0];
        return item;
    }

    public bool Contains(T item)
    {
        return data.Contains(item);
    }

    public List<T> ToList()
    {
        return data;
    }
}
