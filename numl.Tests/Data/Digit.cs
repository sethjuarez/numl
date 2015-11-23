using numl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.Data
{
    public class Digit
    {
        /// <summary>
        /// An array of diode intensity values from an LED array.
        /// </summary>
        [EnumerableFeature(10)]
        public double[] Reading { get; set; }

        /// <summary>
        /// The number diodes with high intensity values.
        /// </summary>
        [Label]
        public int Label { get; set; }

        /// <summary>
        /// Returns a new dataset of Digits.
        /// </summary>
        /// <returns></returns>
        public static Digit[] GetTrainingDigits()
        {
            return new Digit[]
            {
                // 10's
                new Digit() { Label = 10, Reading = new double[] { 0.834,    0.571,    0.564,    0.761,    0.585,    0.632,    0.862,    0.756,    0.587,    0.880 } },
                new Digit() { Label = 10, Reading = new double[] { 0.618,    0.629,    0.753,    0.660,    0.691,    0.631,    0.759,    0.519,    0.906,    0.788 } },
                new Digit() { Label = 10, Reading = new double[] { 0.699,    0.577,    0.546,    0.852,    0.907,    0.770,    0.523,    0.855,    0.575,    0.952 } },
                new Digit() { Label = 10, Reading = new double[] { 0.846,    0.758,    0.594,    0.522,    0.654,    0.791,    0.910,    0.503,    0.768,    0.902 } },
                new Digit() { Label = 10, Reading = new double[] { 0.874,    0.693,    0.575,    0.514,    0.526,    0.890,    0.813,    0.558,    0.625,    0.886 } },
                new Digit() { Label = 10, Reading = new double[] { 0.645,    0.824,    0.573,    0.523,    0.802,    0.758,    0.526,    0.710,    0.853,    0.814 } },
                new Digit() { Label = 10, Reading = new double[] { 0.834,    0.571,    0.564,    0.761,    0.585,    0.632,    0.862,    0.756,    0.587,    0.880 } },
                new Digit() { Label = 10, Reading = new double[] { 0.618,    0.629,    0.753,    0.660,    0.691,    0.631,    0.759,    0.519,    0.906,    0.788 } },
                new Digit() { Label = 10, Reading = new double[] { 0.699,    0.577,    0.546,    0.852,    0.907,    0.770,    0.523,    0.855,    0.575,    0.952 } },
                new Digit() { Label = 10, Reading = new double[] { 0.846,    0.758,    0.594,    0.522,    0.654,    0.791,    0.910,    0.503,    0.768,    0.902 } },
                new Digit() { Label = 10, Reading = new double[] { 0.874,    0.693,    0.575,    0.514,    0.526,    0.890,    0.813,    0.558,    0.625,    0.886 } },
                new Digit() { Label = 10, Reading = new double[] { 0.645,    0.824,    0.573,    0.523,    0.802,    0.758,    0.526,    0.710,    0.853,    0.814 } },
                // 9's
                new Digit() { Label = 9,  Reading = new double[] { 0.044,    0.508,    0.683,    0.634,    0.535,    0.859,    0.536,    0.732,    0.507,    0.898 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.088,    0.757,    0.750,    0.505,    0.914,    0.507,    0.503,    0.533,    0.827,    0.518 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.182,    0.964,    0.731,    0.688,    0.808,    0.523,    0.687,    0.572,    0.719,    0.896 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.106,    0.701,    0.649,    0.953,    0.793,    0.867,    0.511,    0.940,    0.978,    0.667 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.142,    0.582,    0.552,    0.726,    0.667,    0.888,    0.575,    0.568,    0.608,    0.796 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.103,    0.993,    0.883,    0.836,    0.857,    0.645,    0.776,    0.994,    0.825,    0.513 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.044,    0.508,    0.683,    0.634,    0.535,    0.859,    0.536,    0.732,    0.507,    0.898 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.088,    0.757,    0.750,    0.505,    0.914,    0.507,    0.503,    0.533,    0.827,    0.518 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.182,    0.964,    0.731,    0.688,    0.808,    0.523,    0.687,    0.572,    0.719,    0.896 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.106,    0.701,    0.649,    0.953,    0.793,    0.867,    0.511,    0.940,    0.978,    0.667 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.142,    0.582,    0.552,    0.726,    0.667,    0.888,    0.575,    0.568,    0.608,    0.796 } },
                new Digit() { Label = 9,  Reading = new double[] { 0.103,    0.993,    0.883,    0.836,    0.857,    0.645,    0.776,    0.994,    0.825,    0.513 } },
                // 8's
                new Digit() { Label = 8,  Reading = new double[] { 0.069,    0.185,    0.515,    0.610,    0.633,    0.520,    0.609,    0.796,    0.711,    0.548 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.198,    0.025,    0.554,    0.661,    0.662,    0.875,    0.912,    0.506,    0.522,    0.920 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.178,    0.048,    0.962,    0.922,    0.640,    0.773,    0.817,    0.812,    0.597,    0.649 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.037,    0.007,    0.616,    0.966,    0.993,    0.890,    0.568,    0.941,    0.594,    0.648 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.112,    0.111,    0.856,    0.562,    0.799,    0.696,    0.940,    0.723,    0.674,    0.981 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.093,    0.109,    0.973,    0.905,    0.565,    0.554,    0.502,    0.592,    0.969,    0.860 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.069,    0.185,    0.515,    0.610,    0.633,    0.520,    0.609,    0.796,    0.711,    0.548 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.198,    0.025,    0.554,    0.661,    0.662,    0.875,    0.912,    0.506,    0.522,    0.920 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.178,    0.048,    0.962,    0.922,    0.640,    0.773,    0.817,    0.812,    0.597,    0.649 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.037,    0.007,    0.616,    0.966,    0.993,    0.890,    0.568,    0.941,    0.594,    0.648 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.112,    0.111,    0.856,    0.562,    0.799,    0.696,    0.940,    0.723,    0.674,    0.981 } },
                new Digit() { Label = 8,  Reading = new double[] { 0.093,    0.109,    0.973,    0.905,    0.565,    0.554,    0.502,    0.592,    0.969,    0.860 } },
                // 7's
                new Digit() { Label = 7,  Reading = new double[] { 0.081,    0.118,    0.080,    0.563,    0.955,    0.981,    0.944,    0.587,    0.667,    0.771 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.123,    0.188,    0.164,    0.873,    0.641,    0.670,    0.763,    0.902,    0.624,    0.943 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.133,    0.039,    0.056,    0.615,    0.916,    0.820,    0.542,    0.710,    0.902,    0.939 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.155,    0.190,    0.188,    0.733,    0.945,    0.984,    0.579,    0.794,    0.606,    0.817 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.100,    0.141,    0.023,    0.902,    0.674,    0.761,    0.860,    0.601,    0.765,    0.894 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.116,    0.040,    0.186,    0.527,    0.529,    0.608,    0.657,    0.586,    0.646,    0.636 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.081,    0.118,    0.080,    0.563,    0.955,    0.981,    0.944,    0.587,    0.667,    0.771 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.123,    0.188,    0.164,    0.873,    0.641,    0.670,    0.763,    0.902,    0.624,    0.943 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.133,    0.039,    0.056,    0.615,    0.916,    0.820,    0.542,    0.710,    0.902,    0.939 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.155,    0.190,    0.188,    0.733,    0.945,    0.984,    0.579,    0.794,    0.606,    0.817 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.100,    0.141,    0.023,    0.902,    0.674,    0.761,    0.860,    0.601,    0.765,    0.894 } },
                new Digit() { Label = 7,  Reading = new double[] { 0.116,    0.040,    0.186,    0.527,    0.529,    0.608,    0.657,    0.586,    0.646,    0.636 } },
                // 6's
                new Digit() { Label = 6,  Reading = new double[] { 0.176,    0.094,    0.074,    0.091,    0.881,    0.831,    0.999,    0.678,    0.928,    0.656 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.037,    0.187,    0.118,    0.122,    0.694,    0.977,    0.540,    0.736,    0.940,    0.657 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.183,    0.176,    0.177,    0.004,    0.650,    0.790,    0.510,    0.945,    0.706,    0.838 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.131,    0.196,    0.060,    0.187,    0.876,    0.516,    0.671,    0.828,    0.522,    0.842 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.104,    0.073,    0.069,    0.044,    0.802,    0.822,    0.803,    0.661,    0.832,    0.551 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.028,    0.168,    0.059,    0.195,    0.676,    0.501,    0.790,    0.676,    0.578,    0.592 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.176,    0.094,    0.074,    0.091,    0.881,    0.831,    0.999,    0.678,    0.928,    0.656 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.037,    0.187,    0.118,    0.122,    0.694,    0.977,    0.540,    0.736,    0.940,    0.657 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.183,    0.176,    0.177,    0.004,    0.650,    0.790,    0.510,    0.945,    0.706,    0.838 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.131,    0.196,    0.060,    0.187,    0.876,    0.516,    0.671,    0.828,    0.522,    0.842 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.104,    0.073,    0.069,    0.044,    0.802,    0.822,    0.803,    0.661,    0.832,    0.551 } },
                new Digit() { Label = 6,  Reading = new double[] { 0.028,    0.168,    0.059,    0.195,    0.676,    0.501,    0.790,    0.676,    0.578,    0.592 } },
                // 5's
                new Digit() { Label = 5,  Reading = new double[] { 0.129,    0.022,    0.188,    0.037,    0.131,    0.600,    0.821,    0.826,    0.930,    0.678 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.123,    0.084,    0.086,    0.040,    0.158,    0.777,    0.769,    0.836,    0.706,    0.827 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.194,    0.139,    0.046,    0.063,    0.108,    0.906,    0.847,    0.659,    0.794,    0.875 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.090,    0.197,    0.100,    0.171,    0.009,    0.707,    0.710,    0.582,    0.781,    0.703 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.001,    0.190,    0.092,    0.062,    0.097,    0.857,    0.707,    0.549,    0.595,    0.898 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.093,    0.181,    0.187,    0.100,    0.042,    0.973,    0.941,    0.816,    0.820,    0.921 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.129,    0.022,    0.188,    0.037,    0.131,    0.600,    0.821,    0.826,    0.930,    0.678 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.123,    0.084,    0.086,    0.040,    0.158,    0.777,    0.769,    0.836,    0.706,    0.827 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.194,    0.139,    0.046,    0.063,    0.108,    0.906,    0.847,    0.659,    0.794,    0.875 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.090,    0.197,    0.100,    0.171,    0.009,    0.707,    0.710,    0.582,    0.781,    0.703 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.001,    0.190,    0.092,    0.062,    0.097,    0.857,    0.707,    0.549,    0.595,    0.898 } },
                new Digit() { Label = 5,  Reading = new double[] { 0.093,    0.181,    0.187,    0.100,    0.042,    0.973,    0.941,    0.816,    0.820,    0.921 } },
                // 4's
                new Digit() { Label = 4,  Reading = new double[] { 0.101,    0.065,    0.014,    0.023,    0.180,    0.125,    0.959,    0.784,    0.578,    0.689 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.028,    0.075,    0.023,    0.002,    0.130,    0.005,    0.618,    0.957,    0.913,    0.912 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.080,    0.119,    0.027,    0.158,    0.058,    0.109,    0.526,    0.502,    0.825,    0.548 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.106,    0.157,    0.105,    0.144,    0.179,    0.135,    0.944,    0.676,    0.702,    0.929 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.158,    0.168,    0.187,    0.162,    0.132,    0.019,    0.898,    0.966,    0.923,    0.594 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.142,    0.192,    0.161,    0.162,    0.089,    0.181,    0.598,    0.636,    0.910,    0.680 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.101,    0.065,    0.014,    0.023,    0.180,    0.125,    0.959,    0.784,    0.578,    0.689 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.028,    0.075,    0.023,    0.002,    0.130,    0.005,    0.618,    0.957,    0.913,    0.912 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.080,    0.119,    0.027,    0.158,    0.058,    0.109,    0.526,    0.502,    0.825,    0.548 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.106,    0.157,    0.105,    0.144,    0.179,    0.135,    0.944,    0.676,    0.702,    0.929 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.158,    0.168,    0.187,    0.162,    0.132,    0.019,    0.898,    0.966,    0.923,    0.594 } },
                new Digit() { Label = 4,  Reading = new double[] { 0.142,    0.192,    0.161,    0.162,    0.089,    0.181,    0.598,    0.636,    0.910,    0.680 } },
                // 3's
                new Digit() { Label = 3,  Reading = new double[] { 0.115,    0.183,    0.183,    0.013,    0.169,    0.070,    0.169,    0.695,    0.553,    0.987 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.027,    0.176,    0.102,    0.193,    0.007,    0.111,    0.131,    0.659,    0.566,    0.600 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.149,    0.024,    0.031,    0.093,    0.077,    0.113,    0.195,    0.854,    0.638,    0.996 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.096,    0.124,    0.106,    0.010,    0.057,    0.175,    0.185,    0.940,    0.981,    0.658 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.048,    0.082,    0.165,    0.174,    0.012,    0.165,    0.091,    0.918,    0.891,    0.944 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.051,    0.175,    0.029,    0.124,    0.123,    0.092,    0.153,    0.727,    0.838,    0.642 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.115,    0.183,    0.183,    0.013,    0.169,    0.070,    0.169,    0.695,    0.553,    0.987 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.027,    0.176,    0.102,    0.193,    0.007,    0.111,    0.131,    0.659,    0.566,    0.600 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.149,    0.024,    0.031,    0.093,    0.077,    0.113,    0.195,    0.854,    0.638,    0.996 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.096,    0.124,    0.106,    0.010,    0.057,    0.175,    0.185,    0.940,    0.981,    0.658 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.048,    0.082,    0.165,    0.174,    0.012,    0.165,    0.091,    0.918,    0.891,    0.944 } },
                new Digit() { Label = 3,  Reading = new double[] { 0.051,    0.175,    0.029,    0.124,    0.123,    0.092,    0.153,    0.727,    0.838,    0.642 } },
                // 2's
                new Digit() { Label = 2,  Reading = new double[] { 0.074,    0.029,    0.023,    0.023,    0.099,    0.103,    0.071,    0.094,    0.842,    0.578 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.125,    0.154,    0.071,    0.162,    0.191,    0.068,    0.057,    0.057,    0.705,    0.777 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.023,    0.029,    0.184,    0.077,    0.148,    0.120,    0.096,    0.181,    0.715,    0.911 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.023,    0.095,    0.041,    0.136,    0.189,    0.021,    0.059,    0.144,    0.689,    0.771 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.095,    0.196,    0.031,    0.126,    0.053,    0.095,    0.105,    0.066,    0.830,    0.638 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.036,    0.097,    0.010,    0.026,    0.035,    0.045,    0.043,    0.145,    0.683,    0.945 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.074,    0.029,    0.023,    0.023,    0.099,    0.103,    0.071,    0.094,    0.842,    0.578 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.125,    0.154,    0.071,    0.162,    0.191,    0.068,    0.057,    0.057,    0.705,    0.777 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.023,    0.029,    0.184,    0.077,    0.148,    0.120,    0.096,    0.181,    0.715,    0.911 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.023,    0.095,    0.041,    0.136,    0.189,    0.021,    0.059,    0.144,    0.689,    0.771 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.095,    0.196,    0.031,    0.126,    0.053,    0.095,    0.105,    0.066,    0.830,    0.638 } },
                new Digit() { Label = 2,  Reading = new double[] { 0.036,    0.097,    0.010,    0.026,    0.035,    0.045,    0.043,    0.145,    0.683,    0.945 } },
                // 1's
                new Digit() { Label = 1,  Reading = new double[] { 0.008,    0.088,    0.009,    0.172,    0.009,    0.032,    0.114,    0.134,    0.173,    0.930 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.010,    0.113,    0.180,    0.173,    0.016,    0.026,    0.197,    0.060,    0.025,    0.635 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.017,    0.031,    0.050,    0.032,    0.129,    0.125,    0.086,    0.017,    0.102,    0.861 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.176,    0.113,    0.072,    0.097,    0.101,    0.008,    0.189,    0.059,    0.117,    0.555 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.180,    0.170,    0.092,    0.159,    0.047,    0.122,    0.116,    0.036,    0.010,    0.525 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.048,    0.183,    0.121,    0.020,    0.162,    0.176,    0.039,    0.176,    0.043,    0.708 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.008,    0.088,    0.009,    0.172,    0.009,    0.032,    0.114,    0.134,    0.173,    0.930 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.010,    0.113,    0.180,    0.173,    0.016,    0.026,    0.197,    0.060,    0.025,    0.635 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.017,    0.031,    0.050,    0.032,    0.129,    0.125,    0.086,    0.017,    0.102,    0.861 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.176,    0.113,    0.072,    0.097,    0.101,    0.008,    0.189,    0.059,    0.117,    0.555 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.180,    0.170,    0.092,    0.159,    0.047,    0.122,    0.116,    0.036,    0.010,    0.525 } },
                new Digit() { Label = 1,  Reading = new double[] { 0.048,    0.183,    0.121,    0.020,    0.162,    0.176,    0.039,    0.176,    0.043,    0.708 } },

            };
        }

        public static Digit[] GetTestDigits()
        {
            return new Digit[]
            {
                // 10's
                new Digit() { Label = 10, Reading = new double[] { 0.934,    0.871,    0.564,    0.661,    0.685,    0.732,    0.962,    0.656,    0.987,    0.880 } },
                // 9's
                new Digit() { Label = 9,  Reading = new double[] { 0.044,    0.508,    0.683,    0.534,    0.535,    0.659,    0.636,    0.632,    0.507,    0.698 } },
                // 8's
                new Digit() { Label = 8,  Reading = new double[] { 0.069,    0.185,    0.515,    0.610,    0.633,    0.520,    0.609,    0.796,    0.711,    0.548 } },
                // 7's
                new Digit() { Label = 7,  Reading = new double[] { 0.081,    0.118,    0.080,    0.563,    0.955,    0.981,    0.944,    0.587,    0.667,    0.771 } },
                // 6's
                new Digit() { Label = 6,  Reading = new double[] { 0.176,    0.094,    0.074,    0.091,    0.881,    0.831,    0.999,    0.678,    0.928,    0.656 } },
                // 5's
                new Digit() { Label = 5,  Reading = new double[] { 0.129,    0.022,    0.188,    0.037,    0.131,    0.600,    0.821,    0.826,    0.930,    0.678 } },
                // 4's
                new Digit() { Label = 4,  Reading = new double[] { 0.101,    0.065,    0.014,    0.023,    0.180,    0.125,    0.959,    0.784,    0.578,    0.689 } },
                // 3's
                new Digit() { Label = 3,  Reading = new double[] { 0.115,    0.183,    0.183,    0.013,    0.169,    0.070,    0.169,    0.695,    0.553,    0.987 } },
                // 2's
                new Digit() { Label = 2,  Reading = new double[] { 0.074,    0.029,    0.023,    0.023,    0.099,    0.103,    0.071,    0.094,    0.842,    0.578 } },
                // 1's
                new Digit() { Label = 1,  Reading = new double[] { 0.008,    0.088,    0.009,    0.172,    0.009,    0.032,    0.114,    0.134,    0.173,    0.930 } },

            };
        }
    }
}
