﻿using Microsoft.JSInterop;

namespace BlazorAppMSAuth.Client.Services;

public class HostingEnvironmentService
{
    public bool IsWebAssembly { get; private set; }

    public HostingEnvironmentService(IJSRuntime jsRuntime)
    {
        IsWebAssembly = jsRuntime is IJSInProcessRuntime;
    }

    public string EnvironmentName => IsWebAssembly ? "WebAssembly" : "Server";
}