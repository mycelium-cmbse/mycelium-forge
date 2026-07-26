i like the following libraries

 - Autofac: for dependency injection
 - Carter: for API routing
 - FluentValidation: for validation
 - FluentResults: returns an object indicating success or failure of an operation instead of throwing/using exceptions.
 - Serilog: logging
 - MessagePack: for messagepack serialization support (low level code generated)
 - system.text.json: json serialization (low level code generated)
 
 
rest api should be a carter project
forge web application shall use blazor blueprint and tailwind
forge needs to scale horizontaly when we get too many users to serve from 1 instance - there will be a database where the mbse projects are stored (kpar, capella, cdp4-comet/ecss-e-tm-10-25, sysml v1) -> storing more kinds of artifacts expands forge beyond kpar, which is the intent
i thnink use docker is the way to go for deployment
i think a devcontainer makes sense to make sure we can have the same development environment for the whole team and in the near future run claude in the container
code geneneration will be done using uml4net
end-to-end testing with playwright


with forge we will be competing with https://sysand.com/

help design using the right combination of SSR + InteractiveServer + InteractiveWebAssembly taking the figma design into account


tailwind integration msbuild:

<!-- Directory.Build.targets -->
<Project>
  <PropertyGroup Condition="'$(UseTailwind)' == 'true'">
    <TailwindVersion>4.2.1</TailwindVersion>
    <TailwindExpectedSha256>a3f9...</TailwindExpectedSha256>
    <TailwindFeedUrl>https://nexus.internal.mycelium/raw/tailwind</TailwindFeedUrl>
    <TailwindToolsDir>$(MSBuildThisFileDirectory)build/tools/tailwind/$(TailwindVersion)/</TailwindToolsDir>
    <TailwindFileName Condition="$([MSBuild]::IsOSPlatform('Windows'))">tailwindcss-windows-x64.exe</TailwindFileName>
    <TailwindFileName Condition="$([MSBuild]::IsOSPlatform('Linux'))">tailwindcss-linux-x64</TailwindFileName>
    <TailwindFileName Condition="$([MSBuild]::IsOSPlatform('OSX'))">tailwindcss-macos-arm64</TailwindFileName>
    <TailwindExe>$(TailwindToolsDir)$(TailwindFileName)</TailwindExe>
    <TailwindMinify Condition="'$(Configuration)' == 'Release'">--minify</TailwindMinify>
  </PropertyGroup>

  <Target Name="RestoreTailwind"
          Condition="'$(UseTailwind)' == 'true' AND !Exists('$(TailwindExe)')">
    <MakeDir Directories="$(TailwindToolsDir)" />
    <DownloadFile SourceUrl="$(TailwindFeedUrl)/$(TailwindVersion)/$(TailwindFileName)"
                  DestinationFolder="$(TailwindToolsDir)" />
    <Exec Command="chmod +x &quot;$(TailwindExe)&quot;"
          Condition="!$([MSBuild]::IsOSPlatform('Windows'))" />
    <GetFileHash Files="$(TailwindExe)" Algorithm="SHA256">
      <Output TaskParameter="Items" ItemName="_TwHash" />
    </GetFileHash>
    <Error Condition="'%(_TwHash.FileHash)' != '$(TailwindExpectedSha256)'"
           Text="Tailwind CLI checksum mismatch. Expected $(TailwindExpectedSha256), got %(_TwHash.FileHash)." />
  </Target>

  <Target Name="BuildTailwind" BeforeTargets="Build"
          DependsOnTargets="RestoreTailwind"
          Condition="'$(UseTailwind)' == 'true'">
    <Exec Command="&quot;$(TailwindExe)&quot; -i Styles/app.css -o wwwroot/css/app.css $(TailwindMinify)" />
  </Target>
</Project>