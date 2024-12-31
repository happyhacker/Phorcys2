using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phorcys.Services.Utilities;
using Xunit;

namespace Phorcys.Tests
{
	public class CalculatorTests
	{
		[Fact]
		public void Add_ReturnsCorrectSum()
		{
			// Arrange
			var calculator = new Calculator();

			// Act
			var result = calculator.Add(2, 3);

			// Assert
			Assert.Equal(5, result);
		}
	}
}