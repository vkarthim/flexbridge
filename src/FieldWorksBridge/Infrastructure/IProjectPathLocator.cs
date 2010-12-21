﻿using System.Collections.Generic;

namespace FieldWorksBridge.Infrastructure
{
	/// <summary>
	/// Interface that locates one, or more, base paths to FieldWorks projects.
	/// </summary>
	internal interface IProjectPathLocator
	{
		HashSet<string> BaseFolderPaths { get; }
	}
}