#Introduction 

The Repository Contains Demo Application and DemoWinApp For Decrypt web service operations 
    1. DecryptCardSwipe
    2. DecryptData
    3. GenerateMac

# Clone the repository
 1. Navigate to the main page of the  **repository**. 
 2. Under the  **repository**  name, click  **Clone** .
 3. Use any Git Client(eg.: GitBash, Git Hub for windows, Source tree ) to  **clone**  the  **repository**  using HTTPS.

 Note: reference for  [Cloning a Github Repository](https://help.github.com/en/articles/cloning-a-repository)

# Getting Started

1.  Install .net core (3.1)

    - Demo app requires dotnet core 3.1 is installed
    - DecryptDemoWinApp requires dotnet core 3.1 is installed

2.  Software dependencies( The Following nuget packages are automatically installed when we open and run the project), please recheck and add the references from nuget
 
     Microsoft.Extensions.DependencyInjection

     Microsoft.Extensions.Configuration

     Microsoft.Extensions.Configuration.EnvironmentVariables

     Microsoft.Extensions.Configuration.Json
     
     Microsoft.Extensions.Configuration.Binder
###Note###
 - All operations are dependent on SSL Certificate.
 - Place the certificate in root folder(<<Path Of Cloned Folder>>\DecryptV2.SDemoApp).
    - For windows app 
      - Place the certificate in root folder(<<Path Of Cloned Folder>>\DecryptDemoWinApp).

3. Latest releases
    - Initial release with all commits and changes as on 1st Sep 2020

#Build and Test

Steps to Build and run DecryptV2.DemoApp project ( .net core 3.1)

 From the cmd,  Navigate to the cloned folder and go to DecryptV2Samples
    
 Run the following commands
    
 ```dotnet clean DecryptV2.DemoApps```

 ```dotnet build DecryptV2.DemoApps```

 Navigate from command prompt to DecryptV2.DemoApp folder containing DecryptV2.DemoApp.csproj and run below command

 ```dotnet run --project DecryptV2.DemoApp.csproj```

 This should open the application running in console.

Steps are same as above for DecryptDemoWinApp(.net core 3.1)
