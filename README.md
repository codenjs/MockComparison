# MockComparison
Simple examples to compare mocking tools

I've used Moq for a number of years, so I'm very comfortable with it, but I could see the NSubstitute syntax being easier for someone new to mocking.
One complaint with NSubstitute is the scenario where I want to access the parameters received by a mock, and I have to use the CallInfo object.

## Give the mock behavior, by doing something with the parameters it receives

Moq lets you use a lambda expression and specify each parameter. In this example, the mock takes the two params (called userId and qty) and returns them in a string:
```
var service = new Mock<IService>();

service.Setup(s => s.GetData(It.IsAny<int>(), It.IsAny<int>()))
    .Returns<int, int>((userId, qty) => string.Format("{0}:{1}", userId, qty));
```

NSubstitute lets you use a lambda expression, but it must always accept one parameter (a "CallInfo" object) and then you access your parameters inside of that by index, using the callinfo.ArgAt<int>() syntax:
```
var service = Substitute.For<IService>();

service.GetData(Arg.Any<int>(), Arg.Any<int>())
    .Returns(callinfo => string.Format("{0}:{1}", callinfo.ArgAt<int>(0), callinfo.ArgAt<int>(1)));
```

## Make the mock save a copy of a parameter it received
This is a common scenario where the mock saves a copy of the parameter it received, so we can make assertions about the parameter at the end of the test.

Moq requires two lambda expressions, a Setup and a Callback
```
var repository = new Mock<IRepository>();
string dataSentToRepository = null;

repository.Setup(r => r.Save(It.IsAny<string>()))
    .Callback<string>(d => dataSentToRepository = d);

// Invoke test...

Assert.AreEqual("1", dataSentToRepository);
```

NSubstitute can do it with one lambda expression by using the Arg.Do() syntax, and you don't have to use the CallInfo object:
```
var repository = Substitute.For<IRepository>();
string dataSentToRepository = null;

repository.Save(Arg.Do<string>(d => dataSentToRepository = d));

// Invoke test...

Assert.AreEqual("1", dataSentToRepository);
```
