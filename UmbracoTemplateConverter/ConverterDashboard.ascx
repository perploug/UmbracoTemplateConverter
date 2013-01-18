<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConverterDashboard.ascx.cs" Inherits="UmbracoTemplateConverter.ConverterDashboard" %>

<asp:Button runat="server" OnClick="Convert" Text="Convert all templates"/>

<asp:Literal runat="server" ID="lt_output"></asp:Literal>