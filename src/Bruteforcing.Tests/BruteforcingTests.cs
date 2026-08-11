// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BruteforcingTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="Bruteforcing" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BruteForcing.Tests;

/// <summary>
/// A class to test the <see cref="Bruteforcing"/> class.
/// </summary>
[TestClass]
public class BruteforcingTests
{
    /// <summary>
    /// The charset that all combination tests are built from.
    /// </summary>
    private const string Charset = "abc";

    /// <summary>
    /// Collects every result of an <see cref="IBruteforcing"/> into a list, using the same foreach loop as the
    /// Readme sample.
    /// </summary>
    /// <param name="bruteforce">The brute forcing instance to enumerate.</param>
    /// <returns>All produced strings in the order they were yielded.</returns>
    private static List<string> Enumerate(IBruteforcing bruteforce)
    {
        var results = new List<string>();

        foreach (string result in bruteforce)
        {
            results.Add(result);
        }

        return results;
    }

    /// <summary>
    /// Checks whether the constructor stores the charset, the minimum and the maximum as they were passed.
    /// </summary>
    [TestMethod]
    public void ConstructorStoresCharsetMinimumAndMaximum()
    {
        var bruteforce = new Bruteforcing(Charset, 2, 5);

        Assert.AreEqual(Charset, bruteforce.Charset);
        Assert.AreEqual(2, bruteforce.Minimum);
        Assert.AreEqual(5, bruteforce.Maximum);
    }

    /// <summary>
    /// Checks whether the charset, the minimum and the maximum can be changed after construction.
    /// </summary>
    [TestMethod]
    public void PropertiesCanBeChangedAfterConstruction()
    {
        var bruteforce = new Bruteforcing(Charset, 1, 1)
        {
            Charset = "xy",
            Minimum = 3,
            Maximum = 4,
        };

        Assert.AreEqual("xy", bruteforce.Charset);
        Assert.AreEqual(3, bruteforce.Minimum);
        Assert.AreEqual(4, bruteforce.Maximum);
    }

    /// <summary>
    /// Checks whether a length range of one over a single character charset yields exactly that one character.
    /// </summary>
    [TestMethod]
    public void SingleCharacterCharsetWithLengthOneYieldsThatCharacter()
    {
        var results = Enumerate(new Bruteforcing("a", 1, 1));

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("a", results[0]);
    }

    /// <summary>
    /// Checks whether the full set of combinations for the range one to two is produced, no more and no less.
    /// </summary>
    [TestMethod]
    public void EnumeratingCoversEveryCombinationOfTheRange()
    {
        var results = Enumerate(new Bruteforcing("ab", 1, 2));

        var expected = new[] { "a", "b", "aa", "ab", "ba", "bb" };
        CollectionAssert.AreEquivalent(expected, results);
    }

    /// <summary>
    /// Checks whether the number of results equals the sum of the charset length raised to each length in the range.
    /// </summary>
    [TestMethod]
    public void ResultCountEqualsTheSumOfThePowersOverTheRange()
    {
        var results = Enumerate(new Bruteforcing(Charset, 1, 3));

        // 3^1 + 3^2 + 3^3 = 3 + 9 + 27 = 39.
        Assert.AreEqual(39, results.Count);
    }

    /// <summary>
    /// Checks whether every produced combination is unique.
    /// </summary>
    [TestMethod]
    public void EnumeratingProducesOnlyDistinctResults()
    {
        var results = Enumerate(new Bruteforcing(Charset, 1, 3));

        Assert.AreEqual(results.Count, results.Distinct().Count());
    }

    /// <summary>
    /// Checks whether every produced combination has a length inside the requested range.
    /// </summary>
    [TestMethod]
    public void EveryResultHasALengthWithinTheRange()
    {
        var results = Enumerate(new Bruteforcing(Charset, 2, 4));

        Assert.IsTrue(results.All(result => result.Length is >= 2 and <= 4));
    }

    /// <summary>
    /// Checks whether every character of every produced combination comes from the charset.
    /// </summary>
    [TestMethod]
    public void EveryResultConsistsOnlyOfCharsetCharacters()
    {
        var results = Enumerate(new Bruteforcing(Charset, 1, 3));

        Assert.IsTrue(results.All(result => result.All(Charset.Contains)));
    }

    /// <summary>
    /// Checks whether a minimum that is larger than the maximum yields nothing.
    /// </summary>
    [TestMethod]
    public void MinimumLargerThanMaximumYieldsNothing()
    {
        var results = Enumerate(new Bruteforcing(Charset, 3, 1));

        Assert.AreEqual(0, results.Count);
    }
}
