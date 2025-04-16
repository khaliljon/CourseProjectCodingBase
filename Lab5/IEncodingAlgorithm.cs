﻿namespace CourseProjectCodingBase.Lab5
{
    public interface IEncodingAlgorithm
    {
        string Encode(string input);
        string Decode(string encodedInput);
        double CalculateEfficiency(string input, string encodedInput);
    }
}
