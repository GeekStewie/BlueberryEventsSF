using BlueBerry;
using sfdc;
using System;
using System.Net;
using System.Web.UI;

public partial class _Default : System.Web.UI.Page
{

    private string Main_Colour = "";
    protected string MainColour {
        get
        {
            return this.Main_Colour;
        }
        set
        {
            this.Main_Colour = value;
        }
    }
    private string Secondry_Colour = "";
    protected string SecondryColour
    {
        get
        {
            return this.Secondry_Colour;
        }
        set
        {
            this.Secondry_Colour = value;
        }
    }
    private string Background_Image = "";
    protected string BackgroundImage
    {
        get
        {
            return this.Background_Image;
        }
        set
        {
            this.Background_Image = value;
        }
    }

    //protected void

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["id"]) == true)
        {
            SetNullPage();
        }
        else
        {
            LoadEventInformation();
        }
    }

    #region SalesforceAuthentication

    //Main SalesforceAPI Binding Object
    SforceService SfdcBinding = null;

    /// <summary>
    /// Salesforce Web Service Authentication.
    /// </summary>
    private void SalesforceAuth()
    {
        SfdcBinding = new SforceService();
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //**ADD SALESFORCE CREDENTIALS HERE**
            const string userName = "salesforce.email@here.com";
            const string password = "SalesforcePasswordSalesforceSecurityToken";
            LoginResult CurrentLoginResult = SfdcBinding.login(userName, password);

            SfdcBinding.Url = CurrentLoginResult.serverUrl;
            SfdcBinding.SessionHeaderValue = new SessionHeader();
            SfdcBinding.SessionHeaderValue.sessionId = CurrentLoginResult.sessionId;
        }
        catch (System.Web.Services.Protocols.SoapException e)
        {
            SfdcBinding = null;
            SetResults("Database Connection Error (1):" + e.Message);
        }
        catch (Exception e)
        {
            SfdcBinding = null;
            SetResults("Database Connection Error (2):" + e.Message);
        }
    }

    #endregion

    private void SetNullPage()
    {
        SetTheme();
        Page.Title = "Event Registration Closed";
        _EventDescription.Text = "Event Registration Closed";
        _EventName.Text = "No Event Found";
        _EventDate.Text = "";
        _EventVenueAddress.Text = "";
        btnRegister.Visible = false;
        SetResults("This is not the event you are looking for. Sorry the event you are looking for was not found or registration is already closed.");
    }

    private void SetResults(string Message){
        intro.Visible = false;
        schedule.Visible = false;
        venue.Visible = false;
        ResultBox.Visible = true;
        ResultText.Text = Message;
    }

    private void LoadEventInformation()
    {
        try
        {
            SalesforceAuth();
            QueryResult queryResult = SfdcBinding.query(String.Format("SELECT Name, Description, StartDate, blueberry__Theme__c,  blueberry__Schedule__c, blueberry__Travel_Information__c, blueberry__background_image__c, blueberry__Location__c, blueberry__Publishing_Title__c FROM Campaign WHERE Id = '{0}' and IsActive = true and Status <> 'Completed'", Request.QueryString["id"]));
            if (queryResult.size > 0)
            {
                Campaign CReturned = (Campaign)queryResult.records[0];
                try
                {

                    //Load Information
                    Page.Title = string.IsNullOrEmpty(CReturned.blueberry__Publishing_Title__c) ? CReturned.Name : CReturned.blueberry__Publishing_Title__c + " Event Registration";

                    SetTheme(CReturned.blueberry__Theme__c, CReturned.blueberry__background_image__c );

                    _EventDescription.Text = string.IsNullOrEmpty(CReturned.Description) ? "TBC" : CReturned.Description;
                    _EventName.Text = string.IsNullOrEmpty(CReturned.blueberry__Publishing_Title__c) ? CReturned.Name : CReturned.blueberry__Publishing_Title__c;
                    _EventDate.Text = String.Format("{0:ddd, MMM d, yyyy}", CReturned.StartDate);
                    _EventVenueAddress.Text = string.IsNullOrEmpty(CReturned.blueberry__Location__c) ? "TBC" : CReturned.blueberry__Location__c;
                    _EventVenueAddress.Text = "TBC";
                    SmallAddress.Text = string.IsNullOrEmpty(CReturned.blueberry__Location__c) ? "TBC" : CReturned.blueberry__Location__c;

                    if (string.IsNullOrEmpty(CReturned.blueberry__Schedule__c) == false)
                    {
                        _EventScheduleDescription.Text = CReturned.blueberry__Schedule__c;
                        schedule.Visible = true;
                    }
                    else
                    {
                        schedule.Visible = false;
                    }


                    ParkingInfo.Text = string.IsNullOrEmpty(CReturned.blueberry__Travel_Information__c) ? "No Parking Information" : CReturned.blueberry__Travel_Information__c;
                    OtherInformation.Text = string.IsNullOrEmpty(CReturned.blueberry__Other_Information__c) ? "No Other Information" : CReturned.blueberry__Other_Information__c;

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            else
            {
                SetNullPage();
            }

        }
        catch (Exception ex)
        {
            SetNullPage();
            SetResults("Error: Unable to connect to database. Error Detail: " + ex.Message);
        }
    }

    protected void btnRegister_Click1(object sender, EventArgs e)
    {
        Response.Redirect("register.aspx?id=" + Request.QueryString["id"]);
    }


    private void SetTheme(string ThemeName = "", string BackgroundImageURL = "")
    {
        EventTheme CurrentEventTheme = new EventTheme(ThemeName, BackgroundImageURL);
        Main_Colour = CurrentEventTheme.MainColour;
        Secondry_Colour = CurrentEventTheme.HighLightColour;
        WebsiteURL.HRef = CurrentEventTheme.WebsiteURL;
        WebsiteURL.InnerText = CurrentEventTheme.WebSiteText;
        Social.Text = CurrentEventTheme.SocialHTML;
        Background_Image = CurrentEventTheme.DefaultBackground;
        EmailLink.Text = string.Format("<a href='mailto:{0}'>{1}</a>", CurrentEventTheme.Email, CurrentEventTheme.Email);
    }
}