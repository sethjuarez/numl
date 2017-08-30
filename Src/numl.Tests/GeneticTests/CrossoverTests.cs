using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using numl.Genetic;
using numl.Genetic.Functions.Crossover;

using MATH = System.Math;

namespace numl.Tests.GeneticTests
{
    [Trait("Category", "Genetic")]
    public class CrossoverTests : BaseGenetic
    {
        [Fact]
        public void Test_Point_Crossover()
        {
            double prob = 0.25;
            int points = 5;
            int length = 50;

            while (prob <= 1.0)
            {
                var crossover = new PointCrossover(points) { Probability = prob };

                this.Test_Crossover(() => crossover, length,
                    (p1, p2, child) =>
                    {
                        bool result = false;

                        var sequence = child.Sequence;
                        
                        double count1 = p1.Sequence.Intersect(sequence).Count();
                        double count2 = p2.Sequence.Intersect(sequence).Count();
                        
                        double p = (MATH.Pow(prob, points)) / 2;
                        
                        double c1 = 0, c2 = 0;

                        int i; double diff; double t1 = 0, t2 = 0;
                        for (i = 0; i < crossover.Points.Length - 1; i++)
                        {
                            diff = (crossover.Points[i + 1] - crossover.Points[i]);

                            if (i % 2 == 0)
                            {
                                c1 += diff;
                                t1 += (diff / length) * (1.0 - p);
                            }
                            else
                            {
                                c2 += diff;
                                t2 += (diff / length) * p;
                            }
                        }

                        diff = length - crossover.Points[i];

                        if (i % 2 == 0)
                        {
                            c1 += diff;
                            t1 += (diff / length) * (1.0 - p);
                        }
                        else
                        {
                            c2 += diff;
                            t2 += (diff / length) * p;
                        }

                        t1 = t1 / (t1 + t2);
                        t2 = t2 / (t1 + t2);

                        result = (t1 >= (1.0 - p * 2.0) && t2 <= p * 2.0);

                        return result;
                    });

                prob += 0.25;
            }
        }
    }
}
