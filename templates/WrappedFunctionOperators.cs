<% # -*- ft=ps1 -*-
Set-StrictMode -Version latest

. templates\Wrapped_common.ps1

-%>
using CNTK;

// DO NOT EDIT
// This file was automatically generated at <% (Get-Date).ToString("yyyy/MM/dd HH:mm:ss") %>

namespace Horker.PSCNTK {

    public partial class WrappedFunction {
<%
foreach ($op in $OPERATORS.Keys) {
-%>

        public static WrappedFunction operator<% $op %>(WrappedFunction left, WrappedFunction right)
        {
            return <% $OPERATORS[$op] -f "left", "right" %>;
        }
<%
  foreach ($type in $TYPES.Keys) {
    if ($type -eq "WrappedFunction") { continue; }
-%>

        public static WrappedFunction operator<% $op %>(WrappedFunction left, <% $type %> right)
        {
            return <% $OPERATORS[$op] -f "left", ($TYPES[$type] -f "right") %>;
        }
<%
    if ($type -ne "WrappedVariable") {
-%>

        public static WrappedFunction operator<% $op %>(<% $type %> left, WrappedFunction right)
        {
            return <% $OPERATORS[$op] -f ($TYPES[$type] -f "left"), "right" %>;
        }
<%
    }
  }
}
-%>
    }
}
