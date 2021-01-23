# Property-based testing with [FsCheck](https://fscheck.github.io/FsCheck/)

## Walk-through

This repository serves as a walk-through about FsCheck and property-based
testing basics. There are ten steps, git tagged simply as `1`, `2`, ..., `10`.

* Use `git checkout <i>` to get into *i*-th step
* Use `git log -1 -p` to see what is added in this step and why

## Unit tests vs Property-based tests

* Unit tests
  * "Example-based" testing, with a finite table of inputs and expected outputs
  * Easy to write
  * Good for sanity check, avoiding regressions and testing known corner cases
* Property-based tests
  * Random valid inputs and known expected high-level properties for outputs
  * Much harder to write, requires more thinking
  * Generally more powerful, covering a large subset of the input space and
    having potential to find obscure bugs

**Use both approaches! Each one has different purposes and advantages.**

Add failing inputs discovered by property-based testing as unit tests for
regression testing.

## Generation and shrinking

* Built-in combinators
  * Create data with `Choose`, `OneOf`, `ListOf`, ...
  * Compose primitive types with `Select`, `Zip`, ...
  * Ensure validity and sanity with `Where`, `Frequency`, ...
* FsCheck ensures growing complexity of generated inputs
  * *Ex.*: string composed of ascii letters and numbers, then adding symbols
    (`{`, `#`), then command characters (`\n`), then unicode characters (`ß`,
    `💣`)
* Automatic shrinking of a failing input to a (hopefully) minimal
  counter-example for easy manual debugging
  * *Ex.*: numbers get smaller, lists get fewer elements
* Use `Select` at al. from FsCheck, not LINQ, for preserving shrinkability

## Properties

* Examples
  * Output of a sorting algorithm is the array being sorted
  * A complex super-efficient algorithm behaves the same as a naive
    easy-to-write implementation
* There are some general patterns for inspiration:
  * [Choosing properties for property-based
    testing](https://fsharpforfunandprofit.com/posts/property-based-testing-2/)
* Coming up with properties of the output may be difficult
* Requires a lot of thinking, creativity and practice

## Remember

* Testing your code on random data should give you more confidence, but it is
  still not a proof!
* An output can be incorrect and still satisfy all your properties
  * *Ex.*: `int[] Sort(int[] array) => new int[] { 1, 2, 3 }` satisfies *sorted*
    property, but is obviously not a correct implementation

## Inspiration

* Test if your web API conforms to its OpenAPI specification and does not crash
  with [schemathesis](https://schemathesis.readthedocs.io/en/stable/cli.html#basic-usage)
  * Randomly generates requests according to an OpenAPI specification
  * Checks not returning 5xx HTTP errors and specification conformance for status code, content type, headers and response schema
  * In [future](https://github.com/schemathesis/schemathesis/pull/831), "negative testing" with invalid requests
* Use [model-based](https://medium.com/@tylerneely/reliable-systems-series-model-based-property-testing-e89a433b360#09d6)
  approach for testing stateful code with a sequence of operations/events
  ([documentation](https://fscheck.github.io/FsCheck/StatefulTesting.html) for FsCheck)
  * If you are developing a key-value database, the abstracted model is simply a
    `Dictionary`
  * If you are developing a job scheduler, you can check whether all jobs were
    executed, and in somewhat correct order, and the pipeline successfully
    finished
  * If you are developing an IoT device or gateway, simulate connection drops as
    an event and check whether some data were lost

## Success stories

*Warning: these are epic examples, but PBT helps in day-to-day life too*

All taken from [Introduction to Stateful Property Based Testing - Lambda Days 2019](https://www.infoq.com/news/2019/08/lamda-days-2019-stateful-pbt/).

* When the volume of the radio was being turned up or down in a Volvo car, the
  brakes did not work
* Modelling and PBT was able to reproduce a phantom keys issue in LevelDB
  database, where the minimal example consisted of 17 consecutive operations
* Modelling the behavior of Dropbox (a distributed, highly-concurrent software)
  identified some orderings of operations which led to loss of data
