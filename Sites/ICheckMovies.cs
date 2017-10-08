using System.IO;

namespace TraktRater.Sites
{
    // ReSharper disable once InconsistentNaming
    internal class ICheckMovies : IRateSite
    {
        private string iCheckMoviesFilename;
        private bool importCancelled;

        public ICheckMovies(string iCheckMoviesFilename)
        {
            this.iCheckMoviesFilename = iCheckMoviesFilename;
            Enabled = File.Exists(iCheckMoviesFilename);
        }

        public string Name => "ICheckMovies";

        public bool Enabled { get; set; }

        public void Cancel()
        {
            importCancelled = true;
        }

        public void ImportRatings()
        {
            if (importCancelled)
            {
                return;
            }

        }
    }
}