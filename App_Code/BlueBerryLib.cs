using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BlueBerryLib
/// </summary>
/// 

namespace BlueBerry
{
	public class BlueBerryLib
    {
	    public BlueBerryLib()
	    {
		//
		// TODO: Add constructor logic here
		//
	    }
    }

    public class EventTheme
    {
        private string _MainColour;
        private string _HighlightColour;
        private string _DefaultBackground;
        private string _WebsiteURL;
        private string _SocialHTML;
        private string _WebsiteText;
        private string _ThemeName;
        private string _EmaiLink = "test@email.com";
        private string _PhoneNumber = "555 111 2222";

        public EventTheme(string ThemeName = "", string BackgroundImageURL = "")
        {

            if (String.IsNullOrEmpty(ThemeName) == true)
                ThemeName = "Default";

            this._ThemeName = ThemeName;
            
            SetThemeSettings();

            if (string.IsNullOrEmpty(BackgroundImageURL) == false && BackgroundImageURL.Contains("/") == true)
            {
                this._DefaultBackground = BackgroundImageURL;
            }
            else
            {
                this._DefaultBackground = "img/slider/slide1.jpg";
            }
        }

        public string Theme
        {
            get { return this._ThemeName; }
            set { this._ThemeName = value; }    
        }

        public string MainColour
        {
            get { return this._MainColour; }
        }

        public string DefaultBackground
        {
            get { return this._DefaultBackground; }
        }

        public string WebsiteURL
        {
            get { return this._WebsiteURL; }
        }

        public string WebSiteText
        {
            get { return this._WebsiteText; }
        }

        public string SocialHTML
        {
            get { return this._SocialHTML; }
        }

        public string HighLightColour
        {
            get { return this._HighlightColour; }
        }

        public string Email
        {
            get { return this._EmaiLink; }
        }

        public string Phone
        {
            get { return this._PhoneNumber; }
        }



        //**ADD ADDITIONAL THEMES HERE** THE CASE NAME MUST MATCH THE VALUE FROM THE DROP-DOWN in SALESFORCE
        private void SetThemeSettings()
        {
            switch (_ThemeName)
            {
                case "Default":
                    this._MainColour = "140F2D";
                    this._HighlightColour = "140F2D";
                    this._WebsiteURL = "http://geekstewie.wordpress.com";
                    this._WebsiteText = "geekstewie.wordpress.com";
                    this._SocialHTML = "<li><a class=\"tips\" href=\"https://twitter.com/geekstewie\" target=\"_blank\" title=\"Follow Us on Twitter\"><i class=\"fa fa-twitter\"></i></a></li>" +
                                    "<li><a class=\"tips\" href=\"http://instagram.com/geekstewie\" target=\"_blank\" title=\"Follow Us on Instagram\"><i class=\"fa fa-instagram\"></i></a></li>" +
                                    "<li><a class=\"tips\" href=\"https://www.facebook.com/geekstewie\" target=\"_blank\" title=\"Follow Us on Facebook\"><i class=\"fa fa-facebook\"></i></a></li>";
                    break;
                //case "New Theme":
                //    this._MainColour = "140F2D";
                //    this._HighlightColour = "140F2D";
                //    this._WebsiteURL = "http://geekstewie.wordpress.com";
                //    this._WebsiteText = "geekstewie.wordpress.com";
                //    this._SocialHTML = "<li><a class=\"tips\" href=\"https://twitter.com/geekstewie\" target=\"_blank\" title=\"Follow Us on Twitter\"><i class=\"fa fa-twitter\"></i></a></li>" +
                //                    "<li><a class=\"tips\" href=\"http://instagram.com/geekstewie\" target=\"_blank\" title=\"Follow Us on Instagram\"><i class=\"fa fa-instagram\"></i></a></li>" +
                //                    "<li><a class=\"tips\" href=\"https://www.facebook.com/geekstewie\" target=\"_blank\" title=\"Follow Us on Facebook\"><i class=\"fa fa-facebook\"></i></a></li>";
                //    break;
                default:
                    this._MainColour = "140F2D";
                    this._HighlightColour = "140F2D";
                    this._WebsiteURL = "#";
                    this._WebsiteText = "#";
                    this._SocialHTML = "<li><a class=\"tips\" href=\"https://twitter.com/test\" target=\"_blank\" title=\"Follow Us on Twitter\"><i class=\"fa fa-twitter\"></i></a></li>";
                    break;
            }
        }



    }
}

