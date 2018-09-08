using System.Collections.Generic;
using CNTK;

namespace Horker.PSCNTK
{
    public class WrappedVariable
    {
        private Variable _va;

        #region Properties

        // Delegates

        public int CurrentValueTimeStamp => _va.CurrentValueTimeStamp;
        public DataType DataType => _va.DataType;
        public IList<Axis> DynamicAxes => _va.DynamicAxes;
        public bool IsConstant => _va.IsConstant;
        public bool IsInput => _va.IsInput;
        public bool IsOutput => _va.IsOutput;
        public bool IsParameter => _va.IsParameter;
        public bool IsPlaceholder => _va.IsPlaceholder;
        public bool IsSparse => _va.IsSparse;
        public VariableKind Kind => _va.Kind;
        public string Name => _va.Name;
        public bool NeedsGradient => _va.NeedsGradient;
        public Function Owner => _va.Owner;
        public NDShape Shape => _va.Shape;
        public string Uid => _va.Uid;

        // Additional

        public NDArrayView Value => _va.GetValue();

        #endregion

        #region Delegate methods

        public NDArrayView GetValue() => _va.GetValue();
        public bool HasBatchAxis() => _va.HasBatchAxis();
        public bool HasSequenceAxis() => _va.HasSequenceAxis();
        public bool IsInitialized() => _va.IsInitialized();
        public WrappedFunction ToFunction() => _va.ToFunction();

        #endregion

        #region Constructor

        public WrappedVariable(Variable va)
        {
            _va = va;
        }

        #endregion

        #region Implicit type conversions

        public static implicit operator WrappedVariable(Variable va)
        {
            return new WrappedVariable(va);
        }

        public static implicit operator WrappedVariable(Function f)
        {
            return new WrappedVariable(f);
        }

        public static implicit operator Variable(WrappedVariable va)
        {
            return va._va;
        }

        public static implicit operator Function(WrappedVariable va)
        {
            return va._va;
        }

        public static implicit operator WrappedFunction(WrappedVariable va)
        {
            return new WrappedFunction(va._va);
        }

        #endregion

        #region Unary operator +

        public static WrappedFunction operator+(WrappedVariable va)
        {
            return new WrappedFunction(va._va);
        }

        #endregion

        #region Unary operator -

        public static WrappedFunction operator-(WrappedVariable va)
        {
            return CNTKLib.Negate(va);
        }

        #endregion

        #region Binary operator +

        public static WrappedFunction operator+(WrappedVariable left, WrappedVariable right)
        {
            return CNTKLib.Plus(left, right);
        }

        public static WrappedFunction operator+(WrappedVariable left, Variable right)
        {
            return CNTKLib.Plus(left, right);
        }

        public static WrappedFunction operator+(Variable left, WrappedVariable right)
        {
            return CNTKLib.Plus(left, right);
        }

        public static WrappedFunction operator+(WrappedVariable left, double right)
        {
            return CNTKLib.Plus(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedFunction operator+(double left, WrappedVariable right)
        {
            return CNTKLib.Plus(Constant.Scalar(DataType.Float, left), right);
        }

        #endregion

        #region Binary operator -

        public static WrappedVariable operator-(WrappedVariable left, WrappedVariable right)
        {
            return CNTKLib.Minus(left, right);
        }

        public static WrappedVariable operator-(WrappedVariable left, Variable right)
        {
            return CNTKLib.Minus(left, right);
        }

        public static WrappedVariable operator-(Variable left, WrappedVariable right)
        {
            return CNTKLib.Minus(left, right);
        }

        public static WrappedVariable operator-(WrappedVariable left, double right)
        {
            return CNTKLib.Minus(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedVariable operator-(double left, WrappedVariable right)
        {
            return CNTKLib.Minus(Constant.Scalar(DataType.Float, left), right);
        }

        #endregion

        #region Binary operator *

        public static WrappedVariable operator*(WrappedVariable left, WrappedVariable right)
        {
            return CNTKLib.ElementTimes(left, right);
        }

        public static WrappedVariable operator*(WrappedVariable left, Variable right)
        {
            return CNTKLib.ElementTimes(left, right);
        }

        public static WrappedVariable operator*(Variable left, WrappedVariable right)
        {
            return CNTKLib.ElementTimes(left, right);
        }

        public static WrappedVariable operator*(WrappedVariable left, double right)
        {
            return CNTKLib.ElementTimes(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedVariable operator*(double left, WrappedVariable right)
        {
            return CNTKLib.ElementTimes(Constant.Scalar(DataType.Float, left), right);
        }

        #endregion

        #region Binary operator /

        public static WrappedVariable operator/(WrappedVariable left, WrappedVariable right)
        {
            return CNTKLib.ElementDivide(left, right);
        }

        public static WrappedVariable operator/(WrappedVariable left, Variable right)
        {
            return CNTKLib.ElementDivide(left, right);
        }

        public static WrappedVariable operator/(Variable left, WrappedVariable right)
        {
            return CNTKLib.ElementDivide(left, right);
        }

        public static WrappedVariable operator/(WrappedVariable left, double right)
        {
            return CNTKLib.ElementDivide(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedVariable operator/(double left, WrappedVariable right)
        {
            return CNTKLib.ElementDivide(Constant.Scalar(DataType.Float, left), right);
        }

        #endregion

        #region Binary operator &

        public static WrappedVariable operator&(WrappedVariable left, WrappedVariable right)
        {
            return CNTKLib.ElementAnd(left, right);
        }

        public static WrappedVariable operator&(WrappedVariable left, Variable right)
        {
            return CNTKLib.ElementAnd(left, right);
        }

        public static WrappedVariable operator&(Variable left, WrappedVariable right)
        {
            return CNTKLib.ElementAnd(left, right);
        }

        public static WrappedVariable operator&(WrappedVariable left, double right)
        {
            return CNTKLib.ElementAnd(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedVariable operator&(double left, WrappedVariable right)
        {
            return CNTKLib.ElementAnd(Constant.Scalar(DataType.Float, left), right);
        }

        #endregion

        #region Binary operator |

        public static WrappedVariable operator|(WrappedVariable left, WrappedVariable right)
        {
            return CNTKLib.ElementOr(left, right);
        }

        public static WrappedVariable operator|(WrappedVariable left, Variable right)
        {
            return CNTKLib.ElementOr(left, right);
        }

        public static WrappedVariable operator|(Variable left, WrappedVariable right)
        {
            return CNTKLib.ElementOr(left, right);
        }

        public static WrappedVariable operator|(WrappedVariable left, double right)
        {
            return CNTKLib.ElementOr(left, Constant.Scalar(DataType.Float, right));
        }

        public static WrappedVariable operator|(double left, WrappedVariable right)
        {
            return CNTKLib.ElementOr(Constant.Scalar(DataType.Float, left), right);
        }

        #endregion
    }
}
