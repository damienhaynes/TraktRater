TraktRater
==========

TraktRater is a tool written in C# to help users transfer user episode, show and movies ratings from multiple media database sites around the web to the popular social media site [trakt.tv](http://trakt.tv).

[**DOWNLOADS**](https://github.com/damienhaynes/TraktRater/releases)

Usage
-----

 * First ensure you are running on a Windows Platform with [.Net Framework 4.0](http://www.microsoft.com/en-us/download/details.aspx?id=17851) installed.
 * Download the latest TraktRater release from the repository [downloads](https://github.com/damienhaynes/TraktRater/downloads) section.
 * Run the TraktRater.exe tool from the download location.
 * Enter in your [trakt.tv](http://trakt.tv) username and password in the trakt section.
 * Enter in any details from 3rd Party data sources such as your [thetvdb.com](http://thetvdb.com) account identifier. If you leave any information from the data sources blank then it will be ignored on import.
 * If you wish to mark episodes and movies as watched if you have rated them, then keep the corresponding option checked, otherwise uncheck.
 * Press the **Start Ratings Import** button.

Data Sources
------------
The following data sources are currently supported by this tool:

###IMDb###
The [IMDb](http://imdb.com) provider supports tv show, episode and movie ratings. TraktRater also supports import of your watchlist from IMDb to trakt.tv. Import can either be via CSV export (recommended) or by scraping online using your user id:

 * Login to [IMDb](http://imdb.com).
 * Click on the **Account** link in the top right hand corner of page.
 * From the **History** table click on [ratings history](http://www.imdb.com/list/ratings).
 * Scroll to the bottom of the page and click on the **Export this List** link.
 * Save the csv file to a folder on your computer.
 * In the TraktRater tool, enter in the filename in the textbox provided.
 * Repeat process to export your Watchlist.csv file, if you don't want to import your watchlist then the keep field empty.

If you choose to do a **Web Scrape**, ensure your profile is marked as **Public** otherwise it will fail. Your IMDb User ID can be found from the address bar in your browser window, look for something like **ur13828119**.
 
###TVDb###
The [TVDb](http://thetvdb.com) provider supports tv show and episode ratings. TraktRater requires your Account Identifier, **NOT** your username. You can find your account id by following these steps:

 * Login to [TVDb](http://thetvdb.com).
 * Click on the [**account**](http://thetvdb.com/?tab=userinfo) link in the toolbar.
 * Copy the 16 character string in the **Account Identifier** textbox.
 * Paste the account identifier into the corresponding text field in TraktRater.
 
We also cache any data we get from [TVDb](http://thetvdb.com) so that subsequent retries do not take as long. The series cache expires every 7 days and the UserRatings every 1 day. The cache folder is located here:

    WinVista/Windows7 : C:\Users\$(username)\AppData\Roaming\TraktRater
    WinXP             : C:\Documents and Settings\Application Data\$(username)\TraktRater

###TMDb###
The [TMDb](http://themoviedb.org) provider supports movie/show ratings and watchlists. TraktRater requires access to your account information. To allow Trakt Rater access to your account info follow these steps:

 * Click on the **Start Request Process** in the Trakt Rater application.
 * When a valid request token is recieved from [TMDb](http://themoviedb.org) you will be re-directed in a webbrowser to approve or deny Trakt Rater.
 * Click on the **Allow Button** in webbrowser.
 * Enable or disable the **Sync Watchlist* button. Ratings are done by default.
 * You are now ready to import ratings from [TMDb](http://themoviedb.org), exit the browser and return to the application. If you do not start importing with-in 60 minutes or restart the application before importing then you will need to repeat process.
 
If at any time you want to disable access or request persmission again you can click on the **Disable TMDb Support** link and then repeat steps above. 
 
###Listal###
The [Listal](http://www.listal.com) provider supports tv shows and movie ratings. TraktRater also supports import of your wanted list from Listal, it converts this into a watchlist. Once you login to the listal website, you can follow the link provided in the UI to download the respective XML files for Shows and Movies.

 * Logon to www.listal.com if you haven't already.
 * Download your Movies and/or Shows from [Export](http://www.listal.com/user/export)
 * Use the Movie and/or Show text boxes to browse to the downloaded XML files.
 
Contributing
------------

1. Fork it.
2. Create a branch (`git checkout -b my_traktrater`)
3. Commit your changes (`git commit -am "Added new rating source"`)
4. Push to the branch (`git push origin my_traktrater`)
5. Create an [Issue][1] with a link to your branch

[1]: https://github.com/damienhaynes/TraktRater/issues
