using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using numl.AI;

namespace numl.Tests.AITests
{
    [TestFixture, Category("AI")]
    public class SquareTests
    {
        private static int[] CreateSquare(int i)
        {
            var square = new int[9];
            square[i] = 0;
            for (int j = 0; j < 8; j++)
            {
                if (j >= i)
                    square[j + 1] = j + 1;
                else
                    square[j] = j + 1;
            }
            return square;
        }

        private static void PrintSolution(IEnumerable<ISuccessor> solution)
        {
            foreach (var successor in solution)
            {
                //Console.WriteLine(successor.Action);
                //Console.WriteLine(successor.State);
            }
        }

        [Test]
        public void Test_Square_Expansion()
        {
            for (int i = 0; i < 9; i++)
            {
                int[] square = CreateSquare(i);
                IState init = new Square(square);
                Console.WriteLine(init.ToString());
                foreach (var s in init.GetSuccessors())
                {
                    Console.WriteLine("---------\n{0} ({1}{2})", s.Action, s.Cost, s.State.IsTerminal ? ", Goal" : "");
                    Console.WriteLine(s.State);
                }

                Console.WriteLine("------------------------------------------");
            }
        }

        [Test]
        public void Test_Easy_Square_BFS()
        {
            IState init = new Square(new[] { 1, 4, 2, 3, 5, 8, 6, 0, 7 });
            Console.WriteLine(init);
            SimpleSearch bfs = new SimpleSearch(new BreadthFirstSearch());
            var solution = bfs.Find(init);

            if (solution) PrintSolution(bfs.Solution);
            Assert.IsTrue(solution);
        }


        [Test]
        public void Test_Hard_AStar()
        {
            IState init = new Square(new[] { 1, 2, 3, 4, 5, 6, 7, 0, 8 });

            AStarSearch strategy = new AStarSearch()
            {
                Heuristic = s => s.Heuristic()
            };

            SimpleSearch a = new SimpleSearch(strategy);
            var solution = a.Find(init);
            if (solution) PrintSolution(a.Solution);
            Assert.IsTrue(solution);
        }

    }
}
