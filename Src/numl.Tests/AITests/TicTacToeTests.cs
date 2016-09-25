using System;
using System.Linq;
using System.Collections.Generic;

using numl.AI;
using numl.AI.Search;

using Xunit;

namespace numl.Tests.AITests
{
    [Trait("Category", "AI")]
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

        [Fact]
        public void Test_Expansion()
        {

            TicTacToe t = new TicTacToe(false, new[] { -1, 0, -1, 1, 0, 0, 0, 1, 0 });
            Console.WriteLine(t);
            foreach (var successor in t.GetSuccessors())
            {
                PrintSuccessor(successor);
            }
        }

        [Fact]
        public void SuperSimpleWinMinimax()
        {
            var m = new Minimax<IState, ISuccessor>();
            m.Depth = 1;
            var initial = new TicTacToe(false, new[] { -1, 0, -1, -1, 1, 1, 1, 0, 1 });
            m.Find(initial);
        }
    }
}
