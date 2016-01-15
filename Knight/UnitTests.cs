using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsChess
{
    class UnitTests
    {
        static void Main(string[] args)
        {
            new Solution().CalcMinNumberOfMoves(new Point(Int32.MinValue, Int32.MinValue), new Point(Int32.MaxValue, Int32.MaxValue));
            //new Solution().CalcMinNumberOfMoves(new Point(0, 0), new Point(Int32.MaxValue, Int32.MaxValue)); 


            //each testcase tests the inverse and negatives of each case as well 
            TestCase[] testCases = {
                                       
                                       new TestCase(0, 11, 7), 
                                       new TestCase(0, 0, 0), 
                                       new TestCase(1, 0, 3), 
                                       new TestCase(1, 1, 2), 
                                       new TestCase(2, 1, 1), 
                                       new TestCase(2, 2, 4), 

                                       new TestCase(10, 2, 6), 
                                       new TestCase(10, 3, 5), 
                                       new TestCase(11, 7, 6), 
                                       new TestCase(15, 12, 9), 
                                       new TestCase(15, 11, 10), 

                                       new TestCase(5, 4, 3), 
                                       new TestCase(5, 5, 4), 
                                       new TestCase(5, 6, 5), 
                                       new TestCase(5, 7, 4), 
                                       new TestCase(5, 8, 5), 
                                       new TestCase(5, 9, 6), 
                                       new TestCase(6, 4, 4), 
                                       new TestCase(7, 4, 5), 
                                       new TestCase(8, 4, 4), 
                                       new TestCase(9, 4, 5), 
                                       new TestCase(10, 4, 6), 

                                       new TestCase(4, 4, 4), 
                                       new TestCase(3, 3, 2), 
                                       
                                       new TestCase(9, 0, 5), 
                                       new TestCase(10, 0, 6), 
                                       new TestCase(11, 0, 7), 
                                       new TestCase(12, 0, 6), 
                                       new TestCase(13, 0, 7), 
                                       new TestCase(14, 0, 8), 
                                       
                                       new TestCase(9, 1, 6), 
                                       new TestCase(10, 1, 5), 
                                       new TestCase(11, 1, 6), 
                                       new TestCase(12, 1, 7), 
                                       new TestCase(13, 1, 8), 
                                       new TestCase(14, 1, 7), 
                                       
                                       new TestCase(9, 2, 5), 
                                       new TestCase(10, 2, 6), 
                                       new TestCase(11, 2, 7), 
                                       new TestCase(12, 2, 6), 
                                       new TestCase(13, 2, 7), 
                                       new TestCase(14, 2, 8),
                                       
                                       new TestCase(9,  3, 6), 
                                       new TestCase(10, 3, 5), 
                                       new TestCase(11, 3, 6), 
                                       new TestCase(12, 3, 7), 
                                       new TestCase(13, 3, 8), 
                                       new TestCase(14, 3, 7), 
                                       
                                       new TestCase(9,  4, 5), 
                                       new TestCase(10, 4, 6), 
                                       new TestCase(11, 4, 7), 
                                       new TestCase(12, 4, 6), 
                                       new TestCase(13, 4, 7), 
                                       new TestCase(14, 4, 8),
                                       
                                       new TestCase(9,  5, 6), 
                                       new TestCase(10, 5, 5), 
                                       new TestCase(11, 5, 6), 
                                       new TestCase(12, 5, 7), 
                                       new TestCase(13, 5, 8), 
                                       new TestCase(14, 5, 7), 
                                       
                                       new TestCase(9,  6, 5), 
                                       new TestCase(10, 6, 6), 
                                       new TestCase(11, 6, 7), 
                                       new TestCase(12, 6, 6), 
                                       new TestCase(13, 6, 7), 
                                       new TestCase(14, 6, 8), 
                                       
                                       new TestCase(9,  7, 6), 
                                       new TestCase(10, 7, 7), 
                                       new TestCase(11, 7, 6), 
                                       new TestCase(12, 7, 7), 
                                       new TestCase(13, 7, 8), 
                                       new TestCase(14, 7, 7), 
                                       
                                       new TestCase(9,  8, 7), 
                                       new TestCase(10, 8, 6), 
                                       new TestCase(11, 8, 7), 
                                       new TestCase(12, 8, 8), 
                                       new TestCase(13, 8, 7), 
                                       new TestCase(14, 8, 8), 
                                       
                                       new TestCase(9,  9, 6), 
                                       new TestCase(10, 9, 7), 
                                       new TestCase(11, 9, 8), 
                                       new TestCase(12, 9, 7), 
                                       new TestCase(13, 9, 8), 
                                       new TestCase(14, 9, 9), 
                                       
                                       new TestCase(9,  10, 7), 
                                       new TestCase(10, 10, 8), 
                                       new TestCase(11, 10, 7), 
                                       new TestCase(12, 10, 8), 
                                       new TestCase(13, 10, 9), 
                                       new TestCase(14, 10, 8)
            };

            int casesPassed = 0;
            int casesFailed = 0; 
            foreach (var testCase in testCases)
            {
                if (testCase.Run())
                    casesPassed++;
                else
                    casesFailed++; 
            }

            Console.WriteLine();
            Console.WriteLine("{0} cases passed. {1} cases failed.", casesPassed, casesFailed); 

            Console.ReadLine();
        }

        private class TestCase
        {
            private int _x;
            private int _y;
            private int _result;
            private Point _origin = Point.Origin;

            public TestCase(int x, int y, int result)
            {
                _x = x;
                _y = y;
                _result = result;
            }

            public TestCase(Point origin, int x, int y, int result)
            {
                _x = x;
                _y = y;
                _result = result;
            }

            public bool Run()
            {
                bool passed = true;

                if (!Run(_x, _y))
                    passed = false;
                if (!Run(_x * -1, _y))
                    passed = false;
                if (!Run(_x * -1, _y * -1))
                    passed = false;
                if (!Run(_x, _y * -1))
                    passed = false;

                if (_x != _y)
                {
                    if (!Run(_y, _x))
                        passed = false;
                    if (!Run(_y * -1, _x))
                        passed = false;
                    if (!Run(_y * -1, _x * -1))
                        passed = false;
                    if (!Run(_y, _x * -1))
                        passed = false;
                }

                return passed;
            }

            private bool Run(int x, int y)
            {
                var r = new Solution().CalcMinNumberOfMoves(_origin, new Point(x, y));
                if (r != _result)
                {
                    Console.WriteLine("\n{0},{1} to {2},{3}, got {4}, should be {5}", _origin.X, _origin.Y, x, y, r, _result);
                    return false;
                }
                else
                {
                    Console.Write(".");
                    return true;
                }
            }
        }

        private class BenchmarkTimer
        {
            public void Start()
            {
            }

            public void Stop()
            {
            }
        }
    }
}
