using System;

namespace ChessRatingCalculator
{
    internal class Program
    {
        /// <remarks>
        /// <para>This program calculates the adjusted ratings <c>An</c> and <c>Bn</c> after a single game or multiple games. 
        /// If <c>n</c> (number of games) is not provided, it is assumed that only one game has been played.</para>
        /// <para>The parameter <c>q</c> represents the actual performance from the perspective of player A in the game(s):
        /// <list type="bullet">
        /// <item><description>If player A loses a single game, <c>q</c> is 0.</description></item>
        /// <item><description>If the game is a draw, <c>q</c> is 50.</description></item>
        /// <item><description>If player A wins a single game, <c>q</c> is 100.</description></item>
        /// </list>
        /// </para>
        /// <para>If multiple games have been played, <c>q</c> should reflect player A's performance across the entire match.
        /// For example, if two games are played and player A wins one and loses the other, <c>q</c> would be 50, representing 
        /// a 50% performance across the match. Thus, <c>q</c> scales from 0 (all losses) to 100 (all wins), based on the number 
        /// of games.</para>
        /// <para><strong>Note:</strong> A sanity check for <c>q</c> is performed only if <c>n = 1</c>, in which case <c>q</c> 
        /// must be 0, 50, or 100. For <c>n &gt; 1</c>, the user is responsible for ensuring <c>q</c> accurately reflects 
        /// player A's performance over multiple games.</para>
        /// <para>The adjustment factor <c>K</c> is calculated as <c>n / 20</c> to proportionally adjust ratings based 
        /// on the number of games played, with a maximum value of 1. Setting <c>K &lt;= 1</c> ensures that no grade 
        /// stretching occurs, meaning the ratings do not diverge uncontrollably. This value of <c>K</c> effectively 
        /// limits rating changes over multiple games. If <c>K</c> were allowed to exceed 1, the sum of rating changes 
        /// would grow proportionally with each game, causing exponential divergence or "stretching" in the ratings.</para>
        /// <para>The grade stretching test in the program will return <c>True</c> even when <c>K &gt; 1</c>, indicating that ratings 
        /// balance out individually for one calculation. However, multiple calculations using <c>K &gt; 1</c> would result 
        /// in cumulative stretching, where each player's rating could move outward over time. This is avoided by maintaining 
        /// <c>K</c> as a maximum of 1.</para>
        /// </remarks>
        static void Main(string[] args)
        {
            // Check if at least 5 arguments are provided
            if (args.Length < 5)
            {
                Console.WriteLine("Usage: ChessRatingCalculator <A> <aA> <B> <aB> <q> [<n>]");
                Console.WriteLine("Example: ChessRatingCalculator 1530 true 1450 true 50 1");
                return;
            }

            // Parse and validate input arguments
            double A, B, q;
            bool aA, aB;
            int n = 1; // Default number of games

            // Parse rating A and check range
            if (!double.TryParse(args[0], out A) || A < 0 || A > 3000)
            {
                Console.WriteLine("Error: A must be a number between 0 and 3000.");
                return;
            }

            // Parse aA and check validity
            if (!bool.TryParse(args[1], out aA))
            {
                Console.WriteLine("Error: aA must be 'true' or 'false'.");
                return;
            }

            // Parse rating B and check range
            if (!double.TryParse(args[2], out B) || B < 0 || B > 3000)
            {
                Console.WriteLine("Error: B must be a number between 0 and 3000.");
                return;
            }

            // Parse aB and check validity
            if (!bool.TryParse(args[3], out aB))
            {
                Console.WriteLine("Error: aB must be 'true' or 'false'.");
                return;
            }

            // Parse game result q and check range
            if (!double.TryParse(args[4], out q) || q < 0 || q > 100)
            {
                Console.WriteLine("Error: q must be a number between 0 and 100.");
                return;
            }

            // Parse optional number of games n, if provided, and check range
            if (args.Length > 5 && (!int.TryParse(args[5], out n) || n < 1))
            {
                Console.WriteLine("Error: n must be a positive integer representing the number of games.");
                return;
            }

            // Sanity check: if n == 1, then q should be 0, 50, or 100
            if (n == 1 && q != 0 && q != 50 && q != 100)
            {
                Console.WriteLine("Error: For a single game (n = 1), q must be 0 (loss), 50 (draw), or 100 (win).");
                return;
            }

            // Display the input values
            Console.WriteLine($"Initial Ratings: A = {A}, B = {B}");
            Console.WriteLine($"Active Status: aA = {aA}, aB = {aB}");
            Console.WriteLine($"Game Result (Actual Performance): q = {q}");
            Console.WriteLine($"Number of Games Played: n = {n}");

            // Constants
            double g = 50;                // Factor for expected performance calculation
            double K = Math.Min(n * 1.0 / 20.0, 1.0); // Adjustment factor, applies gradual change to grades

            // Calculate grade difference `d`
            double d = (A - B) / 8.0;
            Console.WriteLine($"Grade Difference d: {d}");

            // Determine `ka` and `kb` based on activity status
            double ka, kb;
            if (aA && aB)
            {
                ka = 1.0 / 2.0;
                kb = 1.0 / 2.0;
            }
            else if (!aA && !aB)
            {
                ka = 0;
                kb = 0;
            }
            else if (aA)
            {
                ka = 1;
                kb = 0;
            }
            else
            {
                ka = 0;
                kb = 1;
            }
            Console.WriteLine($"Coefficients: ka = {ka}, kb = {kb}");

            // Calculate expected performance `p`
            double p = 100.0 / (1.0 + Math.Pow(10, -d / g));
            Console.WriteLine($"Expected Performance p: {p}");

            // Calculate adjusted ratings `An` and `Bn`
            double An = A + K * ka * (q - p) * 8.0;
            double Bn = B + K * kb * ((100 - q) - (100 - p)) * 8.0;

            // Display adjusted ratings
            Console.WriteLine($"Adjusted Rating An: {Math.Round(An)}");
            Console.WriteLine($"Adjusted Rating Bn: {Math.Round(Bn)}");

            // Additional information
            Console.WriteLine($"Adjustment factor K: {K}");
            Console.WriteLine($"K <= 1 check (no grade stretching): {K <= 1}");  // Display True if K <= 1, False otherwise
            Console.WriteLine($"Sum of rating changes check (no grade stretching): {Math.Round((An - A) + (B - Bn), 2) == Math.Round(K * (q - p) * 8, 2)}");
            Console.WriteLine($"Total grade preservation check: {Math.Round(A + B) == Math.Round(An + Bn)}");
        }
    }
}
