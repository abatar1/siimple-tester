using SimpleFluentTester.Reporter;
using SimpleFluentTester.TestSuite;
using SimpleFluentTester.Validators.Core;

namespace SimpleFluentTester.UnitTests.Tests;

public sealed class TestSuiteBuilderTests
{
    [Fact]
    public void UseOperation_InvalidReturnType_ShouldBeInvalid()
    {
        // Arrange
        var setup = TestSuite.TestSuite.Sequential
            .WithComparer<string>((x, y) => x == y);
        
        // Act
        var reporter = setup
            .UseOperation(TestHelpers.Adder)
            .Run();

        // Assert
        AssertInvalidContextValidation(reporter, ValidationSubject.Operation);
    }
    
    [Fact]
    public void UseOperation_NoReturn_ShouldBeInvalid()
    {
        // Arrange
        var setup = TestSuite.TestSuite.Sequential
            .WithComparer<int>((x, y) => x == y);
        
        // Act
        var reporter = setup
            .UseOperation((int x, int y) => { })
            .Run();

        // Assert
        AssertInvalidContextValidation(reporter, ValidationSubject.Operation);
    }
    
    [Fact]
    public void UseOperation_NoOperationSet_ShouldBeInvalid()
    {
        // Arrange
        var setup = TestSuite.TestSuite.Sequential
            .WithComparer<int>((x, y) => x == y);
        
        // Act
        var reporter = setup
            .Run();

        // Assert
        AssertInvalidContextValidation(reporter, ValidationSubject.Operation);
    }
    
    [Fact]
    public void UseOperation_WithCustomObjectWithoutSetOperation_ShouldBeInvalid()
    {
        // Arrange
        var setup = TestSuite.TestSuite.Sequential
            .WithComparer<int>((x, y) => x == y);
        
        // Act
        var reporter = setup
            .Run();

        // Assert
        AssertInvalidContextValidation(reporter, ValidationSubject.Operation);
    }
    
    [Fact]
    public void UseOperation_ValidReturnType_ShouldBeInvalid()
    {
        // Arrange
        var setup = TestSuite.TestSuite.Sequential
            .WithComparer<int>((x, y) => x == y);
        
        // Act
        var reporter = setup
            .UseOperation(TestHelpers.Adder)
            .Run();

        // Assert
        AssertValidContextValidation(reporter, ValidationSubject.Operation);
    }
    
    
    [Fact]
    public void Expect_NoReturnTypeSpecifiedWithInvalidExpectedType_ShouldBeInvalid()
    {
        // Arrange
        var builder = TestSuite.TestSuite.Sequential
            .UseOperation(TestHelpers.Adder);
            
        // Act    
        var reporter = builder
            .Expect("123").WithInput(1, 1)
            .Run();
        
        // Assert
        AssertInvalidTestCaseValidation(reporter, 1, ValidationSubject.Operation);
    }
    
    [Fact]
    public void Expect_OperationWithoutNullableParameterButNullExpected_ShouldBeInvalid()
    {
        // Arrange
        var builder = TestSuite.TestSuite.Sequential
            .UseOperation((int a, int b) => a + b);
            
        // Act    
        var reporter = builder
            .Expect(null).WithInput(1, 1)
            .Run();
        
        // Assert
        AssertInvalidTestCaseValidation(reporter, 1, ValidationSubject.Operation);
    }
    
    [Fact]
    public void Expect_EqualOperationAndExpectType_ShouldBeValid()
    {
        // Arrange
        var builder = TestSuite.TestSuite.Sequential
            .UseOperation((int a, int b) => a + b);
            
        // Act    
        var reporter = builder
            .Expect(2).WithInput(1, 1)
            .Run();
        
        // Assert
        AssertValidTestCaseValidation(reporter, 1, ValidationSubject.Operation);
    }
    
    [Fact]
    public void WithInput_ParametersNumberMoreThanExpected_ShouldBeInvalid()
    {
        // Arrange
        var builder = TestSuite.TestSuite.Sequential
            .UseOperation(TestHelpers.Adder);
            
        // Act    
        var reporter = builder
            .Expect(2).WithInput(1, 1, 1)
            .Run();
        
        // Assert
        AssertInvalidTestCaseValidation(reporter, 1, ValidationSubject.Inputs);
    }
    
    [Fact]
    public void WithInput_ParametersNumberLessThanExpected_ShouldBeInvalid()
    {
        // Arrange
        var builder = TestSuite.TestSuite.Sequential
            .UseOperation(TestHelpers.Adder);
            
        // Act    
        var reporter = builder
            .Expect(2).WithInput(1)
            .Run();
        
        // Assert
        AssertInvalidTestCaseValidation(reporter, 1, ValidationSubject.Inputs);
    }
    
    [Fact]
    public void WithInput_ParametersWrongType_ShouldBeInvalid()
    {
        // Arrange
        var builder = TestSuite.TestSuite.Sequential
            .UseOperation(TestHelpers.Adder);
            
        // Act    
        var reporter = builder
            .Expect(2).WithInput(1, "test")
            .Run();
        
        // Assert
        AssertInvalidTestCaseValidation(reporter, 1, ValidationSubject.Inputs);
    }
    
