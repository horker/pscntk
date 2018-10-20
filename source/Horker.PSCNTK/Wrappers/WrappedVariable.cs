using System.Collections.Generic;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class WrappedVariable : IWrappedNode
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
        public WrappedFunction Owner => _va.Owner;
        public Shape Shape => _va.Shape;
        public string Uid => _va.Uid;

        // Additional

        public NDArrayView Value => _va.GetValue();

        #endregion

        #region Delegate methods

        public override int GetHashCode() => _va.GetHashCode();
        public override string ToString() => _va.AsString();

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

        public static implicit operator WrappedVariable(double value)
        {
            return Constant.Scalar(DataType.Float, value);
        }

        public static implicit operator WrappedVariable(int value)
        {
            return Constant.Scalar(DataType.Float, value);
        }

        public static implicit operator WrappedVariable(Value value)
        {
            return new Constant(value.Data);
        }

        public static implicit operator WrappedVariable(NDArrayView value)
        {
            return new Constant(value);
        }

        #endregion

        #region Unary operators

        public static WrappedFunction operator+(WrappedVariable va)
        {
            return new WrappedFunction(va._va);
        }

        public static WrappedFunction operator-(WrappedVariable va)
        {
            return CNTKLib.Negate(va);
        }

        #endregion
    }
}
