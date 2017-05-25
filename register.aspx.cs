using sfdc;
using System;
using System.Web.UI;
using BlueBerry;
using System.Drawing;
using ZXing;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Drawing.Imaging;
using ZXing.Common;
using ZXing.QrCode;
using System.Net;

public partial class _Default : System.Web.UI.Page
{
    private string Main_Colour = "";
    private string Secondry_Colour = "";
    private string Background_Image = "";

    protected string MainColour
    {
        get
        {
            return this.Main_Colour;
        }
        set
        {
            this.Main_Colour = value;
        }
    }

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

    protected void page_load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["id"]) == true)
        {
            SetTheme();
            SetResult("This is not the event you are looking for. Sorry the event you are looking for was not found or registration is already closed.");
            Page.Title = "Event Registration Closed";
        }
        else
        {
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                Session["IsNewLead"] = false;
                Session["IsLead"] = false;
                Session["id"] = "NONE";
                Session["theme"] = "";
                Session["CMID"] = "";
            }
            SetPageTheme();
        }

        if (Request.QueryString["debug"] == "1")
            DebugBox.Visible = true;
    }


    private void AddDebugMessage(string MessageStr)
    {
        DebugMessage.Text = String.Format("{0}<br />{1}", DebugMessage.Text, MessageStr);
    }

    private string GetBody(string EventName, string EventAddress)
    {
        return string.Format("You have registered for {0} with email address {1} which is going to be held at {2}. Also attached is a QR code which can be used for quick registration on the day, this can be used on your mobile device or printed off.", EventName, email.Value, EventAddress);
    }
    /// <summary>
    /// Registration Button Click Event Handler
    /// </summary>
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        //Connect to Salesforce
        SalesforceAuth();

        //Check if User Exists already
        string ContactID = GetContactID(email.Value.Trim());
        if (string.IsNullOrEmpty(ContactID))
        {
            string LeadID = GetLeadID(email.Value.Trim());
            if (!string.IsNullOrEmpty(LeadID))
            {
                Session["id"] = LeadID;
                Session["IsLead"] = true;
                Session["IsNewLead"] = false;
            } else
            {
                string InsertedId = CreateNewLead();
                Session["id"] = InsertedId;
                Session["IsLead"] = true;
                Session["IsNewLead"] = true;
            }
        }
        else
        {
            Session["id"] = ContactID;
            Session["IsLead"] = false;
            Session["IsNewLead"] = false;
        }

        //Check User Is not already Registered
        bool AlreadyRegistered = CheckIfRegistered((string)(Session["id"]), (bool)(Session["IsLead"]));
        if (AlreadyRegistered == true)
        {
            SetResult(String.Format("Hi {0}, you are already registered for this event - Look forward to seeing you there!", name.Value));
            btnEventDetails.Visible = true;
            return;
        }
        else
        {
            //Register New Campaign Member
            if (RegisterAttendee((string)(Session["id"]), (bool)(Session["IsLead"])) == true)
            {
                string EventName = string.Empty;
                string EventAddress = string.Empty;
                QueryResult EventInfo = SfdcBinding.query(string.Format("Select c.Campaign.blueberry__Publishing_Title__c, c.Campaign.blueberry__Location__c, c.Campaign.Name From CampaignMember c WHERE c.Id = '{0}'", Session["CMID"]));
                if (EventInfo != null && EventInfo.size > 0)
                {
                    CampaignMember TempCM = (CampaignMember)EventInfo.records[0];
                    EventName = string.IsNullOrEmpty(TempCM.Campaign.blueberry__Publishing_Title__c) ? TempCM.Campaign.Name : TempCM.Campaign.blueberry__Publishing_Title__c;
                    EventAddress = string.IsNullOrEmpty(TempCM.Campaign.blueberry__Location__c) ? "Venue TBC. Please check back later." : TempCM.Campaign.blueberry__Location__c;
                }

                //**START QR CODE GENERATION**
                IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
                string QRCodeString = string.Format("{0}|{1}", Request.QueryString["id"], Session["CMID"]);
                AddDebugMessage(QRCodeString);
                BitMatrix bitMatrix = new QRCodeWriter().encode(QRCodeString, BarcodeFormat.QR_CODE, 500, 500);
                var result = writer.Write(bitMatrix);
                var barcodeBitmap = new Bitmap(result);
                //**END QR CODE GENERATION**

                //**START EMAIL GENERATION**

                bool EmailSendError = false;
                MailMessage mail = new MailMessage() { IsBodyHtml = true, Subject = "Registration Confirmation for " + EventName, Body = GetBody(EventName, EventAddress) };

                //**ADD FROM ADDRESS HERE**
                mail.From = new MailAddress("email@address.here", "Blueberry Events");
                mail.To.Add(email.Value.ToString().Trim());

                using (MemoryStream mstream = new MemoryStream())
                {
                    ContentType type = new ContentType()
                    {
                        MediaType = MediaTypeNames.Image.Jpeg,
                        Name = "Registration.jpg"
                    };
                    barcodeBitmap.Save(mstream, ImageFormat.Jpeg);
                    mstream.Position = 0;

                    //COMMENT THIS LINE OUT TO REMOVE QR IMAGE FROM EMAILS
                    mail.Attachments.Add(new System.Net.Mail.Attachment(mstream, type));

                    //ADD SMTP CREDENTIALS HERE
                    SmtpClient client = new SmtpClient() { Host = "smtp.gmail.com", Port = 587, DeliveryMethod = SmtpDeliveryMethod.Network, EnableSsl = true, Credentials = new System.Net.NetworkCredential("email@address.here", "emailPassWordHere") };


                    try
                    {
                        client.Send(mail);
                    }
                    catch (Exception)
                    {
                        EmailSendError = true;
                    }


                }

                //**END EMAIL GENERATION

                SetResult(String.Format("Thanks for registering {0} - Look forward to seeing you at the event! {1}", name.Value, (EmailSendError ? "You are registered but there appears to be an issue with our mail system at the moment so you will not receive a confirmation email." : "You will also receive an email confirming your registration within the next 15 minutes.")));
                btnEventDetails.Visible = true;
            }
            else
            {
                SetResult("There was a problem registering you for the event.");
            }
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
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

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
                SetResult("Database Connection Error (1):" + e.Message);
            }
            catch (Exception e)
            {
                SfdcBinding = null;
                SetResult("Database Connection Error (2):" + e.Message);
            }
        }

    #endregion

    #region SalesforceHelpers

        /// <summary>
        /// Returns Contact Id for passed Email Address
        /// </summary>
        /// <param name="EmailAddress">Email Address as String</param>
        /// <returns>Id as String</returns>
        private string GetContactID(string EmailAddress)
        {
            QueryResult queryResult = SfdcBinding.query(string.Format("select Id FROM Contact where email = '{0}' limit 1", EmailAddress));
            if (queryResult != null && queryResult.size > 0)
            {
                Contact ContactReturned = (Contact)queryResult.records[0];
                return ContactReturned.Id;
            }
            else
            {
                return string.Empty;
            }
    }

        /// <summary>
        /// Returns the Lead Id for a passed Email Address
        /// </summary>
        /// <param name="EmailAddress">Email Address as a String</param>
        /// <returns>Id as a String</returns>
        private string GetLeadID(string EmailAddress)
        {
            QueryResult queryResult = SfdcBinding.query(string.Format("select Id FROM Lead where email = '{0}' AND IsConverted = false limit 1", EmailAddress));
            if (queryResult != null && queryResult.size > 0)
            {
                Lead LeadReturned = (Lead)queryResult.records[0];
                return LeadReturned.Id;
            }
            else
            {
                return string.Empty;
            }
    }

        //Boolean Check to see if the attendee is already registered
        private Boolean CheckIfRegistered(string AttendeeID, Boolean IsLead)
        {
            //Set Lookup Column
            string ColumnName = "ContactId";
            if (IsLead == true)
            {
                ColumnName = "LeadId";
            }
            //Lookup Campaign Member
            QueryResult queryResult = SfdcBinding.query(string.Format("Select Id from CampaignMember Where {0} = '{1}' AND CampaignId = '{2}'", ColumnName, AttendeeID, Request.QueryString["id"]));
            if (queryResult != null && queryResult.size > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Registers a new Attendee for the selected Event (Campaign)
        private Boolean RegisterAttendee(string AttendeeID, Boolean IsLead)
        {
            //Set Lookup Column
            string ColumnName = "ContactId";
            if (IsLead == true)
            {
                ColumnName = "LeadId";
            }

            //Lookup Campaign Member
            QueryResult queryResult = SfdcBinding.query(string.Format("Select Id from CampaignMember Where {0} = '{1}' AND CampaignId = '{2}'", ColumnName, AttendeeID, Request.QueryString["id"]));
            if (queryResult.size > 0)
            {
                CampaignMember CMReturned = (CampaignMember)queryResult.records[0];
                Session["CMID"] = CMReturned.Id;
                try
                {
                    CampaignMember uCampaignMember = new CampaignMember() { Id = CMReturned.Id, Status = "Responded" };
                    SaveResult[] saveResults = SfdcBinding.update(new sObject[] { uCampaignMember });
                    if (saveResults[0].success)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            else
            {
                try
                {
                    CampaignMember cmNew = new CampaignMember() { CampaignId = Request.QueryString["id"].ToString(), Status = "Responded" };
                    if (IsLead == true)
                    {
                        cmNew.LeadId = AttendeeID;
                    }
                    else
                    {
                        cmNew.ContactId = AttendeeID;
                    }

                    SaveResult[] cmsaveResults = SfdcBinding.create(new sObject[] { cmNew });
                    if (cmsaveResults[0].success)
                    {
                        Session["CMID"] = cmsaveResults[0].id;
                        return true;
                    }
                    else
                    {
                        throw new Exception(String.Format("Event Registration Failed to Create new Campaign Member. {0} {1}", AttendeeID, cmsaveResults[0].errors[0].message));
                    }
                }
                catch (Exception e)
                {

                    throw (e);
                }
            }


        }

        //Inserts a new Lead into the Database
        private string CreateNewLead()
        {

            //Check that Name and Email have been entered
            if (string.IsNullOrEmpty(name.Value) == false && string.IsNullOrEmpty(lastname.Value) == false)
            {
                //Set Information for new Lead
                Lead NewLead = new Lead();
                NewLead.FirstName = name.Value;
                NewLead.LastName = lastname.Value;
                NewLead.Email = email.Value;
                NewLead.LeadSource = HearAboutUs.Value;
                NewLead.Phone = Phone.Value;
                NewLead.Company = string.Format("TBC - {0} {1}", name.Value, lastname.Value);

                //Save new Lead
                SaveResult[] cmsaveResults = SfdcBinding.create(new sObject[] { NewLead });
                if (cmsaveResults[0].success)
                {
                    //Return ID
                    return cmsaveResults[0].id;
                }
                else
                {
                    throw new Exception("Error: Unable to create a new database entry. Error Details:" + cmsaveResults[0].errors[0].message);
                }
            }
            else
            {
                throw new Exception("Error: Unable to create a new database entry. No Email and Name found. Please enter your First name, Last name and Email address into the relevant fields.");
            }
        }

        //Checks to ensure that a valid entry has been selected for the drop-downs
        private Boolean CheckValidEntry(string SelectedOption)
        {
            if (string.IsNullOrEmpty(SelectedOption) == false && SelectedOption.Contains("?") == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Updates current users Email subscription
        private void SetSubscription(string AttendeeID, Boolean IsLead)
        {
        }

        //Saves a note with the Twitter information
        private void SaveTwitterInfo(string AttendeeId)
        {
            if (string.IsNullOrEmpty(twitter.Value) == false && string.IsNullOrEmpty(AttendeeId) == false)
            {
            Note NewNote = new Note() { Body = "Twitter Account Info", Title = "Twitter Handle: " + twitter.Value, ParentId = AttendeeId };
            SaveResult[] saveResults = SfdcBinding.create(new sObject[] { NewNote });
                if (saveResults[0].success == false)
                {
                    throw new Exception("Error: Unable to save Twitter information. Error details: " + saveResults[0].errors[0].message);
                }
            }
        }
        
        //Reads the Background and Theme settings from Salesforce, then applies them using Themesetter
        private void SetPageTheme(){

            try
            {
                SalesforceAuth();
                QueryResult queryResult = SfdcBinding.query(String.Format("SELECT blueberry__background_image__c, blueberry__Theme__c FROM Campaign WHERE Id = '{0}'", Request.QueryString["id"]));
                if (queryResult.size > 0){
                    Campaign CReturned = (Campaign)queryResult.records[0];
                    string ThemeName = CReturned.blueberry__Theme__c;
                    string BackgroundImage = CReturned.blueberry__background_image__c;
                    Session["theme"] = ThemeName ?? "Default";
                    SetTheme(ThemeName, BackgroundImage);
                } 
            }
            catch (SystemException)
            {
                SetTheme();
            }  
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
        }

    #endregion

    #region UIHelpers

        //Sets Result Message and changes visibility of DIVS
        private void SetResult(string MessageString)
        {
            intro.Visible = false;
            result.Visible = true;
            ResultText.Text = MessageString;
        }

        //Allows end user to return to Event Details Page
        protected void btnEventDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx?id=" + Request.QueryString["id"]);
        }

    #endregion

    
}