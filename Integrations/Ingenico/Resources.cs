using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [DebuggerNonUserCode]
    [CompilerGenerated]
    internal class Resources
    {
        private static ResourceManager resourceMan;

        private static CultureInfo resourceCulture;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan == null)
                {
                    resourceMan = new ResourceManager("Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static string _10000 => ResourceManager.GetString("10000", resourceCulture);

        internal static string _10001 => ResourceManager.GetString("10001", resourceCulture);

        internal static string _10002 => ResourceManager.GetString("10002", resourceCulture);

        internal static string DllNotFoundErrorText => ResourceManager.GetString("DllNotFoundErrorText", resourceCulture);

        internal static string ErrorTitle => ResourceManager.GetString("ErrorTitle", resourceCulture);

        internal static string PairingStartedText => ResourceManager.GetString("PairingStartedText", resourceCulture);

        internal static string Ready => ResourceManager.GetString("Ready", resourceCulture);

        internal static string SingletonErrorText => ResourceManager.GetString("SingletonErrorText", resourceCulture);

        internal static string UncompatibleDllText => ResourceManager.GetString("UncompatibleDllText", resourceCulture);

        internal Resources()
        {
        }
    }

}
