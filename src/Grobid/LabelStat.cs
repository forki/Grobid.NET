﻿namespace Grobid.NET
{
    public class LabelStat
    {
        public int Expected { get; set; }
        public int Observed { get; set; }
        public int FalsePositive { get; set; }
        public int FalseNegative { get; set; }
    }
}