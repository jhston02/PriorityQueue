using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PriorityQueue;
using Xunit;

namespace PriorityQueueTests
{
    public class PriorityQueueTests
    {
        public IEnumerable<(string description, int priority)> GetValues()
        {
            yield return ("test1", 1);
            yield return ("test2", 2);
            yield return ("test3", 3);
            yield return ("test4", 4);
            yield return ("test5", 5);
        }
        
        [Fact]
        public void PriorityQueue_InitialData_InPriorityOrder()
        {
            var pq = new HeapPriorityQueue(10, GetValues().ToArray());
            for (int i = 5; i <= 1; i--)
            {
                Assert.Equal((description: "test" + i, priority: i), pq.Top());
            }
        }

        [Fact]
        public void PriorityQueue_AddHighestPriority_CanPeek()
        {
            var pq = new HeapPriorityQueue(10, GetValues().ToArray());
            pq.Insert("test6", 6);
            Assert.Equal((description: "test6", priority: 6), pq.Peek());
        }

        [Fact]
        public void PriorityQueue_ChangeLastElementToHighPriority_CanPeek()
        {
            var pq = new HeapPriorityQueue(10, GetValues().ToArray());
            pq.Update("test1", 6);
            Assert.Equal((description: "test1", priority: 6), pq.Peek());
        }

        [Fact]
        public void PriorityQueue_ChangeFirstElementPriority_NoLongerTop()
        {
            var pq = new HeapPriorityQueue(10, GetValues().ToArray());
            pq.Update("test5", 3);
            Assert.Equal((description: "test4", priority: 4), pq.Peek());
        }
    }
}