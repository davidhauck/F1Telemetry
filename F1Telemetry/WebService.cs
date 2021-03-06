﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=4.0.30319.33440.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="WebServiceSoap", Namespace="http://telemetry.azurewebsites.net/RaceUpload")]
public partial class WebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback UploadRaceOperationCompleted;
    
    /// <remarks/>
    public WebService() {
        this.Url = "http://telemetry.azurewebsites.net/WebService.asmx";
    }
    
    /// <remarks/>
    public event UploadRaceCompletedEventHandler UploadRaceCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://telemetry.azurewebsites.net/RaceUpload/UploadRace", RequestNamespace="http://telemetry.azurewebsites.net/RaceUpload", ResponseNamespace="http://telemetry.azurewebsites.net/RaceUpload", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void UploadRace(string username, string trackName, string fastestLap, double sector1, double sector2, double sector3) {
        this.Invoke("UploadRace", new object[] {
                    username,
                    trackName,
                    fastestLap,
                    sector1,
                    sector2,
                    sector3});
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginUploadRace(string username, string trackName, string fastestLap, double sector1, double sector2, double sector3, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadRace", new object[] {
                    username,
                    trackName,
                    fastestLap,
                    sector1,
                    sector2,
                    sector3}, callback, asyncState);
    }
    
    /// <remarks/>
    public void EndUploadRace(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void UploadRaceAsync(string username, string trackName, string fastestLap, double sector1, double sector2, double sector3) {
        this.UploadRaceAsync(username, trackName, fastestLap, sector1, sector2, sector3, null);
    }
    
    /// <remarks/>
    public void UploadRaceAsync(string username, string trackName, string fastestLap, double sector1, double sector2, double sector3, object userState) {
        if ((this.UploadRaceOperationCompleted == null)) {
            this.UploadRaceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadRaceOperationCompleted);
        }
        this.InvokeAsync("UploadRace", new object[] {
                    username,
                    trackName,
                    fastestLap,
                    sector1,
                    sector2,
                    sector3}, this.UploadRaceOperationCompleted, userState);
    }
    
    private void OnUploadRaceOperationCompleted(object arg) {
        if ((this.UploadRaceCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.UploadRaceCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
public delegate void UploadRaceCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
