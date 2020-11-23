using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Serilog;
using System;
using System.Threading.Tasks;

namespace BlazorBaseComponent.Components.Common
{
    public partial class OwnBaseComponent : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] NotificationService NotificationService { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }

        public async Task ShowNotification(NotificationMessage message)
        {
            NotificationService.Notify(message);
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected NavigationManager Navigator => NavigationManager;

        protected async Task<bool> ShowConfirm(string message) => 
            await JsRuntime.InvokeAsync<bool>("confirm", message);        

        protected async Task<T> DoSafeFunc<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                var mesage = ex.Message;

                if (ex.GetType() != typeof(ApplicationException))
                {
                    mesage += " (zapisano do loga)";
                    Log.Error(ex, "Wystąpił wyjątek");
                }

                await ShowNotification(new NotificationMessage 
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Summary = mesage,
                    Duration = 8000
                });
            }

            return default;
        }

        protected async Task DoSafeAction(Action action, NotificationMessage onSuccesMessage)
        {
            try
            {
                action();

                await ShowNotification(onSuccesMessage);
            }
            catch (Exception ex)
            {
                await ShowNotification(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Summary = "Błąd",
                    Duration = 6000
                });
            }
        }
    }
}
