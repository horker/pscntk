<% # -*- ft=ps1 -*-
Set-StrictMode -Version latest

. templates\Wrapped_common.ps1

-%>
using CNTK;

// DO NOT EDIT
// This file was automatically generated at <% (Get-Date).ToString("yyyy/MM/dd HH:mm:ss") %>

namespace Horker.PSCNTK {

    public partial class WrappedVariable {
<%
foreach ($op in $OPERATORS.Keys) {
-%>

        public static WrappedFunction operator<% $op %>(WrappedVariable left, WrappedVariable right)
        {
            return <% $OPERATORS[$op] -f "left", "right" %>;
        }
<%
  foreach ($type in $TYPES.Keys) {
    if ($type -eq "WrappedVariable") { continue; }
-%>

        public static WrappedFunction operator<% $op %>(WrappedVariable left, <% $type %> right)
        {
            return <% $OPERATORS[$op] -f "left", ($TYPES[$type] -f "right") %>;
        }

        public static WrappedFunction operator<% $op %>(<% $type %> left, WrappedVariable right)
        {
            return <% $OPERATORS[$op] -f ($TYPES[$type] -f "left"), "right" %>;
        }
<%
  }
}
-%>
    }
}
