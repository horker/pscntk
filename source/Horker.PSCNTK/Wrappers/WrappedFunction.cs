using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
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
        public IList<WrappedVariable> Constants() => _f.Constants().Select(x => (WrappedVariable)x).ToList();
        public CNTKDictionary GetCustomAttributes() => _f.GetCustomAttributes();
        public IList<WrappedVariable> Parameters() => _f.Parameters().Select(x => (WrappedVariable)x).ToList();
        public IList<WrappedVariable> Placeholders() => _f.Placeholders().Select(x => (WrappedVariable)x).ToList();
        public void ResetCustomAttributes() => _f.ResetCustomAttributes();
        public void Restore(string filepath) => _f.Restore(filepath);
        public byte[] Save() => _f.Save();
        public void Save(string filepath) => _f.Save(filepath);
        public void SetName(string name) => _f.SetName(name);

        // Additionally defined

        public WrappedFunction Clone(ParameterCloningMethod parameterCloningMethod, Hashtable replacements)
        {
            var converter = new Func<object, Variable>(x => {
                if (x is PSObject)
                    x = (x as PSObject).BaseObject;

                if (x is Variable)
                    return x as Variable;

                if (x is WrappedVariable)
                    return x as WrappedVariable;

                if (x is Function)
                    return x as Function;

                if (x is WrappedFunction)
                    return x as WrappedFunction;

                throw new ArgumentException("Can't convert to Variable");
            });

            var rep = Converter.HashtableToDictionary<Variable, Variable>(replacements, converter, converter);
            return _f.Clone(parameterCloningMethod, rep);
        }

        public void ReplacePlaceholders(IDictionary<Variable, Variable> placeholderReplacements)
        {
            _f.ReplacePlaceholders(placeholderReplacements);
        }

        public void ReplacePlaceholders(Hashtable placeholderReplacements)
        {
            var converter = new Func<object, Variable>(x => {
                Variable va;
                if (x is Variable v)
                    va = v;
                else if (x is WrappedVariable wv)
                    va = wv;
                else if (x is Function f)
                    va = f;
                else if (x is WrappedFunction wf)
                    va = wf;
                else
                    va = (Variable)x;

                return va;
            });

            ReplacePlaceholders(Converter.HashtableToDictionary(placeholderReplacements, converter, converter));
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
            if (f == null)
                return null;
            return f._f;
        }

        public static implicit operator Variable(WrappedFunction f)
        {
            if (f == null)
                return null;
            return f._f;
        }

        public static implicit operator WrappedVariable(WrappedFunction f)
        {
            if (f == null)
                return null;
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
