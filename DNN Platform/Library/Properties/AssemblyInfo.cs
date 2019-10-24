#region Copyright
//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2018
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion
#region Usings

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using DotNetNuke.Application;
#endregion

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

// Review the values of the assembly attributes

[assembly: AssemblyTitle("DotNetNuke")]
[assembly: AssemblyDescription(@"
Open Source Web Application Framework
Patches contains:
5/2/2019: merge the changes from released version which fix issues around export/import area (#)
5/8/2019: do not export packages when the ""Include Extensions"" options is disabled (#)
5/9/2019: match the destination page with correct culture code checking during import process (#)
7/3/2019: make sure the import process running completely and handled the modules has wrong deleted flag. (#619)
8/14/2019: export themes will be hang up if there have too many files in the themes folder (#1057)
8/28/2019: url write mapped to wrong page in different portal. (#892,#1142)
9/2/2019: avoid null reference exception when call to Globals.LinkClick method. (#922)
9/17/2019: enable upload empty files (#1222)
9/24/2019: fix error during delete a site process (#)
")]
[assembly: CLSCompliant(true)]

[assembly: AssemblyStatus(ReleaseMode.Stable)]

// Allow internal variables to be visible to testing projects
[assembly: InternalsVisibleTo("DotNetNuke.Tests.Core")]

// This assembly is the default dynamic assembly generated Castle DynamicProxy,
// used by Moq. Paste in a single line.
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("DotNetNuke.Web")]
[assembly: InternalsVisibleTo("DotNetNuke.HttpModules")]
[assembly: InternalsVisibleTo("DotNetNuke.Modules.MemberDirectory")]
[assembly: InternalsVisibleTo("DotNetNuke.Provider.AspNetProvider")]
[assembly: InternalsVisibleTo("DotNetNuke.Tests.Content")]
[assembly: InternalsVisibleTo("DotNetNuke.Tests.Web")]
[assembly: InternalsVisibleTo("DotNetNuke.Tests.Urls")]
[assembly: InternalsVisibleTo("DotNetNuke.Tests.Professional")]
[assembly: InternalsVisibleTo("DotNetNuke.SiteExportImport")]