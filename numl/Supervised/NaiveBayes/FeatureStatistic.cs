using System;
using numl.Utils;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Model;
using numl.Math;
using System.Drawing;
using System.Collections;

namespace numl.Supervised.NaiveBayes
{
    public class FeatureStatistic
    {
        private Dictionary<double, Dictionary<Range, double>> _conditional;
        private Dictionary<double, double> _statistic;
        public bool Discrete { get; set; }

        public FeatureStatistic(Vector x, Vector y, int width = 0)
        {
            Discrete = width <= 0;

            if (width <= 0)
                Initialize(x.Select(d => Range.Make(d, d)), y.Distinct());
            else
                Initialize(x.Segment(width), y.Distinct());

            Count(x, y);
            Normalize();
        }

        public double Compute(Vector x)
        {

            return 0;
        }

        private void Initialize(IEnumerable<Range> x, IEnumerable<double> y)
        {
            _conditional = new Dictionary<double, Dictionary<Range, double>>();
            _statistic = new Dictionary<double, double>();
            foreach (double item in y)
            {
                _conditional[item] = new Dictionary<Range, double>();
                _statistic[item] = 0;
                foreach (Range range in x.OrderBy(d => d.Min))
                {
                    _conditional[item][range] = 1;
                    _statistic[item]++;
                }
            }
        }

        private void Count(Vector x, Vector y)
        {
            if (x.Length != y.Length)
                throw new InvalidOperationException();

            for (int i = 0; i < x.Length; i++)
            {
                var range = FindRange(x[i], y[i]);
                if (range == null) throw new IndexOutOfRangeException();
                _conditional[y[i]][range]++;
                _statistic[y[i]]++;
            }
        }

        private void Normalize()
        {
            var sum = _statistic.Select(kv => kv.Value).Sum();
            for (int i = 0; i < _statistic.Keys.Count; i++)
            {
                var key = _statistic.Keys.ElementAt(i);
                _statistic[key] /= sum;
                var cond = _conditional[key];
                var csum = cond.Select(kv => kv.Value).Sum();
                for (int j = 0; j < cond.Keys.Count; j++)
                {
                    var ckey = cond.Keys.ElementAt(j);
                    _conditional[key][ckey] /= csum;
                }
            }
        }

        //public double this[double x, double y]
        //{
        //    get
        //    {
        //        var range = FindRange(x, y);
        //        if (range == null) throw new IndexOutOfRangeException();
        //        return _conditional[y][range];
        //    }
        //    set
        //    {
        //        var range = FindRange(x, y);
        //        if (range == null) throw new IndexOutOfRangeException();
        //        _conditional[y][range] = value;
        //    }
        //}

        

        private Range FindRange(double x, double y)
        {
            var cond = _conditional[y];
            foreach (var i in cond)
                if (Discrete && i.Key.Min == x)
                    return i.Key;
                else if (i.Key.Test(x))
                    return i.Key;
            return null;
        }


    }
}
