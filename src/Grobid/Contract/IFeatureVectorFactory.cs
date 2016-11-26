﻿namespace Grobid.NET.Contract
{
    interface IFeatureVectorFactory<T>
    {
        T Create(BlockState blockState);
    }
}