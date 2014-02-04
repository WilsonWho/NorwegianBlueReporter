using System.IO;

namespace StatsReader
{
    public interface IStatsReader
    {
        // Given a TextReader stream, parse it and store it internally
        void ParseInput(TextReader input);

    }
}
