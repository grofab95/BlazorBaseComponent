using Radzen;
using System;
using System.Threading.Tasks;

namespace BlazorBaseComponent.Components
{
    public partial class TestsComponent
    {
        protected override async Task OnInitializedAsync()
        {
            await ShowNotification(new NotificationMessage
            {
                Severity = NotificationSeverity.Info,
                Detail = "Hello World",
                Duration = 4000,
                Summary = "Info"
            });
        }

        private async Task TestFunc()
        {
            var result = await DoSafeFunc(() =>
            {
                var x = 2;
                var y = 3;

                return x + y;
            });

            await ShowNotification(new NotificationMessage
            {
                Severity = NotificationSeverity.Info,
                Detail = "Test func",
                Duration = 4000,
                Summary = $"Wynik: {result}"
            });
        }

        private async Task TestFuncWithException()
        {
            if (!(await ShowConfirm("Czy napewno chcesz wywołać wyjątek?")))
                return;

            var result = await DoSafeFunc(() =>
            {
                var x = 2;
                var y = 0;

                return x / y;
            });

            if (result == 0)
                return;

            await ShowNotification(new NotificationMessage
            {
                Severity = NotificationSeverity.Info,
                Detail = "Test func",
                Duration = 4000,
                Summary = $"Wynik: {result}"
            });
        }

        private async Task TestAction()
        {
            await DoSafeAction(() => Console.WriteLine("Hello World"), new NotificationMessage
            {
                Severity = NotificationSeverity.Info,
                Detail = $"Test Action",
                Duration = 4000,
                Summary = $"Ok !"
            });
        }

        private void GotToCounter()
        {
            Navigator.NavigateTo("Counter");
        }
    }
}
