﻿// --------------------------------------------------------------------------------------------
// Copyright (C) 2010-2013 SIL International. All rights reserved.
//
// Distributable under the terms of the MIT License, as specified in the license.rtf file.
// --------------------------------------------------------------------------------------------

using System.Xml.Linq;
using Chorus.merge.xml.generic;

namespace FLEx_ChorusPlugin.Infrastructure.Handling.CustomProperties
{
	/// <summary>
	/// Class that creates a descriptor that can be used later to find the element again, as when reviewing conflict.
	/// </summary>
	internal sealed class FieldWorksCustomPropertyContextGenerator : IGenerateContextDescriptor
	{
		#region Implementation of IGenerateContextDescriptor

		public ContextDescriptor GenerateContextDescriptor(string mergeElement, string filePath)
		{
			var customPropertyElement = XElement.Parse(mergeElement);
			var label = customPropertyElement.Attribute(SharedConstants.Class).Value + ": " + customPropertyElement.Attribute(SharedConstants.Name).Value;
			return new ContextDescriptor(label, "FIXTHIS");
		}

		#endregion
	}
}
