using System;

namespace numl.Math.LinearAlgebra
{
    public class SingularMatrixException : Exception
    {
        public SingularMatrixException()
        {

        }

        public SingularMatrixException(string message)
            : base(message)
        {

        }
    }
}
