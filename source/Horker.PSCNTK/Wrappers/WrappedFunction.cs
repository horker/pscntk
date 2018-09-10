using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class WrappedFunction : IWrappedNode
    {
        private Function _f;

        #region Properties

        public IList<WrappedVariable> Arguments => _f.Arguments.Select(x => (WrappedVariable)x).ToList();
        public int CurrentVersion => _f.CurrentVersion;
        public IList<WrappedVariable> Inputs => _f.Inputs.Select(x => (WrappedVariable)x).ToList();
        public bool IsBlock => _f.IsBlock;
        public bool IsComposite => _f.IsComposite;
        public bool IsPrimitive => _f.IsPrimitive;
        public string Name => _f.Name;
        public string OpName => _f.OpName;
        public WrappedVariable Output => _f.Output;
        public IList<WrappedVariable> Outputs => _f.Outputs.Select(x => (WrappedVariable)x).ToList();
        public WrappedFunction RootFunction => _f.RootFunction;
        public string Uid => _f.Uid;

        #endregion

        #region Methods

        // Delegates

        public override int GetHashCode() => _f.GetHashCode();
        public override string ToString() => _f.AsString();

        public CNTKDictionary Attributes() => _f.Attributes();
        public WrappedFunction BlockRoot() => _f.BlockRoot();
        public WrappedFunction Clone(ParameterCloningMethod parameterCloningMethod = 0) => _f.Clone(parameterCloningMethod);
        public WrappedFunction Clone(ParameterCloningMethod parameterCloneMethod, IDictionary<Variable, Variable> replacements) => _f.Clone(parameterCloneMethod, replacements);
        public WrappedFunction CloneFlattened() => _f.CloneFlattened();
        public WrappedFunction CloneFlattened(ParameterCloningMethod parameterCloningMethod) => _f.CloneFlattened(parameterCloningMethod);
        public ConstantVector Constants() => _f.Constants();
        public CNTKDictionary GetCustomAttributes() => _f.GetCustomAttributes();
        public IList<Parameter> Parameters() => _f.Parameters();
        public VariableVector Placeholders() => _f.Placeholders();
        // TODO: ReplacePlaceholder()
        public void ResetCustomAttributes() => _f.ResetCustomAttributes();
        public void Restore(string filepath) => _f.Restore(filepath);
        public byte[] Save() => _f.Save();
        public void Save(string filepath) => _f.Save(filepath);
        public void SetName(string name) => _f.SetName(name);

        // Additionally defined

        public WrappedFunction Clone(ParameterCloningMethod parameterCloningMethod, Hashtable replacements)
        {
            var rep = new Dictionary<Variable, Variable>();
            foreach (DictionaryEntry entry in replacements)
                rep.Add((Variable)entry.Key, (Variable)entry.Value);

            return _f.Clone(parameterCloningMethod, rep);
        }

        #endregion

        #region Constructor

        public WrappedFunction(Function f)
        {
            _f = f;
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator WrappedFunction(Function f)
        {
            return new WrappedFunction(f);
        }

        public static implicit operator WrappedFunction(Variable va)
        {
            return new WrappedFunction(va);
        }

        public static implicit operator Function(WrappedFunction f)
        {
            return f._f;
        }

        public static implicit operator Variable(WrappedFunction f)
        {
            return f._f;
        }

        public static implicit operator WrappedVariable(WrappedFunction f)
        {
            return new WrappedVariable(f._f);
        }

        #endregion

        #region Unary operators

        public static WrappedFunction operator +(WrappedFunction f)
        {
            return f;
        }

        public static WrappedFunction operator -(WrappedFunction f)
        {
            return CNTKLib.Negate(f._f);
        }

        #endregion
    }
}
