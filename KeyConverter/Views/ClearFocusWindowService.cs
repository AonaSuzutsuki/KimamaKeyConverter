using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonStyleLib.Views;

namespace KeyConverterGUI.Views
{
    public class ClearFocusWindowService : WindowService, IClearFocus
    {
        public void ClearFocus()
        {
            if (Owner is IClearFocus clearFocus)
                clearFocus.ClearFocus();
        }
    }
}
