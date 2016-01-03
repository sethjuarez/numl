using System;
using numl.AI;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

namespace numl.Tests.AITests
{
    [TestFixture, Category("AI")]
    public class TicTacToeTests
    {
        private static void PrintSolution(IEnumerable<ISuccessor> solution)
        {
            foreach (var successor in solution)
            {
                Console.WriteLine(successor.Action);
                Console.WriteLine(successor.State);
            }
        }

        private static void PrintSuccessor(ISuccessor successor)
        {
            Console.WriteLine("{0}\n{1}", successor.Action, successor.State);
            Console.WriteLine("{0} {1}\n--------\n", successor.State.IsTerminal, (successor.State as IAdversarialState).Utility);
        }

        [Test]
        public void Test_Expansion()
        {

            TicTacToe t = new TicTacToe(false, new[] { -1, 0, -1, 1, 0, 0, 0, 1, 0 });
            Console.WriteLine(t);
            foreach (var successor in t.GetSuccessors())
            {
                PrintSuccessor(successor);
            }
        }

        [Test]
        public void SuperSimpleWinMinimax()
        {
            Minimax m = new Minimax();
            m.Depth = 1;
            var initial = new TicTacToe(false, new[] { -1, 0, -1, -1, 1, 1, 1, 0, 1 });
            m.Find(initial);
        }
    }
}
