using System;
using System.Threading.Tasks;
using HourglassServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HourglassServer.Custom.Exception
{
    public class ExceptionHandler
    {
        public static async Task<IActionResult> TryAsyncApiAction(Controller controller, Func<Task<IActionResult>> apiAction)
        {
            return await TryAsyncApiAction(controller, apiAction, ExceptionTag.BadValue);
        }

        public static IActionResult TryApiAction(Controller controller, Func<IActionResult> apiAction)
        {
            return TryApiAction(controller, apiAction, ExceptionTag.BadValue);
        }

        public static async Task<IActionResult> TryAsyncApiAction(Controller controller, Func<Task<IActionResult>> apiAction, string defaultTag)
        {
            try
            {
                return await apiAction();
            }
            catch (HourglassException e)
            {
                return controller.BadRequest(e);
            }
            catch (DbUpdateException e)
            {
                return controller.BadRequest(new HourglassException(e.ToString(), ExceptionTag.QueryFailed));
            }
            catch (System.Exception e)
            {
                return controller.BadRequest(new HourglassException(e.ToString(), defaultTag));
            }
        }

        public static IActionResult TryApiAction(Controller controller, Func<IActionResult> apiAction, string defaultTag)
        {
            try
            {
                return apiAction();
            }
            catch (HourglassException e)
            {
                return controller.BadRequest(e);
            }
            catch(DbUpdateException e)
            {
                return controller.BadRequest(new HourglassException(e.ToString(), ExceptionTag.QueryFailed));
            }
            catch (System.Exception e)
            {
                return controller.BadRequest(new HourglassException(e.ToString(), defaultTag));
            }
        }
    }
}
