using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GlobalVar
{
    static public bool isUIOpen = false;
    static public void CloseUI()
    {
        isUIOpen = false;
    }
    
}
