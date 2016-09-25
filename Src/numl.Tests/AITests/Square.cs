using System;
using numl.AI;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using numl.Data;

namespace numl.Tests.AITests
{
    public class Square : IState
    {
        private static int _Id = 0;

        public int Id { get; set; }
        public bool IsTerminal { get; private set; }

        private readonly int[] _square = new int[9];
        private readonly Action[] _moves = new string[] { "Left", "Right", "Up", "Down" }
                                                    .Select(s => new Action(s)).ToArray();
        private readonly int[] _modeIdx = new[] { -1, 1, -3, 3 };
        public Square(int[] square)
        {
            _square = square;
            IsTerminal = CalculateTerminal(_square);
            Id = ++_Id;
        }

        private static bool CalculateTerminal(int[] square)
        {
            for (int i = 0; i < square.Length; i++)
                if (i != square[i])
                    return false;
            return true;
        }
        public IEnumerable<ISuccessor> GetSuccessors()
        {
            for (int i = 0; i < 4; i++)
            {
                var move = _moves[i];
                int idx = Array.IndexOf(_square, 0);

                if (Test(idx, move))
                {
                    var shift = _modeIdx[i];
                    yield return new SquareMove(Swap(idx, idx + shift), move);
                }
            }
        }
                
        public static bool Test(int idx, IAction action)
        {
            if (action.Name == "Left" && idx % 3 != 0)
                return true;
            else if (action.Name == "Right" && (idx + 1) % 3 != 0)
                return true;
            else if (action.Name == "Up" && idx > 2)
                return true;
            else if (action.Name == "Down" && idx < 6)
                return true;
            else
                return false;
        }

        private IState Swap(int a, int b)
        {
            var newSquare = (int[])_square.Clone();
            var t = newSquare[a];
            newSquare[a] = newSquare[b];
            newSquare[b] = t;
            return new Square(newSquare);
        }
        
        public bool IsEqualTo(IVertex state)
        {
            if (state == null) return false;
            if (!(state is Square)) return false;
            Square square = (Square)state;
            for (int i = 0; i < _square.Length; i++)
                if (_square[i] != square._square[i])
                    return false;
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _square.Length; i++)
            {
                sb.Append(string.Format("  {0} ", _square[i] == 0 ? "_" : _square[i].ToString()));
                if ((i + 1) % 3 == 0) sb.Append("\n");
            }
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            return IsEqualTo(obj as IState);
        }

        public int CompareTo(object obj)
        {
            return StateComparer.Compare(this, obj as IState);
        }

        public override int GetHashCode()
        {
            return _square.GetHashCode();
        }

        public double Heuristic()
        {
            // heuristic for missplaced items
            int problem = 0;
            for (int i = 0; i < _square.Length; i++)
                if (i != _square[i]) problem++;
            return problem;
        }
    }

    public class SquareMove : ISuccessor
    {

        public SquareMove(IState state, IAction action)
        {
            State = state;
            Action = action;
        }

        public double Cost { get { return 1; } }
        public IAction Action { get; private set; }
        public IState State { get; private set; }
    }
}
