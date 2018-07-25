using System;

namespace Horker.PSCNTK
{
    public class Shape
    {
        public int[] Dimensions;
        public int Rank => Dimensions.Length;
        public int TotalSize => GetSize(Dimensions.Length - 1);

        public int this[int i]
        {
            get => Dimensions[i];
        }

        public Shape(int[] dimensions, int dataCount = -1)
        {
            Dimensions = ExamineDimensions(dimensions, dataCount);
        }

        public int GetSize(int toAxis)
        {
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
                    if (dimensions[i] == -1 && inferredIndex == -1)
                        inferredIndex = i;
                    else
                        throw new ArgumentException("invalid dimensions");
                }
                else
                {
                    total *= dimensions[i];
                }
            }

            if (inferredIndex != -1)
            {
                if (dataCount == -1)
                    throw new ArgumentException("data count must be specified to infer a dimension");

                if (total == 0 || dataCount % total != 0)
                    throw new ArgumentException("dimensions are inconsistent with the data size");

                var newDimensions = dimensions.Clone() as int[];
                newDimensions[inferredIndex] = dataCount / total;

                return newDimensions;
            }
            else
            {
                if (dataCount != -1 && dataCount != total)
                    throw new ArgumentException("dimensions are inconsistent with the data size");

                return dimensions.Clone() as int[];
            }
        }

        public override string ToString()
        {
            return "[" + string.Join(" x ", Dimensions) + "]";
        }
    }
}
