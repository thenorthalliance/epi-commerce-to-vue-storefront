using System.Collections.Generic;
using System.Diagnostics;

namespace EPiServer.Vsf.Core
{
    public static class CallTest
    {
        public class TestTast
        {
            private readonly string _apiCallName;
            private Stopwatch sw = new Stopwatch();

            public TestTast(string apiCallName)
            {
                _apiCallName = apiCallName;
                sw.Start();
            }

            public void EndTask()
            {
                CallTest.EndStart(_apiCallName, sw.ElapsedMilliseconds);
            }
        }

        private static Dictionary<string, int> counters = new Dictionary<string, int>();
        private static Dictionary<string, long> totalTime = new Dictionary<string, long>();

        public static TestTast Start(string type)
        {
            return new TestTast(type);
        }

        public static void EndStart(string type, long duration)
        {
            if(!counters.ContainsKey(type))
                counters.Add(type, 0);
            
            if(!totalTime.ContainsKey(type))
                totalTime.Add(type, 0);


            counters[type] += 1;
            totalTime[type] += duration;
        }

        public static void PrintCnter()
        {
            Debug.WriteLine("Incstumentaion ......");

            foreach (var k in totalTime.Keys)
            {
                Debug.WriteLine($"[{k}] cnt: {counters[k]}, tt: {totalTime[k]}, mt: {(double) totalTime[k] / (double) counters[k]}");
            }
        }

        public static void Reset()
        {
            counters.Clear();
            totalTime.Clear();
        }
    }
}