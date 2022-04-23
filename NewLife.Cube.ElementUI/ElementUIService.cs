﻿using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using NewLife.Cube.Extensions;
using NewLife.Cube.Modules;
using NewLife.Cube.Services;
using NewLife.Model;

namespace NewLife.Cube.ElementUI;

/// <summary>ElementUI服务</summary>
public static class ElementUIService
{
    /// <summary>使用魔方UI</summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseElementUI(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        // 独立静态文件设置，魔方自己的静态资源内嵌在程序集里面
        var options = new StaticFileOptions();
        {
            var embeddedProvider = new CubeEmbeddedFileProvider(Assembly.GetExecutingAssembly(), "NewLife.Cube.ElementUI.wwwroot");
            if (!env.WebRootPath.IsNullOrEmpty() && Directory.Exists(env.WebRootPath))
                options.FileProvider = new CompositeFileProvider(new PhysicalFileProvider(env.WebRootPath), embeddedProvider);
            else
                options.FileProvider = embeddedProvider;
        }
        app.UseStaticFiles(options);

        var ui = ModelExtension.GetService<UIService>(app.ApplicationServices);
        if (ui != null)
        {
            ui.AddTheme("ElementUI");
            ui.AddSkin("ElementUI");
        }

        return app;
    }
}

[DisplayName("Element皮肤")]
internal class ElementUIModule : IModule
{
    public void Add(IServiceCollection services) { }
    public void Use(IApplicationBuilder app, IWebHostEnvironment env) => app.UseElementUI(env);
}