    [Fact]
    public void WithInput_InputNumberAndTypesCompatibleWithOperation_ShouldBeValid()
    {
        // Arrange
        var builder = TestSuite.TestSuite.Sequential
            .UseOperation(TestHelpers.Adder);
            
        // Act    
        var reporter = builder
            .Expect(2).WithInput(1, 1)
            .Run();
        
        // Assert
        AssertValidTestCaseValidation(reporter, 1, ValidationSubject.Inputs);
    }
    
    [Fact]
    public void WithComparer_ExpectStringWithIntComparer_ShouldThrow()
    {
        // Arrange
        var setup = TestSuite.TestSuite.Sequential
            .Expect("test").WithInput("test");
        
        // Act
        var runFunc = () => setup
            .WithComparer<int>((x, y) => x == y)
            .Run();

        // Assert
        Assert.Throws<InvalidCastException>(runFunc);
    }
    
    [Fact]
    public void WithComparer_UseCustomObjectWithoutComparer_ShouldBeInvalid()
    {
        // Arrange
        var setup = TestSuite.TestSuite.Sequential
            .Expect(new TestObject(1)).WithInput(new TestObject(1))
            .UseOperation((TestObject x) => x);
        
        // Act
        var reporter = setup
            .Run();

        // Assert
        AssertInvalidContextValidation(reporter, ValidationSubject.Comparer);
    }
    
    [Fact]
    public void WithComparer_ExpectIntWithIntComparer_ShouldBeValid()
    {
        // Arrange
        var setup = TestSuite.TestSuite.Sequential
            .UseOperation((int x) => x)
            .Expect(1).WithInput(1);
        
        // Act
        var reporter = setup
            .WithComparer<int>((x, y) => x == y)
            .Run();

        // Assert
        AssertValidContextValidation(reporter, ValidationSubject.Operation);
    }
    
    [Fact]
    public void Run_InvalidIterationNumber_ShouldBeInvalid()
    {
        // Arrange
        var builder1 = TestSuite.TestSuite.Sequential
            .UseOperation(TestHelpers.Adder);
        var builder2 = TestSuite.TestSuite.Sequential
            .UseOperation(TestHelpers.Adder);
        
        // Act
        var reporter1 = builder1
            .Expect(2).WithInput(1, 1)
            .Run(2);
        var reporter2 = builder2
            .Expect(2).WithInput(1, 1)
            .Run(1, 2);
        
        // Assert
        AssertInvalidContextValidation(reporter1, ValidationSubject.TestNumbers);
        AssertInvalidContextValidation(reporter2, ValidationSubject.TestNumbers);
    }
    
    [Fact]
    public void TestRunBuilder_InvalidDelegateReturnType_ShouldBeInvalid()
    {
        // Arrange
        var context = TestHelpers.CreateEmptyContext<int>();
        var builder = new TestSuiteBuilder<int>(context);
        
        // Act
        var reporter = builder
            .UseOperation((int _, int _) => "test")
            .Expect(2).WithInput(1, 1)
            .Run();
        
        // Assert
        AssertInvalidContextValidation(reporter, ValidationSubject.Operation);
    }
    
    private static void AssertValidContextValidation<TOutput>(ITestSuiteReporter<TOutput> reporter,
        ValidationSubject validationSubject)
    {
        var validationResult = reporter.TestSuiteResult.ContextValidationResults
            .SingleOrDefault(x => x.ValidationSubject == validationSubject);
        
        Assert.NotNull(validationResult);
        Assert.True(validationResult.IsValid);
        Assert.Equal(validationSubject, validationResult.ValidationSubject);
        Assert.Null(validationResult.Message);
    }
    
    private static void AssertInvalidContextValidation<TOutput>(ITestSuiteReporter<TOutput> reporter, 
        ValidationSubject validationSubject)
    {
        var validationResult = reporter.TestSuiteResult.ContextValidationResults
            .SingleOrDefault(x => x.ValidationSubject == validationSubject);
        
        Assert.NotNull(validationResult);
        Assert.False(validationResult.IsValid);
        Assert.Equal(validationSubject, validationResult.ValidationSubject);
        Assert.NotNull(validationResult.Message);
    }
    
    private static void AssertValidTestCaseValidation<TOutput>(ITestSuiteReporter<TOutput> reporter,
        int testCaseNumber,
        ValidationSubject validationSubject)
    {
        var validationResult = reporter.TestSuiteResult.TestCases
            .FirstOrDefault(x => x.Number == testCaseNumber)?.ValidationResults
            .SingleOrDefault(x => x.ValidationSubject == validationSubject);
        
        Assert.NotNull(validationResult);
        Assert.True(validationResult.IsValid);
        Assert.Equal(validationSubject, validationResult.ValidationSubject);
        Assert.Null(validationResult.Message);
    }
    
    private static void AssertInvalidTestCaseValidation<TOutput>(ITestSuiteReporter<TOutput> reporter, 
        int testCaseNumber,
        ValidationSubject validationSubject)
    {
        var validationResult = reporter.TestSuiteResult.TestCases
            .FirstOrDefault(x => x.Number == testCaseNumber)?.ValidationResults
            .SingleOrDefault(x => x.ValidationSubject == validationSubject);
        
        Assert.NotNull(validationResult);
        Assert.False(validationResult.IsValid);
        Assert.Equal(validationSubject, validationResult.ValidationSubject);
        Assert.NotNull(validationResult.Message);
    }
    
    private class TestObject(int value)
    {
        public int Value { get; } = value;
    }
}