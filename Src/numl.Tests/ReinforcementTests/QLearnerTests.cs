using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.AI;
using numl.AI.Functions;
using numl.Reinforcement;
using numl.Model;
using numl.Math.LinearAlgebra;

using Xunit;

namespace numl.Tests.ReinforcementTests
{
    [Trait("Category", "Reinforcement")]
    public class QLearnerTests
    {
        [Fact]
        public void Test_QLearning_Path_Finder()
        {
            // start 
            var master = new MDPState(2);
            var kitchen = new MDPState(3);
            master.Successors.Add(new MDPSuccessorState(new AI.Action(1, "Goto Kitchen"), 0.1, kitchen, 0));

            var entrance = new MDPState(1);
            var lounge = new MDPState(4);
            kitchen.Successors.Add(new MDPSuccessorState(new AI.Action(2, "Goto Lounge"), 0.1, lounge, -15));
            kitchen.Successors.Add(new MDPSuccessorState(new AI.Action(3, "Goto Entrance Hall"), 0, entrance, -30));

            var spare = new MDPState(0);
            lounge.Successors.Add(new MDPSuccessorState(new AI.Action(4, "Goto Spare Room"), 0.1, spare, -10));

            var outside = new MDPState(5);
            lounge.Successors.Add(new MDPSuccessorState(new AI.Action(5, "Go Outside"), 0.1, outside, 30));
            entrance.Successors.Add(new MDPSuccessorState(new AI.Action(6, "Go Outside"), 0.1, outside, 50));
            outside.Successors.Add(new MDPSuccessorState(new AI.Action(7, "Stay Outside"), 0.2, outside, 50));

            var examples = MDPConverter.ToExamples(master);

            Assert.Equal(7, examples.Item1.Rows);
            Assert.Equal(7, examples.Item2.Length);
            Assert.Equal(7, examples.Item3.Rows);
            Assert.Equal(7, examples.Item4.Length);

            var generator = new Reinforcement.QLearning.QLearnerGenerator() { Lambda = 0.9 };
            Reinforcement.QLearning.QLearnerModel model = (Reinforcement.QLearning.QLearnerModel) generator.Generate(examples.Item1, examples.Item2, examples.Item3, examples.Item4);

            Assert.Equal(3, (int) model.Predict(kitchen.ToVector())/*, "Expected to move from kitchen to entrance hall"*/);
            Assert.Equal(5, (int) model.Predict(lounge.ToVector())/*, "Expected to move from lounge to outside"*/);
            Assert.Equal(7, (int) model.Predict(outside.ToVector())/*, "Expected to stay outside"*/);

            string path = "Start: " + master.Id; IMDPState current = master;
            int counter = 0;
            while (current.Id != outside.Id)
            {
                if (counter > 20) break;

                double v = model.Predict(current.ToVector());
                var next = current.GetSuccessors().Where(w => w.Action.Id == (int) v).FirstOrDefault() as IMDPSuccessor;
                if (next == null) break;

                current = next.State as IMDPState;

                counter++;

                path += $"\n next: { current.Id } ({ next.Reward.ToString("N2") })";
            }

            Console.Write(path);
        }
    }
}
