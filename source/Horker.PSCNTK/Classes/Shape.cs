using System;

namespace Horker.PSCNTK
{
    [Serializable]
    public class Shape
    {
        public int[] Dimensions;
        public int Rank => Dimensions.Length;
        public int TotalSize => GetSize(Rank - 1);

        public int this[int i]
        {
            get
            {
                if (i < 0)
                    i = Rank + i;
                return Dimensions[i];
            }

            set
            {
                if (i < 0)
                    i = Rank + i;
                Dimensions[i] = value;
            }
        }

        public Shape(int[] dimensions, int dataCount = -1)
        {
            Dimensions = ExamineDimensions(dimensions, dataCount);
        }

        public int GetSize(int toAxis)
        {
            if (toAxis < 0)
                toAxis = Rank + toAxis;

            var size = 1;
            for (var i = 0; i <= toAxis; ++i)
                size *= Dimensions[i];

            return size;
        }

        public int GetSequentialIndex(params int[] indexes)
        {
            int seq = indexes[indexes.Length - 1];
            for (int i = indexes.Length - 2; i >= 0; --i)
            {
                seq *= Dimensions[i];
                seq += indexes[i];
            }

            return seq;
        }

        public Shape Clone()
        {
            return new Shape(Dimensions);
        }

        public static implicit operator Shape(int[] dimensions)
        {
            return new Shape(dimensions);
        }

        public static implicit operator int[] (Shape shape)
        {
            return shape.Dimensions;
        }

        public void Reshape(int[] dimensions, int dataCount = -1)
        {
            if (dataCount == -1)
                dataCount = TotalSize;

            Dimensions = ExamineDimensions(dimensions, dataCount);
        }

        private static int[] ExamineDimensions(int[] dimensions, int dataCount = -1)
        {
            var total = 1;
            var inferredIndex = -1;
            for (var i = 0; i < dimensions.Length; ++i)
            {
                if (dimensions[i] <= 0)
                {
                    if (dimensions[i] == -1)
                        if (inferredIndex == -1)
                            inferredIndex = i;
                        else
                            throw new ArgumentException("Multiple dimensions specified to infer");
                }
                else
                {
                    total *= dimensions[i];
                }
            }

            if (inferredIndex != -1)
            {
                if (dataCount == -1)
                    throw new ArgumentException("Data count must be specified to infer a dimension");

                if (total == 0 || dataCount % total != 0)
                    throw new ArgumentException("Dimensions are inconsistent with the data size");

                var newDimensions = dimensions.Clone() as int[];
                newDimensions[inferredIndex] = dataCount / total;

                return newDimensions;
            }
            else
            {
                if (dataCount != -1 && dataCount != total)
                    throw new ArgumentException("Dimensions are inconsistent with the data size");

                return dimensions.Clone() as int[];
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Shape))
                return false;

            var other = obj as Shape;

            if (this.Rank != other.Rank)
                return false;

            for (var i = 0; i < this.Rank; ++i)
                if (this.Dimensions[i] != other.Dimensions[i])
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 123456789;
            for (var i = 0; i < Rank; ++i)
                hash ^= Dimensions[i];

            return hash;
        }

        public override string ToString()
        {
            return "[" + string.Join(" x ", Dimensions) + "]";
        }
    }
}
