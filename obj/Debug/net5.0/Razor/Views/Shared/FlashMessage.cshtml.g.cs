#pragma checksum "C:\Users\tardi\Rocker_Elevator_Customer_Portal\Views\Shared\FlashMessage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7677f06a1b3158f93dd22a556fb9113db08b0f30"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_FlashMessage), @"mvc.1.0.view", @"/Views/Shared/FlashMessage.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/FlashMessage.cshtml", typeof(AspNetCore.Views_Shared_FlashMessage))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\tardi\Rocker_Elevator_Customer_Portal\Views\_ViewImports.cshtml"
using Rocker_Elevator_Customer_Portal;

#line default
#line hidden
#line 2 "C:\Users\tardi\Rocker_Elevator_Customer_Portal\Views\_ViewImports.cshtml"
using Rocker_Elevator_Customer_Portal.Models;

#line default
#line hidden
#line 1 "C:\Users\tardi\Rocker_Elevator_Customer_Portal\Views\Shared\FlashMessage.cshtml"
using Rocker_Elevator_Customer_Portal.Controllers;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7677f06a1b3158f93dd22a556fb9113db08b0f30", @"/Views/Shared/FlashMessage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"af4e7b9ceeae8a9d1fa4bceda59f4693d19c345d", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_FlashMessage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(52, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
#line 4 "C:\Users\tardi\Rocker_Elevator_Customer_Portal\Views\Shared\FlashMessage.cshtml"
  
    string text = (string)TempData["FlashMessage.Text"];
    string cssClass = "alert alert-";
    if (!string.IsNullOrWhiteSpace(text))
    {
        FlashMessageType type = (FlashMessageType)TempData["FlashMessage.Type"];
        cssClass += type.ToString().ToLower();
    }

#line default
#line hidden
#line 13 "C:\Users\tardi\Rocker_Elevator_Customer_Portal\Views\Shared\FlashMessage.cshtml"
 if (!string.IsNullOrWhiteSpace(text))
{

#line default
#line hidden
            BeginContext(390, 8, true);
            WriteLiteral("    <div");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 398, "\"", 415, 1);
#line 15 "C:\Users\tardi\Rocker_Elevator_Customer_Portal\Views\Shared\FlashMessage.cshtml"
WriteAttributeValue("", 406, cssClass, 406, 9, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(416, 266, true);
            WriteLiteral(@">
        <p>
            <button
                  type=""button""
                  class=""close""
                  data-dismiss=""alert""
                  aria-hidden=""true""
                >
                  &times;
                </button>
            ");
            EndContext();
            BeginContext(683, 4, false);
#line 25 "C:\Users\tardi\Rocker_Elevator_Customer_Portal\Views\Shared\FlashMessage.cshtml"
       Write(text);

#line default
#line hidden
            EndContext();
            BeginContext(687, 28, true);
            WriteLiteral("\r\n        </p>\r\n    </div>\r\n");
            EndContext();
#line 28 "C:\Users\tardi\Rocker_Elevator_Customer_Portal\Views\Shared\FlashMessage.cshtml"
}

#line default
#line hidden
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591