namespace Assignment.GameSystem
{
    /// <summary>
    /// Defines the logging abstraction used by the game.
    /// </summary>
    /// <remarks>
    /// Domain classes (<see cref="Character"/>, <see cref="Game"/>) depend on this
    /// interface instead of calling <c>Console</c> directly, so UI output is a
    /// separate responsibility that can be replaced (e.g. in tests).
    /// </remarks>
    public interface IGameLogger
    {
        /// <summary>
        /// Writes a message without a trailing new line.
        /// </summary>
        /// <param name="message">The message to write.</param>
        void Log(string message);

        /// <summary>
        /// Writes a message followed by a new line.
        /// </summary>
        /// <param name="message">The message to write, or <c>null</c> for an empty line.</param>
        void LogLine(string? message = null);
    }

    /// <summary>
    /// Writes game log messages to the console.
    /// </summary>
    /// <remarks>
    /// The default <see cref="IGameLogger"/> implementation used by <see cref="Game"/>.
    /// </remarks>
    public sealed class ConsoleGameLogger : IGameLogger
    {
        /// <summary>
        /// Writes a message to the console without a trailing new line.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Log(string message) => Console.Write(message);

        /// <summary>
        /// Writes a message to the console followed by a new line.
        /// </summary>
        /// <param name="message">The message to write, or <c>null</c> for an empty line.</param>
        public void LogLine(string? message = null) => Console.WriteLine(message);
    }
}
