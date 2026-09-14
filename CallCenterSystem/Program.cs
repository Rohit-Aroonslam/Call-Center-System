using CallCenterSystem.Components;
using CallCenterSystem.Interfaces;
using CallCenterSystem.Services;
using CallCenterSystem.Services.Proxy;

namespace CallCenterSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddSingleton<CallCenterAppService>();
            // --- Proxy pattern (Bantu) ---
            builder.Services.AddSingleton<AccessAuditLog>();
            builder.Services.AddSingleton<ISessionContext, SessionContext>();
            builder.Services.AddSingleton<ICallCenterService>(sp =>
                new CallCenterServiceProxy(
                    new CallCenterServiceAdapter(sp.GetRequiredService<CallCenterAppService>()),
                    sp.GetRequiredService<ISessionContext>(),
                    sp.GetRequiredService<AccessAuditLog>()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
