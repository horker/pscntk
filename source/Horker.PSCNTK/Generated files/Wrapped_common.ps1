$TYPES = @{
  "Variable" = "{0}"
  "Function" = "{0}"
  "WrappedVariable" = "{0}"
  "WrappedFunction" = "{0}"
  "double" = "Constant.Scalar(DataType.Float, {0})"
  "int" = "Constant.Scalar(DataType.Float, {0})"
}

$OPERATORS = @{
  "+" = "CNTKLib.Plus({0}, {1})"
  "-" = "CNTKLib.Minus({0}, {1})"
  "*" = "CNTKLib.ElementTimes({0}, {1})"
  "/" = "CNTKLib.ElementDivide({0}, {1})"
  "&" = "CNTKLib.ElementAnd({0}, {1})"
  "|" = "CNTKLib.ElementOr({0}, {1})"
  "^" = "CNTKLib.ElementXor({0}, {1})"
}

