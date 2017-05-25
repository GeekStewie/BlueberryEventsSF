<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1">
    <title></title>
    <!-- Mobile Specific Meta -->
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <!-- Stylesheets -->
    <link href="css/bootstrap.min.css" rel="stylesheet" media="screen" type="text/css" />
    <link rel="stylesheet" href="css/style.css" media="screen" type="text/css" />
    <!-- Google Font Code -->
    <link href='http://fonts.googleapis.com/css?family=Roboto:400,100,100italic,300,300italic,400italic,500,500italic,700,700italic,900,900italic' rel='stylesheet' type='text/css'>
    <link href="fonts/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/responsive.css" rel="stylesheet" media="screen" type="text/css" />
    <link rel="stylesheet" href="css/prettyPhoto.css" type="text/css" media="screen" title="prettyPhoto main stylesheet" />
    <style type="text/css"></style>
    <!--[if lt IE 9]>
	    <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->
    <link rel="apple-touch-icon" href="img/icons/apple-touch-icon.png">
    <link rel="apple-touch-icon" sizes="72x72" href="img/icons/apple-touch-icon-72x72.png">
    <link rel="apple-touch-icon" sizes="114x114" href="img/icons/apple-touch-icon-114x114.png">
    <link rel="apple-touch-icon" sizes="144x144" href="img/icons/apple-touch-icon.png">

    <script type="text/javascript" src="js/modernizr-1.0.min.js"></script>
</head>
<body onload="initialize()">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div id="texture">

            <!--BEGIN-INTRO-BOX-->
            <section id="main" class="page-block" runat="server">
                <div class="container">
                    <header class="clearfix">
                        <div class="text-center">
                            <div class="venue">

                                <h1>
                                    <asp:Literal ID="_EventName" runat="server"></asp:Literal><br>
                                    <small>On
                                        <asp:Literal ID="_EventDate" runat="server"></asp:Literal></small></h1>
                                <h2>
                                    <asp:Literal ID="_EventVenueName" runat="server"></asp:Literal></h2>
                                <p class="text-muted">
                                    <asp:Literal ID="_EventVenueAddress" runat="server"></asp:Literal>
                                </p>
                            </div>
                            <button runat="server" id="btnRegister" onserverclick="btnRegister_Click1" type="button" class="btn btn-default">Register for this Event</button>
                        </div>
                    </header>
                </div>
            </section>

            <!--BEGIN-INTRO-BOX-->
            <section id="intro" class="page-block" runat="server">
                <div class="container">
                    <div class="section">
                        <article>
                            <p>
                                <asp:Literal ID="_EventDescription" runat="server"></asp:Literal>
                            </p>
                        </article>
                    </div>
                    <!--end-section-->
                </div>
            </section>
            <!--END-INTRO-BOX-->

            <section id="ResultBox" class="page-block" runat="server" visible="false">
                <div class="container">
                    <div class="section">
                        <article>
                            <h3><span>oops, something went wrong...</span></h3>
                            <p>
                                <asp:Literal ID="ResultText" runat="server"></asp:Literal>
                            </p>

                        </article>
                    </div>
                </div>
            </section>

            

            <section id="schedule" class="page-block" runat="server">
                <div class="container">
                    <div class="section no-padding clearfix">
                        <div class="tab-pane active" id="event-schedule">
                            <h3><i class="fa fa-clock-o"></i>Event <span>Schedule</span></h3>
                            <asp:Literal ID="_EventScheduleDescription" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
            </section>

            <!--BEGIN-VENUE-->
            <section id="venue" class="page-block" runat="server">
                <div class="container">
                    <div class="section clearfix bottom-margin">
                        <header class="page-head colored clearfix">
                            <h2>Venue</h2>
                            <p class="text-muted">Location that you'll be looking for</p>
                        </header>
                        <article>
                            <div class="row clearfix">
                                <div class="col-md-4 col-sm-12 column">
                                    <div class="column-content">
                                        <h3>Event <span>Location</span></h3>
                                        <ul class="address">
                                            <li><i class="fa fa-map-marker"></i><strong>Visit Us</strong><br>
                                                <asp:Literal ID="SmallAddress" runat="server"></asp:Literal></li>
                                            <li><i class="fa fa-phone"></i><strong>Call Us</strong><br>
                                                1111 222 3333 </li>
                                            <li><i class="fa fa-envelope"></i><strong>Email Us</strong><br>
                                                <asp:Literal ID="EmailLink" runat="server"></asp:Literal>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-12 column directions-input">
                                    <div class="column-content">
                                        <h3><span>Parking</span> Details</h3>
                                        <asp:Literal ID="ParkingInfo" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-12 column directions-results">
                                    <div class="column-content">
                                        <h3><span>Other</span> Information</h3>
                                        <asp:Literal ID="OtherInformation" runat="server"></asp:Literal>
                                    </div>
                                </div>

                            </div>
                            <!--end-of-row-->
                        </article>
                    </div>
                </div>
            </section>
            <!--END-VENUE-->
                        <!--BEGIN-FOOTER-->
            <footer id="footer" class="page-block">
                <div class="container">
                    <div class="section no-border">
                        <header class="page-head colored clearfix">

                            <div class="col-md-6 col-sm-12">
                                <h3 class="subscribeHeading">Check out the Website</h3>
                                <div class="form-row">
                                    <a runat="server" id="WebsiteURL" href="#" target="_blank" style="color:white !important;"></a>
                                </div>
                            </div>

                            <div class="col-md-5 col-md-offset-1 col-sm-12">
                                <h3>Let's Get Social</h3>
                                <div class="social">
                                    <ul class="list-inline">
                                        <asp:Literal ID="Social" runat="server"></asp:Literal>
                                    </ul>
                                </div>
                                <!-- end social -->
                            </div>
                        </header>
                        <!--end-of-section-->
                    </div>
                    </div>
            </footer>

        </div>

    </form>

    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/jquery.backstretch.min.js" type="text/javascript"></script>
    <script src="js/placeholders.js" type="text/javascript"></script>
    <script src="js/jquery.hoverdir.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/jquery.prettyPhoto.js" type="text/javascript"></script>
    <script src="js/custom.js" type="text/javascript"></script>
    <script>
        var main_color = '<%=MainColour%>';
        var page_background_color = '<%=SecondryColour%>';
        $.backstretch([
            "<%=BackgroundImage%>"
        ], {
            fade: 1000,
            duration: 7000
        });
    </script>
    <script src="js/onevent.js" type="text/javascript"></script>



</body>
</html>
