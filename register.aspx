<%@ Page Language="C#" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="_Default" %>

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
    <link rel="stylesheet" href="css/prettyPhoto.css" type="text/css" media="screen" title="prettyPhoto main stylesheet" charset="utf-8" />
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
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>
        <div id="texture">

            <section id="main" class="page-block">
                <div class="container">
                    <header class="clearfix">
                        <div class="text-center">
                            <div class="venue">
                                <h1>Registration</h1>
                            </div>
                        </div>
                    </header>
                </div>
            </section>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                <ContentTemplate>
                                <!--BEGIN-INTRO-BOX-->
            <section id="intro" class="page-block" runat="server">
                <div class="container">
                    <div class="section">
                        <article>
                            <h3>About <span>You</span></h3>
                            <p>Please complete the form below and we can get you registered for the event. Note: That if you have contacted us before then please use the same email address.</p>

                            <div class="row">
                                <div class="col-md-4 form-row">
                                    <input type="text" class="form-control" id="name" name="name" placeholder="Firstname" runat="server" required="required" />
                                </div>
                                <div class="col-md-4 form-row">
                                    <input type="text" class="form-control" id="lastname" name="lastname" placeholder="Lastname" runat="server" required="required" />
                                </div>
                                <div class="col-md-4 form-row">
                                    <input type="email" class="form-control" id="email" name="email" placeholder="Your Email" runat="server" required="required" data-toggle="popover" title="Email" data-content="If you have contacted us before then please use the same email address." />
                                </div>
                            </div>

                           <div class="row">
                                <div class="col-md-4 form-row">
                                    <input type="text" class="form-control" id="Phone" name="Phone" placeholder="Best telephone number to reach you on?" runat="server" data-toggle="popover" title="Telephone Number" data-content="What is the best telephone number to reach you on in case we need to check anything with you." />
                                </div>
                                <div class="col-md-4 form-row">
                                    <input type="text" class="form-control" id="twitter" name="twitter" placeholder="Twitter Username (optional)" runat="server" />
                                </div>
                                <div class="col-md-4 form-row">
                                    <select id="HearAboutUs" runat="server" class="form-control">
                                        <option value="" disabled="disabled" selected="selected">How did you first hear about us? (Optional)</option>
                                        <option value="Family/Friend">Family/Friend</option>
                                        <option value="Poster">Poster</option>
                                        <option value="Leaflet">Leaflet</option>
                                        <option value="Event">Event</option>
                                        <option value="Internet">Internet</option>
                                        <option value="Word of Mouth">Word of Mouth</option>
                                        <option value="Other">Other</option>
                                    </select>
                                </div>
                            </div>

                            <div class="text-center">
                                <br />
                                <button runat="server" id="btnRegister" onserverclick="btnRegister_Click" type="button" class="btn btn-default">Register for this Event</button>
                            </div>


                        </article>
                    </div>
                    <!--end-section-->
                </div>
            </section>
            <!--END-INTRO-BOX-->

            <section id="result" class="page-block" runat="server" visible="false">
                <div class="container">
                    <div class="section">
                        <article>
                            <h3><span>Result</span></h3>
                            <asp:Literal ID="ResultText" runat="server"></asp:Literal>
                            <div class="text-center">
                                <br />
                                <button runat="server" id="btnEventDetails" onserverclick="btnEventDetails_Click" type="button" class="btn btn-default" visible="false">View Event Information</button>

                            </div>
                        </article>
                    </div>
                    <!--end-section-->
                </div>
            </section>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnRegister" />
                </Triggers>

            </asp:UpdatePanel>

            <section id="DebugBox" class="page-block" runat="server" visible="false">
                <div class="container">
                    <div class="section">
                        <article>
                            <h3><span>BeBug Information</span></h3>
                            <p>
                                <asp:Literal ID="DebugMessage" runat="server"></asp:Literal>
                            </p>

                        </article>
                    </div>
                </div>
            </section>

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
        <!--END-FOOTER-->


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
    <script type="text/javascript">

        //Handle Popovers
        $('#email, #Phone').popover({
            placement: "auto top",
            trigger: "click hover focus"
        });

    </script>
</body>
</html>
