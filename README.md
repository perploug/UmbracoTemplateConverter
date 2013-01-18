##Umbraco template to views converter

_This project is based upon teleriks Razor converter, which is available here: https://github.com/telerik/razor-converter_

However, a couple of extra things have been added to this converter, so it handles umbraco templates a bit better

* Converts all .master files in the /masterpages directory and places them /views
* Converts umbraco:macro elements to @Umbraco.RenderMacro
* Converts umbraco:item elements to @Umbraco.Field
* Converts asp:content and asp:contentplaceholder to the corresponding @section and @RenderSection
* Handles misc serverside controls a bit differently the the standard telerik converter
* Handles master directives and sets the correct parent view

##Video
Video on how it works here:

http://www.screenr.com/VdX7

##How to install

Make sure, you set the default rendering mode to Mvc in the /config/umbracosettings.config file

Either compile from source and place the 2 dll's in the /bin and the ConverterDashboard.ascx into /usercontrols, or 
just get the pre-compiled files from /build in the project folder.

Then register the usercontrol in the /config/dashboard.config file like so: 

    <control showOnce="true" addPanel="true" panelCaption="Convert masterpages to views">
        /usercontrols/ConverterDashboard.ascx
    </control>

 Then open the dashboard in Umbraco, and click the convert button, and you're done.
 
 You will probably need to go through some of the templates to sort of the areas where the converter
 didnt do things properly, there are areas it cannot handle good enough, but non of your templates
 are lost, they are still in the /masterpages folder for reference. 