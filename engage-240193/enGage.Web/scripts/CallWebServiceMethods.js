
// [start] functions for add client page
function lst_clicked(o)
{
    RRE.Web.Services.WebService.SetBroker(o.value, SucceededBindSetBrokerCallback);
    RRE.Web.Services.WebService.GetAddress(o.value, SucceededBindAddressCallback);
    RRE.Web.Services.WebService.GetPostCode(o.value, SucceededBindPostCodeCallback);
    RRE.Web.Services.WebService.GetStateCode(o.value, SucceededBindStateCodeCallback);
    RRE.Web.Services.WebService.ListSuburbs(o.value, SucceededBindSuburbsCallback);
    RRE.Web.Services.WebService.GetSuburb(o.value, SucceededBindSuburbCallback);        
}

// This is the callback function invoked if the Web service
// succeeded.
// It accepts the result object as a parameter.
function SucceededBindSetBrokerCallback(result, eventArgs)
{
   
}
function SucceededBindAddressCallback(result, eventArgs)
{
   document.getElementById(this.getAddressClientId()).value = result;
}
function SucceededBindPostCodeCallback(result, eventArgs)
{
   document.getElementById(this.getPostCodeClientId()).value = result;
}
function SucceededBindStateCodeCallback(result, eventArgs)
{
   document.getElementById(this.getStateCodeClientId()).value = result;
}
function SucceededBindSuburbsCallback(result, eventArgs)
{
   var control = document.getElementById(this.getSuburbClientId())
   control.options.length = 0;
   var a = result;
   for (i=0;i<a.length;i++)
   {
        var opt = document.createElement("option");
        opt.text = a[i];
        opt.value = a[i];
        control.options.add(opt);
   }
}
function SucceededBindSuburbCallback(result, eventArgs)
{
   document.getElementById(this.getSuburbClientId()).value = result.toUpperCase();
}

// [end] functions for add client page


// This is the callback function invoked if the Web service
// succeeded.
// It accepts the result object, the user context, and the 
// calling method name as parameters.
function SucceededCallbackWithContext(result, userContext, methodName)
{
    var output;
    
    // Page element to display feedback.
    var RsltElem = document.getElementById("ResultId");
    
    var readResult;
    if (userContext == "XmlDocument")
	{
	
	    if (document.all) 
	        readResult = 
		        result.documentElement.firstChild.text;
		else
		    // Firefox
		   readResult =
		        result.documentElement.firstChild.textContent;
		
	     RsltElem.innerHTML = "XmlDocument content: " + readResult;
	}
    
}

// This is the callback function invoked if the Web service
// succeeded.
// It accepts the result object as a parameter.
function SucceededCallback(result, eventArgs)
{
    // Page element to display feedback.
    var RsltElem = document.getElementById("ResultId");
    RsltElem.innerHTML = result;
}


// This is the callback function invoked if the Web service
// failed.
// It accepts the error object as a parameter.
function FailedCallback(error)
{
    // Display the error.    
    var RsltElem = 
        document.getElementById("ResultId");
    RsltElem.innerHTML = 
    "Service Error: " + error.get_message();
}

if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
