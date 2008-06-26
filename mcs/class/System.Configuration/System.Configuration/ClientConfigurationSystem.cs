//
// System.Configuration.ClientConfigurationSystem.cs
//
// Authors:
//  Chris Toshok (toshok@ximian.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (C) 2006 Novell, Inc (http://www.novell.com)
//

#if NET_2_0

using System;
using System.Reflection;
using System.Configuration.Internal;

namespace System.Configuration
{
	internal class ClientConfigurationSystem : IInternalConfigSystem
	{
		Configuration cfg;

		public ClientConfigurationSystem ()
		{
		}

		private Configuration Configuration {
			get {
				if (cfg == null) {
					Assembly a = Assembly.GetEntryAssembly();
					string exePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

					if (a == null && exePath == null)
						exePath = string.Empty;

					try {
						cfg = ConfigurationManager.OpenExeConfigurationInternal (
							ConfigurationUserLevel.None, a, exePath);
					} catch (Exception ex) {
						throw new ConfigurationErrorsException ("Error Initializing the configuration system.", ex);
					}
				}
				return cfg;
			}
		}

		object IInternalConfigSystem.GetSection (string configKey)
		{
			ConfigurationSection s = Configuration.GetSection (configKey);
			return s != null ? s.GetRuntimeObject () : null;
		}

		void IInternalConfigSystem.RefreshConfig (string sectionName)
		{
		}

		bool IInternalConfigSystem.SupportsUserConfig {
			get { return false; }
		}
	}
}

#endif
