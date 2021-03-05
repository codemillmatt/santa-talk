# Santa Talk Challenge

Extend the intelligent, serverless Santa Talk Xamarin application and get some cool swag! See [this article](https://devblogs.microsoft.com/xamarin/santa-talk-xamarin-challenge/?WT.mc_id=mobile-0000-masoucou) for more details!

## The Challenge

The Santa Talk app in this repo allows you to send a note to Santa, and then find out whether he'll deliver a gift to your house.

It's built using Xamarin.Forms, [Microsoft Cognitive Services Text Analytics](https://docs.microsoft.com/azure/cognitive-services/text-analytics/overview?WT.mc_id=mobile-0000-masoucou), and Azure Functions. Read [this article](https://devblogs.microsoft.com/xamarin/santa-talk-challenge-build-an-intelligent-serverless-xamarin-app?WT.mc_id=mobile-0000-masoucou), and this [blog post](https://codemilltech.com/santa-talk-an-intelligent-serverless-xamarin-app/), to see how everything fits together.

Your challenge is to extend the app in some way.

Some quick things that come to mind are:

* Add in the ability to take a photo, and use [Cognitive Services Computer Vision](https://docs.microsoft.com/azure/cognitive-services/computer-vision/home?WT.mc_id=mobile-0000-masoucou) to have Santa figure out if you've been naughty or nice.
* Save the responses from Santa on device and display them with a [`CarouselView`](https://docs.microsoft.com/xamarin/xamarin-forms/user-interface/carouselview/?WT.mc_id=mobile-0000-masoucou).
* Redesign the user interface using [`Shell`](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/shell/?WT.mc_id=mobile-0000-masoucou).
* Track the user's location when they make a request, and make [annotations on a map](https://docs.microsoft.com/xamarin/xamarin-forms/user-interface/map/?WT.mc_id=mobile-0000-masoucou) where the naughty and nice developers live.
* Connect to [Azure Logic Apps](https://docs.microsoft.com/azure/logic-apps/?WT.mc_id=mobile-0000-masoucou) to send a Tweet whenever Santa gets a "letter".

Get creative! There are no right or wrong answers!

When you've thought of something, clone or fork this repo, make your changes, and then put in a pull request.

Along with that, let us know the features you added/replaced/made better. What you thought of the Azure services and Xamarin products you used. And what went well and what didn't.

## Get Up and Running

To get you up and running as quickly as possible, here is a way to get Text Analytics and Functions to work without any cost to you!

### Creating the Text Analytics Service

1. If you don't already have one, [sign up for a free Azure subscription here](https://azure.microsoft.com/free/?WT.mc_id=mobile-0000-masoucou).
2. Once done, open up the Azure portal: https://portal.azure.com/?WT.mc_id=mobile-0000-masoucou - and sign in.
3. Then open up the [`Azure Cloud Shell`](https://docs.microsoft.com/azure/cloud-shell/overview?WT.mc_id=mobile-0000-masoucou). You can do that by clicking on the button that looks like the command prompt.
![Azure portal cloud shell screen shot](https://res.cloudinary.com/code-mill-technologies-inc/image/upload/v1576715254/command-prompt_dxgndc.png)
4. Once the `Cloud Shell` opens, you'll want to do two things. Create a `Resource Group` and then the `Text Analytics` service. So go ahead and think of names for both right now ... you'll need them for the next step.
5. Enter the following command in the `Cloud Shell`

```language-bash
az group create -l westus2 -g YOUR-RESOURCE-GROUP-NAME-GOES-HERE
```

6. Once the `Resource Group` is created, then you can create the `Text Analytics` service. Enter the following command in the `Cloud Shell`.

```language-bash
az cognitiveservices account create \
--kind TextAnalytics \
--location westus2 \
--sku F0 \
--resource-group YOUR-RESOURCE-GROUP-NAME-GOES-HERE \
--name YOUR-SERVICE-NAME-GOES-HERE
```

That's it! What's neat is that the `--sku F0` indicates the free tier of Text Analytics.

Then you'll be able to browse the `Text Analytics` service you just created. Make note of the `Endpoint` url and the `Key` name.

![Text analytics endpoint url and key screenshot](https://res.cloudinary.com/code-mill-technologies-inc/image/upload/v1576003971/Annotation_2019-12-10_104045_zefzuv.png)

### Running Azure Functions Locally

You'll also want to get the Azure Functions runtime locally. So you can run everything from your machine without having to deploy it to Azure. (Although if you want to deploy to Azure - by all means do so!)

Follow the [documentation](https://docs.microsoft.com/azure/cognitive-services/welcome?WT.mc_id=mobile-0000-masoucou) to get everything installed for the 2.x version of Functions.

What I like to do is run my Xamarin app from Visual Studio, then run my Functions app from VS Code.

So in VS Code, open up the `Functions` folder, and then if VS Code prompts you, allow it to optimize the project to run for VS Code.

You'll also need to do one last thing. And that's create a `local.settings.json` file for the Functions project.

At the same level as the *csproj file for the Functions project add a file named `local.settings.json` and add to it the following contents.

```language-json
{
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "APIKey": "YOUR KEY WILL GO HERE",
    "APIEndpoint": "https://westus2.api.cognitive.microsoft.com/"
  }
}
```

Make sure you replace the `APIKey` and `APIEndpoint` with the values you found from your Text Analytics service in the Azure portal.

And if you're really digging the Serverless Lifestyle - [here are several FREE courses on creating serverless apps with Azure Functions!](https://docs.microsoft.com/learn/paths/create-serverless-applications/?WT.mc_id=mobile-0000-masoucou)

## Make Some Changes!

That should be it! You should now be ready to go and make some changes to the app... we can't wait to see what you come up with!
