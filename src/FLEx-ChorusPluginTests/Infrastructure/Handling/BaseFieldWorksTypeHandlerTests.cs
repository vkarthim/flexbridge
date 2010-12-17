﻿using System.Linq;
using Chorus.FileTypeHanders;
using NUnit.Framework;

namespace FLEx_ChorusPluginTests.Infrastructure.Handling
{
	public abstract class BaseFieldWorksTypeHandlerTests
	{
		protected IChorusFileTypeHandler FileHandler;

		[TestFixtureSetUp]
		public virtual void FixtureSetup()
		{
			FileHandler = (from handler in ChorusFileTypeHandlerCollection.CreateWithInstalledHandlers().Handlers
						   where handler.GetType().Name == "FieldWorksCommonFileHandler"
						   select handler).First();
		}

		[TestFixtureTearDown]
		public virtual void FixtureTearDown()
		{
			FileHandler = null;
		}
	}
}