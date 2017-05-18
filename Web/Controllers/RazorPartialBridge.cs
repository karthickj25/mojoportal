using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

//Reference: http://stackoverflow.com/a/1074061/626911
public class RazorPartialBridge
{
    public class WebFormShimController : Controller { }
    public static void RenderPartial(string partialName, object model)
    {
        //get a wrapper for the legacy WebForm context
        var httpCtx = new HttpContextWrapper(System.Web.HttpContext.Current);

        //create a mock route that points to the empty controller
        var rt = new RouteData();
        rt.Values.Add("controller", "WebFormShimController");

        //create a controller context for the route and http context
        var ctx = new ControllerContext(
            new RequestContext(httpCtx, rt), new WebFormShimController());

        //find the partial view using the viewengine
        var view = ViewEngines.Engines.FindPartialView(ctx, partialName).View;

        //create a view context and assign the model
        var vctx = new ViewContext(ctx, view,
            new ViewDataDictionary { Model = model },
            new TempDataDictionary(), httpCtx.Response.Output);

        //render the partial view
        view.Render(vctx, System.Web.HttpContext.Current.Response.Output);
    }
}