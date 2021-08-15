using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace PriorityQueue
{
    public class HeapPriorityQueue
    {
        private Dictionary<string, int> descriptionToIndexMapping = new Dictionary<string, int>();
        private (string description, int priority)[] _pairs;
        private int tailIndex = 0;

        public HeapPriorityQueue(int estimateQueueSize, (string description, int priority)[] initial = null)
        {
            _pairs = new (string description, int priority)[estimateQueueSize];
            if (initial != null)
            {
                foreach (var element in initial)
                {
                    _pairs[tailIndex] = element;
                    descriptionToIndexMapping[element.description] = tailIndex;
                    tailIndex++;
                }
                Heapify();
            } 
        }
        
        //Note this does not do dynamic array resizing
        public void Insert(string element, int priority)
        {
            _pairs[tailIndex] = (element, priority);
            descriptionToIndexMapping[element] = tailIndex;
            tailIndex++;
            BubbleUpOptimized();
        }

        public void Update(string description, int priority)
        {
            if (!descriptionToIndexMapping.ContainsKey(description))
                throw new Exception("Key not found");
            var index = descriptionToIndexMapping[description];
            var oldPriority = _pairs[index].priority;
            _pairs[index] = (description, priority);
            if (priority > oldPriority)
                BubbleUpOptimized(index);
            else
                PushDownOptimized(index);
        }

        private void Heapify()
        {
            for (int i = (tailIndex - 2) / 2; i >= 0; i--)
            {
                PushDownOptimized(i);
            }
        }

        public (string description, int priority) Peek()
        {
            if (tailIndex == 0)
                throw new Exception("Empty");
            return _pairs[0];
        }

        public (string description, int priority) Top()
        {
            if (tailIndex == 0)
                throw new Exception("Empty");
            var lastElement = _pairs[tailIndex - 1];
            tailIndex--;
            if (tailIndex == 0)
            {
                descriptionToIndexMapping.Clear();
                return lastElement;
            }
            else
            {
                var firstElement = _pairs[0];
                descriptionToIndexMapping.Remove(firstElement.description);
                _pairs[0] = lastElement;
                descriptionToIndexMapping[lastElement.description] = 0;
                PushDownOptimized();
                return firstElement;
            }
        }

        private void BubbleUpOptimized(int? indexToBubble = null)
        {
            var index = indexToBubble ?? tailIndex - 1;
            var current = _pairs[index];
            while (index > 0)
            {
                var parentIndex = GetParentIndex(index);
                if (_pairs[parentIndex].priority < current.priority)
                {
                    _pairs[index] = _pairs[parentIndex];
                    descriptionToIndexMapping[_pairs[parentIndex].description] = index;
                    index = parentIndex;
                }
                else
                {
                    break;
                }
            }

            _pairs[index] = current;
            descriptionToIndexMapping[current.description] = index;
        }

        private void PushDownOptimized(int? indexToPush = null)
        {
            var currentIndex = indexToPush ?? 0;
            var current = _pairs[currentIndex];
            while (currentIndex < FirstLeafIndex())
            {
                ((string description, int priority), int childIndex) = HighestPriorityChild(currentIndex);
                if (priority > current.priority)
                {
                    _pairs[currentIndex] = _pairs[childIndex];
                    descriptionToIndexMapping[_pairs[childIndex].description] = currentIndex;
                    currentIndex = childIndex;
                }
                else
                {
                    break;
                }
            }

            _pairs[currentIndex] = current;
            descriptionToIndexMapping[current.description] = currentIndex;
        }

        private ((string description, int priority), int childIndex) HighestPriorityChild(int index)
        {
            var fcIndex = FirstChildIndex(index);
            var scIndex = SecondChildIndex(index);
            var firstChild = (_pairs[fcIndex], fcIndex);
            var secondChild = (_pairs[scIndex], scIndex);
            if (firstChild.Item1.priority > secondChild.Item1.priority)
                return firstChild;
            else
                return secondChild;
        }

        private void Swap(int currentIndex, int parentIndex)
        {
            (_pairs[currentIndex], _pairs[parentIndex]) = (_pairs[parentIndex], _pairs[currentIndex]);
        }

        private int FirstLeafIndex()
        {
            return (tailIndex - 3) / 2 + 1;
        }

        private int FirstChildIndex(int index)
        {
            return 2 * index + 1;
        }

        private int SecondChildIndex(int index)
        {
            return 2 * (index + 1);
        }
        
        private int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }
    }
}