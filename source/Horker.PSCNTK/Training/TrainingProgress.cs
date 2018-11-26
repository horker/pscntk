﻿using System;

namespace Horker.PSCNTK
{
    public class TrainingProgress
    {
        public int Epoch;
        public int Iteration;
        public int SampleCount;
        public double Loss;
        public double Metric;
        public double Validation;
        public double LearningRate;
        public TimeSpan Elapsed;
    }
}
