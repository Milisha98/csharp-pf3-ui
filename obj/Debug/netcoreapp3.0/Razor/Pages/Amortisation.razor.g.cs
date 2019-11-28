#pragma checksum "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\Amortisation.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8deba446296a8449c524b59aefdac130e4f74e6f"
// <auto-generated/>
#pragma warning disable 1591
namespace PF3_UI.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using PF3_UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using PF3_UI.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\Amortisation.razor"
using PF3_UI.Model;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\Amortisation.razor"
using PF3_UI.Mortgage;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\Amortisation.razor"
using PF3_UI.Mortgage.Events;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/amortisation")]
    public class Amortisation : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1>Amortisation</h1>\n\n");
            __builder.OpenElement(1, "div");
            __builder.AddAttribute(2, "class", "container");
            __builder.AddMarkupContent(3, "\n    ");
            __builder.OpenElement(4, "div");
            __builder.AddAttribute(5, "class", "row");
            __builder.AddMarkupContent(6, "\n        ");
            __builder.AddMarkupContent(7, "<label for=\"txtBalance\" class=\"col-sm-1\">Balance:</label>\n        ");
            __builder.OpenElement(8, "input");
            __builder.AddAttribute(9, "id", "txtBalance");
            __builder.AddAttribute(10, "type", "text");
            __builder.AddAttribute(11, "class", "col-sm-2");
            __builder.AddAttribute(12, "value", 
#nullable restore
#line 11 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\Amortisation.razor"
                                                                    model.Balance

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(13, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 11 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\Amortisation.razor"
                                                                                               e => Update(new SetBalance(e))

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.AddMarkupContent(14, "\n        ");
            __builder.OpenElement(15, "p");
            __builder.AddContent(16, 
#nullable restore
#line 12 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\Amortisation.razor"
            model.Balance

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(17, "\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(18, "\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 16 "C:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\Amortisation.razor"
       
    private MortgageUI model = new MortgageUI();

    private async void Update(IUpdateEvent e)
    {        
        model = MortgageUpdate.Update(model, e);
        await Task.Delay(100);
        StateHasChanged();
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591