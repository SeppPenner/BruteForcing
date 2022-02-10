// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBruteforcing.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The brute forcing interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BruteForcing;

/// <summary>
/// The brute forcing interface.
/// </summary>
public interface IBruteforcing
{
    /// <summary>
    /// Gets the brute forcing enumerator.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/>.</returns>
    IEnumerator GetEnumerator();
}
