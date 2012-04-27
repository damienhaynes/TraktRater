TraktRater
==========

TraktRater is a tool written in C# to help users transfer user episode, show and movies ratings from multiple media database sites around the web to the popular social media site [trakt.tv](http://trakt.tv).

Usage
-----

 * First ensure you are running on a Windows Platform with [.Net Framework 4.0](http://www.microsoft.com/en-us/download/details.aspx?id=17851) installed.
 * Download the latest TraktRater release from the repository [downloads](https://github.com/damienhaynes/TraktRater/downloads) section.
 * Run the TraktRater.exe tool from the download location.
 * Enter in your [trakt.tv](http://trakt.tv) username and password in the trakt section.
 * Enter in any details from 3rd Party data sources such as your [thetvdb.com](http://thetvdb.com) account identifier. If you leave any information from the data sources blank then it will be ignored on import.
 * Press the **Start Ratings Import** button.

Data Sources
------------
The following data sources are currently supported by this tool:

###IMDb###
The [IMDb](http://imdb.com) provider supports tv show and movie ratings. Episode ratings may be added in a future release. TraktRater requires the user ratings from IMDb to be first exported to a csv file, to do this follow these steps:

 * Login to [IMDb](http://imdb.com).
 * Click on the **Account** link in the top right hand corner of page.
 * From the **History** table click on [ratings history](http://www.imdb.com/list/ratings).
 * Scroll to the bottom of the page and click on the **Export this List** link.
 * Save the csv file to a folder on your computer.
 * In the TraktRater tool, enter in the filename in the textbox provided.

###TVDb###
The [TVDb](http://thetvdb.com) provider supports tv show and episode ratings. TraktRater requires your Account Identifier, **NOT** your username. You can find your account id by following these steps:

 * Login to [TVDb](http://thetvdb.com).
 * Click on the [**account**](http://thetvdb.com/?tab=userinfo) link in the toolbar.
 * Copy the 16 character string in the **Account Identifier** textbox.
 * Paste the account identifier into the corresponding text field in TraktRater.

Contributing
------------

1. Fork it.
2. Create a branch (`git checkout -b my_traktrater`)
3. Commit your changes (`git commit -am "Added new rating source"`)
4. Push to the branch (`git push origin my_traktrater`)
5. Create an [Issue][1] with a link to your branch

[1]: https://github.com/damienhaynes/TraktRater/issues