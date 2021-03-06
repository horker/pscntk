using CNTK;

// DO NOT EDIT
// This file was automatically generated at 2018/10/27 13:07:50

namespace Horker.PSCNTK {

    public partial class WrappedFunction {

        public static WrappedFunction operator^(WrappedFunction left, WrappedFunction right)
        {
            return CNTKLib.ElementXor(left, right);
        }

        public static WrappedFunction operator^(WrappedFunction left, Variable right)
        {
            return CNTKLib.ElementXor(left, right);
        }

        public static WrappedFunction operator^(Variable left, WrappedFunction right)
        {
            return CNTKLib.ElementXor(left, right);
        }

        public static WrappedFunction operator^(WrappedFunction left, WrappedVariable right)
        {
            return CNTKLib.ElementXor(left, right);
        }

        public static WrappedFunction operator^(WrappedFunction left, Function right)
        {
            return CNTKLib.ElementXor(left, right);
        }

        public static WrappedFunction operator^(Function left, WrappedFunction right)
        {
            return CNTKLib.ElementXor(left, right);
        }

        public static WrappedFunction operator^(WrappedFunction left, int right)
        {
            return CNTKLib.ElementXor(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator^(int left, WrappedFunction right)
        {
            return CNTKLib.ElementXor(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator^(WrappedFunction left, double right)
        {
            return CNTKLib.ElementXor(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator^(double left, WrappedFunction right)
        {
            return CNTKLib.ElementXor(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator+(WrappedFunction left, WrappedFunction right)
        {
            return CNTKLib.Plus(left, right);
        }

        public static WrappedFunction operator+(WrappedFunction left, Variable right)
        {
            return CNTKLib.Plus(left, right);
        }

        public static WrappedFunction operator+(Variable left, WrappedFunction right)
        {
            return CNTKLib.Plus(left, right);
        }

        public static WrappedFunction operator+(WrappedFunction left, WrappedVariable right)
        {
            return CNTKLib.Plus(left, right);
        }

        public static WrappedFunction operator+(WrappedFunction left, Function right)
        {
            return CNTKLib.Plus(left, right);
        }

        public static WrappedFunction operator+(Function left, WrappedFunction right)
        {
            return CNTKLib.Plus(left, right);
        }

        public static WrappedFunction operator+(WrappedFunction left, int right)
        {
            return CNTKLib.Plus(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator+(int left, WrappedFunction right)
        {
            return CNTKLib.Plus(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator+(WrappedFunction left, double right)
        {
            return CNTKLib.Plus(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator+(double left, WrappedFunction right)
        {
            return CNTKLib.Plus(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator&(WrappedFunction left, WrappedFunction right)
        {
            return CNTKLib.ElementAnd(left, right);
        }

        public static WrappedFunction operator&(WrappedFunction left, Variable right)
        {
            return CNTKLib.ElementAnd(left, right);
        }

        public static WrappedFunction operator&(Variable left, WrappedFunction right)
        {
            return CNTKLib.ElementAnd(left, right);
        }

        public static WrappedFunction operator&(WrappedFunction left, WrappedVariable right)
        {
            return CNTKLib.ElementAnd(left, right);
        }

        public static WrappedFunction operator&(WrappedFunction left, Function right)
        {
            return CNTKLib.ElementAnd(left, right);
        }

        public static WrappedFunction operator&(Function left, WrappedFunction right)
        {
            return CNTKLib.ElementAnd(left, right);
        }

        public static WrappedFunction operator&(WrappedFunction left, int right)
        {
            return CNTKLib.ElementAnd(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator&(int left, WrappedFunction right)
        {
            return CNTKLib.ElementAnd(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator&(WrappedFunction left, double right)
        {
            return CNTKLib.ElementAnd(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator&(double left, WrappedFunction right)
        {
            return CNTKLib.ElementAnd(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator|(WrappedFunction left, WrappedFunction right)
        {
            return CNTKLib.ElementOr(left, right);
        }

        public static WrappedFunction operator|(WrappedFunction left, Variable right)
        {
            return CNTKLib.ElementOr(left, right);
        }

        public static WrappedFunction operator|(Variable left, WrappedFunction right)
        {
            return CNTKLib.ElementOr(left, right);
        }

        public static WrappedFunction operator|(WrappedFunction left, WrappedVariable right)
        {
            return CNTKLib.ElementOr(left, right);
        }

        public static WrappedFunction operator|(WrappedFunction left, Function right)
        {
            return CNTKLib.ElementOr(left, right);
        }

        public static WrappedFunction operator|(Function left, WrappedFunction right)
        {
            return CNTKLib.ElementOr(left, right);
        }

        public static WrappedFunction operator|(WrappedFunction left, int right)
        {
            return CNTKLib.ElementOr(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator|(int left, WrappedFunction right)
        {
            return CNTKLib.ElementOr(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator|(WrappedFunction left, double right)
        {
            return CNTKLib.ElementOr(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator|(double left, WrappedFunction right)
        {
            return CNTKLib.ElementOr(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator*(WrappedFunction left, WrappedFunction right)
        {
            return CNTKLib.ElementTimes(left, right);
        }

        public static WrappedFunction operator*(WrappedFunction left, Variable right)
        {
            return CNTKLib.ElementTimes(left, right);
        }

        public static WrappedFunction operator*(Variable left, WrappedFunction right)
        {
            return CNTKLib.ElementTimes(left, right);
        }

        public static WrappedFunction operator*(WrappedFunction left, WrappedVariable right)
        {
            return CNTKLib.ElementTimes(left, right);
        }

        public static WrappedFunction operator*(WrappedFunction left, Function right)
        {
            return CNTKLib.ElementTimes(left, right);
        }

        public static WrappedFunction operator*(Function left, WrappedFunction right)
        {
            return CNTKLib.ElementTimes(left, right);
        }

        public static WrappedFunction operator*(WrappedFunction left, int right)
        {
            return CNTKLib.ElementTimes(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator*(int left, WrappedFunction right)
        {
            return CNTKLib.ElementTimes(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator*(WrappedFunction left, double right)
        {
            return CNTKLib.ElementTimes(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator*(double left, WrappedFunction right)
        {
            return CNTKLib.ElementTimes(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator-(WrappedFunction left, WrappedFunction right)
        {
            return CNTKLib.Minus(left, right);
        }

        public static WrappedFunction operator-(WrappedFunction left, Variable right)
        {
            return CNTKLib.Minus(left, right);
        }

        public static WrappedFunction operator-(Variable left, WrappedFunction right)
        {
            return CNTKLib.Minus(left, right);
        }

        public static WrappedFunction operator-(WrappedFunction left, WrappedVariable right)
        {
            return CNTKLib.Minus(left, right);
        }

        public static WrappedFunction operator-(WrappedFunction left, Function right)
        {
            return CNTKLib.Minus(left, right);
        }

        public static WrappedFunction operator-(Function left, WrappedFunction right)
        {
            return CNTKLib.Minus(left, right);
        }

        public static WrappedFunction operator-(WrappedFunction left, int right)
        {
            return CNTKLib.Minus(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator-(int left, WrappedFunction right)
        {
            return CNTKLib.Minus(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator-(WrappedFunction left, double right)
        {
            return CNTKLib.Minus(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator-(double left, WrappedFunction right)
        {
            return CNTKLib.Minus(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator/(WrappedFunction left, WrappedFunction right)
        {
            return CNTKLib.ElementDivide(left, right);
        }

        public static WrappedFunction operator/(WrappedFunction left, Variable right)
        {
            return CNTKLib.ElementDivide(left, right);
        }

        public static WrappedFunction operator/(Variable left, WrappedFunction right)
        {
            return CNTKLib.ElementDivide(left, right);
        }

        public static WrappedFunction operator/(WrappedFunction left, WrappedVariable right)
        {
            return CNTKLib.ElementDivide(left, right);
        }

        public static WrappedFunction operator/(WrappedFunction left, Function right)
        {
            return CNTKLib.ElementDivide(left, right);
        }

        public static WrappedFunction operator/(Function left, WrappedFunction right)
        {
            return CNTKLib.ElementDivide(left, right);
        }

        public static WrappedFunction operator/(WrappedFunction left, int right)
        {
            return CNTKLib.ElementDivide(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator/(int left, WrappedFunction right)
        {
            return CNTKLib.ElementDivide(Constant.Scalar(DataType.Float, left), right);
        }

        public static WrappedFunction operator/(WrappedFunction left, double right)
        {
            return CNTKLib.ElementDivide(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator/(double left, WrappedFunction right)
        {
            return CNTKLib.ElementDivide(Constant.Scalar(DataType.Float, left), right);
        }
    }
}

