using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using TraktRater.UI;
using TraktRater.Settings;

namespace TraktRater
{
    static class Export
    {
        public static bool Cancel { get; set; }

        public static void CreateWatchedEpisodeCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting watched episodes from trakt.tv");
            var watchedShows = TraktAPI.TraktAPI.GetWatchedShows();
            if (watchedShows != null)
            {
                // create a flattened structure suitable for csv i.e. show and episode details at the same level in the heiracrchy
                var episodesWatched = new List<object>();
                foreach (var show in watchedShows)
                {
                    foreach (var season in show.Seasons)
                    {
                        foreach (var episode in season.Episodes)
                        {
                            episodesWatched.Add(new
                            {
                                ShowTraktId = show.Show.Ids.Trakt,
                                ShowTvdbId = show.Show.Ids.TvdbId,
                                ShowImdbId = show.Show.Ids.ImdbId,
                                ShowTmdbId = show.Show.Ids.TmdbId,
                                ShowSlug = show.Show.Ids.Slug,
                                ShowTitle = show.Show.Title,
                                ShowYear = show.Show.Year,
                                Episode = episode.Number,
                                Season = season.Number,
                                Plays = episode.Plays,
                                WatchedAt = episode.WatchedAt
                            });
                        }
                    }
                }

                UIUtils.UpdateStatus("Creating watched episodes csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "watched_episodes.csv"), episodesWatched);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of watched epsiodes from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateWatchedMoviesCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting watched movies from trakt.tv");
            var watchedMovies = TraktAPI.TraktAPI.GetWatchedMovies();
            if (watchedMovies != null)
            {
                var moviesWatched = new List<object>();
                foreach (var item in watchedMovies)
                {
                    moviesWatched.Add(new
                    {
                        TraktId = item.Movie.Ids.Trakt,
                        ImdbId = item.Movie.Ids.ImdbId,
                        TmdbId = item.Movie.Ids.TmdbId,
                        Slug = item.Movie.Ids.Slug,
                        Title = item.Movie.Title,
                        Year = item.Movie.Year,
                        WatchedAt = item.WatchedAt,
                        Plays = item.Plays                        
                    });
                }

                UIUtils.UpdateStatus("Creating watched movies csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "watched_movies.csv"), moviesWatched);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of watched movies from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateCollectedEpisodeCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting collected shows from trakt.tv");
            var collectedShows = TraktAPI.TraktAPI.GetCollectedShows();
            if (collectedShows != null)
            {
                var episodesCollected = new List<object>();
                foreach (var show in collectedShows)
                {
                    foreach (var season in show.Seasons)
                    {
                        foreach (var episode in season.Episodes)
                        {
                            episodesCollected.Add(new
                            {
                                ShowTraktId = show.Show.Ids.Trakt,
                                ShowTvdbId = show.Show.Ids.TvdbId,
                                ShowImdbId = show.Show.Ids.ImdbId,
                                ShowTmdbId = show.Show.Ids.TmdbId,
                                ShowSlug = show.Show.Ids.Slug,
                                ShowTitle = show.Show.Title,
                                ShowYear = show.Show.Year,
                                Episode = episode.Number,
                                Season = season.Number,
                                CollectedAt = episode.CollectedAt
                            });
                        }
                    }
                }

                UIUtils.UpdateStatus("Creating collected episodes csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "collected_episodes.csv"), episodesCollected);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of collected shows from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateCollectedMoviesCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting collected movies from trakt.tv");
            var collectedMovies = TraktAPI.TraktAPI.GetCollectedMovies();
            if (collectedMovies != null)
            {
                var moviesCollected = new List<object>();
                foreach (var item in collectedMovies)
                {
                    moviesCollected.Add(new
                    {
                        TraktId = item.Movie.Ids.Trakt,
                        ImdbId = item.Movie.Ids.ImdbId,
                        TmdbId = item.Movie.Ids.TmdbId,
                        Slug = item.Movie.Ids.Slug,
                        Title = item.Movie.Title,
                        Year = item.Movie.Year,
                        CollectedAt = item.CollectedAt
                    });
                }

                UIUtils.UpdateStatus("Creating watched movies csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "collected_movies.csv"), moviesCollected);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of collected movies from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateRatedEpisodeCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting rated episodes from trakt.tv");
            var ratedEpisodes = TraktAPI.TraktAPI.GetRatedEpisodes();
            if (ratedEpisodes != null)
            {
                var episodesRated = new List<object>();
                foreach (var item in ratedEpisodes)
                {
                    episodesRated.Add(new
                    {
                        ShowTraktId = item.Show.Ids.Trakt,
                        ShowTvdbId = item.Show.Ids.TvdbId,
                        ShowImdbId = item.Show.Ids.ImdbId,
                        ShowTmdbId = item.Show.Ids.TmdbId,
                        ShowSlug = item.Show.Ids.Slug,
                        ShowTitle = item.Show.Title,
                        ShowYear = item.Show.Year,
                        Episode = item.Episode.Season,
                        Season = item.Episode.Number,
                        EpisodeImdbId = item.Episode.Ids.ImdbId,
                        EpisodeTmdbId = item.Episode.Ids.TmdbId,
                        EpisodeTraktId = item.Episode.Ids.Trakt,
                        EpisodeTvdbId = item.Episode.Ids.TvdbId,
                        EpisodeTitle = item.Episode.Title,
                        RatedAt = item.RatedAt,
                        Rating = item.Rating
                    });      
                }

                UIUtils.UpdateStatus("Creating rated episodes csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "rated_episodes.csv"), episodesRated);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of rated episodes from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateRatedShowsCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting rated shows from trakt.tv");
            var ratedShows = TraktAPI.TraktAPI.GetRatedShows();
            if (ratedShows != null)
            {
                var showsRated = new List<object>();
                foreach (var item in ratedShows)
                {
                    showsRated.Add(new
                    {
                        Title = item.Show.Title,
                        Year = item.Show.Year,
                        ImdbId = item.Show.Ids.ImdbId,
                        TmdbId = item.Show.Ids.TmdbId,
                        TvdbId = item.Show.Ids.TvdbId,
                        TraktId = item.Show.Ids.Trakt,
                        Slug = item.Show.Ids.Slug,
                        RatedAt = item.RatedAt,
                        Rating = item.Rating
                    });
                }

                UIUtils.UpdateStatus("Creating rated shows csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "rated_shows.csv"), showsRated);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of rated shows from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateRatedSeasonsCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting rated seasons from trakt.tv");
            var ratedSeasons = TraktAPI.TraktAPI.GetRatedSeasons();
            if (ratedSeasons != null)
            {
                var seasonsRated = new List<object>();
                foreach (var item in ratedSeasons)
                {
                    seasonsRated.Add(new
                    {
                        Title = item.Show.Title,
                        Year = item.Show.Year,
                        ImdbId = item.Show.Ids.ImdbId,
                        TmdbId = item.Show.Ids.TmdbId,
                        TvdbId = item.Show.Ids.TvdbId,
                        TraktId = item.Show.Ids.Trakt,
                        Slug = item.Show.Ids.Slug,
                        Season = item.Season.Number,
                        RatedAt = item.RatedAt,
                        Rating = item.Rating
                    });
                }

                UIUtils.UpdateStatus("Creating rated seasons csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "rated_seasons.csv"), seasonsRated);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of rated seasons from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateRatedMoviesCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting rated movies from trakt.tv");
            var ratedMovies = TraktAPI.TraktAPI.GetRatedMovies();
            if (ratedMovies != null)
            {
                var moviesRated = new List<object>();
                foreach (var item in ratedMovies)
                {
                    moviesRated.Add(new
                    {
                        Title = item.Movie.Title,
                        Year = item.Movie.Year,
                        ImdbId = item.Movie.Ids.ImdbId,
                        TmdbId = item.Movie.Ids.TmdbId,
                        TraktId = item.Movie.Ids.Trakt,
                        Slug = item.Movie.Ids.Slug,
                        RatedAt = item.RatedAt,
                        Rating = item.Rating
                    });
                }

                UIUtils.UpdateStatus("Creating rated movies csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "rated_movies.csv"), moviesRated);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of rated movies from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateWatchlistEpisodesCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting watchlist episodes from trakt.tv");
            var watchlistedEpisodes = TraktAPI.TraktAPI.GetWatchlistEpisodes();
            if (watchlistedEpisodes != null)
            {
                var episodesWatchlisted = new List<object>();
                foreach (var item in watchlistedEpisodes)
                {
                    episodesWatchlisted.Add(new
                    {
                        ShowTraktId = item.Show.Ids.Trakt,
                        ShowTvdbId = item.Show.Ids.TvdbId,
                        ShowImdbId = item.Show.Ids.ImdbId,
                        ShowTmdbId = item.Show.Ids.TmdbId,
                        ShowSlug = item.Show.Ids.Slug,
                        ShowTitle = item.Show.Title,
                        ShowYear = item.Show.Year,                        
                        EpisodeImdbId = item.Episode.Ids.ImdbId,
                        EpisodeTmdbId = item.Episode.Ids.TmdbId,
                        EpisodeTraktId = item.Episode.Ids.Trakt,
                        EpisodeTvdbId = item.Episode.Ids.TvdbId,
                        Episode = item.Episode.Number,
                        Season = item.Episode.Season,
                        WatchlistedAt = item.InsertedAt,
                        Rank = item.Rank
                    });
                }

                UIUtils.UpdateStatus("Creating watchlisted episodes csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "watchlisted_episodes.csv"), episodesWatchlisted);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of watchlisted episodes from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateWatchlistSeasonsCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting watchlist seasons from trakt.tv");
            var watchlistedSeasons = TraktAPI.TraktAPI.GetWatchlistSeasons();
            if (watchlistedSeasons != null)
            {
                var seasonsWatchlisted = new List<object>();
                foreach (var item in watchlistedSeasons)
                {
                    seasonsWatchlisted.Add(new
                    {
                        ShowTraktId = item.Show.Ids.Trakt,
                        ShowTvdbId = item.Show.Ids.TvdbId,
                        ShowImdbId = item.Show.Ids.ImdbId,
                        ShowTmdbId = item.Show.Ids.TmdbId,
                        ShowSlug = item.Show.Ids.Slug,
                        ShowTitle = item.Show.Title,
                        ShowYear = item.Show.Year,
                        Season = item.Season.Number,
                        SeasonTvdbId = item.Season.Ids.TvdbId,
                        SeasonTmdbId = item.Season.Ids.TmdbId,
                        WatchlistedAt = item.InsertedAt,
                        Rank = item.Rank
                    });
                }

                UIUtils.UpdateStatus("Creating watchlisted seasons csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "watchlisted_seasons.csv"), seasonsWatchlisted);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of watchlisted seasons from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateWatchlistShowsCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting watchlist shows from trakt.tv");
            var watchlistedShows = TraktAPI.TraktAPI.GetWatchlistShows();
            if (watchlistedShows != null)
            {
                var showsWatchlisted = new List<object>();
                foreach (var item in watchlistedShows)
                {
                    showsWatchlisted.Add(new
                    {
                        TraktId = item.Show.Ids.Trakt,
                        TvdbId = item.Show.Ids.TvdbId,
                        ImdbId = item.Show.Ids.ImdbId,
                        TmdbId = item.Show.Ids.TmdbId,
                        Slug = item.Show.Ids.Slug,
                        Title = item.Show.Title,
                        Year = item.Show.Year,
                        WatchlistedAt = item.InsertedAt,
                        Rank = item.Rank
                    });
                }

                UIUtils.UpdateStatus("Creating watchlisted shows csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "watchlisted_shows.csv"), showsWatchlisted);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of watchlisted shows from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateWatchlistMoviesCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting watchlist movies from trakt.tv");
            var watchlistedMovies = TraktAPI.TraktAPI.GetWatchlistMovies();
            if (watchlistedMovies != null)
            {
                var moviesWatchlisted = new List<object>();
                foreach (var item in watchlistedMovies)
                {
                    moviesWatchlisted.Add(new
                    {
                        TraktId = item.Movie.Ids.Trakt,                        
                        ImdbId = item.Movie.Ids.ImdbId,
                        TmdbId = item.Movie.Ids.TmdbId,
                        Slug = item.Movie.Ids.Slug,
                        Title = item.Movie.Title,
                        Year = item.Movie.Year,
                        WatchlistedAt = item.InsertedAt,
                        Rank = item.Rank
                    });
                }

                UIUtils.UpdateStatus("Creating watchlisted movies csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "watchlisted_movies.csv"), moviesWatchlisted);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of watchlisted movies from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreatePausedEpisodesCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting paused episodes from trakt.tv");
            var pausedEpisodes = TraktAPI.TraktAPI.GetPausedEpisodes();
            if (pausedEpisodes != null)
            {
                var episodesPaused = new List<object>();
                foreach (var item in pausedEpisodes)
                {
                    episodesPaused.Add(new
                    {
                        Id = item.Id,
                        ShowTraktId = item.Show.Ids.Trakt,
                        ShowTvdbId = item.Show.Ids.TvdbId,
                        ShowImdbId = item.Show.Ids.ImdbId,
                        ShowTmdbId = item.Show.Ids.TmdbId,
                        ShowSlug = item.Show.Ids.Slug,
                        ShowTitle = item.Show.Title,
                        ShowYear = item.Show.Year,
                        EpisodeImdbId = item.Episode.Ids.ImdbId,
                        EpisodeTmdbId = item.Episode.Ids.TmdbId,
                        EpisodeTraktId = item.Episode.Ids.Trakt,
                        EpisodeTvdbId = item.Episode.Ids.TvdbId,
                        Episode = item.Episode.Number,
                        Season = item.Episode.Season,
                        PausedAt = item.PausedAt,
                        Progress = item.Progress
                    });
                }

                UIUtils.UpdateStatus("Creating paused episodes csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "paused_episodes.csv"), episodesPaused);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of paused episodes from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreatePausedMoviesCsv()
        {
            if (Cancel) return;

            UIUtils.UpdateStatus("Getting paused movies from trakt.tv");
            var pausedMovies = TraktAPI.TraktAPI.GetPausedMovies();
            if (pausedMovies != null)
            {
                var moviesPaused = new List<object>();
                foreach (var item in pausedMovies)
                {
                    moviesPaused.Add(new
                    {
                        Id = item.Id,
                        TraktId = item.Movie.Ids.Trakt,
                        ImdbId = item.Movie.Ids.ImdbId,
                        TmdbId = item.Movie.Ids.TmdbId,
                        Slug = item.Movie.Ids.Slug,
                        Title = item.Movie.Title,
                        Year = item.Movie.Year,
                        PausedAt = item.PausedAt,
                        Progress = item.Progress
                    });
                }

                UIUtils.UpdateStatus("Creating paused movies csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "paused_movies.csv"), moviesPaused);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of paused movies from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateCustomListsCsv()
        {
            if (Cancel) return;

            // First get the users custom lists

            UIUtils.UpdateStatus("Getting custom lists from trakt.tv");
            var customLists = TraktAPI.TraktAPI.GetCustomLists();
            if (customLists != null)
            {
                var lists = new List<object>();
                foreach (var item in customLists)
                {
                    lists.Add(new
                    {
                        TraktId = item.Ids.Trakt,
                        Slug = item.Ids.Slug,
                        Name = item.Name,
                        Description = item.Description,
                        Privacy = item.Privacy,
                        DisplayNumbers = item.DisplayNumbers,
                        AllowComments = item.AllowComments,
                        SortBy = item.SortBy,
                        SortHow = item.SortHow,
                        ItemCount = item.ItemCount,
                        CommentCount = item.Comments,
                        Likes = item.Likes,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt
                    });
                }

                UIUtils.UpdateStatus("Creating custom lists csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "custom_lists.csv"), lists);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current custom lists from trakt.tv", true);
                Thread.Sleep(2000);
                return;
            }

            // Next get the items for each custom list

            foreach(var list in customLists)
            {
                UIUtils.UpdateStatus($"Getting custom list items for '{list.Name}' from trakt.tv");
                var customListItems = TraktAPI.TraktAPI.GetCustomListItems(list.Ids.Trakt.ToString());
                if (customListItems != null)
                {
                    var listItems = new List<object>();
                    foreach (var item in customListItems)
                    {
                        // important to check for null as an item may be of a different type
                        // NB: the season object does not return for episodes, so get season number from episode object
                        listItems.Add(new
                        {
                            Rank = item.Rank,
                            Id = item.Id,
                            Type = item.Type,
                            ListedAt = item.ListedAt,
                            MovieTitle = item.Movie?.Title,
                            MovieYear = item.Movie?.Year,
                            MovieTraktId = item.Movie?.Ids.Trakt,
                            MovieImdbId = item.Movie?.Ids.ImdbId,
                            MovieTmdbId = item.Movie?.Ids.TmdbId,
                            MovieSlug = item.Movie?.Ids.Slug,
                            ShowTitle = item.Show?.Title,
                            ShowYear = item.Show?.Year,
                            ShowTraktId = item.Show?.Ids.Trakt,
                            ShowTvdbId = item.Show?.Ids.TvdbId,
                            ShowTmdbId = item.Show?.Ids.TmdbId,
                            ShowSlug = item.Show?.Ids.Slug,
                            SeasonTvdbId = item.Season?.Ids.TvdbId,
                            SeasonTmdbId = item.Season?.Ids.TmdbId,
                            SeasonNumber = item.Season?.Number.ToString() ?? item.Episode?.Season.ToString(),
                            EpisodeNumber = item.Episode?.Number,
                            EpisodeTraktId = item.Episode?.Ids.Trakt,
                            EpisodeTvdbId = item.Episode?.Ids.TvdbId,
                            EpisodeTmdbId = item.Episode?.Ids.TmdbId,
                            EpisodeImdbId = item.Episode?.Ids.ImdbId,
                            PersonName = item.Person?.Name,
                            PersonTraktId = item.Person?.Ids.Trakt,
                            PersonImdbId = item.Person?.Ids.ImdbId,
                            PersonTmdbId = item.Person?.Ids.TmdbId,
                            PersonSlug = item.Person?.Ids.Slug
                        });                        
                    }

                    UIUtils.UpdateStatus($"Creating custom list '{list.Name}' csv file");
                    // using the slug for the filename should be file system friendly
                    WriteToCsv(Path.Combine(AppSettings.CsvExportPath, $"custom_list_{list.Ids.Slug}.csv"), listItems);
                }
                else
                {
                    UIUtils.UpdateStatus($"Failed to get current custom list items for '{list.Name}' from trakt.tv", true);
                    Thread.Sleep(2000);
                }

            }
        }

        public static void CreateCommentedEpisodesCsv()
        {
            if (Cancel) return;

            int i = 0;
            UIUtils.UpdateStatus($"Getting commented episodes from trakt.tv, Page: {++i}");
            var firstPage = TraktAPI.TraktAPI.GetUsersComments(type: "episodes");
            if (firstPage != null)
            {
                int pageCount = firstPage.TotalPages;

                // store the results from the first page request
                var pagedItems = firstPage.Comments;
                
                for (i = 2; i <= firstPage.TotalPages; i++)
                {
                    UIUtils.UpdateStatus($"Getting commented episodes from trakt.tv, Page: {i}/{pageCount}");
                    var nextPage = TraktAPI.TraktAPI.GetUsersComments(type: "episodes", page: i);
                    if (nextPage == null) break;

                    // add the next page of items to the collection
                    pagedItems = pagedItems.Union(nextPage.Comments);
                }
                
                var episodesCommented = new List<object>();
                foreach (var item in pagedItems)
                {
                    episodesCommented.Add(new
                    {
                        Id = item.Comment.Id,
                        ParentId = item.Comment.ParentId,
                        Comment = item.Comment.Text,
                        IsSpoiler = item.Comment.IsSpoiler,
                        IsReview = item.Comment.IsReview,
                        Replies = item.Comment.Replies,
                        Likes = item.Comment.Likes,
                        UserRating = item.Comment.UserRating,
                        CreatedAt = item.Comment.CreatedAt,
                        UpdatedAt = item.Comment.UpdatedAt,
                        ShowTraktId = item.Show.Ids.Trakt,
                        ShowTvdbId = item.Show.Ids.TvdbId,
                        ShowImdbId = item.Show.Ids.ImdbId,
                        ShowTmdbId = item.Show.Ids.TmdbId,
                        ShowSlug = item.Show.Ids.Slug,
                        ShowTitle = item.Show.Title,
                        ShowYear = item.Show.Year,
                        EpisodeImdbId = item.Episode.Ids.ImdbId,
                        EpisodeTmdbId = item.Episode.Ids.TmdbId,
                        EpisodeTraktId = item.Episode.Ids.Trakt,
                        EpisodeTvdbId = item.Episode.Ids.TvdbId,
                        Episode = item.Episode.Number,
                        Season = item.Episode.Season
                    });
                }

                UIUtils.UpdateStatus("Creating commented episodes csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "commented_episodes.csv"), episodesCommented);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of commented episodes from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateCommentedSeasonsCsv()
        {
            if (Cancel) return;

            int i = 0;
            UIUtils.UpdateStatus($"Getting commented seasons from trakt.tv, Page: {++i}");
            var firstPage = TraktAPI.TraktAPI.GetUsersComments(type: "seasons");
            if (firstPage != null)
            {
                int pageCount = firstPage.TotalPages;

                // store the results from the first page request
                var pagedItems = firstPage.Comments;

                for (i = 2; i <= firstPage.TotalPages; i++)
                {
                    UIUtils.UpdateStatus($"Getting commented seasons from trakt.tv, Page: {i}/{pageCount}");
                    var nextPage = TraktAPI.TraktAPI.GetUsersComments(type: "seasons", page: i);
                    if (nextPage == null) break;

                    // add the next page of items to the collection
                    pagedItems = pagedItems.Union(nextPage.Comments);
                }

                var seasonsCommented = new List<object>();
                foreach (var item in pagedItems)
                {
                    seasonsCommented.Add(new
                    {
                        Id = item.Comment.Id,
                        ParentId = item.Comment.ParentId,
                        Comment = item.Comment.Text,
                        IsSpoiler = item.Comment.IsSpoiler,
                        IsReview = item.Comment.IsReview,
                        Replies = item.Comment.Replies,
                        Likes = item.Comment.Likes,
                        UserRating = item.Comment.UserRating,
                        CreatedAt = item.Comment.CreatedAt,
                        UpdatedAt = item.Comment.UpdatedAt,
                        ShowTraktId = item.Show.Ids.Trakt,
                        ShowTvdbId = item.Show.Ids.TvdbId,
                        ShowImdbId = item.Show.Ids.ImdbId,
                        ShowTmdbId = item.Show.Ids.TmdbId,
                        ShowSlug = item.Show.Ids.Slug,
                        ShowTitle = item.Show.Title,
                        ShowYear = item.Show.Year,
                        SeasonTraktId = item.Season.Ids.Trakt,           
                        SeasonTmdbId = item.Season.Ids.TmdbId,
                        SeasonsTvdbId = item.Season.Ids.TvdbId,
                        Season = item.Season.Number
                    });
                }

                UIUtils.UpdateStatus("Creating commented seasons csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "commented_seasons.csv"), seasonsCommented);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of commented seasons from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateCommentedShowsCsv()
        {
            if (Cancel) return;

            int i = 0;
            UIUtils.UpdateStatus($"Getting commented shows from trakt.tv, Page: {++i}");
            var firstPage = TraktAPI.TraktAPI.GetUsersComments(type: "shows");
            if (firstPage != null)
            {
                int pageCount = firstPage.TotalPages;

                // store the results from the first page request
                var pagedItems = firstPage.Comments;

                for (i = 2; i <= firstPage.TotalPages; i++)
                {
                    UIUtils.UpdateStatus($"Getting commented shows from trakt.tv, Page: {i}/{pageCount}");
                    var nextPage = TraktAPI.TraktAPI.GetUsersComments(type: "shows", page: i);
                    if (nextPage == null) break;

                    // add the next page of items to the collection
                    pagedItems = pagedItems.Union(nextPage.Comments);
                }

                var showsCommented = new List<object>();
                foreach (var item in pagedItems)
                {
                    showsCommented.Add(new
                    {
                        Id = item.Comment.Id,
                        ParentId = item.Comment.ParentId,
                        Comment = item.Comment.Text,
                        IsSpoiler = item.Comment.IsSpoiler,
                        IsReview = item.Comment.IsReview,
                        Replies = item.Comment.Replies,
                        Likes = item.Comment.Likes,
                        UserRating = item.Comment.UserRating,
                        CreatedAt = item.Comment.CreatedAt,
                        UpdatedAt = item.Comment.UpdatedAt,
                        ShowTraktId = item.Show.Ids.Trakt,
                        ShowTvdbId = item.Show.Ids.TvdbId,
                        ShowImdbId = item.Show.Ids.ImdbId,
                        ShowTmdbId = item.Show.Ids.TmdbId,
                        ShowSlug = item.Show.Ids.Slug,
                        ShowTitle = item.Show.Title,
                        ShowYear = item.Show.Year
                    });
                }

                UIUtils.UpdateStatus("Creating commented shows csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "commented_shows.csv"), showsCommented);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of commented shows from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateCommentedMoviesCsv()
        {
            if (Cancel) return;

            int i = 0;
            UIUtils.UpdateStatus($"Getting commented movies from trakt.tv, Page: {++i}");
            var firstPage = TraktAPI.TraktAPI.GetUsersComments(type: "movies");
            if (firstPage != null)
            {
                int pageCount = firstPage.TotalPages;

                // store the results from the first page request
                var pagedItems = firstPage.Comments;

                for (i = 2; i <= firstPage.TotalPages; i++)
                {
                    UIUtils.UpdateStatus($"Getting commented movies from trakt.tv, Page: {i}/{pageCount}");
                    var nextPage = TraktAPI.TraktAPI.GetUsersComments(type: "movies", page: i);
                    if (nextPage == null) break;

                    // add the next page of items to the collection
                    pagedItems = pagedItems.Union(nextPage.Comments);
                }

                var moviesCommented = new List<object>();
                foreach (var item in pagedItems)
                {
                    moviesCommented.Add(new
                    {
                        Id = item.Comment.Id,
                        ParentId = item.Comment.ParentId,
                        Comment = item.Comment.Text,
                        IsSpoiler = item.Comment.IsSpoiler,
                        IsReview = item.Comment.IsReview,
                        Replies = item.Comment.Replies,
                        Likes = item.Comment.Likes,
                        UserRating = item.Comment.UserRating,
                        CreatedAt = item.Comment.CreatedAt,
                        UpdatedAt = item.Comment.UpdatedAt,
                        MovieTraktId = item.Movie.Ids.Trakt,
                        MovieImdbId = item.Movie.Ids.ImdbId,
                        MovieTmdbId = item.Movie.Ids.TmdbId,
                        MovieSlug = item.Movie.Ids.Slug,
                        MovieTitle = item.Movie.Title,
                        MovieYear = item.Movie.Year
                    });
                }

                UIUtils.UpdateStatus("Creating commented movies csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "commented_movies.csv"), moviesCommented);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of commented movies from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void CreateCommentedListsCsv()
        {
            if (Cancel) return;

            int i = 0;
            UIUtils.UpdateStatus($"Getting commented lists from trakt.tv, Page: {++i}");
            var firstPage = TraktAPI.TraktAPI.GetUsersComments(type: "lists");
            if (firstPage != null)
            {
                int pageCount = firstPage.TotalPages;

                // store the results from the first page request
                var pagedItems = firstPage.Comments;

                for (i = 2; i <= firstPage.TotalPages; i++)
                {
                    UIUtils.UpdateStatus($"Getting commented lists from trakt.tv, Page: {i}/{pageCount}");
                    var nextPage = TraktAPI.TraktAPI.GetUsersComments(type: "lists", page: i);
                    if (nextPage == null) break;

                    // add the next page of items to the collection
                    pagedItems = pagedItems.Union(nextPage.Comments);
                }

                var listsCommented = new List<object>();
                foreach (var item in pagedItems)
                {
                    listsCommented.Add(new
                    {
                        Id = item.Comment.Id,
                        ParentId = item.Comment.ParentId,
                        Comment = item.Comment.Text,
                        IsSpoiler = item.Comment.IsSpoiler,
                        IsReview = item.Comment.IsReview,
                        Replies = item.Comment.Replies,
                        Likes = item.Comment.Likes,
                        UserRating = item.Comment.UserRating,
                        CreatedAt = item.Comment.CreatedAt,
                        UpdatedAt = item.Comment.UpdatedAt,
                        ListId = item.List.Ids.Trakt,
                        ListSlug = item.List.Ids.Slug,
                        ListName = item.List.Name
                    });
                }

                UIUtils.UpdateStatus("Creating commented lists csv file");
                WriteToCsv(Path.Combine(AppSettings.CsvExportPath, "commented_lists.csv"), listsCommented);
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of commented lists from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        private static void WriteToCsv(string filename, List<object> records)
        {
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var writer = new StreamWriter(filename))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
