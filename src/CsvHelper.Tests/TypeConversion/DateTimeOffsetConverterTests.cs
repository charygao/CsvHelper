﻿// Copyright 2009-2017 Josh Close and Contributors
// This file is a part of CsvHelper and is dual licensed under MS-PL and Apache 2.0.
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html for MS-PL and http://opensource.org/licenses/Apache-2.0 for Apache 2.0.
// https://github.com/JoshClose/CsvHelper
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvHelper.Tests.TypeConversion
{
	[TestClass]
	public class DateTimeOffsetConverterTests
	{
		[TestMethod]
		public void ConvertToStringTest()
		{
			var converter = new DateTimeOffsetConverter();
			var propertyMapData = new CsvPropertyMapData( null )
			{
				TypeConverter = converter,
				TypeConverterOptions = { CultureInfo = CultureInfo.CurrentCulture }
			};

			var dateTime = DateTimeOffset.Now;

			// Valid conversions.
			Assert.AreEqual( dateTime.ToString(), converter.ConvertToString( dateTime, null, propertyMapData ) );

			// Invalid conversions.
			Assert.AreEqual( "1", converter.ConvertToString( 1, null, propertyMapData ) );
			Assert.AreEqual( "", converter.ConvertToString( null, null, propertyMapData ) );
		}

		[TestMethod]
		public void ConvertFromStringTest()
		{
			var converter = new DateTimeOffsetConverter();

			var propertyMapData = new CsvPropertyMapData( null );
			propertyMapData.TypeConverterOptions.CultureInfo = CultureInfo.CurrentCulture;

			var dateTime = DateTimeOffset.Now;

			// Valid conversions.
			Assert.AreEqual( dateTime.ToString(), converter.ConvertFromString( dateTime.ToString(), null, propertyMapData ).ToString() );
			Assert.AreEqual( dateTime.ToString(), converter.ConvertFromString( dateTime.ToString( "o" ), null, propertyMapData ).ToString() );
			Assert.AreEqual( dateTime.ToString(), converter.ConvertFromString( " " + dateTime + " ", null, propertyMapData ).ToString() );

			// Invalid conversions.
			try
			{
				converter.ConvertFromString( null, null, propertyMapData );
				Assert.Fail();
			}
			catch( CsvTypeConverterException )
			{
			}
		}

#if !PCL
		[TestMethod]
		public void ComponentModelCompatibilityTest()
		{
			var converter = new DateTimeOffsetConverter();
			var cmConverter = new System.ComponentModel.DateTimeOffsetConverter();

			var propertyMapData = new CsvPropertyMapData( null );
			propertyMapData.TypeConverterOptions.CultureInfo = CultureInfo.CurrentCulture;

			try
			{
				cmConverter.ConvertFromString( null );
				Assert.Fail();
			}
			catch( NotSupportedException ) { }

			try
			{
				converter.ConvertFromString( null, null, propertyMapData );
				Assert.Fail();
			}
			catch( CsvTypeConverterException ) { }

			try
			{
				cmConverter.ConvertFromString( "blah" );
				Assert.Fail();
			}
			catch( FormatException ) { }

			try
			{
				converter.ConvertFromString( "blah", null, propertyMapData );
			}
			catch( FormatException ) { }
		}
#endif
	}
}
