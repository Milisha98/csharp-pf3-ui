#pragma checksum "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\FetchData.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3b3f93941dc5a988a725568d1aa1cdb18485fceb"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace PF3_UI.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using PF3_UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\_Imports.razor"
using PF3_UI.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\FetchData.razor"
using PF3_UI.Data;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/fetchdata")]
    public class FetchData : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 39 "c:\Users\nathan.schultz.CAPRICORN\source\repos\csharp\PF3-UI\Pages\FetchData.razor"
       
    WeatherForecast[] forecasts;

    protected override async Task OnInitializedAsync()
    {
        forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private WeatherForecastService ForecastService { get; set; }
    }
}
#pragma warning restore 1591
