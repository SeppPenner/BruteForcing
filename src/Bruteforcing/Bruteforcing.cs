// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bruteforcing.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The brute forcing class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BruteForcing
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <inheritdoc cref="IEnumerable"/>
    /// <inheritdoc cref="IBruteforcing"/>
    /// <summary>
    /// The brute forcing class.
    /// </summary>
    /// <seealso cref="IBruteforcing"/>
    /// <seealso cref="IEnumerable"/>
    public class Bruteforcing : IBruteforcing, IEnumerable
    {
        /// <summary>
        /// The length.
        /// </summary>
        private ulong length;

        /// <summary>
        /// The string builder.
        /// </summary>
        private StringBuilder stringBuilder = new StringBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="Bruteforcing"/> class.
        /// </summary>
        /// <param name="charset">The charset.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        public Bruteforcing(string charset, int minimum, int maximum)
        {
            this.Charset = charset;
            this.Maximum = maximum;
            this.Minimum = minimum;
        }

        /// <summary>
        /// Gets or sets the charset.
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        public int Minimum { get; set; }

        /// <summary>
        /// Gets the brute forcing enumerator.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/>.</returns>
        public IEnumerator GetEnumerator()
        {
            this.length = (ulong)this.Charset.Length;
            for (double x = this.Minimum; x <= this.Maximum; x++)
            {
                var total = (ulong)Math.Pow(this.Charset.Length, x);
                ulong counter = 0;
                while (counter < total)
                {
                    var a = this.Factoradic(counter, x - 1);
                    yield return a;
                    counter++;
                }
            }
        }

        /// <summary>
        /// Builds a factoradic.
        /// </summary>
        /// <param name="l">The length.</param>
        /// <param name="power">The power.</param>
        /// <returns>A factoradic as <see cref="string"/>.</returns>
        private string Factoradic(ulong l, double power)
        {
            this.stringBuilder.Length = 0;

            while (power >= 0)
            {
                this.stringBuilder = this.stringBuilder.Append(this.Charset[(int)(l % this.length)]);
                l /= this.length;
                power--;
            }

            return this.stringBuilder.ToString();
        }
    }
}