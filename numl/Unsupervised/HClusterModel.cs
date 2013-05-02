﻿using System;
using numl.Model;
using System.Linq;
using numl.Math.Linkers;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Unsupervised
{
    public class HClusterModel
    {
        public Descriptor Descriptor { get; set; }
        public ILinker Linker { get; set; }

        public Cluster Generate(Descriptor desc, IEnumerable<object> examples, ILinker linker)
        {
            // Load data 
            var exampleArray = examples.ToArray();
            Descriptor = desc;
            Matrix X = Descriptor.Convert(examples).ToMatrix();

            return GenerateClustering(X, linker, exampleArray);
        }

        public Cluster Generate(Matrix x, ILinker linker)
        {
            return GenerateClustering(x, linker);
        }

        private Cluster GenerateClustering(Matrix X, ILinker linker, object[] data = null)
        {
            // Initialize
            Linker = linker;

            var clusters = new List<Cluster>();
            var distances = new Dictionary<Tuple<int, int>, double>();

            // Create a new cluster for each data point
            for (int i = 0; i < X.Rows; i++)
                clusters.Add(new Cluster
                {
                    Id = i,
                    Points = new Vector[] { (Vector)X.Row(i) },
                    Members = data != null ? new object[] { data[i] } : new object[] { X.Row(i) }
                });

            // Set the current closest distance/pair to the first pair of clusters
            var key = new Tuple<int, int>(0, 0);
            var distance = 0.0;

            var clusterId = X.Rows;

            while (clusters.Count > 1)
            {
                var closestClusters = new Tuple<int, int>(0, 1);
                var smallestDistance = Linker.Distance(clusters[0].Points, clusters[1].Points);

                // this needs to be parallelized....
                // Loop through each of the clusters looking for the two closest
                for (int i = 0; i < clusters.Count; i++)
                {
                    for (int j = i + 1; j < clusters.Count; j++)
                    {
                        key = new Tuple<int, int>(clusters[i].Id, clusters[j].Id);

                        // Cache the distance if it hasn't been calculated yet
                        if (!distances.ContainsKey(key))
                            distances.Add(key, Linker.Distance(clusters[i].Points, clusters[j].Points));

                        // Update closest clusters and distance if necessary
                        distance = distances[key];

                        if (distance < smallestDistance)
                        {
                            smallestDistance = distance;
                            closestClusters = new Tuple<int, int>(i, j);
                        }
                    }
                }

                // order clusters by distance
                var min = System.Math.Min(closestClusters.Item1, closestClusters.Item2);
                var max = System.Math.Max(closestClusters.Item1, closestClusters.Item2);

                var newCluster = new Cluster(clusterId, clusters[min], clusters[max]);

                // Remove the merged clusters
                clusters.RemoveAt(min);
                clusters.RemoveAt(max - 1);

                // Add new cluster
                clusters.Add(newCluster);
                clusterId++;
            }

            return clusters.Single();
        }
    }
}
